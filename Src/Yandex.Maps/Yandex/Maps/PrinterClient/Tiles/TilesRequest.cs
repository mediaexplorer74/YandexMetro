// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Tiles.TilesRequest
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Tiles
{
  [XmlRoot("tiles")]
  [ComVisible(true)]
  public class TilesRequest
  {
    public TilesRequest() => this.Tiles = new List<TileRequest>();

    [XmlElement("tile")]
    public List<TileRequest> Tiles { get; set; }
  }
}
