// Decompiled with JetBrains decompiler
// Type: Yandex.Input.InertialTouchManipulationSource
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Reactive.Linq;
using Yandex.Common;
using Yandex.Input.Events;
using Yandex.Input.Interfaces;
using Yandex.Media;
using Yandex.Media.Animation;
using Yandex.PAL.Interfaces;

namespace Yandex.Input
{
  internal class InertialTouchManipulationSource : TouchManipulationSource
  {
    private const double Duration = 300.0;
    private const double DurationOpposite = 0.0033333333333333335;
    private const double EPSILON = 1E-05;
    private readonly IStopwatch _stopwatch;
    private readonly IEasingFunction _easingFunction;
    private readonly TimeSpan _heartbeatInterval = TimeSpan.FromMilliseconds(20.0);
    private IDisposable _inertialSubscription;
    private readonly object _inertialLock = new object();
    private readonly IObservable<long> _heartbeatObservable;

    public InertialTouchManipulationSource(
      TimeSpan tapHoldTimespanThreshold,
      TimeSpan tapTimeSpanThreshold,
      [NotNull] IVelocityCalculator velocityCalculator,
      [NotNull] IStopwatch stopwatch,
      [NotNull] IEasingFunction easingFunction)
      : base(tapHoldTimespanThreshold, tapTimeSpanThreshold, velocityCalculator)
    {
      if (stopwatch == null)
        throw new ArgumentNullException(nameof (stopwatch));
      if (easingFunction == null)
        throw new ArgumentNullException(nameof (easingFunction));
      this._stopwatch = stopwatch;
      this._easingFunction = easingFunction;
      this._heartbeatObservable = Observable.Interval(this._heartbeatInterval);
    }

    protected override void OnTap(Point origin)
    {
      if (this.TryStopInertia())
        this.OnCompleted();
      base.OnTap(origin);
    }

    protected override void OnMultiTap(Point origin, uint fingersCount)
    {
      if (this.TryStopInertia())
        this.OnCompleted();
      base.OnMultiTap(origin, fingersCount);
    }

    protected override void OnDoubleTap(Point origin)
    {
      if (this.TryStopInertia())
        this.OnCompleted();
      base.OnDoubleTap(origin);
    }

    protected override void OnManipulationStarted()
    {
      lock (this._inertialLock)
      {
        if (this._inertialSubscription != null)
          return;
        base.OnManipulationStarted();
      }
    }

    protected override void OnManipulationDelta(TouchManipulationDeltaEventArgs e)
    {
      this.TryStopInertia();
      base.OnManipulationDelta(e);
    }

    private bool TryStopInertia()
    {
      if (this._inertialSubscription == null)
        return false;
      lock (this._inertialLock)
      {
        if (this._inertialSubscription == null)
          return false;
        this._inertialSubscription.Dispose();
        this._inertialSubscription = (IDisposable) null;
        this._stopwatch.Stop();
        return true;
      }
    }

    protected override void OnManipulationCompleted(TouchManipulationCompletedEventArgs e)
    {
      if ((this.ManipulationModes & ManipulationModes.TranslateInertia) != ManipulationModes.TranslateInertia)
      {
        base.OnManipulationCompleted(e);
      }
      else
      {
        Point linearVelocity = e.FinalVelocities.LinearVelocity;
        if (Math.Abs(linearVelocity.X) > 1E-05 || Math.Abs(linearVelocity.Y) > 1E-05)
          this.DoInertia(e);
        else
          base.OnManipulationCompleted(e);
      }
    }

    private void DoInertia(TouchManipulationCompletedEventArgs e)
    {
      Point linearVelocity = e.FinalVelocities.LinearVelocity;
      double last = 0.0;
      IObservable<TouchManipulationDeltaEventArgs> source = this._heartbeatObservable.Select<long, double>((Func<long, double>) (nil => this._stopwatch.ElapsedMilliseconds)).TakeWhile<double>((Func<double, bool>) (elapsed => elapsed <= 300.0)).Select<double, TouchManipulationDeltaEventArgs>((Func<double, TouchManipulationDeltaEventArgs>) (current =>
      {
        double num1 = current - last;
        last = current;
        double num2 = this._easingFunction.Ease(current * (1.0 / 300.0));
        double num3 = num1 * (1.0 - num2);
        return new TouchManipulationDeltaEventArgs(true)
        {
          DeltaManipulation = new TouchManipulationDelta(1.0, 1.0, linearVelocity.X * num3, linearVelocity.Y * num3)
        };
      }));
      lock (this._inertialLock)
      {
        if (this._inertialSubscription != null)
          this.TryStopInertia();
        this._stopwatch.Restart();
        this._inertialSubscription = source.Subscribe<TouchManipulationDeltaEventArgs>(new Action<TouchManipulationDeltaEventArgs>(this.OnNextDelta), new Action<Exception>(this.OnError), new Action(this.OnCompleted));
      }
    }

    private void OnNextDelta(TouchManipulationDeltaEventArgs e) => base.OnManipulationDelta(e);

    private void OnCompleted()
    {
      this.TryStopInertia();
      base.OnManipulationCompleted(new TouchManipulationCompletedEventArgs(new TouchManipulationVelocities()));
    }

    private void OnError(Exception ex) => Logger.TrackException(ex);
  }
}
