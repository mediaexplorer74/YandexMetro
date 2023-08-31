// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.GeoPosition
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Positioning
{
  internal class GeoPosition
  {
    public GeoPosition()
    {
      this.GeoCoordinate = new GeoCoordinate();
      this.TimeStamp = new DateTimeOffset();
    }

    public GeoPosition(
      GeoCoordinate geoCoordinates,
      DateTimeOffset timeStamp,
      double horizontalAccuracy,
      double verticalAccuracy,
      double altitude,
      double speed,
      double course)
    {
      this.GeoCoordinate = geoCoordinates != null ? geoCoordinates : throw new ArgumentNullException(nameof (geoCoordinates));
      this.TimeStamp = timeStamp;
      this.HorizontalAccuracy = horizontalAccuracy;
      this.VerticalAccuracy = verticalAccuracy;
      this.Altitude = altitude;
      this.Speed = speed;
      this.Course = course;
    }

    public GeoCoordinate GeoCoordinate { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public double HorizontalAccuracy { get; set; }

    public double VerticalAccuracy { get; set; }

    public double Altitude { get; set; }

    public double Speed { get; set; }

    public double Course { get; private set; }

    public override bool Equals(object obj) => obj != null && obj is GeoPosition geoPosition && this.DoublesAreEqual(geoPosition.Altitude, this.Altitude) && this.DoublesAreEqual(geoPosition.Course, this.Course) && (this.GeoCoordinate == null || this.GeoCoordinate.Equals((object) geoPosition.GeoCoordinate)) && this.DoublesAreEqual(geoPosition.HorizontalAccuracy, this.HorizontalAccuracy) && this.DoublesAreEqual(geoPosition.Speed, this.Speed);

    private bool DoublesAreEqual(double a, double b)
    {
      if (a == b)
        return true;
      return double.IsNaN(a) && double.IsNaN(b);
    }
  }
}
