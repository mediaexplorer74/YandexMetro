// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TileManagerBase`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps
{
  internal class TileManagerBase<T> : ITileManager<T>, IFlusheable where T : ITile
  {
    private const int MaxReloadAttempts = 3;
    private readonly ITileRepository<T> _tileRepository;
    private readonly IQueryQueueManager<T> _queryQueueManager;
    private readonly IMappingTileQueue _mappingTileQueue;
    private readonly ITileFactory<T> _tileFactory;
    private readonly ITileInfoNormalizer _tileInfoNormalizer;
    private readonly object _reloadTilesSync = new object();
    private OperationMode _operationMode;

    protected TileManagerBase(
      ITileRepository<T> tileRepository,
      IQueryQueueManager<T> queryQueueManager,
      IMappingTileQueue mappingTileQueue,
      ITileFactory<T> tileFactory,
      [NotNull] ITileInfoNormalizer tileInfoNormalizer)
    {
      if (tileRepository == null)
        throw new ArgumentNullException(nameof (tileRepository));
      if (queryQueueManager == null)
        throw new ArgumentNullException(nameof (queryQueueManager));
      if (mappingTileQueue == null)
        throw new ArgumentNullException(nameof (mappingTileQueue));
      if (tileFactory == null)
        throw new ArgumentNullException(nameof (tileFactory));
      if (tileInfoNormalizer == null)
        throw new ArgumentNullException(nameof (tileInfoNormalizer));
      this._tileRepository = tileRepository;
      this._queryQueueManager = queryQueueManager;
      this._mappingTileQueue = mappingTileQueue;
      this._tileFactory = tileFactory;
      this._tileInfoNormalizer = tileInfoNormalizer;
      this._tileRepository.GetTilesComplete += new EventHandler<TilesReadyEventArgs<T>>(this.TileRepositoryGetTilesComplete);
      this._queryQueueManager.TilesReady += new EventHandler<TilesReadyEventArgs<T>>(this.QueryQueueManagerTilesReady);
      this._queryQueueManager.RequestFailed += new EventHandler<TileRequestFailedEventArgs>(this.QueryQueueManagerRequestFailed);
      this.OperationMode = OperationMode.None;
    }

    public event EventHandler<TilesReadyEventArgs<T>> TilesReady;

    public void QueryTiles(IEnumerable<ITileInfo> infiniteTiles)
    {
      if (infiniteTiles == null)
        throw new ArgumentNullException(nameof (infiniteTiles));
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      lock (this._reloadTilesSync)
      {
        foreach (ITileInfo infiniteTile in infiniteTiles)
        {
          TileQueueEntry entry;
          if (this._mappingTileQueue.TryEnqueue(infiniteTile, out entry))
          {
            entry.State = TileQueueState.AskingRepository;
            tileInfoList.Add(entry.Key);
          }
        }
      }
      this.AskTilesFromRepository((IEnumerable<ITileInfo>) tileInfoList);
    }

    public void RemoveTiles(Func<T, bool> removeCondition) => this._tileRepository.Remove(removeCondition);

    public IAsyncResult ClearPersistentCache(AsyncCallback callback, object state) => this._tileRepository.ClearPersistentCache(callback, state);

    public OperationMode OperationMode
    {
      get => this._operationMode;
      set
      {
        if (this._operationMode == value)
          return;
        this._operationMode = value;
        this.OnOperationModeChanged(value);
      }
    }

    public void Flush()
    {
      lock (this._reloadTilesSync)
        this._mappingTileQueue.Clear();
      this._tileRepository.Flush();
    }

    public void ResetCachedTileBitmaps() => this._tileRepository.ResetCachedTileBitmaps();

    private void AskTilesFromRepository([NotNull] IEnumerable<ITileInfo> tileInfos)
    {
      ITileInfo[] array = tileInfos.Distinct<ITileInfo>().ToArray<ITileInfo>();
      if (((IEnumerable<ITileInfo>) array).Any<ITileInfo>())
        this._tileRepository.BeginGetTiles((IList<ITileInfo>) array);
      else
        this.Worker();
    }

    protected void UpdateTileStateAndVersion(
      IEnumerable<ITileInfo> tiles,
      TileQueueState source,
      TileQueueState destination)
    {
      lock (this._reloadTilesSync)
      {
        foreach (ITileInfo tile in tiles)
        {
          TileQueueEntry entry;
          if (this._mappingTileQueue.TryGetEntry(tile, out entry) && entry.State == source)
          {
            entry.State = destination;
            entry.Key.MapVersion = tile.MapVersion;
            entry.Key.Checksum = tile.Checksum;
          }
        }
      }
    }

    protected void Worker()
    {
      if (this.OperationMode < OperationMode.Full)
        return;
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      lock (this._reloadTilesSync)
      {
        TileQueueEntry[] array = this._mappingTileQueue.Values.Where<TileQueueEntry>((Func<TileQueueEntry, bool>) (entry => entry.State == TileQueueState.NeedToAskServer)).ToArray<TileQueueEntry>();
        if (this._queryQueueManager.State == ServiceState.Ready)
        {
          foreach (TileQueueEntry tileQueueEntry in array)
          {
            if (++tileQueueEntry.ReloadAttempts <= 3U)
            {
              tileQueueEntry.State = TileQueueState.AskingServer;
              tileInfoList.Add(tileQueueEntry.Key);
            }
            else
              this.OnTilesReady(new TilesReadyEventArgs<T>(tileQueueEntry.MappedTileInfos, (IList<T>) tileQueueEntry.MappedTileInfos.Select<ITileInfo, T>((Func<ITileInfo, T>) (tileInfo => (T) new Tile(tileInfo, TileStatus.CannotLoad))).ToList<T>()));
          }
        }
        else
        {
          foreach (QueueEntry<TileQueueState, ITileInfo> queueEntry in array)
            this.DequeueTile(queueEntry.Key);
        }
      }
      if (!tileInfoList.Any<ITileInfo>())
        return;
      this._queryQueueManager.QueryTileRange((IEnumerable<ITileInfo>) tileInfoList);
    }

    protected virtual void DequeueTile(ITileInfo tileInfo)
    {
      lock (this._reloadTilesSync)
        this._mappingTileQueue.Dequeue(tileInfo);
    }

    protected virtual void OnOperationModeChanged(OperationMode value) => this.Worker();

    protected virtual void OnTilesReady(TilesReadyEventArgs<T> e)
    {
      EventHandler<TilesReadyEventArgs<T>> tilesReady = this.TilesReady;
      if (tilesReady == null)
        return;
      List<T> tiles = new List<T>();
      IList<ITileInfo> requestedTiles = this.PeekQueryTiles((IEnumerable<ITileInfo>) e.RequestedTiles);
      lock (this._reloadTilesSync)
      {
        foreach (T tile1 in (IEnumerable<T>) e.Tiles)
        {
          T tile = tile1;
          TileQueueEntry entry = (TileQueueEntry) null;
          if (tile.Status == TileStatus.NeedsReload)
            this._mappingTileQueue.TryGetEntry(tile.TileInfo, out entry);
          else if (this._mappingTileQueue.Contains(tile.TileInfo))
            entry = this._mappingTileQueue.Dequeue(tile.TileInfo);
          if (entry != null)
            tiles.AddRange(entry.MappedTileInfos.Select<ITileInfo, T>((Func<ITileInfo, T>) (infinite => !infinite.Equals((object) tile.TileInfo) ? this._tileFactory.CopyTile(tile, infinite) : tile)));
        }
      }
      tilesReady((object) this, new TilesReadyEventArgs<T>(requestedTiles, (IList<T>) tiles));
    }

    [NotNull]
    protected IList<ITileInfo> PopQueryTiles([NotNull] IEnumerable<ITileInfo> tiles)
    {
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      lock (this._reloadTilesSync)
      {
        foreach (ITileInfo tile in tiles)
        {
          if (this._mappingTileQueue.Contains(tile))
          {
            TileQueueEntry tileQueueEntry = this._mappingTileQueue.Dequeue(tile);
            if (tileQueueEntry != null)
              tileInfoList.AddRange((IEnumerable<ITileInfo>) tileQueueEntry.MappedTileInfos);
          }
        }
      }
      return (IList<ITileInfo>) tileInfoList;
    }

    [NotNull]
    private IList<ITileInfo> PeekQueryTiles(IEnumerable<ITileInfo> tiles)
    {
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      lock (this._reloadTilesSync)
      {
        foreach (ITileInfo tile in tiles)
        {
          TileQueueEntry entry;
          if (this._mappingTileQueue.TryGetEntry(tile, out entry))
            tileInfoList.AddRange((IEnumerable<ITileInfo>) entry.MappedTileInfos);
        }
      }
      return (IList<ITileInfo>) tileInfoList;
    }

    private void TileRepositoryGetTilesComplete(object sender, TilesReadyEventArgs<T> e) => this.OnTileRepositoryGetTilesComplete(e);

    private void QueryQueueManagerTilesReady(object sender, TilesReadyEventArgs<T> e) => this.OnQueryQueueManagerTilesReady(e);

    private void QueryQueueManagerRequestFailed(object sender, TileRequestFailedEventArgs e) => this.OnQueryQueueManagerRequestFailed(e);

    internal virtual void OnTileRepositoryGetTilesComplete(TilesReadyEventArgs<T> e) => throw new NotImplementedException();

    protected virtual void OnQueryQueueManagerRequestFailed(TileRequestFailedEventArgs e)
    {
      this.UpdateTileStateAndVersion((IEnumerable<ITileInfo>) e.RequestedTiles, TileQueueState.AskingServer, TileQueueState.NeedToAskServer);
      this.Worker();
    }

    protected virtual void OnQueryQueueManagerTilesReady(TilesReadyEventArgs<T> e) => this._tileRepository.BeginAddTiles(e.Tiles);

    public void CancelQueryingTilesExcept(IList<ITileInfo> validInfiniteTiles)
    {
      ITileInfo[] normalizedTiles = validInfiniteTiles.Select<ITileInfo, ITileInfo>((Func<ITileInfo, ITileInfo>) (tileInfo => this._tileInfoNormalizer.ConvertToFiniteCoordinates(tileInfo))).ToArray<ITileInfo>();
      lock (this._reloadTilesSync)
      {
        foreach (ITileInfo tileInfo in this._mappingTileQueue.Keys.Where<ITileInfo>((Func<ITileInfo, bool>) (tile => !((IEnumerable<ITileInfo>) normalizedTiles).Any<ITileInfo>(new Func<ITileInfo, bool>(tile.EqualsCoordinates)))).ToArray<ITileInfo>())
          this.DequeueTile(tileInfo);
      }
      this._queryQueueManager.CancelQueryingTilesExcept((IEnumerable<ITileInfo>) normalizedTiles);
    }
  }
}
