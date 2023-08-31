// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Collect.JamCollectPoint
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Traffic.DTO.Collect
{
  [ComVisible(false)]
  public class JamCollectPoint
  {
    private const string lat = "lat";
    private const string lon = "lon";
    private const string avg_speed = "avg_speed";
    private const string direction = "direction";
    private const string time = "time";
    private const string floatFormat = "0.000000";
    private const string dateTimeFormat = "ddMMyyyy:HHmmss";

    public JamCollectPoint()
    {
    }

    public JamCollectPoint(JamCollectPointData point)
    {
      this.FormattedTime = point.Time.ToUniversalTime().ToString("ddMMyyyy:HHmmss", (IFormatProvider) CultureInfo.InvariantCulture);
      this.FormattedLat = point.Lat.ToString("0.000000", (IFormatProvider) CultureInfo.InvariantCulture);
      this.FormattedLon = point.Lon.ToString("0.000000", (IFormatProvider) CultureInfo.InvariantCulture);
      this.AvgSpeed = (int) Math.Round(point.AvgSpeed * 3.6);
      this.Dirrection = (int) Math.Round(point.Direction);
    }

    [XmlAttribute("time")]
    public string FormattedTime { get; set; }

    [XmlAttribute("lat")]
    public string FormattedLat { get; set; }

    [XmlAttribute("lon")]
    public string FormattedLon { get; set; }

    [XmlAttribute("avg_speed")]
    public int AvgSpeed { get; set; }

    [XmlAttribute("direction")]
    public int Dirrection { get; set; }
  }
}
