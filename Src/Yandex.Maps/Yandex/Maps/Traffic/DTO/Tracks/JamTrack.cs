// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Tracks.JamTrack
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using Yandex.Positioning;

namespace Yandex.Maps.Traffic.DTO.Tracks
{
  [XmlRoot("track")]
  [ComVisible(true)]
  public class JamTrack
  {
    private const string severity = "severity";
    private const string style_id = "style_id";
    private const string avg_speed = "avg_speed";
    private const string street_category = "street_category";

    public JamTrack() => this.Coordinates = new List<GeoCoordinate>();

    [XmlAttribute("severity")]
    public ushort Severity { get; set; }

    [XmlAttribute("style_id")]
    public int StyleId { get; set; }

    [XmlAttribute("avg_speed")]
    public double AvgSpeed { get; set; }

    [XmlAttribute("street_category")]
    public int StreetCategory { get; set; }

    [XmlIgnore]
    public List<GeoCoordinate> Coordinates { get; set; }

    [XmlText]
    public string CoordinatesText
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (GeoCoordinate coordinate in this.Coordinates)
          stringBuilder.Append(coordinate.Latitude).Append(" ").Append(coordinate.Longitude).Append(" ");
        return stringBuilder.ToString().TrimEnd();
      }
      set
      {
        string[] strArray = value.Split(new string[1]{ " " }, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < strArray.Length; index += 2)
          this.Coordinates.Add(this.CreateGeoCoordinates(double.Parse(strArray[index], (IFormatProvider) CultureInfo.InvariantCulture), double.Parse(strArray[index + 1], (IFormatProvider) CultureInfo.InvariantCulture)));
      }
    }

    private GeoCoordinate CreateGeoCoordinates(double latitude, double longitude) => new GeoCoordinate(latitude, longitude);
  }
}
