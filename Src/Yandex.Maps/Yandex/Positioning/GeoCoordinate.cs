// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.GeoCoordinate
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Yandex.Positioning
{
  [DataContract(Name = "GeoCoordinate")]
  [ComVisible(true)]
  public class GeoCoordinate
  {
    private const double Epsilon = 1E-10;

    public GeoCoordinate()
    {
    }

    public GeoCoordinate(double latitude, double longitude)
    {
      this.Latitude = latitude;
      this.Longitude = longitude;
    }

    public GeoCoordinate(string value, bool invert = false)
    {
      Match match = new Regex("([-+]?\\d+\\.\\d+)[\\,\\s]\\s*([-+]?\\d+\\.\\d+)").Match(value);
      if (match.Groups.Count < 3)
        throw new ArgumentException("Invalid coordinates");
      double num1 = double.Parse(match.Groups[1].Value, (IFormatProvider) CultureInfo.InvariantCulture);
      double num2 = double.Parse(match.Groups[2].Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (!invert)
      {
        this.Latitude = num1;
        this.Longitude = num2;
      }
      else
      {
        this.Latitude = num2;
        this.Longitude = num1;
      }
    }

    [DataMember]
    public double Latitude { get; set; }

    [DataMember]
    public double Longitude { get; set; }

    public override bool Equals(object obj) => obj != null && obj is GeoCoordinate geoCoordinate && Math.Abs(this.Longitude - geoCoordinate.Longitude) < 1E-10 && Math.Abs(this.Latitude - geoCoordinate.Latitude) < 1E-10;

    public override int GetHashCode() => this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode();

    public override string ToString() => this.Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + this.Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
