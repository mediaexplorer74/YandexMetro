// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.RelativeRect
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Positioning
{
  [ComVisible(true)]
  public sealed class RelativeRect
  {
    public RelativeRect()
    {
    }

    public RelativeRect(double north, double west, double south, double east) => this.Initialize(north, west, south, east);

    private void Initialize(double north, double west, double south, double east)
    {
      if (west > east)
        ++east;
      this.North = north;
      this.West = west;
      this.South = south;
      this.East = east;
    }

    public static bool TryCreate(IEnumerable<Point> geoCoordinates, out RelativeRect relativeRect)
    {
      if (geoCoordinates == null)
        throw new ArgumentNullException(nameof (geoCoordinates));
      double num1 = 1.0;
      double num2 = -1.0;
      double num3 = 1.0;
      double num4 = -1.0;
      int num5 = 0;
      foreach (Point geoCoordinate in geoCoordinates)
      {
        num1 = Math.Min(num1, geoCoordinate.Y);
        num2 = Math.Max(num2, geoCoordinate.Y);
        num3 = Math.Min(num3, geoCoordinate.X);
        num4 = Math.Max(num4, geoCoordinate.X);
        ++num5;
      }
      if (num5 > 1)
      {
        relativeRect = new RelativeRect(num1, num3, num2, num4);
        return true;
      }
      relativeRect = (RelativeRect) null;
      return false;
    }

    public double North { get; set; }

    public double South { get; set; }

    public double East { get; set; }

    public double West { get; set; }
  }
}
