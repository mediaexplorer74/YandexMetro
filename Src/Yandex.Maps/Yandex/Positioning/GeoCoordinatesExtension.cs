// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.GeoCoordinatesExtension
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Yandex.Positioning
{
  [ComVisible(true)]
  public static class GeoCoordinatesExtension
  {
    private const double GPS_PI = 3.141592653589;
    private const double GPS_PI_DIV_180 = 0.017453292519938889;
    private const double e2 = 0.006705621329494961;
    private const double R = 6378137.0;

    public static double GetDistanceTo(this GeoCoordinate startCoord, GeoCoordinate endCoord)
    {
      if (startCoord.Latitude == endCoord.Latitude && startCoord.Longitude == endCoord.Longitude)
        return 0.0;
      double rad1 = GeoCoordinatesExtension.GPS_Math_Deg_To_Rad(startCoord.Longitude - endCoord.Longitude);
      double rad2 = GeoCoordinatesExtension.GPS_Math_Deg_To_Rad(startCoord.Latitude - endCoord.Latitude);
      double num1 = Math.Sin(GeoCoordinatesExtension.GPS_Math_Deg_To_Rad((startCoord.Latitude + endCoord.Latitude) * 0.5));
      double num2 = num1 * num1;
      double num3 = GeoCoordinatesExtension.SafeDiv(6335367.6284903595, Math.Pow(1.0 - 0.006705621329494961 * num2, 1.5));
      double num4 = GeoCoordinatesExtension.SafeDiv(6378137.0, Math.Sqrt(1.0 - 0.006705621329494961 * num2));
      double num5 = Math.Sin(rad2 * 0.5);
      double num6 = Math.Sin(rad1 * 0.5);
      double d = Math.Sqrt(num5 * num5 + Math.Cos(endCoord.Latitude * (Math.PI / 180.0)) * Math.Cos(startCoord.Latitude * (Math.PI / 180.0)) * num6 * num6);
      if (d < -1.0)
        d = -1.0;
      if (d > 1.0)
        d = 1.0;
      double a = 2.0 * Math.Asin(d);
      double num7 = Math.Cos(endCoord.Latitude * (Math.PI / 180.0)) * Math.Sin(rad1) / Math.Sin(a);
      if (num7 < -1.0)
        num7 = -1.0;
      if (num7 > 1.0)
        num7 = 1.0;
      double num8 = num7 * num7;
      double num9 = GeoCoordinatesExtension.SafeDiv(num3 * num4, num3 * num8 + num4 * (1.0 - num8));
      return a * num9;
    }

    private static double GPS_Math_Deg_To_Rad(double v) => v * (Math.PI / 180.0);

    private static double SafeDiv(double a, double b)
    {
      if (b != 0.0)
        return a / b;
      return a == 0.0 ? 1.0 : 0.0;
    }

    public static string ToHumanReadableString(this GeoCoordinate value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      return Math.Round(value.Latitude, 5).ToString((IFormatProvider) CultureInfo.CurrentUICulture) + "; " + Math.Round(value.Longitude, 5).ToString((IFormatProvider) CultureInfo.CurrentUICulture);
    }
  }
}
