// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamTileBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Maps.Traffic.Interfaces;

namespace Yandex.Maps.Traffic
{
  [UsedImplicitly]
  internal class JamTileBuilder : IJamTileBuilder, ITileFactory<IJamTile>
  {
    private readonly IJamInformerManager _jamInformerManager;

    public JamTileBuilder(IJamInformerManager jamInformerManager) => this._jamInformerManager = jamInformerManager != null ? jamInformerManager : throw new ArgumentNullException(nameof (jamInformerManager));

    public IJamTile CreateTile(ITileInfo tileInfo, JamMeta meta)
    {
      if (tileInfo == null)
        throw new ArgumentNullException(nameof (tileInfo));
      DateTime utcNow = DateTime.UtcNow;
      DateTime nextUpdateAt = utcNow.AddSeconds((double) meta.NextUpdateIn);
      DateTime jamsExpireAt = utcNow.AddSeconds((double) meta.JamsExpireIn);
      IList<TrafficInformer> list = (IList<TrafficInformer>) meta.Informers.Select<JamInformer, TrafficInformer>((Func<JamInformer, TrafficInformer>) (i => this._jamInformerManager.GetInformer(i.Cooridnates))).ToList<TrafficInformer>();
      return (IJamTile) new JamTile(tileInfo, nextUpdateAt, jamsExpireAt, list);
    }

    public IJamTile CreateTile(ITileInfo tileInfo, TileStatus tileStatus) => throw new NotImplementedException();

    public IJamTile CreateTile(
      ITileInfo tileInfo,
      byte[] bytes,
      int dataOffset,
      int dataLength,
      TileStatus tileStatus)
    {
      throw new NotImplementedException();
    }

    public IJamTile CreateTile(
      int x,
      int y,
      byte zoom,
      BaseLayers layers,
      byte[] bytes,
      int dataOffset,
      int dataLength,
      TileStatus tileStatus)
    {
      throw new NotImplementedException();
    }

    public IJamTile CopyTile(IJamTile tile, ITileInfo newTileInfo)
    {
      JamTile jamTile = new JamTile(newTileInfo, tile.NextUpdateAt, tile.JamsExpireAt, tile.Informers);
      jamTile.BitmapSource = tile.BitmapSource;
      jamTile.Bytes = tile.Bytes;
      jamTile.DataLength = tile.DataLength;
      jamTile.DataOffset = tile.DataOffset;
      jamTile.HeaderLength = tile.HeaderLength;
      jamTile.Metadata = tile.Metadata;
      return (IJamTile) jamTile;
    }
  }
}
