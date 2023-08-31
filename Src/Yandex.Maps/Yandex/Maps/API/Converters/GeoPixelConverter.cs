// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Converters.GeoPixelConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Units;
using Yandex.Maps.Units.Interfaces;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.API.Converters
{
  [ComVisible(true)]
  public class GeoPixelConverter : IGeoPixelConverter
  {
    private const double MaxSizeInPixels = 2147483648.0;
    private const double R = 6378137.0;
    private const double earthEquatorLength2 = 20037508.342789244;
    private const double DefaultFactor = 53.5865939582453;
    private const double AntiDefaultFactor = 0.018661383867375494;
    private const double ab = 0.0033565514688796939;
    private const double bb = 6.57187271079536E-06;
    private const double cb = 1.764564338702E-08;
    private const double db = 5.328478445E-11;
    private const double GPS_PI = 3.141592653589;
    private const double GPS_PI_2 = 1.5707963267945;
    private const double GPS_PI_DIV_180 = 0.017453292519938889;
    private const double e2 = 0.006705621329494961;
    private readonly IZoomInfo _zoomInfo;
    private readonly double[] _zoomWidthInPixels;
    private readonly double[] _zoomHeightInPixels;

    public GeoPixelConverter([NotNull] IZoomInfo zoomInfo, [NotNull] IConfigMediator configMediator)
    {
      if (zoomInfo == null)
        throw new ArgumentNullException(nameof (zoomInfo));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      this._zoomInfo = zoomInfo;
      this._zoomWidthInPixels = new double[(int) this._zoomInfo.MaxZoom + 1];
      this._zoomHeightInPixels = new double[(int) this._zoomInfo.MaxZoom + 1];
      configMediator.PropertyChanged += new PropertyChangedEventHandler(this.ConfigMediatorPropertyChanged);
      this.RebuildZoomSizeCache();
    }

    private void ConfigMediatorPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "TilesStretchFactor":
          this.RebuildZoomSizeCache();
          break;
      }
    }

    private void RebuildZoomSizeCache()
    {
      for (byte zoom = 0; (int) zoom <= (int) this._zoomInfo.MaxZoom; ++zoom)
      {
        this._zoomWidthInPixels[(int) zoom] = (double) this._zoomInfo.GetWidthInPixels(zoom);
        this._zoomHeightInPixels[(int) zoom] = (double) this._zoomInfo.GetHeightInPixels(zoom);
      }
    }

    public DistanceXY RelativeDistanceToMaxSizeDistance(double x) => new DistanceXY((long) (x * 2147483648.0));

    public double RelativeDistanceToMeters(double lat, double x)
    {
      double d = lat * (Math.PI / 180.0);
      return x * 2147483648.0 * Math.Cos(d) * 0.018661383867375494;
    }

    public Point CoordinatesToZoomPoint(GeoCoordinate coord, byte zoom)
    {
      PointXY maxZoomPoint = this.CoordinatesToMaxZoomPoint(coord);
      return new Point(this._zoomWidthInPixels[(int) zoom] * ((double) maxZoomPoint.X * 4.6566128730773926E-10), this._zoomHeightInPixels[(int) zoom] * ((double) maxZoomPoint.Y * 4.6566128730773926E-10));
    }

    public Point CoordinatesToRelativePoint(GeoCoordinate coord)
    {
      PointXY pointXy = coord != null ? this.CoordinatesToMaxZoomPoint(coord) : throw new ArgumentNullException(nameof (coord));
      return new Point((double) pointXy.X * 4.6566128730773926E-10, (double) pointXy.Y * 4.6566128730773926E-10);
    }

    public GeoCoordinate RelativePointToCoordinates(Point relativePoint) => this.MaxZoomPointToCoordinates(this.RelativePointToMaxZoomPoint(relativePoint));

    public Point MaxZoomPointToRelativePoint(PointXY maxZoomPoint) => new Point((double) maxZoomPoint.X * 4.6566128730773926E-10, (double) maxZoomPoint.Y * 4.6566128730773926E-10);

    public PointXY RelativePointToMaxZoomPoint(Point relativePoint) => new PointXY((long) (relativePoint.X * 2147483648.0), (long) (relativePoint.Y * 2147483648.0));

    public GeoCoordinate ZoomPointToCoordinates(Point point, byte zoom)
    {
      double zoomWidthInPixel = this._zoomWidthInPixels[(int) zoom];
      double zoomHeightInPixel = this._zoomHeightInPixels[(int) zoom];
      return this.MaxZoomPointToCoordinates(new PointXY((long) (point.X * (2147483648.0 / zoomWidthInPixel)), (long) (point.Y * (2147483648.0 / zoomHeightInPixel))));
    }

    [Obsolete("Use Yandex.Positioning.GeoCoordinatesExtension.GetDistanceTo extension method instead.")]
    public double Distance(GeoCoordinate startCoord, GeoCoordinate endCoord) => startCoord.GetDistanceTo(endCoord);

    public double MetersToRelativeDistance(double lat, double m)
    {
      double d = lat * (Math.PI / 180.0);
      return 2.495320232503363E-08 * m / Math.Cos(d);
    }

    public double MaxZoomDistancetoMeters(double lat, DistanceXY xy)
    {
      double d = lat * (Math.PI / 180.0);
      return (double) xy.Value * Math.Cos(d) * 0.018661383867375494;
    }

    public DistanceXY MetersToMaxZoomDistance(double lat, double m)
    {
      double d = lat * (Math.PI / 180.0);
      return new DistanceXY((long) (53.5865939582453 * m / Math.Cos(d)));
    }

    private static GeoCoordinate CreateGeoCoordinate(double latitude, double longitude) => new GeoCoordinate(latitude, longitude);

    public GeoCoordinate MaxZoomPointToCoordinates(PointXY point)
    {
      long num1 = (long) ((double) point.X * 0.018661383867375494 - 20037508.342789244);
      double num2 = Math.PI / 2.0 - 2.0 * Math.Atan(1.0 / Math.Exp((double) (long) ((double) -point.Y * 0.018661383867375494 + 20037508.342789244) * 1.5678559428873979E-07));
      double num3 = num2 + 0.0033565514688796939 * Math.Sin(2.0 * num2) + 6.57187271079536E-06 * Math.Sin(4.0 * num2) + 1.764564338702E-08 * Math.Sin(6.0 * num2) + 5.328478445E-11 * Math.Sin(8.0 * num2);
      double num4 = (double) num1 * 1.5678559428873979E-07;
      double v1 = Math.Abs(num3) > Math.PI / 2.0 ? Math.PI / 2.0 : num3;
      double v2 = Math.Abs(num4) > Math.PI ? Math.PI : num4;
      return GeoPixelConverter.CreateGeoCoordinate(GeoPixelConverter.GPS_Math_Rad_To_Deg(v1), GeoPixelConverter.GPS_Math_Rad_To_Deg(v2));
    }

    public double GetMaxZoomSegmentLengthInMeters(ISegment segment)
    {
      double num1 = Math.PI / 2.0 - 2.0 * Math.Atan(1.0 / Math.Exp((double) (long) ((double) -segment.Middle.Y * 0.018661383867375494 + 20037508.342789244) * 1.5678559428873979E-07));
      double num2 = num1 + 0.0033565514688796939 * Math.Sin(2.0 * num1) + 6.57187271079536E-06 * Math.Sin(4.0 * num1) + 1.764564338702E-08 * Math.Sin(6.0 * num1) + 5.328478445E-11 * Math.Sin(8.0 * num1);
      double d = Math.Abs(num2) > Math.PI / 2.0 ? Math.PI / 2.0 : num2;
      return (double) segment.LengthXY.Value * Math.Cos(d) * 0.018661383867375494;
    }

    public Rect RelativeRectangleToMaxSizeRectangle(Rect rect) => new Rect((double) (long) this.RelativeDistanceToMaxSizeDistance(rect.X), (double) (long) this.RelativeDistanceToMaxSizeDistance(rect.Y), (double) (long) this.RelativeDistanceToMaxSizeDistance(rect.Width), (double) (long) this.RelativeDistanceToMaxSizeDistance(rect.Height));

    public PointXY CoordinatesToMaxZoomPoint(GeoCoordinate coord)
    {
      double v1 = coord.Latitude;
      double v2 = coord.Longitude;
      if (v1 > 89.3)
        v1 = 89.3;
      if (v1 < -89.3)
        v1 = -89.3;
      if (v2 > 180.0)
        v2 = 180.0;
      if (v2 < -180.0)
        v2 = -180.0;
      double rad1 = GeoPixelConverter.GPS_Math_Deg_To_Rad(v1);
      double rad2 = GeoPixelConverter.GPS_Math_Deg_To_Rad(v2);
      double num1 = 6378137.0;
      double num2 = (num1 - 0.0033528106647474805 * num1) / num1;
      double num3 = Math.Sqrt(1.0 - num2 * num2);
      double num4 = num1;
      double num5 = Math.Sin(rad1);
      double num6 = Math.Cos(rad1);
      double num7 = num3 * num5;
      double d = (num5 + 1.0) / num6 * Math.Pow((num7 + 1.0) / Math.Sqrt(1.0 - num7 * num7), -num3);
      long num8 = (long) (num4 * Math.Log(d));
      return new PointXY((long) (53.5865939582453 * ((double) (long) (num4 * rad2) + 20037508.342789244)), (long) (-53.5865939582453 * ((double) num8 - 20037508.342789244)));
    }

    protected static double GPS_Math_Rad_To_Deg(double v) => v * (180.0 / Math.PI);

    protected static double GPS_Math_Deg_To_Rad(double v) => v * (Math.PI / 180.0);

    private static double SafeDiv(double a, double b)
    {
      if (b != 0.0)
        return a / b;
      return a == 0.0 ? 1.0 : 0.0;
    }
  }
}
