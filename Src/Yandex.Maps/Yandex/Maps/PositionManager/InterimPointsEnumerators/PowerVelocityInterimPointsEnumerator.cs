// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.InterimPointsEnumerators.PowerVelocityInterimPointsEnumerator
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
  internal class PowerVelocityInterimPointsEnumerator : IInterimPointsEnumerator
  {
    private const double Threshold = 0.001;
    private readonly double _slowdownCoefficientPerMillisecond;
    private readonly Point _startRelativePoint;
    private readonly Point _statrtVelocityInPixelsPerMillisecond;
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly double _zoomLevel;
    private bool _isEnumerationFinished;
    private double _timeFromStartInMilliseconds;

    public PowerVelocityInterimPointsEnumerator(
      double slowdownCoefficientPerMillisecond,
      IViewportPointConveter viewportPointConveter,
      Position currentPosition,
      Point velocityInPixelsPerMillisecond)
    {
      this._slowdownCoefficientPerMillisecond = slowdownCoefficientPerMillisecond;
      this._viewportPointConveter = viewportPointConveter;
      this._startRelativePoint = currentPosition.RelativePoint;
      this._zoomLevel = currentPosition.Zoom;
      this._statrtVelocityInPixelsPerMillisecond = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(velocityInPixelsPerMillisecond, this._zoomLevel));
    }

    public bool UseLastInterimPoint => false;

    public Position GetNextInterimPoint(double timeFromLastCallInMilliseconds)
    {
      this._timeFromStartInMilliseconds += timeFromLastCallInMilliseconds;
      double num1 = Math.Pow(this._slowdownCoefficientPerMillisecond, this._timeFromStartInMilliseconds);
      if (num1 < 0.001)
      {
        this._isEnumerationFinished = true;
        return (Position) null;
      }
      double num2 = 1.0 / Math.Log(this._slowdownCoefficientPerMillisecond);
      double num3 = num2 * (num1 * this._timeFromStartInMilliseconds + num2 - num1 / Math.Log(this._slowdownCoefficientPerMillisecond));
      return new Position()
      {
        RelativePoint = new Point(this._startRelativePoint.X - num3 * this._statrtVelocityInPixelsPerMillisecond.X, this._startRelativePoint.Y - num3 * this._statrtVelocityInPixelsPerMillisecond.Y),
        Zoom = this._zoomLevel
      };
    }

    public Position GetLastInterimPoint(double timeFromLastCallInMilliseconds) => (Position) null;

    public bool IsEnumerationFinished => this._isEnumerationFinished;

    public bool StopInterimPointsEnumerationIfCenterPointIsNotValidForZoomlevel => true;
  }
}
