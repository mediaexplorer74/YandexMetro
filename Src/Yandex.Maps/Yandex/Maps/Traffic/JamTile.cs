// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamTile
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Traffic.Interfaces;

namespace Yandex.Maps.Traffic
{
  internal class JamTile : Tile, IJamTile, ITile, Yandex.Common.ICloneable
  {
    public JamTile(
      ITileInfo tileInfo,
      DateTime nextUpdateAt,
      DateTime jamsExpireAt,
      IList<TrafficInformer> informers)
      : base(tileInfo, TileStatus.Ok)
    {
      this.NextUpdateAt = nextUpdateAt;
      this.JamsExpireAt = jamsExpireAt;
      this.Informers = informers;
    }

    public DateTime NextUpdateAt { get; private set; }

    public DateTime JamsExpireAt { get; private set; }

    public IList<TrafficInformer> Informers { get; private set; }

    public IList<Track> JamTracks { get; set; }

    public object Clone()
    {
      JamTile jamTile = new JamTile(this.TileInfo, this.NextUpdateAt, this.JamsExpireAt, this.Informers);
      jamTile.Time = this.Time;
      jamTile.HeaderLength = this.HeaderLength;
      jamTile.Records = this.Records;
      jamTile.Version = this.Version;
      jamTile.Metadata = this.Metadata;
      jamTile.BitmapSource = this.BitmapSource;
      jamTile.DataOffset = this.DataOffset;
      jamTile.DataLength = this.DataLength;
      jamTile.Bytes = this.Bytes;
      jamTile.JamTracks = this.JamTracks;
      return (object) jamTile;
    }
  }
}
