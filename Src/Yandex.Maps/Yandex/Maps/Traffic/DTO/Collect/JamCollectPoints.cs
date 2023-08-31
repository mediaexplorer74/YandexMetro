// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Collect.JamCollectPoints
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Traffic.DTO.Collect
{
  [XmlRoot("traffic_collect")]
  [ComVisible(false)]
  public class JamCollectPoints
  {
    public JamCollectPoints() => this.Points = new List<JamCollectPoint>();

    public JamCollectPoints(List<JamCollectPoint> points) => this.Points = points;

    [XmlElement("point")]
    public List<JamCollectPoint> Points { get; private set; }
  }
}
