// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.QueryQueueManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Yandex.Collections.Interfaces;
using Yandex.ItemsCounter;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Repository
{
  [ComVisible(true)]
  public class QueryQueueManager : IQueryQueueManager<ITile>, IStateService
  {
    private readonly IEnumerableQueue<ITileInfo> _queue;
    private readonly ITileFetcher<ITile> _fetcher;
    private readonly IItemCounter _itemCounter;
    private readonly object _queueLock = new object();

    public event EventHandler<TilesReadyEventArgs<ITile>> TilesReady;

    public event EventHandler<TileRequestFailedEventArgs> RequestFailed;

    public QueryQueueManager(
      IEnumerableQueue<ITileInfo> queue,
      ITileFetcher<ITile> fetcher,
      IItemCounter counter)
    {
      if (queue == null)
        throw new ArgumentNullException(nameof (queue));
      if (fetcher == null)
        throw new ArgumentNullException(nameof (fetcher));
      if (counter == null)
        throw new ArgumentNullException(nameof (counter));
      this._queue = queue;
      this._fetcher = fetcher;
      this._itemCounter = counter;
      this._fetcher.TilesReady += new EventHandler<TilesReadyEventArgs<ITile>>(this.FetcherTilesReady);
      this._fetcher.RequestFailed += new EventHandler<TileRequestFailedEventArgs>(this.FetcherRequestFailed);
      this._itemCounter.CountChanged += new EventHandler(this.ItemCounterCountChanged);
    }

    private void FetcherTilesReady(object sender, TilesReadyEventArgs<ITile> e)
    {
      IList<ITile> tiles = e.Tiles;
      if (tiles != null)
        this.OnTilesReady(new TilesReadyEventArgs<ITile>(e.RequestedTiles, tiles));
      this.SendQuery();
    }

    private void FetcherRequestFailed(object sender, TileRequestFailedEventArgs e)
    {
      this.OnRequestFailed(e);
      this.SendQuery();
    }

    private void ItemCounterCountChanged(object sender, EventArgs e)
    {
      if (this._itemCounter.Count >= (long) this.MaxRequestsCount)
        return;
      this.SendQuery();
    }

    protected virtual void SendQuery()
    {
      List<ITileInfo> tileInfoList = new List<ITileInfo>((int) this._fetcher.MaxTilesInSingleQuery);
      while (this._itemCounter.Count < (long) this.MaxRequestsCount && this._queue.Count > 0)
      {
        ITileInfo tileInfo = (ITileInfo) null;
        lock (this._queueLock)
        {
          if (this._queue.Count > 0)
            tileInfo = this._queue.Dequeue();
        }
        if (tileInfo != null)
        {
          tileInfoList.Add(tileInfo);
          if (this._fetcher.MaxTilesInSingleQuery == 0U || (long) tileInfoList.Count >= (long) this._fetcher.MaxTilesInSingleQuery)
          {
            this._fetcher.QueryTilesAsync((IEnumerable<ITileInfo>) tileInfoList);
            tileInfoList.Clear();
          }
        }
      }
      if (tileInfoList.Count <= 0)
        return;
      this._fetcher.QueryTilesAsync((IEnumerable<ITileInfo>) tileInfoList);
    }

    public uint MaxRequestsCount { get; set; }

    public void QueryTileRange(IEnumerable<ITileInfo> ti)
    {
      lock (this._queueLock)
      {
        foreach (ITileInfo tileInfo in ti)
          this._queue.Enqueue(tileInfo);
      }
      this.SendQuery();
    }

    public void Clear()
    {
      lock (this._queueLock)
        this._queue.Clear();
    }

    protected virtual void OnTilesReady(TilesReadyEventArgs<ITile> e)
    {
      EventHandler<TilesReadyEventArgs<ITile>> tilesReady = this.TilesReady;
      if (tilesReady == null)
        return;
      tilesReady((object) this, e);
    }

    protected virtual void OnRequestFailed(TileRequestFailedEventArgs e)
    {
      EventHandler<TileRequestFailedEventArgs> requestFailed = this.RequestFailed;
      if (requestFailed == null)
        return;
      requestFailed((object) this, e);
    }

    public event EventHandler<StateChangedEventArgs> StateChanged
    {
      add => this._fetcher.StateChanged += value;
      remove => this._fetcher.StateChanged -= value;
    }

    public ServiceState State => this._fetcher.State;

    public void CancelQueryingTilesExcept(IEnumerable<ITileInfo> tiles)
    {
      lock (this._queueLock)
      {
        foreach (ITileInfo tileInfo in this._queue.Except<ITileInfo>(tiles).ToArray<ITileInfo>())
          this._queue.Remove(tileInfo);
      }
    }
  }
}
