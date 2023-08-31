// Decompiled with JetBrains decompiler
// Type: Yandex.Input.TouchManipulationSource
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Common;
using Yandex.Controls.Helpers;
using Yandex.Input.Events;
using Yandex.Input.Interfaces;
using Yandex.Media;
using Yandex.Touch.Events;

namespace Yandex.Input
{
  [ComVisible(false)]
  public class TouchManipulationSource : ITouchHandler, IManipulationSource, IDisposable
  {
    private const double DoubleTapPositionThreshold = 50.0;
    private const double MultiFingerTapScaleTheshold = 1.05;
    private const double TapDistanceThreshold = 20.0;
    private const double ScaleIgnoreLimit = 0.04;
    private const double Epsilon = 1E-09;
    private readonly TimeSpan _tapTimeSpanThreshold;
    private ITouchWrapper _touchWrapper;
    private readonly TimeSpan _tapHoldTimespanThreshold;
    private GestureStatus _gesture;
    private readonly Timer _timer;
    private readonly IVelocityCalculator _velocityCalculator;

    public event EventHandler<TouchManipulationStartedEventArgs> ManipulationStarted;

    protected virtual void OnManipulationStarted()
    {
      EventHandler<TouchManipulationStartedEventArgs> manipulationStarted = this.ManipulationStarted;
      if (manipulationStarted == null)
        return;
      manipulationStarted((object) this, new TouchManipulationStartedEventArgs());
    }

    public event EventHandler<TouchManipulationDeltaEventArgs> ManipulationDelta;

    protected virtual void OnManipulationDelta(TouchManipulationDeltaEventArgs e)
    {
      EventHandler<TouchManipulationDeltaEventArgs> manipulationDelta = this.ManipulationDelta;
      if (manipulationDelta == null)
        return;
      manipulationDelta((object) this, e);
    }

    public event EventHandler<TouchManipulationCompletedEventArgs> ManipulationCompleted;

    protected virtual void OnManipulationCompleted(TouchManipulationCompletedEventArgs e)
    {
      EventHandler<TouchManipulationCompletedEventArgs> manipulationCompleted = this.ManipulationCompleted;
      if (manipulationCompleted == null)
        return;
      manipulationCompleted((object) this, e);
    }

    public event EventHandler<PointerMovedEventArgs> PointerMoved;

    public event EventHandler<TapEventArgs> Tap;

    protected virtual void OnTap(Point origin)
    {
      EventHandler<TapEventArgs> tap = this.Tap;
      if (tap == null)
        return;
      tap((object) this, new TapEventArgs(origin));
    }

    public event EventHandler<DoubleTapEventArgs> DoubleTap;

    protected virtual void OnDoubleTap(Point origin)
    {
      EventHandler<DoubleTapEventArgs> doubleTap = this.DoubleTap;
      if (doubleTap == null)
        return;
      doubleTap((object) this, new DoubleTapEventArgs(origin));
    }

    public event EventHandler<MultiTapEventArgs> MultiTap;

    protected virtual void OnMultiTap(Point origin, uint fingersCount)
    {
      EventHandler<MultiTapEventArgs> multiTap = this.MultiTap;
      if (multiTap == null)
        return;
      multiTap((object) this, new MultiTapEventArgs(origin, fingersCount));
    }

    public event EventHandler<TapHoldEventArgs> TapHold;

    public event EventHandler<FlickEventArgs> Flick;

    public TouchManipulationSource(
      TimeSpan tapHoldTimespanThreshold,
      TimeSpan tapTimeSpanThreshold,
      [NotNull] IVelocityCalculator velocityCalculator)
    {
      if (velocityCalculator == null)
        throw new ArgumentNullException(nameof (velocityCalculator));
      this._tapHoldTimespanThreshold = tapHoldTimespanThreshold;
      this._tapTimeSpanThreshold = tapTimeSpanThreshold;
      this._timer = new Timer(new TimerCallback(this.TapHeartBeatCompleted), (object) null, -1, (int) this._tapTimeSpanThreshold.TotalMilliseconds);
      this._velocityCalculator = velocityCalculator;
      this.ManipulationModes = ManipulationModes.All;
    }

