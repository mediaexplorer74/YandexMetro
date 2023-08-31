// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Dto.AnyMetdataFeaturesDto
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Repository.Dto
{
  [ComVisible(true)]
  public class AnyMetdataFeaturesDto
  {
    [XmlElement("routing")]
    public bool Routing { get; set; }

    [XmlElement("routing_public_transport")]
    public bool RoutingPublicTransport { get; set; }

    [XmlElement("gps_buses")]
    public bool GpsBuses { get; set; }

    [XmlElement("voice")]
    public bool Voice { get; set; }

    [XmlElement("streetview")]
    public bool Streetview { get; set; }

    [XmlElement("semaphore")]
    public bool Semaphore { get; set; }

    [XmlElement("routeguidance")]
    public bool Routeguidance { get; set; }

    [XmlElement("vmap")]
    public bool Vmap { get; set; }

    [XmlElement("widgets")]
    public bool Widgets { get; set; }
  }
}
