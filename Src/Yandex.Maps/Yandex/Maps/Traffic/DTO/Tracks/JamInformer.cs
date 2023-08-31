// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Tracks.JamInformer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Positioning;

namespace Yandex.Maps.Traffic.DTO.Tracks
{
  [XmlRoot("informer")]
  [ComVisible(true)]
  public class JamInformer
  {
    private const string lat = "lat";
    private const string lon = "lon";
    private const string value = "value";
    private const string color = "color";

    public JamInformer() => this.Cooridnates = new GeoCoordinate(0.0, 0.0);

    [XmlAttribute("lat")]
    public double Latitude
    {
      get => this.Cooridnates.Latitude;
      set => this.Cooridnates.Latitude = value;
    }

    [XmlAttribute("lon")]
    public double Longitude
    {
      get => this.Cooridnates.Longitude;
      set => this.Cooridnates.Longitude = value;
    }

    [XmlAttribute("value")]
    public ushort Value { get; set; }

    [XmlAttribute("color")]
    public JamColors JamColor { get; set; }

    [XmlIgnore]
    public GeoCoordinate Cooridnates { get; set; }

    public string Title { get; set; }
  }
}
