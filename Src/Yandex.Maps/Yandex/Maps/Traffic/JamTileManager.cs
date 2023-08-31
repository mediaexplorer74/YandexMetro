// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamTileManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Traffic
{
  [UsedImplicitly]
  internal class JamTileManager : TileManagerBase<IJamTile>
  {
    private readonly IQueryQueueManager<IJamTile> _queryQueueManager;

    public JamTileManager(
      ITileRepository<IJamTile> tileRepository,
      IQueryQueueManager<IJamTile> queryQueueManager,
      IMappingTileQueue mappingTileQueue,
      IJamTileBuilder jamTileBuilder,
      [NotNull] ITileInfoNormalizer tileInfoNormalizer)
      : base(tileRepository, queryQueueManager, mappingTileQueue, (ITileFactory<IJamTile>) jamTileBuilder, tileInfoNormalizer)
    {
      this._queryQueueManager = queryQueueManager != null ? queryQueueManager : throw new ArgumentNullException(nameof (queryQueueManager));
      this.OperationMode = OperationMode.None;
    }

    internal override void OnTileRepositoryGetTilesComplete(TilesReadyEventArgs<IJamTile> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      DateTime now = DateTime.UtcNow;
      IEnumerable<ITileInfo> tileInfos = e.RequestedTiles.Where<ITileInfo>((Func<ITileInfo, bool>) (t => !e.Tiles.Any<IJamTile>((Func<IJamTile, bool>) (tile => tile.TileInfo.Equals((object) t))) || e.Tiles.Any<IJamTile>((Func<IJamTile, bool>) (tile => tile.TileInfo.Equals((object) t) && tile.Status == TileStatus.NeedsReload))));
      if (this._queryQueueManager.State == ServiceState.Ready)
      {
        IEnumerable<ITileInfo> second = e.Tiles.Where<IJamTile>((Func<IJamTile, bool>) (tile => tile.NextUpdateAt < now)).Select<IJamTile, ITileInfo>((Func<IJamTile, ITileInfo>) (tile => tile.TileInfo));
        this._queryQueueManager.QueryTileRange(tileInfos.Union<ITileInfo>(second));
      }
      else
        this.PopQueryTiles(tileInfos);
      IList<IJamTile> list = (IList<IJamTile>) e.Tiles.Where<IJamTile>((Func<IJamTile, bool>) (tile => tile.JamsExpireAt > now)).ToList<IJamTile>();
      this.OnTilesReady(new TilesReadyEventArgs<IJamTile>(e.RequestedTiles, list));
    }

    protected override void OnQueryQueueManagerTilesReady(TilesReadyEventArgs<IJamTile> e)
    {
      IList<Track> trackList = e != null ? e.Status as IList<Track> : throw new ArgumentNullException(nameof (e));
      foreach (IJamTile tile in (IEnumerable<IJamTile>) e.Tiles)
        tile.JamTracks = trackList;
      this.OnTilesReady(new TilesReadyEventArgs<IJamTile>(e.RequestedTiles, e.Tiles));
      base.OnQueryQueueManagerTilesReady(e);
    }
  }
}