    public ITouchWrapper TouchWrapper
    {
      get => this._touchWrapper;
      set
      {
        if (this._touchWrapper == value)
          return;
        if (this._touchWrapper != null)
          this._touchWrapper.FrameReported -= new EventHandler<TouchEventArgs>(this.TouchFrameReported);
        this._touchWrapper = value;
        if (this._touchWrapper == null)
          return;
        this._touchWrapper.FrameReported += new EventHandler<TouchEventArgs>(this.TouchFrameReported);
      }
    }

    private void TapHeartBeatCompleted(object state)
    {
      if (!this._gesture.DoubleTapIsPending)
        return;
      this._gesture.IsPending = false;
      this._gesture.DoubleTapIsPending = false;
      this.OnTap(this._gesture.DoubleTapStartPoint);
    }

    private void TouchFrameReported(object sender, TouchEventArgs e)
    {
      if (this.Control != null && (!(e.PrimaryTouchPoint.DirectlyOver is UIElement directlyOver) || this.Control != directlyOver && !VisualTreeHelperEx.IsChild(this.Control as DependencyObject, (DependencyObject) directlyOver)))
      {
        if (!this._gesture.IsPending)
          return;
        this.CompleteGesture(e);
      }
      else
      {
        if (!this._gesture.IsPending)
          this._gesture.Initialize(e);
        if (!e.TouchPoints.Any<ITouchPoint>((Func<ITouchPoint, bool>) (point => point.Action != TouchAction.Up)))
        {
          this.CompleteGesture(e);
        }
        else
        {
          int num = e.TouchPoints.Count<ITouchPoint>((Func<ITouchPoint, bool>) (point => point.Action != TouchAction.Up));
          if (num == 1)
          {
            this.ProcessSinglePoint(e);
          }
          else
          {
            if (num <= 1)
              return;
            this.ProcessMultiplePoints(e);
          }
        }
      }
    }

    private void ProcessMultiplePoints(TouchEventArgs e)
    {
      bool tapIsPending = this._gesture.TapIsPending;
      this._gesture.TapIsPending = false;
      this._gesture.DoubleTapIsPending = false;
      Point centralPoint = TouchManipulationSource.GetCentralPoint((IEnumerable<ITouchPoint>) e.TouchPoints, e.TouchPoints.Count);
      double cumulativeScaleDelta;
      double scaleDelta = this.CalculateScaleDelta(e.TouchPoints, e.TouchPoints.Count, out double _, out cumulativeScaleDelta);
      if (this._gesture.MultiFingerTapIsPending)
      {
        if (e.TimestampMilliseconds - this._gesture.StartTimestamp < this._tapHoldTimespanThreshold.TotalMilliseconds && TouchManipulationSource.PositionDelta(this._gesture.StartPoint.X, this._gesture.StartPoint.Y, centralPoint.X, centralPoint.Y) < 20.0 && cumulativeScaleDelta < 1.04 && cumulativeScaleDelta > 0.96)
        {
          this._gesture.TapFramesQueue.Enqueue(e);
        }
        else
        {
          this._gesture.MultiFingerTapIsPending = false;
          this.OnManipulationStarted();
          this.DumpFramesQueue();
          this.ProcessFrame(e);
        }
      }
      else if (tapIsPending)
      {
        this._gesture.MultiFingerTapIsPending = true;
        this._gesture.FirstScaleDistance = scaleDelta;
        this._gesture.StartTimestamp = e.TimestampMilliseconds;
        this._gesture.StartPoint = centralPoint;
        this._gesture.TapFramesQueue.Enqueue(e);
      }
      else
      {
        this.DumpFramesQueue();
        this.ProcessFrame(e);
      }
    }

    private void DumpFramesQueue()
    {
      while (this._gesture.TapFramesQueue.Count > 0)
        this.ProcessFrame(this._gesture.TapFramesQueue.Dequeue());
    }

