// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Controls.PositionManagerRestictions
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.Controls
{
  [ComVisible(true)]
  public class PositionManagerRestictions : IPositionManagerRestictions
  {
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly IZoomInfo _zoomInfo;
    private readonly double _maxVelocityModule;

    public PositionManagerRestictions(
      IViewportPointConveter viewportPointConveter,
      IZoomInfo zoomInfo,
      double maxVelocityModule)
    {
      if (viewportPointConveter == null)
        throw new ArgumentNullException(nameof (viewportPointConveter));
      if (zoomInfo == null)
        throw new ArgumentNullException(nameof (zoomInfo));
      this._viewportPointConveter = viewportPointConveter;
      this._zoomInfo = zoomInfo;
      this._maxVelocityModule = maxVelocityModule;
    }

    public Rect Viewport { get; set; }

    public void UpdateRelativePointIfViewportIsOutOfMap(ref Point relativePoint, double zoomLevel)
    {
      if (double.IsInfinity(zoomLevel))
        throw new ArgumentOutOfRangeException(nameof (zoomLevel));
      if (double.IsNaN(zoomLevel))
        return;
      Point relativePoint1 = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(this.Viewport.Width, this.Viewport.Height), zoomLevel));
      if (relativePoint.Y < relativePoint1.Y * 0.5)
      {
        relativePoint = new Point(relativePoint.X, relativePoint1.Y * 0.5);
      }
      else
      {
        if (relativePoint.Y <= 1.0 - relativePoint1.Y * 0.5)
          return;
        relativePoint = new Point(relativePoint.X, 1.0 - relativePoint1.Y * 0.5);
      }
    }

    public bool CheckRelativePointIfViewportIsOutOfMap(Point relativePoint, double zoomLevel)
    {
      if (double.IsInfinity(zoomLevel))
        throw new ArgumentOutOfRangeException(nameof (zoomLevel));
      if (double.IsNaN(zoomLevel))
        return false;
      Point relativePoint1 = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(this.Viewport.Width, this.Viewport.Height), zoomLevel));
      return relativePoint.Y >= relativePoint1.Y * 0.5 && relativePoint.Y <= 1.0 - relativePoint1.Y * 0.5;
    }

    public Point VelocityRestriction(Point velocityInPixelsPerMillisecond)
    {
      double num1 = Math.Sqrt(velocityInPixelsPerMillisecond.X * velocityInPixelsPerMillisecond.X + velocityInPixelsPerMillisecond.Y * velocityInPixelsPerMillisecond.Y);
      if (num1 <= this._maxVelocityModule)
        return velocityInPixelsPerMillisecond;
      double num2 = this._maxVelocityModule / num1;
      return new Point(velocityInPixelsPerMillisecond.X * num2, velocityInPixelsPerMillisecond.Y * num2);
    }

    public bool MapIsGreaterThanViewport(byte zoomLevel)
    {
      if (this.Viewport.Width == 0.0 || this.Viewport.Height == 0.0)
        return true;
      ulong widthInPixels = this._zoomInfo.GetWidthInPixels(zoomLevel);
      ulong heightInPixels = this._zoomInfo.GetHeightInPixels(zoomLevel);
      return (double) widthInPixels > this.Viewport.Width && (double) heightInPixels > this.Viewport.Height;
    }
  }
}
