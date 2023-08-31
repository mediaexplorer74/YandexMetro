// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.TileRepository
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

namespace Yandex.Maps.Repository
{
  internal class TileRepository : ITileRepository<ITile>, IAsyncTileStorage<ITile>, IFlusheable
  {
    private readonly ITileMemoryCache<ITile> _memoryCache;
    private readonly ITileCache<ITile> _fileCache;

    public event EventHandler<TilesReadyEventArgs<ITile>> GetTilesComplete;

    public event EventHandler<AddTilesCompleteEventArgs<ITile>> AddTilesComplete;

    public TileRepository([NotNull] ITileMemoryCache<ITile> memoryCache, [NotNull] ITileFileCache<ITile> fileCache)
    {
      if (memoryCache == null)
        throw new ArgumentNullException(nameof (memoryCache));
      if (fileCache == null)
        throw new ArgumentNullException(nameof (fileCache));
      this._memoryCache = memoryCache;
      this._fileCache = (ITileCache<ITile>) fileCache;
      this._fileCache.GetTilesComplete += new EventHandler<TilesReadyEventArgs<ITile>>(this.FileCacheGetTilesComplete);
      this._fileCache.AddTilesComplete += new EventHandler<AddTilesCompleteEventArgs<ITile>>(this.FileCacheAddTilesComplete);
    }

    public IAsyncResult ClearPersistentCache(AsyncCallback callback, object state) => ((Action) (() => this._fileCache.Clear())).BeginInvoke(callback, state);

    public void Flush()
    {
      this._memoryCache.Clear();
      this._fileCache.Flush();
    }

    public void RemoveRange(IList<ITileInfo> tileInfos) => this._memoryCache.RemoveRange(tileInfos);

    public void Remove([NotNull] Func<ITile, bool> removeCondition) => this._memoryCache.Remove(removeCondition);

    public void BeginAddTiles(IList<ITile> tiles)
    {
      this._memoryCache.WriteTiles(tiles);
      this.OnAddTilesComplete((IEnumerable<ITile>) tiles);
      this._fileCache.BeginAddTiles(tiles);
    }

    public void BeginGetTiles(IList<ITileInfo> tileInfos)
    {
      IList<ITile> tiles = this._memoryCache.ReadTiles(tileInfos);
      IList<ITileInfo> list1 = (IList<ITileInfo>) tiles.Select<ITile, ITileInfo>((Func<ITile, ITileInfo>) (item => item.TileInfo)).ToList<ITileInfo>();
      if (list1.Any<ITileInfo>())
        this.OnGetTilesComplete(list1, tiles);
      IList<ITileInfo> list2 = (IList<ITileInfo>) tileInfos.Where<ITileInfo>((Func<ITileInfo, bool>) (t => !tiles.Any<ITile>((Func<ITile, bool>) (tile => tile.TileInfo.Equals((object) t))))).ToList<ITileInfo>();
      if (!list2.Any<ITileInfo>())
        return;
      this._fileCache.BeginGetTiles(list2);
    }

    public bool ContainsKey(ITileInfo tileInfo, BaseLayers layers) => this._memoryCache.ContainsKey(tileInfo) || this._fileCache.ContainsKey(tileInfo);

    public ITile this[ITileInfo tileInfo]
    {
      get => throw new NotImplementedException();
      set => this.BeginAddTiles((IList<ITile>) new ITile[1]
      {
        value
      });
    }

    private void FileCacheAddTilesComplete(object sender, AddTilesCompleteEventArgs<ITile> e) => this.OnAddTilesComplete(e.Tiles);

    private void FileCacheGetTilesComplete(object sender, TilesReadyEventArgs<ITile> e)
    {
      this._memoryCache.WriteTiles(e.Tiles);
      this.OnGetTilesComplete(e.RequestedTiles, e.Tiles);
    }

    private void OnAddTilesComplete(IEnumerable<ITile> tiles)
    {
      if (this.AddTilesComplete == null)
        return;
      this.AddTilesComplete((object) this, new AddTilesCompleteEventArgs<ITile>(tiles));
    }

    private void OnGetTilesComplete(IList<ITileInfo> requestedTiles, IList<ITile> tiles)
    {
      if (this.GetTilesComplete == null)
        return;
      this.GetTilesComplete((object) this, new TilesReadyEventArgs<ITile>(requestedTiles, tiles));
    }

    public void ResetCachedTileBitmaps()
    {
      foreach (ITile allTile in (IEnumerable<ITile>) this._memoryCache.GetAllTiles())
        allTile.BitmapSource = (object) null;
    }
  }
}
