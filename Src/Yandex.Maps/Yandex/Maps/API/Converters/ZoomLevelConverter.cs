// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Converters.ZoomLevelConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API.Converters
{
  [ComVisible(true)]
  public class ZoomLevelConverter : IZoomLevelConverter
  {
    private static readonly double _2LogOpposite = 1.0 / Math.Log(2.0);
    private readonly IZoomInfo _zoomInfo;
    private readonly byte _minZoom;
    private readonly byte _maxVisibleZoom;

    public ZoomLevelConverter(IZoomInfo zoomInfo)
    {
      this._zoomInfo = zoomInfo != null ? zoomInfo : throw new ArgumentNullException(nameof (zoomInfo));
      this._minZoom = this._zoomInfo.MinZoom;
      this._maxVisibleZoom = this._zoomInfo.MaxVisibleZoom;
    }

    public double ZoomLevelToStretchFactor(double zoomLevel, byte zoom) => Math.Pow(2.0, zoomLevel - (double) zoom);

    public double StretchFactorToZoomLevel(double stretchFactor, byte zoom) => (double) zoom + Math.Log(stretchFactor) * ZoomLevelConverter._2LogOpposite;

    public double StretchFactorToZoomLevel(double stretchFactor) => this.StretchFactorToZoomLevel(stretchFactor, this._maxVisibleZoom);

    public double SafeZoomLevelToStretchFactor(double zoomLevel)
    {
      if (zoomLevel < (double) this._minZoom)
        zoomLevel = (double) this._minZoom;
      else if (zoomLevel > (double) this._maxVisibleZoom)
        zoomLevel = (double) this._maxVisibleZoom;
      return this.ZoomLevelToStretchFactor(zoomLevel, this._maxVisibleZoom);
    }
  }
}
