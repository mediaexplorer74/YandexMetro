// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Converters.ViewportPointConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.API.Converters
{
  [ComVisible(true)]
  public class ViewportPointConverter : IViewportPointConveter
  {
    private readonly IZoomLevelConverter _zoomLevelConverter;
    private readonly IZoomInfo _zoomInfo;
    private readonly IGeoPixelConverter _geoPixelConverter;

    public ViewportPointConverter(
      IZoomLevelConverter zoomLevelConverter,
      IZoomInfo zoomInfo,
      IGeoPixelConverter geoPixelConverter)
    {
      if (zoomLevelConverter == null)
        throw new ArgumentNullException(nameof (zoomLevelConverter));
      if (zoomInfo == null)
        throw new ArgumentNullException(nameof (zoomInfo));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      this._zoomLevelConverter = zoomLevelConverter;
      this._zoomInfo = zoomInfo;
      this._geoPixelConverter = geoPixelConverter;
    }

    public Point ViewportPointToRelativePoint(ViewportPoint point)
    {
      ulong widthInPixels = this._zoomInfo.GetWidthInPixels(this._zoomInfo.MaxVisibleZoom);
      double num = 1.0 / this._zoomLevelConverter.SafeZoomLevelToStretchFactor(point.ZoomLevel) / (double) widthInPixels;
      return new Point(point.Point.X * num, point.Point.Y * num);
    }

    public Point RelativePointToViewportPoint(Point relativePoint, double zoomLevel)
    {
      ulong widthInPixels = this._zoomInfo.GetWidthInPixels(this._zoomInfo.MaxVisibleZoom);
      double num = this._zoomLevelConverter.SafeZoomLevelToStretchFactor(zoomLevel) * (double) widthInPixels;
      return new Point(relativePoint.X * num, relativePoint.Y * num);
    }

    public Point CoordinatesToViewportPoint(GeoCoordinate value, double zoomLevel) => this.RelativePointToViewportPoint(this._geoPixelConverter.CoordinatesToRelativePoint(value), zoomLevel);

    public GeoCoordinate ViewportPointToCoordinates(Point point, double zoomLevel) => this._geoPixelConverter.RelativePointToCoordinates(this.ViewportPointToRelativePoint(new ViewportPoint(point, zoomLevel)));

    public Rect RelativeRectToViewportRect(Rect relativeRect, double zoomLevel)
    {
      ulong widthInPixels = this._zoomInfo.GetWidthInPixels(this._zoomInfo.MaxVisibleZoom);
      double num = this._zoomLevelConverter.SafeZoomLevelToStretchFactor(zoomLevel) * (double) widthInPixels;
      return new Rect(relativeRect.X * num, relativeRect.Y * num, relativeRect.Width * num, relativeRect.Height * num);
    }
  }
}
