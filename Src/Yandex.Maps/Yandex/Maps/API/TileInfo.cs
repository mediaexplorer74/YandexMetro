// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.TileInfo
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class TileInfo : ITileInfo
  {
    public TileInfo(ITileInfo tileInfo)
      : this(tileInfo.X, tileInfo.Y, tileInfo.Zoom, tileInfo.Layer)
    {
    }

    public TileInfo(int x, int y, byte zoom, BaseLayers layer)
    {
      this.X = x;
      this.Y = y;
      this.Zoom = zoom;
      this.Layer = layer;
    }

    public override bool Equals(object obj) => obj is ITileInfo tileInfo && tileInfo.X == this.X && tileInfo.Y == this.Y && (int) tileInfo.Zoom == (int) this.Zoom && tileInfo.Layer == this.Layer;

    public bool EqualsCoordinates(ITileInfo obj) => obj != null && obj.X == this.X && obj.Y == this.Y && (int) obj.Zoom == (int) this.Zoom;

    public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Zoom.GetHashCode() ^ this.Layer.GetHashCode();

    public override string ToString() => string.Format("TileInfo X:{0} Y:{1} Zoom:{2} Layer:{3}", (object) this.X, (object) this.Y, (object) this.Zoom, (object) this.Layer);

    [XmlAttribute("x")]
    public int X { get; set; }

    [XmlAttribute("y")]
    public int Y { get; set; }

    [XmlAttribute("zoom")]
    public byte Zoom { get; set; }

    [XmlIgnore]
    public byte[] Checksum { get; set; }

    [XmlAttribute("ver")]
    public ushort MapVersion { get; set; }

    [XmlIgnore]
    public BaseLayers Layer { get; set; }

    [XmlIgnore]
    public short Index { get; set; }

    [XmlIgnore]
    public int IdSort { get; set; }

    public static bool operator ==(TileInfo a, TileInfo b) => (object) a != null && a.Equals((object) b);

    public static bool operator !=(TileInfo a, TileInfo b) => !(a == b);

    public static bool operator ==(TileInfo a, ITileInfo b) => (object) a != null && a.Equals((object) b);

    public static bool operator !=(TileInfo a, ITileInfo b) => !(a == b);
  }
}
