// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheStorageUpdater
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Yandex.Common;
using Yandex.IO;
using Yandex.Maps.API;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Serialization.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class CacheStorageUpdater : ICacheStorageUpdater
  {
    private readonly IFileStorage _fileStorage;
    private readonly ICacheFolderNameConstructor _cacheFolderNameConstructor;
    private readonly IDataRepository<CacheMetaData> _cacheMetaDataRepository;

    public CacheStorageUpdater(
      [NotNull] IFileStorage fileStorage,
      [NotNull] ICacheFolderNameConstructor cacheFolderNameConstructor,
      [NotNull] IDataRepository<CacheMetaData> cacheMetaDataRepository)
    {
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      if (cacheFolderNameConstructor == null)
        throw new ArgumentNullException(nameof (cacheFolderNameConstructor));
      if (cacheMetaDataRepository == null)
        throw new ArgumentNullException(nameof (cacheMetaDataRepository));
      this._fileStorage = fileStorage;
      this._cacheFolderNameConstructor = cacheFolderNameConstructor;
      this._cacheMetaDataRepository = cacheMetaDataRepository;
    }

    public void UpdateCacheStorageIfNeeded()
    {
      try
      {
        IEnumerable<string> existingCacheFolders = this.GetExistingCacheFolders();
        if (this._cacheMetaDataRepository.Data.LocalCacheVersion != 130)
          return;
        foreach (string path in existingCacheFolders)
        {
          if (path.EndsWith("2x"))
          {
            this._fileStorage.DeleteDirectory(path);
          }
          else
          {
            string fileName = path + "\\config\\config.xml";
            if (this._fileStorage.FileExists(fileName))
              this._fileStorage.DeleteFile(fileName);
            foreach (Yandex.Maps.Config.MapLayer mapLayer in new List<Yandex.Maps.Config.MapLayer>()
            {
              new Yandex.Maps.Config.MapLayer()
              {
                Layer = BaseLayers.sat | BaseLayers.skl,
                Id = 2
              },
              new Yandex.Maps.Config.MapLayer() { Layer = BaseLayers.pmap, Id = 3 },
              new Yandex.Maps.Config.MapLayer() { Layer = BaseLayers.meta, Id = 4 }
            })
            {
              string str = path + "\\" + (object) (int) mapLayer.Layer;
              string pathTo = path + "\\" + (object) mapLayer.Id;
              if (this._fileStorage.DirectoryExists(str))
                this._fileStorage.MoveFile(str, pathTo);
            }
          }
        }
        this._cacheMetaDataRepository.Data.LocalCacheVersion = 135;
        this._cacheMetaDataRepository.Save();
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
    }

    [NotNull]
    private IEnumerable<string> GetExistingCacheFolders()
    {
      if (this._fileStorage.DirectoryExists(this._cacheFolderNameConstructor.CacheRoot))
      {
        IEnumerable<string> localeDirs = this._fileStorage.GetDirectoryNames(this._cacheFolderNameConstructor.CacheRoot);
        foreach (string localeDir in localeDirs)
        {
          string localePath = this._cacheFolderNameConstructor.CacheRoot + "\\" + localeDir;
          IEnumerable<string> scaleDirs = this._fileStorage.GetDirectoryNames(localePath);
          foreach (string scaleDir in scaleDirs)
            yield return localePath + "\\" + scaleDir;
        }
      }
    }
  }
}
