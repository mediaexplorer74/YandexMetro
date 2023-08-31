// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.Dto.GeocodeObject
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Positioning;

namespace Yandex.Maps.Geocoding.Dto
{
  [ComVisible(true)]
  public class GeocodeObject
  {
    [XmlAttribute("kind")]
    public string Kind { get; set; }

    [XmlAttribute("ll")]
    public string Ll
    {
      get => this.Coordinates.ToString();
      set => this.Coordinates = new GeoCoordinate(value);
    }

    [XmlIgnore]
    public GeoCoordinate Coordinates { get; set; }

    [XmlAttribute("zoomid")]
    public int Zoomid { get; set; }

    [XmlAttribute("title")]
    public string Title { get; set; }

    [XmlAttribute("subtitle")]
    public string Subtitle { get; set; }
  }
}
