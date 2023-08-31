// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.TrafficRequestParameters
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Positioning;

namespace Yandex.Maps.Traffic
{
  internal class TrafficRequestParameters
  {
    public TrafficRequestParameters(
      string uuid,
      uint zoom,
      GeoCoordinate topLeft,
      GeoCoordinate bottomRight,
      IList<ITileInfo> tilesToLoad)
    {
      this.Uuid = uuid;
      this.TopLeft = topLeft;
      this.BottomRight = bottomRight;
      this.Zoom = zoom;
      this.TilesToLoad = tilesToLoad;
    }

    public string Uuid { get; private set; }

    public uint Zoom { get; set; }

    public GeoCoordinate TopLeft { get; private set; }

    public GeoCoordinate BottomRight { get; private set; }

    public IList<ITileInfo> TilesToLoad { get; private set; }
  }
}
