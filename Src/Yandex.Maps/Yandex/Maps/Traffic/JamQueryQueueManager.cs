// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamQueryQueueManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Yandex.Collections.Interfaces;
using Yandex.ItemsCounter;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic.DataAdapters;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Traffic
{
  [ComVisible(false)]
  public class JamQueryQueueManager : IQueryQueueManager<IJamTile>, IStateService
  {
    private readonly IEnumerableQueue<ITileInfo> _queue;
    private readonly ITileFetcher<IJamTile> _jamTilesFetcher;
    private readonly IItemCounter _itemCounter;
    private readonly IJamTileBuilder _jamTileFactory;
    private readonly ITileRectangleBreaker _tileRectangleBreaker;
    private readonly IJamInformerManager _jamInformerManager;
    private readonly ITrackAdapter _trackAdapter;
    private readonly object _queueLock;

    public event EventHandler<TilesReadyEventArgs<IJamTile>> TilesReady;

    public event EventHandler<TileRequestFailedEventArgs> RequestFailed;

    public JamQueryQueueManager(
      IEnumerableQueue<ITileInfo> queue,
      ITileFetcher<IJamTile> jamTilesFetcher,
      IItemCounter itemCounter,
      IJamTileBuilder jamTileFactory,
      IMappingTileQueue mappingTileQueue,
      ITileRectangleBreaker tileRectangleBreaker,
      IJamInformerManager jamInformerManager,
      [NotNull] ITrackAdapter trackAdapter)
    {
      if (queue == null)
        throw new ArgumentNullException(nameof (queue));
      if (jamTilesFetcher == null)
        throw new ArgumentNullException(nameof (jamTilesFetcher));
      if (itemCounter == null)
        throw new ArgumentNullException(nameof (itemCounter));
      if (jamTileFactory == null)
        throw new ArgumentNullException(nameof (jamTileFactory));
      if (tileRectangleBreaker == null)
        throw new ArgumentNullException(nameof (tileRectangleBreaker));
      if (jamInformerManager == null)
        throw new ArgumentNullException(nameof (jamInformerManager));
      if (trackAdapter == null)
        throw new ArgumentNullException(nameof (trackAdapter));
      this._queue = queue;
      this._jamTilesFetcher = jamTilesFetcher;
      this._itemCounter = itemCounter;
      this._jamTileFactory = jamTileFactory;
      this._tileRectangleBreaker = tileRectangleBreaker;
      jamTilesFetcher.TilesReady += new EventHandler<TilesReadyEventArgs<IJamTile>>(this.JamTilesFetcherJamTracksReady);
      jamTilesFetcher.RequestFailed += new EventHandler<TileRequestFailedEventArgs>(this.JamTilesFetcherJamTilesFailed);
      this._jamInformerManager = jamInformerManager;
      this._trackAdapter = trackAdapter;
      this._queueLock = new object();
    }

    private void JamTilesFetcherJamTilesFailed(object sender, TileRequestFailedEventArgs e) => this.OnRequestFailed(e);

    private void JamTilesFetcherJamTracksReady(object sender, TilesReadyEventArgs<IJamTile> e)
    {
      JamTracks jamTracks = e.Status as JamTracks;
      if (jamTracks == null)
        throw new ArgumentException("e.Status is not JamTracks.");
      this._jamInformerManager.UpdateInformers(jamTracks.Meta);
      ITileInfo tileInfo1 = e.RequestedTiles.FirstOrDefault<ITileInfo>();
      if (tileInfo1 == null)
        return;
      byte zoom = tileInfo1.Zoom;
      IList<Track> status = this._trackAdapter.ReadTracks((IList<JamTrack>) jamTracks.Tracks, zoom);
      IList<ITileInfo> requestedTiles = e.RequestedTiles;
      List<IJamTile> list = requestedTiles.Select<ITileInfo, IJamTile>((Func<ITileInfo, IJamTile>) (tileInfo => this._jamTileFactory.CreateTile(tileInfo, jamTracks.Meta))).ToList<IJamTile>();
      this.OnTilesReady(new TilesReadyEventArgs<IJamTile>(requestedTiles, (IList<IJamTile>) list, (object) status));
      this.SendQuery();
    }

    private void SendQuery()
    {
      if (this._queue.Count == 0)
        return;
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      while (this._itemCounter.Count < (long) this.MaxRequestsCount && this._queue.Count > 0)
      {
        lock (this._queueLock)
        {
          while (this._queue.Count > 0)
            tileInfoList.Add(this._queue.Dequeue());
        }
        if (tileInfoList.Any<ITileInfo>())
        {
          foreach (IEnumerable<ITileInfo> rectangle in this._tileRectangleBreaker.GetRectangles((IEnumerable<ITileInfo>) tileInfoList))
          {
            if (this._itemCounter.Count < (long) this.MaxRequestsCount)
            {
              this._jamTilesFetcher.QueryTilesAsync(rectangle);
            }
            else
            {
              lock (this._queueLock)
              {
                foreach (ITileInfo tileInfo in rectangle.Except<ITileInfo>((IEnumerable<ITileInfo>) this._queue).ToArray<ITileInfo>())
                  this._queue.Enqueue(tileInfo);
              }
            }
          }
        }
      }
    }

    public uint MaxRequestsCount { get; set; }

    public void QueryTileRange(IEnumerable<ITileInfo> ti)
    {
      lock (this._queueLock)
      {
        foreach (ITileInfo tileInfo in ti.Except<ITileInfo>((IEnumerable<ITileInfo>) this._queue).ToArray<ITileInfo>())
          this._queue.Enqueue(tileInfo);
      }
      this.SendQuery();
    }

    public void Clear()
    {
      lock (this._queueLock)
        this._queue.Clear();
    }

    protected virtual void OnTilesReady(TilesReadyEventArgs<IJamTile> e)
    {
      EventHandler<TilesReadyEventArgs<IJamTile>> tilesReady = this.TilesReady;
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
      add => this._jamTilesFetcher.StateChanged += value;
      remove => this._jamTilesFetcher.StateChanged -= value;
    }

    public ServiceState State => this._jamTilesFetcher.State;

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
