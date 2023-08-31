// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.InterimPointsEnumerators.LinearVelocityInterimPointsEnumerator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.PositionManager.InterimPointsEnumerators.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.PositionManager.InterimPointsEnumerators
{
  internal class LinearVelocityInterimPointsEnumerator : IInterimPointsEnumerator
  {
    private readonly double _accelerationinPixelsPerSecond;
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly double _zoomLevel;
    private Point _currentRelativePoint;
    private Point _currentVelocityInPixelsPerMillisecond;
    private int _signX;
    private int _signY;
    private double _velocityDeltaX;
    private double _velocityDeltaY;

    public LinearVelocityInterimPointsEnumerator(
      double accelerationinPixelsPerSecond,
      IViewportPointConveter viewportPointConveter,
      Position currentPosition,
      Point velocityInPixelsPerMillisecond)
    {
      this._accelerationinPixelsPerSecond = accelerationinPixelsPerSecond;
      this._viewportPointConveter = viewportPointConveter;
      this._currentRelativePoint = currentPosition.RelativePoint;
      this._zoomLevel = currentPosition.Zoom;
      this.BeginGetVelocityInterimPoints(velocityInPixelsPerMillisecond);
    }

    public bool UseLastInterimPoint => false;

    public Position GetNextInterimPoint(double timeFromLastCallInMilliseconds)
    {
      if (this._signX != Math.Sign(this._currentVelocityInPixelsPerMillisecond.X) || this._signY != Math.Sign(this._currentVelocityInPixelsPerMillisecond.Y))
        return (Position) null;
      Point relativePoint = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(this._currentVelocityInPixelsPerMillisecond.X * timeFromLastCallInMilliseconds, this._currentVelocityInPixelsPerMillisecond.Y * timeFromLastCallInMilliseconds), this._zoomLevel));
      this._currentRelativePoint.X -= relativePoint.X;
      this._currentRelativePoint.Y -= relativePoint.Y;
      this._currentVelocityInPixelsPerMillisecond.X += this._velocityDeltaX * timeFromLastCallInMilliseconds;
      this._currentVelocityInPixelsPerMillisecond.Y += this._velocityDeltaY * timeFromLastCallInMilliseconds;
      return new Position()
      {
        RelativePoint = this._currentRelativePoint,
        Zoom = this._zoomLevel
      };
    }

    public Position GetLastInterimPoint(double timeFromLastCallInMilliseconds) => (Position) null;

    public bool IsEnumerationFinished => throw new NotImplementedException();

    public bool StopInterimPointsEnumerationIfCenterPointIsNotValidForZoomlevel => true;

    private void BeginGetVelocityInterimPoints(Point velocityInPixelsPerMillisecond)
    {
      bool flag = false;
      if (velocityInPixelsPerMillisecond.X == 0.0)
      {
        flag = true;
        velocityInPixelsPerMillisecond.X = velocityInPixelsPerMillisecond.Y;
        velocityInPixelsPerMillisecond.Y = 0.0;
      }
      double num1 = Math.Abs(velocityInPixelsPerMillisecond.Y / velocityInPixelsPerMillisecond.X);
      double num2 = 1.0 / Math.Sqrt(1.0 + num1 * num1);
      this._currentVelocityInPixelsPerMillisecond = velocityInPixelsPerMillisecond;
      double num3 = this._accelerationinPixelsPerSecond * 0.001 * num2;
      if (flag)
      {
        this._velocityDeltaY = (double) Math.Sign(this._currentVelocityInPixelsPerMillisecond.X) * num3;
      }
      else
      {
        this._velocityDeltaX = (double) Math.Sign(this._currentVelocityInPixelsPerMillisecond.X) * num3;
        this._velocityDeltaY = (double) Math.Sign(this._currentVelocityInPixelsPerMillisecond.Y) * num3 * num1;
      }
      this._signX = Math.Sign(this._currentVelocityInPixelsPerMillisecond.X);
      this._signY = Math.Sign(this._currentVelocityInPixelsPerMillisecond.Y);
    }
  }
}
