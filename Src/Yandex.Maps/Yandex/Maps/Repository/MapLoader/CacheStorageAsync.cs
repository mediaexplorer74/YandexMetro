// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheStorageAsync
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.EventArgs;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class CacheStorageAsync : 
    IClearableTileCache,
    ITileFileCache<ITile>,
    ITileCache<ITile>,
    IAsyncTileStorage<ITile>,
    ITileStorage<ITile>,
    IInitializable,
    IFlusheable,
    IDisposable
  {
    public static int CLIENT = BitConverter.ToInt32(new byte[4]
    {
      (byte) 87,
      (byte) 73,
      (byte) 78,
      (byte) 80
    }, 0);
    public static int MAGIC_CACHE = BitConverter.ToInt32(new byte[4]
    {
      (byte) 89,
      (byte) 77,
      (byte) 67,
      (byte) 70
    }, 0);
    private readonly object _queueMutex;
    private readonly object _threadMutex;
    private readonly List<ITile> _tilesToSave;
    private readonly Queue<ITileInfo> _tilesToSearch;
    private readonly ITileInfoNormalizer _tileInfoNormalizer;
    private readonly ITileStorage<ITile> _tileStorage;
    private readonly IMonitor _monitor;
    private bool _forceWriteTiles = true;
    private bool _isDisposed;
    private bool _writeEnabled;
    private bool _initialized;

    public CacheStorageAsync(
      IThread thread,
      Queue<ITileInfo> tilesToSearchQueue,
      ITileInfoNormalizer tileInfoNormalizer,
      ITileStorage<ITile> tileStorage,
      [NotNull] IMonitor monitor)
    {
      if (thread == null)
        throw new ArgumentNullException(nameof (thread));
      if (tilesToSearchQueue == null)
        throw new ArgumentNullException(nameof (tilesToSearchQueue));
      if (tileInfoNormalizer == null)
        throw new ArgumentNullException(nameof (tileInfoNormalizer));
      if (tileStorage == null)
        throw new ArgumentNullException(nameof (tileStorage));
      if (monitor == null)
        throw new ArgumentNullException(nameof (monitor));
      this._tilesToSave = new List<ITile>();
      this._tilesToSearch = tilesToSearchQueue;
      this._tileInfoNormalizer = tileInfoNormalizer;
      this._tileStorage = tileStorage;
      this._monitor = monitor;
      this._forceWriteTiles = false;
      this._writeEnabled = true;
      this._queueMutex = new object();
      this._threadMutex = new object();
      thread.Start(new Action(this.Worker));
    }

    private void Worker()
    {
      while (!this._isDisposed)
      {
        List<ITile> tileList = (List<ITile>) null;
        List<ITileInfo> tileInfoList = (List<ITileInfo>) null;
        lock (this._queueMutex)
        {
          if (this._tilesToSearch.Any<ITileInfo>())
          {
            tileInfoList = this._tilesToSearch.ToList<ITileInfo>();
            this._tilesToSearch.Clear();
          }
          else
          {
            if (!this._forceWriteTiles)
            {
              if (this._tilesToSave.Count <= 0)
                goto label_9;
            }
            tileList = this._tilesToSave.ToList<ITile>();
            this._tilesToSave.Clear();
          }
        }
label_9:
        if (!this._forceWriteTiles)
        {
          if (tileList != null)
          {
            if (!tileList.Any<ITile>())
              goto label_14;
          }
          else
            goto label_14;
        }
        try
        {
          this.WriteTiles((IList<ITile>) tileList);
        }
        catch (Exception ex)
        {
          this._writeEnabled = false;
        }
label_14:
        if (tileInfoList != null)
        {
          if (tileInfoList.Any<ITileInfo>())
          {
            try
            {
              IList<ITile> tiles = this.ReadTiles((IList<ITileInfo>) tileInfoList);
              this.OnGetTilesComplete((IList<ITileInfo>) tileInfoList, tiles);
            }
            catch
            {
              this.OnGetTilesComplete((IList<ITileInfo>) tileInfoList, (IList<ITile>) new ITile[0]);
            }
          }
        }
        if (!this._forceWriteTiles)
        {
          this._monitor.Enter(this._queueMutex);
          if (!this._tilesToSave.Any<ITile>() && !this._tilesToSearch.Any<ITileInfo>())
          {
            lock (this._threadMutex)
            {
              this._monitor.Exit(this._queueMutex);
              this._monitor.Wait(this._threadMutex);
            }
          }
          else
            this._monitor.Exit(this._queueMutex);
        }
      }
    }

    public void Flush()
    {
      if (!this._initialized)
        return;
      this.ResetQueue();
      this._tileStorage.Flush();
    }

    public void WriteTiles(IList<ITile> saveTiles)
    {
      if (saveTiles == null)
        throw new ArgumentNullException(nameof (saveTiles));
      if (!saveTiles.Any<ITile>())
        return;
      this.Initialize();
      this._tileStorage.WriteTiles(saveTiles);
    }

    public IList<ITile> ReadTiles(IList<ITileInfo> tiles)
    {
      if (tiles == null)
        throw new ArgumentNullException(nameof (tiles));
      this.Initialize();
      return this._tileStorage.ReadTiles(tiles);
    }

    protected virtual void OnAddTilesComplete(IEnumerable<ITile> tiles)
    {
      EventHandler<AddTilesCompleteEventArgs<ITile>> addTilesComplete = this.AddTilesComplete;
      if (addTilesComplete == null)
        return;
      addTilesComplete((object) this, new AddTilesCompleteEventArgs<ITile>(tiles));
    }

    protected virtual void OnGetTilesComplete(IList<ITileInfo> requestedTiles, IList<ITile> tiles)
    {
      EventHandler<TilesReadyEventArgs<ITile>> getTilesComplete = this.GetTilesComplete;
      if (getTilesComplete == null)
        return;
      getTilesComplete((object) this, new TilesReadyEventArgs<ITile>(requestedTiles, tiles));
    }

    public void Dispose()
    {
      this.OnDispose();
      GC.SuppressFinalize((object) this);
    }

    private void OnDispose()
    {
      if (this._isDisposed)
        return;
      lock (this._queueMutex)
      {
        this._forceWriteTiles = true;
        this.WriteTiles((IList<ITile>) this._tilesToSave);
      }
      lock (this._threadMutex)
      {
        this._isDisposed = true;
        this._monitor.PulseAll(this._threadMutex);
      }
      if (!(this._tileStorage is IDisposable tileStorage))
        return;
      tileStorage.Dispose();
    }

    public void Initialize()
    {
      if (this._initialized)
        return;
      this._initialized = true;
      this._tileStorage.Initialize();
    }

    ~CacheStorageAsync() => this.OnDispose();

    public event EventHandler<TilesReadyEventArgs<ITile>> GetTilesComplete;

    public event EventHandler<AddTilesCompleteEventArgs<ITile>> AddTilesComplete;

    public void BeginAddTiles(IList<ITile> tiles)
    {
      foreach (ITile tile1 in (IEnumerable<ITile>) tiles)
      {
        ITile tile = tile1;
        if ((tile.Status & TileStatus.Error) != TileStatus.Error)
        {
          lock (this._queueMutex)
          {
            if (this._writeEnabled)
            {
              if (tile != null)
              {
                if (tile.Bytes != null)
                {
                  if (tile.DataLength != 0)
                  {
                    if (this._tileInfoNormalizer.IsNormal(tile.TileInfo))
                    {
                      foreach (ITile tile2 in this._tilesToSave.Where<ITile>((Func<ITile, bool>) (item => item.TileInfo.Equals((object) tile.TileInfo))).ToArray<ITile>())
                        this._tilesToSave.Remove(tile2);
                      this._tilesToSave.Add(tile);
                    }
                  }
                }
              }
            }
          }
        }
      }
      lock (this._threadMutex)
        this._monitor.PulseAll(this._threadMutex);
      this.OnAddTilesComplete((IEnumerable<ITile>) tiles);
    }

    public void BeginGetTiles(IList<ITileInfo> tileInfos)
    {
      lock (this._queueMutex)
      {
        foreach (ITileInfo tileInfo in tileInfos.Except<ITileInfo>((IEnumerable<ITileInfo>) this._tilesToSearch))
          this._tilesToSearch.Enqueue(tileInfo);
      }
      lock (this._threadMutex)
        this._monitor.PulseAll(this._threadMutex);
    }

    public ITile this[ITileInfo tileInfo]
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public bool ContainsKey(ITileInfo tileInfo) => throw new NotImplementedException();

    public void Clear() => throw new NotImplementedException();

    private void ResetQueue()
    {
      lock (this._queueMutex)
        this._tilesToSearch.Clear();
      lock (this._threadMutex)
        this._monitor.PulseAll(this._threadMutex);
    }

    public void RemoveRange(IList<ITileInfo> tileInfos) => throw new NotImplementedException();

    public void Remove(Func<ITile, bool> removeCondition) => throw new NotImplementedException();
  }
}
