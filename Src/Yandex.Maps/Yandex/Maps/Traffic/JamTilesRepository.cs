// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamTilesRepository
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
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Traffic
{
  internal class JamTilesRepository : 
    ITileRepository<IJamTile>,
    IAsyncTileStorage<IJamTile>,
    IFlusheable
  {
    private readonly ITileMemoryCache<IJamTile> _level1TileCache;
    private readonly ITileCache<IJamTile> _level2TileCache;
    private readonly object _levelSync = new object();

    public event EventHandler<TilesReadyEventArgs<IJamTile>> GetTilesComplete;

    public event EventHandler<AddTilesCompleteEventArgs<IJamTile>> AddTilesComplete;

    protected virtual void OnGetTilesComplete(
      IList<ITileInfo> requestedTiles,
      IList<IJamTile> tiles)
    {
      if (this.GetTilesComplete == null)
        return;
      this.GetTilesComplete((object) this, new TilesReadyEventArgs<IJamTile>(requestedTiles, tiles));
    }

    protected virtual void OnAddTilesComplete(IEnumerable<IJamTile> tiles)
    {
      if (this.AddTilesComplete == null)
        return;
      this.AddTilesComplete((object) this, new AddTilesCompleteEventArgs<IJamTile>(tiles));
    }

    public JamTilesRepository(
      [NotNull] ITileMemoryCache<IJamTile> level1TileCache,
      [NotNull] IJamTileMemoryCache level2TileCache)
    {
      if (level1TileCache == null)
        throw new ArgumentNullException(nameof (level1TileCache));
      if (level2TileCache == null)
        throw new ArgumentNullException(nameof (level2TileCache));
      this._level1TileCache = level1TileCache;
      this._level2TileCache = (ITileCache<IJamTile>) level2TileCache;
    }

    public IJamTile this[ITileInfo tileInfo]
    {
      get => throw new NotSupportedException();
      set
      {
        lock (this._levelSync)
          this._level1TileCache[tileInfo] = this._level2TileCache[tileInfo] = value;
      }
    }

    public bool ContainsKey(ITileInfo tileInfo, BaseLayers layers)
    {
      lock (this._levelSync)
        return this._level1TileCache.ContainsKey(tileInfo) || this._level2TileCache.ContainsKey(tileInfo);
    }

    public void Flush()
    {
      lock (this._levelSync)
      {
        this._level1TileCache.Clear();
        this._level2TileCache.Clear();
      }
    }

    public IAsyncResult ClearPersistentCache(AsyncCallback callback, object state) => throw new NotSupportedException();

    public void BeginAddTiles(IList<IJamTile> tiles)
    {
      lock (this._levelSync)
      {
        this._level1TileCache.WriteTiles(tiles);
        this._level2TileCache.WriteTiles(tiles);
      }
      this.OnAddTilesComplete((IEnumerable<IJamTile>) tiles);
    }

    public void BeginGetTiles(IList<ITileInfo> tileInfos)
    {
      lock (this._levelSync)
      {
        int count = tileInfos.Count;
        if (count == 0)
          return;
        IList<IJamTile> jamTileList = this._level1TileCache.ReadTiles(tileInfos);
        List<ITileInfo> list1 = jamTileList.Select<IJamTile, ITileInfo>((Func<IJamTile, ITileInfo>) (tile => tile.TileInfo)).ToList<ITileInfo>();
        if (list1.Count > 0)
          this.OnGetTilesComplete((IList<ITileInfo>) list1, jamTileList);
        if (list1.Count == count)
          return;
        List<ITileInfo> list2 = tileInfos.Except<ITileInfo>((IEnumerable<ITileInfo>) list1).ToList<ITileInfo>();
        if (list2.Count <= 0)
          return;
        IList<IJamTile> tiles = this._level2TileCache.ReadTiles((IList<ITileInfo>) list2);
        this.OnGetTilesComplete((IList<ITileInfo>) list2, tiles);
      }
    }

    public void RemoveRange(IList<ITileInfo> tileInfos)
    {
      lock (this._levelSync)
      {
        this._level1TileCache.RemoveRange(tileInfos);
        this._level2TileCache.RemoveRange(tileInfos);
      }
    }

    public void Remove(Func<IJamTile, bool> removeCondition)
    {
      lock (this._levelSync)
      {
        this._level1TileCache.Remove(removeCondition);
        this._level2TileCache.Remove(removeCondition);
      }
    }

    public void ResetCachedTileBitmaps()
    {
      foreach (ITile allTile in (IEnumerable<IJamTile>) this._level1TileCache.GetAllTiles())
        allTile.BitmapSource = (object) null;
      this._level2TileCache.Clear();
    }

    public void Initialize()
    {
    }
  }
}
