// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TileManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Common;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.Repository;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps
{
  internal class TileManager : TileManagerBase<ITile>
  {
    private readonly IConfigMediator _configMediator;
    private readonly ITileMetadataReader _metadataReader;
    private readonly ICacheTileScaleAdapter _cacheTileScaleAdapter;
    private readonly ITileRepository<ITile> _tileRepository;
    private readonly ITileSerializer _tileReader;
    private readonly IDictionary<ITileInfo, ITile> _updateTileDict;
    private readonly object _updateTileDictLock = new object();

    public TileManager(
      ITileRepository<ITile> tileRepository,
      IQueryQueueManager<ITile> queryQueueManager,
      IMappingTileQueue mappingTileQueue,
      ITileSerializer tileReader,
      ITileFactory<ITile> tileFactory,
      IConfigMediator configMediator,
      ITileMetadataReader metadataReader,
      [NotNull] ICacheTileScaleAdapter cacheTileScaleAdapter,
      [NotNull] ITileInfoNormalizer tileInfoNormalizer)
      : base(tileRepository, queryQueueManager, mappingTileQueue, tileFactory, tileInfoNormalizer)
    {
      if (tileRepository == null)
        throw new ArgumentNullException(nameof (tileRepository));
      if (tileReader == null)
        throw new ArgumentNullException(nameof (tileReader));
      if (tileFactory == null)
        throw new ArgumentNullException(nameof (tileFactory));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (metadataReader == null)
        throw new ArgumentNullException(nameof (metadataReader));
      if (cacheTileScaleAdapter == null)
        throw new ArgumentNullException(nameof (cacheTileScaleAdapter));
      this._tileRepository = tileRepository;
      this._tileReader = tileReader;
      this._configMediator = configMediator;
      this._metadataReader = metadataReader;
      this._cacheTileScaleAdapter = cacheTileScaleAdapter;
      this._updateTileDict = (IDictionary<ITileInfo, ITile>) new Dictionary<ITileInfo, ITile>();
    }

    private bool DeserializeTile(ITile tile)
    {
      if (tile.Bytes == null || tile.DataLength <= 0)
        return false;
      if (tile.BitmapSource != null)
        return true;
      try
      {
        if (this._tileReader.DeserializeTileDetails(tile, tile.Bytes, tile.DataOffset, tile.DataLength))
        {
          if (tile.Records != null)
          {
            if (tile.Records.Length > 0)
            {
              TileRecord record = tile.Records[0];
              if (tile.TileInfo.Layer == BaseLayers.meta)
              {
                tile.Metadata = this._metadataReader.ReadTile(tile.Bytes, (int) ((long) (tile.DataOffset + tile.HeaderLength) + (long) record.DataOffset), (int) record.DataLength);
                tile.BitmapSource = (object) null;
              }
              else
                tile.Metadata = (TileMetadata) null;
              return true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
      return false;
    }

    internal override void OnTileRepositoryGetTilesComplete(TilesReadyEventArgs<ITile> e)
    {
      IDictionary<BaseLayers, Yandex.Maps.Config.MapLayer> mapLayers = this._configMediator.MapLayers;
      short scale = this.GetScale();
      List<ITileInfo> first = new List<ITileInfo>();
      List<ITile> readyTiles = e.Tiles.Where<ITile>((Func<ITile, bool>) (tile => e.RequestedTiles.Contains(tile.TileInfo))).ToList<ITile>();
      foreach (ITile tile in readyTiles.ToArray())
      {
        if (!this.DeserializeTile(tile))
        {
          readyTiles.Remove(tile);
        }
        else
        {
          bool flag = false;
          ITileInfo tileInfo = tile.TileInfo;
          Yandex.Maps.Config.MapLayer mapLayer;
          if (mapLayers.TryGetValue(tileInfo.Layer, out mapLayer))
          {
            if ((int) tileInfo.MapVersion < (int) mapLayer.MapVersion || (int) tile.Scale != (int) scale)
            {
              tileInfo.MapVersion = mapLayer.MapVersion;
              tile.Scale = scale;
              flag = true;
            }
          }
          else
            flag = true;
          if (flag)
          {
            lock (this._updateTileDictLock)
            {
              if (!this._updateTileDict.ContainsKey(tileInfo))
              {
                tile.Status = TileStatus.NeedsReload;
                first.Add(tileInfo);
                this._updateTileDict.Add(tileInfo, tile);
              }
            }
          }
        }
      }
      this.OnTilesReady(new TilesReadyEventArgs<ITile>(e.RequestedTiles, (IList<ITile>) readyTiles));
      this.UpdateTileStateAndVersion(first.Union<ITileInfo>(e.RequestedTiles.Where<ITileInfo>((Func<ITileInfo, bool>) (t => !readyTiles.Any<ITile>((Func<ITile, bool>) (tile => tile.TileInfo.Equals((object) t)))))).Union<ITileInfo>(readyTiles.Where<ITile>((Func<ITile, bool>) (t => (t.Status & TileStatus.NeedsReload) == TileStatus.NeedsReload)).Select<ITile, ITileInfo>((Func<ITile, ITileInfo>) (t => t.TileInfo))), TileQueueState.AskingRepository, TileQueueState.NeedToAskServer);
      this.Worker();
    }

    private short GetScale() => this._cacheTileScaleAdapter.GetCacheScale(this._configMediator.TilesStretchFactor);

    protected override void OnQueryQueueManagerTilesReady(TilesReadyEventArgs<ITile> e)
    {
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      List<ITile> tileList1 = new List<ITile>();
      List<ITile> tileList2 = new List<ITile>();
      short scale = this.GetScale();
      foreach (ITile tile1 in (IEnumerable<ITile>) e.Tiles)
      {
        ITileInfo tileInfo = tile1.TileInfo;
        switch (tile1.Status)
        {
          case TileStatus.Ok:
            this.RemoveFromUpdateTileDict(tileInfo);
            if (this.DeserializeTile(tile1))
            {
              tileList1.Add(tile1);
              tileList2.Add(tile1);
              continue;
            }
            this.DequeueTile(tile1.TileInfo);
            continue;
          case TileStatus.Error:
            throw new ArgumentOutOfRangeException();
          case TileStatus.NotModified:
            ITile tile2;
            lock (this._updateTileDictLock)
            {
              if (this._updateTileDict.TryGetValue(tileInfo, out tile2))
                this._updateTileDict.Remove(tileInfo);
            }
            if (tile2 != null)
            {
              tile2.Scale = scale;
              tile2.Status = TileStatus.Ok;
              tileList2.Add(tile2);
              this._tileReader.SerializeTileDetails(tile2, tile2.Bytes, tile2.DataOffset, tile2.DataLength);
              continue;
            }
            continue;
          case TileStatus.NotFound:
            this.RemoveFromUpdateTileDict(tileInfo);
            tileList1.Add(tile1);
            continue;
          default:
            if ((tile1.Status & TileStatus.NeedsReload) != TileStatus.NeedsReload)
              throw new ArgumentOutOfRangeException();
            tileInfoList.Add(tileInfo);
            continue;
        }
      }
      if (tileList1.Any<ITile>())
        this.OnTilesReady(new TilesReadyEventArgs<ITile>(e.RequestedTiles, (IList<ITile>) tileList1));
      if (tileList2.Any<ITile>())
        this._tileRepository.BeginAddTiles((IList<ITile>) tileList2);
      if (!tileInfoList.Any<ITileInfo>())
        return;
      this.UpdateTileStateAndVersion((IEnumerable<ITileInfo>) tileInfoList, TileQueueState.AskingServer, TileQueueState.NeedToAskServer);
    }

    protected override void DequeueTile(ITileInfo tileInfo)
    {
      this.RemoveFromUpdateTileDict(tileInfo);
      base.DequeueTile(tileInfo);
    }

    private void RemoveFromUpdateTileDict(ITileInfo tileInfo)
    {
      lock (this._updateTileDictLock)
      {
        if (!this._updateTileDict.ContainsKey(tileInfo))
          return;
        this._updateTileDict.Remove(tileInfo);
      }
    }
  }
}
