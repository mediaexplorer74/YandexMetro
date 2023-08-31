// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Dto.GeoObject
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Repository.Dto
{
  [ComVisible(true)]
  public class GeoObject
  {
    [XmlElement("metaDataProperty", Namespace = "http://www.opengis.net/gml")]
    public MetadataPropertyDto MetaDataProperty { get; set; }

    [XmlElement("name", Namespace = "http://www.opengis.net/gml")]
    public string Name { get; set; }

    [XmlElement("Point", Namespace = "http://www.opengis.net/gml")]
    public PointDto Point { get; set; }

    [XmlElement("style", Namespace = "http://maps.yandex.ru/ymaps/1.x")]
    public string Style { get; set; }
  }
}
