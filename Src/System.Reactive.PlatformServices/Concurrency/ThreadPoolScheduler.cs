// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ThreadPoolScheduler
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  public sealed class ThreadPoolScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
  {
    private static readonly ThreadPoolScheduler s_instance = new ThreadPoolScheduler();

    public static ThreadPoolScheduler Instance => ThreadPoolScheduler.s_instance;

    private ThreadPoolScheduler()
    {
    }

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      ThreadPool.QueueUserWorkItem((WaitCallback) (_ =>
      {
        if (d.IsDisposed)
          return;
        d.Disposable = action((IScheduler) this, state);
      }), (object) null);
      return (IDisposable) d;
    }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan dueTime1 = Scheduler.Normalize(dueTime);
      return dueTime1.Ticks == 0L ? this.Schedule<TState>(state, action) : (IDisposable) new ThreadPoolScheduler.Timer<TState>((IScheduler) this, state, dueTime1, action);
    }

    public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return NewThreadScheduler.Default.ScheduleLongRunning<TState>(state, action);
    }

    public override IStopwatch StartStopwatch() => (IStopwatch) new StopwatchImpl();

    public IDisposable SchedulePeriodic<TState>(
      TState state,
      TimeSpan period,
      Func<TState, TState> action)
    {
      if (period <= TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return (IDisposable) new ThreadPoolScheduler.PeriodicTimer<TState>(state, period, action);
    }

    private abstract class Timer
    {
      protected static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
    }

    private sealed class Timer<TState> : ThreadPoolScheduler.Timer, IDisposable
    {
      private readonly MultipleAssignmentDisposable _disposable;
      private readonly IScheduler _parent;
      private readonly TState _state;
      private Func<IScheduler, TState, IDisposable> _action;
      private System.Threading.Timer _timer;
      private bool _hasAdded;
      private bool _hasRemoved;

      public Timer(
        IScheduler parent,
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
      {
        this._disposable = new MultipleAssignmentDisposable();
        this._disposable.Disposable = Disposable.Create(new Action(this.Unroot));
        this._parent = parent;
        this._state = state;
        this._action = action;
        this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), (object) null, dueTime, TimeSpan.FromMilliseconds(-1.0));
        lock (ThreadPoolScheduler.Timer.s_timers)
        {
          if (this._hasRemoved)
            return;
          ThreadPoolScheduler.Timer.s_timers.Add(this._timer, (object) null);
          this._hasAdded = true;
        }
      }

      private void Tick(object state)
      {
        try
        {
          this._disposable.Disposable = this._action(this._parent, this._state);
        }
        finally
        {
          this.Unroot();
        }
      }

      private void Unroot()
      {
        this._action = new Func<IScheduler, TState, IDisposable>(this.Nop);
        System.Threading.Timer key = (System.Threading.Timer) null;
        lock (ThreadPoolScheduler.Timer.s_timers)
        {
          if (!this._hasRemoved)
          {
            key = this._timer;
            this._timer = (System.Threading.Timer) null;
            if (this._hasAdded && key != null)
              ThreadPoolScheduler.Timer.s_timers.Remove(key);
            this._hasRemoved = true;
          }
        }
        key?.Dispose();
      }

      private IDisposable Nop(IScheduler scheduler, TState state) => Disposable.Empty;

      public void Dispose() => this._disposable.Dispose();
    }

    private abstract class PeriodicTimer
    {
      protected static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
    }

    private sealed class PeriodicTimer<TState> : ThreadPoolScheduler.PeriodicTimer, IDisposable
    {
      private readonly AsyncLock _gate;
      private TState _state;
      private Func<TState, TState> _action;
      private System.Threading.Timer _timer;

      public PeriodicTimer(TState state, TimeSpan period, Func<TState, TState> action)
      {
        this._gate = new AsyncLock();
        this._state = state;
        this._action = action;
        this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), (object) null, period, period);
        lock (ThreadPoolScheduler.PeriodicTimer.s_timers)
          ThreadPoolScheduler.PeriodicTimer.s_timers.Add(this._timer, (object) null);
      }

      private void Tick(object state) => this._gate.Wait((Action) (() => this._state = this._action(this._state)));

      public void Dispose()
      {
        System.Threading.Timer key = (System.Threading.Timer) null;
        lock (ThreadPoolScheduler.PeriodicTimer.s_timers)
        {
          key = this._timer;
          this._timer = (System.Threading.Timer) null;
          if (key != null)
            ThreadPoolScheduler.PeriodicTimer.s_timers.Remove(key);
        }
        if (key == null)
          return;
        key.Dispose();
        this._gate.Dispose();
        this._action = Stubs<TState>.I;
      }
    }
  }
}