    private void ProcessSinglePoint(TouchEventArgs e)
    {
      bool flag = e.TouchPoints.Count == 1 && e.TimestampMilliseconds - this._gesture.StartTimestamp < this._tapHoldTimespanThreshold.TotalMilliseconds && TouchManipulationSource.PositionDelta(this._gesture.StartPoint.X, this._gesture.StartPoint.Y, e.TouchPoints[0].Position.X, e.TouchPoints[0].Position.Y) < 20.0;
      if (this._gesture.TapIsPending)
      {
        if (flag)
        {
          this._gesture.TapFramesQueue.Enqueue(e);
        }
        else
        {
          if (this._gesture.DoubleTapIsPending)
          {
            this.OnTap(this._gesture.DoubleTapStartPoint);
            this._gesture.Initialize(e);
          }
          else
            this._gesture.TapIsPending = false;
          this.OnManipulationStarted();
          this.DumpFramesQueue();
          this.ProcessFrame(e);
          this._gesture.DoubleTapIsPending = false;
        }
      }
      else
        this.ProcessFrame(e);
    }

    private void CompleteGesture(TouchEventArgs e)
    {
      if (this._gesture.TapIsPending)
      {
        if (this._gesture.DoubleTapIsPending)
        {
          this._gesture.IsPending = false;
          if (e.TouchPoints.Count == 1 && e.TimestampMilliseconds - this._gesture.DoubleTapStartTimestamp < this._tapHoldTimespanThreshold.TotalMilliseconds && TouchManipulationSource.PositionDelta(this._gesture.DoubleTapStartPoint.X, this._gesture.DoubleTapStartPoint.Y, e.TouchPoints[0].Position.X, e.TouchPoints[0].Position.Y) < 50.0)
          {
            this._gesture.DoubleTapIsPending = false;
            this.OnDoubleTap(e.PrimaryTouchPoint.Position);
          }
          else
          {
            this._gesture.DoubleTapIsPending = false;
            this.OnTap(this._gesture.DoubleTapStartPoint);
            this.OnTap(this._gesture.StartPoint);
          }
        }
        else
        {
          this._gesture.DoubleTapIsPending = true;
          this._gesture.DoubleTapStartTimestamp = this._gesture.StartTimestamp;
          this._gesture.DoubleTapStartPoint = this._gesture.StartPoint;
          this._timer.Change((int) this._tapTimeSpanThreshold.TotalMilliseconds, -1);
        }
      }
      else if (this._gesture.MultiFingerTapIsPending)
      {
        int count = e.TouchPoints.Count;
        Point centralPoint = TouchManipulationSource.GetCentralPoint((IEnumerable<ITouchPoint>) e.TouchPoints, count);
        if (e.TimestampMilliseconds - this._gesture.StartTimestamp < this._tapHoldTimespanThreshold.TotalMilliseconds)
        {
          this.OnMultiTap(centralPoint, (uint) count);
          this._gesture.IsPending = false;
        }
        else
        {
          this.OnManipulationStarted();
          this.DumpFramesQueue();
          this.ProcessFrame(e);
          this._gesture.MultiFingerTapIsPending = false;
        }
      }
      else
        this.ProcessFrame(e, true);
    }

