// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.Map
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Config
{
  [ComVisible(true)]
  public class Map
  {
    private const string cur_map_version = "cur_map_version";
    private const string min_map_version = "min_map_version";

    public Map() => this.MapChanges = new List<MapChange>();

    [XmlAttribute("min_map_version")]
    public string MinMapVersion { get; set; }

    [XmlAttribute("cur_map_version")]
    public string CurMapVersion { get; set; }

    [XmlElement("changes", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
    public List<MapChange> MapChanges { get; set; }
  }
}
