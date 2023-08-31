// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.GeoCoordinatesRect
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Yandex.Positioning
{
  [ComVisible(true)]
  public sealed class GeoCoordinatesRect
  {
    public GeoCoordinatesRect()
    {
    }

    public GeoCoordinatesRect(double north, double west, double south, double east) => this.Initialize(north, west, south, east);

    public GeoCoordinatesRect(GeoCoordinate topLeft, GeoCoordinate bottomRight) => this.Initialize(topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude);

    private void Initialize(double north, double west, double south, double east)
    {
      if (west > east)
        east += 360.0;
      this.North = north;
      this.West = west;
      this.South = south;
      this.East = east;
    }

    public GeoCoordinatesRect(IEnumerable<GeoCoordinate> geoCoordinates)
    {
      if (geoCoordinates == null)
        throw new ArgumentNullException(nameof (geoCoordinates));
      double num1 = -90.0;
      double num2 = 90.0;
      double num3 = 180.0;
      double num4 = -180.0;
      foreach (GeoCoordinate geoCoordinate in geoCoordinates)
      {
        num1 = Math.Max(num1, geoCoordinate.Latitude);
        num2 = Math.Min(num2, geoCoordinate.Latitude);
        num3 = Math.Min(num3, geoCoordinate.Longitude);
        num4 = Math.Max(num4, geoCoordinate.Longitude);
      }
      this.Initialize(num1, num3, num2, num4);
    }

    public double North { get; set; }

    public double South { get; set; }

    public double East { get; set; }

    public double West { get; set; }

    public GeoCoordinate TopLeft => new GeoCoordinate(this.North, this.West);

    public GeoCoordinate BottomRight => new GeoCoordinate(this.South, this.East);

    public bool Contains(GeoCoordinatesRect rect) => this.North >= rect.North && this.West <= rect.West && this.South <= rect.South && this.East >= rect.East;
  }
}