    private void ProcessFrame(TouchEventArgs e, bool forceComplete = false)
    {
      IList<ITouchPoint> touchPoints = e.TouchPoints;
      int num1 = touchPoints.Count<ITouchPoint>();
      if (num1 == 0)
        return;
      Point centralPoint = TouchManipulationSource.GetCentralPoint((IEnumerable<ITouchPoint>) touchPoints, num1);
      int num2 = touchPoints.Count<ITouchPoint>((Func<ITouchPoint, bool>) (point => point.Action != TouchAction.Up));
      double scaleDelta = 1.0;
      double num3 = 0.0;
      if (num1 > 1)
      {
        num3 = this.CalculateScaleDelta(touchPoints, num1, out scaleDelta, out double _);
        this._gesture.CumulativeScaleDelta *= scaleDelta;
      }
      else
        this._gesture.FirstScaleDistance = 0.0;
      TouchManipulationDelta manipulationDelta = (TouchManipulationDelta) null;
      if (this._gesture.LastPoint.HasValue && num1 == this._gesture.LastPointsCount)
      {
        Point point = new Point(centralPoint.X - this._gesture.LastPoint.Value.X, centralPoint.Y - this._gesture.LastPoint.Value.Y);
        this._gesture.CumulativeTranslation = new Point(this._gesture.CumulativeTranslation.X + point.X, this._gesture.CumulativeTranslation.Y + point.Y);
        TouchManipulationDeltaEventArgs e1 = new TouchManipulationDeltaEventArgs(false);
        e1.DeltaManipulation.Translation = point;
        e1.DeltaManipulation.SingleScale = scaleDelta;
        e1.DeltaManipulation.ScaleCenter = centralPoint;
        e1.CumulativeManipulation.SingleScale = this._gesture.CumulativeScaleDelta;
        e1.CumulativeManipulation.Translation = this._gesture.CumulativeTranslation;
        manipulationDelta = e1.DeltaManipulation;
        this.OnManipulationDelta(e1);
        double num4 = e.TimestampMilliseconds - this._gesture.LastFrameTimestamp;
        if (num4 > 0.0)
        {
          double num5 = 1.0 / num4;
          this._gesture.EnqueueVelocity(new Point(point.X * num5, point.Y * num5));
        }
      }
      if (num2 == 0 || forceComplete)
      {
        TouchManipulationCompletedEventArgs e2 = manipulationDelta == null || this._gesture.MultiFingerTapIsPending ? new TouchManipulationCompletedEventArgs(new TouchManipulationVelocities(new Point(1.0, 1.0), new Point())) : new TouchManipulationCompletedEventArgs(new TouchManipulationVelocities(new Point(1.0, 1.0), this._velocityCalculator.CalculateVelocity((IList<Point>) this._gesture.ManipulationVelocities.ToArray())));
        this._gesture.IsPending = false;
        this.OnManipulationCompleted(e2);
      }
      else
      {
        this._gesture.LastPoint = new Point?(centralPoint);
        this._gesture.LastFrameTimestamp = e.TimestampMilliseconds;
        this._gesture.LastScaleDistance = num3;
        this._gesture.LastPointsCount = num1;
        if (0.0 != this._gesture.FirstScaleDistance)
          return;
        this._gesture.FirstScaleDistance = num3;
      }
    }

    private double CalculateScaleDelta(
      IList<ITouchPoint> points,
      int pointsCount,
      out double scaleDelta,
      out double cumulativeScaleDelta)
    {
      double scaleDelta1 = 0.0;
      scaleDelta = 1.0;
      cumulativeScaleDelta = 1.0;
      if (pointsCount > 1)
      {
        scaleDelta1 = TouchManipulationSource.PositionDelta(points[0].Position.X, points[0].Position.Y, points[1].Position.X, points[1].Position.Y);
        scaleDelta = this._gesture.LastScaleDistance <= 0.0 ? 1.0 : scaleDelta1 / this._gesture.LastScaleDistance;
        cumulativeScaleDelta = this._gesture.FirstScaleDistance <= 0.0 ? 1.0 : scaleDelta1 / this._gesture.FirstScaleDistance;
      }
      return scaleDelta1;
    }

    private static double PositionDelta(double x1, double y1, double x2, double y2) => Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

    private static Point GetCentralPoint(IEnumerable<ITouchPoint> points, int count)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      foreach (ITouchPoint point in points)
      {
        num1 += point.Position.X;
        num2 += point.Position.Y;
      }
      return new Point(num1 / (double) count, num2 / (double) count);
    }

    public object Control { get; set; }

    public ManipulationModes ManipulationModes { get; set; }

    public void Dispose() => this.OnDispose();

    protected virtual void OnDispose()
    {
      ITouchWrapper touchWrapper = this.TouchWrapper;
      if (touchWrapper != null)
        touchWrapper.FrameReported -= new EventHandler<TouchEventArgs>(this.TouchFrameReported);
      this._timer.Dispose();
    }
  }
}
