// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.MapLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Config
{
  [ComVisible(true)]
  public class MapLayer
  {
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("request")]
    public string Request { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("ver")]
    public int Version { get; set; }

    [XmlAttribute("service")]
    public int Service { get; set; }

    [XmlAttribute("size_in_pixels")]
    public int SizeInPixels { get; set; }

    [XmlArray]
    public LayerStream[] Streams { get; set; }
  }
}
