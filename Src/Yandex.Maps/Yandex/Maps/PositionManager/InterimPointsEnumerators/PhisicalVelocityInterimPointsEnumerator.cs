// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.InterimPointsEnumerators.PhisicalVelocityInterimPointsEnumerator
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
  internal class PhisicalVelocityInterimPointsEnumerator : IInterimPointsEnumerator
  {
    private readonly double _slowdownCoefficientPerMsX;
    private readonly double _slowdownCoefficientPerMsY;
    private readonly Point _startRelativePoint;
    private readonly Point _startVelocityInPixelsPerMs;
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly double _zoomLevel;
    private bool _finished;
    private double _timeFromStartInMs;

    public PhisicalVelocityInterimPointsEnumerator(
      double slowdownCoefficientPerMillisecond,
      IViewportPointConveter viewportPointConveter,
      Position currentPosition,
      Point velocityInPixelsPerMs)
    {
      this._viewportPointConveter = viewportPointConveter;
      this._startRelativePoint = currentPosition.RelativePoint;
      this._zoomLevel = currentPosition.Zoom;
      this._startVelocityInPixelsPerMs = velocityInPixelsPerMs;
      double num = slowdownCoefficientPerMillisecond / Math.Sqrt(this._startVelocityInPixelsPerMs.X * this._startVelocityInPixelsPerMs.X + this._startVelocityInPixelsPerMs.Y * this._startVelocityInPixelsPerMs.Y);
      this._slowdownCoefficientPerMsX = this._startVelocityInPixelsPerMs.X * num;
      this._slowdownCoefficientPerMsY = this._startVelocityInPixelsPerMs.Y * num;
    }

    public bool UseLastInterimPoint => false;

    public Position GetNextInterimPoint(double timeFromLastCallInMilliseconds)
    {
      if (this._finished)
        return (Position) null;
      this._timeFromStartInMs += timeFromLastCallInMilliseconds;
      Point point = new Point(this._startVelocityInPixelsPerMs.X + this._slowdownCoefficientPerMsX * this._timeFromStartInMs, this._startVelocityInPixelsPerMs.Y + this._slowdownCoefficientPerMsY * this._timeFromStartInMs);
      bool flag1 = Math.Sign(point.X) != Math.Sign(this._startVelocityInPixelsPerMs.X);
      bool flag2 = Math.Sign(point.Y) != Math.Sign(this._startVelocityInPixelsPerMs.Y);
      if (!flag1 && !flag2)
        return this.GetInterimPoint(this._timeFromStartInMs);
      double timeFromStartInMs = flag1 ? -this._startVelocityInPixelsPerMs.X / this._slowdownCoefficientPerMsX : -this._startVelocityInPixelsPerMs.Y / this._slowdownCoefficientPerMsY;
      this._finished = true;
      return this.GetInterimPoint(timeFromStartInMs);
    }

    public Position GetLastInterimPoint(double timeFromLastCallInMilliseconds) => (Position) null;

    public bool IsEnumerationFinished => this._finished;

    public bool StopInterimPointsEnumerationIfCenterPointIsNotValidForZoomlevel => true;

    private Position GetInterimPoint(double timeFromStartInMs)
    {
      double num = timeFromStartInMs * timeFromStartInMs * 0.5;
      Point relativePoint = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(this._startVelocityInPixelsPerMs.X * timeFromStartInMs + this._slowdownCoefficientPerMsX * num, this._startVelocityInPixelsPerMs.Y * timeFromStartInMs + this._slowdownCoefficientPerMsY * num), this._zoomLevel));
      return new Position()
      {
        RelativePoint = new Point(this._startRelativePoint.X - relativePoint.X, this._startRelativePoint.Y - relativePoint.Y),
        Zoom = this._zoomLevel
      };
    }
  }
}
