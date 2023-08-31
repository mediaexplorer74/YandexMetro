// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.IO;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Config;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class CacheStorage : ITileStorage<ITile>, IInitializable, IFlusheable, IDisposable
  {
    private const int MaxOpenCacheHeadFiles = 15;
    private const int CacheHeadFilesToCloseAtOnce = 5;
    private readonly object _fileHeaderMutex;
    private readonly IDictionary<string, CacheFileHeader> _cacheHeadFiles = (IDictionary<string, CacheFileHeader>) new Dictionary<string, CacheFileHeader>();
    private readonly IFileStorage _fileStorage;
    private readonly ITileFactory<ITile> _tileFactory;
    private readonly ITileFileNameConstructor _tileFileNameConstructor;
    private readonly IPath _path;
    private readonly ICacheStorageUpdater _cacheStorageUpdater;
    private readonly ICacheConfigManager _cacheConfigManager;
    private bool _isDisposed;
    private readonly IHeaderBlockSerializer _blockHeaderSerializer = (IHeaderBlockSerializer) new HeaderBlockSerializer();
    private bool _initialized;

    public CacheStorage(
      [NotNull] IFileStorage fileStorage,
      [NotNull] ITileFactory<ITile> tileFactory,
      [NotNull] ITileFileNameConstructor tileFileNameConstructor,
      [NotNull] ITileInfoNormalizer tileInfoNormalizer,
      [NotNull] IPath path,
      [NotNull] ICacheStorageUpdater cacheStorageUpdater,
      [NotNull] ICacheConfigManager cacheConfigManager)
    {
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      if (tileFactory == null)
        throw new ArgumentNullException(nameof (tileFactory));
      if (tileFileNameConstructor == null)
        throw new ArgumentNullException(nameof (tileFileNameConstructor));
      if (tileInfoNormalizer == null)
        throw new ArgumentNullException(nameof (tileInfoNormalizer));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (cacheStorageUpdater == null)
        throw new ArgumentNullException(nameof (cacheStorageUpdater));
      if (cacheConfigManager == null)
        throw new ArgumentNullException(nameof (cacheConfigManager));
      this._fileStorage = fileStorage;
      this._tileFactory = tileFactory;
      this._tileFileNameConstructor = tileFileNameConstructor;
      this._path = path;
      this._cacheStorageUpdater = cacheStorageUpdater;
      this._cacheConfigManager = cacheConfigManager;
      this._fileHeaderMutex = new object();
    }

    public void Flush()
    {
      lock (this._fileHeaderMutex)
      {
        foreach (CacheFileHeader cacheFileHeader in (IEnumerable<CacheFileHeader>) this._cacheHeadFiles.Values)
          cacheFileHeader.Dispose();
        this._cacheHeadFiles.Clear();
      }
      this._cacheConfigManager.SaveConfig();
    }

    public void Initialize()
    {
      this._initialized = true;
      this._cacheStorageUpdater.UpdateCacheStorageIfNeeded();
      this._cacheConfigManager.InitializeConfig();
    }

    public void WriteTiles([NotNull] IList<ITile> saveTiles)
    {
      if (saveTiles == null)
        throw new ArgumentNullException(nameof (saveTiles));
      if (!this._initialized)
        throw new InvalidOperationException("Storage is not initialized");
      if (this._isDisposed)
        return;
      Dictionary<ITileInfo, ITile> dictionary = saveTiles.ToDictionary<ITile, ITileInfo, ITile>((Func<ITile, ITileInfo>) (key => key.TileInfo), (Func<ITile, ITile>) (item => item));
      foreach (KeyValuePair<string, List<ITileInfo>> hashFile in this.GetHashFiles((IEnumerable<ITileInfo>) dictionary.Keys))
      {
        CacheFileHeader fileHeader = this.GetFileHeader(hashFile.Key);
        foreach (ITileInfo key in hashFile.Value)
          fileHeader.AddListCacheSaveElement(dictionary[key], false);
        fileHeader.TryFlush();
      }
    }

    [NotNull]
    public IList<ITile> ReadTiles([NotNull] IList<ITileInfo> tiles)
    {
      if (tiles == null)
        throw new ArgumentNullException(nameof (tiles));
      if (!this._initialized)
        throw new InvalidOperationException("Storage is not initialized");
      if (this._isDisposed || !tiles.Any<ITileInfo>())
        return (IList<ITile>) new ITile[0];
      List<ITile> tileList = new List<ITile>();
      foreach (KeyValuePair<string, List<ITileInfo>> hashFile in this.GetHashFiles((IEnumerable<ITileInfo>) tiles))
      {
        IEnumerable<ITile> collection = this.GetFileHeader(hashFile.Key).ReadRegularBlock((IList<ITileInfo>) hashFile.Value);
        tileList.AddRange(collection);
      }
      return (IList<ITile>) tileList;
    }

    [NotNull]
    private CacheFileHeader GetFileHeader(string keyFile)
    {
      CacheFileHeader fileHeader;
      lock (this._fileHeaderMutex)
      {
        if (!this._cacheHeadFiles.TryGetValue(keyFile, out fileHeader))
        {
          fileHeader = new CacheFileHeader(keyFile, this._fileStorage, this._tileFactory, this._tileFileNameConstructor, this._blockHeaderSerializer, this._path);
          this._cacheHeadFiles.Add(keyFile, fileHeader);
          fileHeader.TryDeserialize();
        }
        fileHeader.LastAccessTime = DateTime.UtcNow;
        if (this._cacheHeadFiles.Count > 15)
        {
          foreach (KeyValuePair<string, CacheFileHeader> keyValuePair in this._cacheHeadFiles.OrderBy<KeyValuePair<string, CacheFileHeader>, DateTime>((Func<KeyValuePair<string, CacheFileHeader>, DateTime>) (pair => pair.Value.LastAccessTime)).Take<KeyValuePair<string, CacheFileHeader>>(5).ToArray<KeyValuePair<string, CacheFileHeader>>())
          {
            this._cacheHeadFiles.Remove(keyValuePair.Key);
            keyValuePair.Value.Dispose();
          }
        }
      }
      return fileHeader;
    }

    private Dictionary<string, List<ITileInfo>> GetHashFiles(IEnumerable<ITileInfo> tiles)
    {
      Dictionary<string, List<ITileInfo>> hashFiles = new Dictionary<string, List<ITileInfo>>();
      foreach (ITileInfo tile in tiles)
      {
        string directoryName;
        string tileFileName = this._tileFileNameConstructor.GetTileFileName(tile, out directoryName);
        if (!this._fileStorage.DirectoryExists(directoryName))
          this._fileStorage.CreateDirectory(directoryName);
        List<ITileInfo> tileInfoList;
        if (!hashFiles.ContainsKey(tileFileName))
        {
          tileInfoList = new List<ITileInfo>();
          hashFiles.Add(tileFileName, tileInfoList);
        }
        else
          tileInfoList = hashFiles[tileFileName];
        tileInfoList.Add(tile);
      }
      return hashFiles;
    }

    public void Dispose()
    {
      this.OnDispose();
      GC.SuppressFinalize((object) this);
    }

    protected virtual void OnDispose()
    {
      if (this._isDisposed)
        return;
      this.Flush();
      this._isDisposed = true;
    }

    ~CacheStorage() => this.OnDispose();
  }
}
