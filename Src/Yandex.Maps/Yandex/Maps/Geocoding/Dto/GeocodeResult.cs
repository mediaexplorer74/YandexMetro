// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.Dto.GeocodeResult
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Positioning;

namespace Yandex.Maps.Geocoding.Dto
{
  [XmlRoot("result")]
  [ComVisible(true)]
  public class GeocodeResult
  {
    [XmlArrayItem(typeof (GeocodeObject), ElementName = "object")]
    [XmlArray("addresses")]
    public GeocodeObject[] Addresses { get; set; }

    [XmlIgnore]
    public GeoCoordinate Coordinates { get; set; }
  }
}
