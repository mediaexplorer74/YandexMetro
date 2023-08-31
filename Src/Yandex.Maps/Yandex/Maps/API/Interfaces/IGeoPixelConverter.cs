// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.IGeoPixelConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Runtime.InteropServices;
using Yandex.Maps.Units;
using Yandex.Maps.Units.Interfaces;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface IGeoPixelConverter
  {
    Point CoordinatesToZoomPoint(GeoCoordinate coord, byte zoom);

    GeoCoordinate ZoomPointToCoordinates(Point point, byte zoom);

    PointXY CoordinatesToMaxZoomPoint(GeoCoordinate coord);

    GeoCoordinate MaxZoomPointToCoordinates(PointXY point);

    Point CoordinatesToRelativePoint([NotNull] GeoCoordinate coord);

    GeoCoordinate RelativePointToCoordinates(Point relativePoint);

    Point MaxZoomPointToRelativePoint(PointXY maxZoomPoint);

    PointXY RelativePointToMaxZoomPoint(Point maxZoomPoint);

    double Distance(GeoCoordinate startCoord, GeoCoordinate endCoord);

    double MaxZoomDistancetoMeters(double lat, DistanceXY xy);

    double MetersToRelativeDistance(double lat, double m);

    DistanceXY RelativeDistanceToMaxSizeDistance(double x);

    DistanceXY MetersToMaxZoomDistance(double lat, double m);

    double RelativeDistanceToMeters(double lat, double x);

    double GetMaxZoomSegmentLengthInMeters(ISegment segment);

    Rect RelativeRectangleToMaxSizeRectangle(Rect rect);
  }
}
