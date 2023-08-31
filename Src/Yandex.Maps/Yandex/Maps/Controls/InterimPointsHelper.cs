// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Controls.InterimPointsHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Interfaces;
using Yandex.Maps.PositionManager.InterimPointsEnumerators;
using Yandex.Maps.PositionManager.InterimPointsEnumerators.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.Controls
{
  [ComVisible(true)]
  public class InterimPointsHelper : IInterimPointsHelper
  {
    private readonly double _slowdownCoefficientPerMillisecond;
    private readonly int _maxDistanceForAnimation;
    private readonly int _minDistanceForAnimation;
    private readonly double _oppositeToStepDistanceInPixels;
    private readonly TimeSpan _stepTimeout;
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly int _zoomStepsCount;

    public InterimPointsHelper(
      IViewportPointConveter viewportPointConveter,
      double animationFramesPerSecond,
      double animationPixelsPerSecond,
      int zoomStepsCount,
      int maxDistanceForAnimation,
      int minDistanceForAnimation,
      double slowdownCoefficientPerMillisecond)
    {
      if (viewportPointConveter == null)
        throw new ArgumentNullException(nameof (viewportPointConveter));
      this._oppositeToStepDistanceInPixels = animationFramesPerSecond / animationPixelsPerSecond;
      this._zoomStepsCount = zoomStepsCount;
      this._maxDistanceForAnimation = maxDistanceForAnimation;
      this._viewportPointConveter = viewportPointConveter;
      this._stepTimeout = TimeSpan.FromSeconds(1.0 / animationFramesPerSecond);
      this._slowdownCoefficientPerMillisecond = slowdownCoefficientPerMillisecond;
      this._minDistanceForAnimation = minDistanceForAnimation;
    }

    public IInterimPointsEnumerator GetVelocityInterimPointsEnumerator(
      Position currentPosition,
      Point velocityRelativePoint)
    {
      return (IInterimPointsEnumerator) new PhisicalVelocityInterimPointsEnumerator(this._slowdownCoefficientPerMillisecond, this._viewportPointConveter, currentPosition, velocityRelativePoint);
    }

    public IInterimPointsEnumerator GetInterimPointsEnumerator(
      Position currentPosition,
      Position targetPosition,
      Point? relativeScaleCenter)
    {
      int pointsNumber = (int) Math.Floor(this.GetDistanceInPixels(currentPosition, targetPosition) * this._oppositeToStepDistanceInPixels) + 1;
      if (pointsNumber < this._zoomStepsCount && currentPosition.Zoom != targetPosition.Zoom)
        pointsNumber = this._zoomStepsCount;
      Point[] interimPoints1 = this.GetInterimPoints(currentPosition.RelativePoint, targetPosition.RelativePoint, pointsNumber);
      double[] interimPoints2 = this.GetInterimPoints(currentPosition.Zoom, targetPosition.Zoom, pointsNumber);
      List<Position> positionsToPass = new List<Position>();
      for (int index = 0; index < pointsNumber; ++index)
        positionsToPass.Add(new Position()
        {
          RelativePoint = interimPoints1[index],
          Zoom = interimPoints2[index],
          RelativeScaleCenter = relativeScaleCenter
        });
      return (IInterimPointsEnumerator) new SimpleInterimPointsEnumerator((IEnumerable<Position>) positionsToPass);
    }

    public bool DistanceIsSuitableForAnimation(Position currentPosition, Position targetPosition)
    {
      double distanceInPixels = this.GetDistanceInPixels(currentPosition, targetPosition);
      if (distanceInPixels > (double) this._maxDistanceForAnimation)
        return false;
      return distanceInPixels >= (double) this._minDistanceForAnimation || Math.Abs(currentPosition.Zoom - targetPosition.Zoom) >= double.Epsilon;
    }

    public TimeSpan StepTimeout => this._stepTimeout;

    public double[] GetInterimPoints(double start, double end, int pointsNumber)
    {
      double[] equalIntervals = this.GetEqualIntervals(end - start, pointsNumber);
      double[] interimPoints = new double[pointsNumber];
      double num = start;
      for (int index = 0; index < pointsNumber; ++index)
      {
        num += equalIntervals[index];
        interimPoints[index] = num;
      }
      interimPoints[pointsNumber - 1] = end;
      return interimPoints;
    }

    public Point[] GetInterimPoints(Point start, Point end, int pointsNumber)
    {
      double[] interimPoints1 = this.GetInterimPoints(start.X, end.X, pointsNumber);
      double[] interimPoints2 = this.GetInterimPoints(start.Y, end.Y, pointsNumber);
      Point[] interimPoints3 = new Point[pointsNumber];
      for (int index = 0; index < pointsNumber; ++index)
        interimPoints3[index] = new Point(interimPoints1[index], interimPoints2[index]);
      return interimPoints3;
    }

    private double GetDistanceInPixels(Position currentPosition, Position targetPosition)
    {
      Point viewportPoint1 = this._viewportPointConveter.RelativePointToViewportPoint(currentPosition.RelativePoint, currentPosition.Zoom);
      Point viewportPoint2 = this._viewportPointConveter.RelativePointToViewportPoint(targetPosition.RelativePoint, currentPosition.Zoom);
      return Math.Sqrt((viewportPoint1.X - viewportPoint2.X) * (viewportPoint1.X - viewportPoint2.X) + (viewportPoint1.Y - viewportPoint2.Y) * (viewportPoint1.Y - viewportPoint2.Y));
    }

    internal double[] GetEqualSlowdownIntervals(
      double pathLength,
      int pointsNumber,
      double slowdownCoefficient)
    {
      double[] points = new double[pointsNumber];
      double num = 1.0;
      for (int index = 0; index < pointsNumber; ++index)
      {
        points[index] = num;
        num *= slowdownCoefficient;
      }
      return this.DistributeUniformly(points, pathLength);
    }

    private double[] GetEqualIntervals(double pathLength, int pointsNumber)
    {
      double[] equalIntervals = new double[pointsNumber];
      double num = pathLength / (double) pointsNumber;
      for (int index = 0; index < pointsNumber; ++index)
        equalIntervals[index] = num;
      return equalIntervals;
    }

    internal double[] DistributeUniformly(double[] points, double target)
    {
      double num1 = ((IEnumerable<double>) points).Sum();
      double num2 = target / num1;
      double[] source = new double[points.Length];
      for (int index = 0; index < points.Length; ++index)
        source[index] = num2 * points[index];
      source[points.Length - 1] -= target - ((IEnumerable<double>) source).Sum();
      return source;
    }
  }
}
