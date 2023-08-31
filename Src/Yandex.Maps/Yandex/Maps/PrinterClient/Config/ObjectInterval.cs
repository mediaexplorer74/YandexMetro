// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.ObjectInterval
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Config
{
  [XmlRoot("interval")]
  [ComVisible(true)]
  public class ObjectInterval
  {
    [XmlAttribute("object")]
    public ObjectTypes ObjectType { get; set; }

    [XmlAttribute("min_zoom")]
    public int MinZoom { get; set; }

    [XmlAttribute("max_zoom")]
    public int MaxZoom { get; set; }
  }
}
