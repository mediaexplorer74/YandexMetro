// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.NewThreadScheduler
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  public sealed class NewThreadScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
  {
    internal static readonly NewThreadScheduler s_instance = new NewThreadScheduler();
    private readonly Func<ThreadStart, Thread> _threadFactory;

    public NewThreadScheduler()
      : this((Func<ThreadStart, Thread>) (action => new Thread(action)))
    {
    }

    public static NewThreadScheduler Default => NewThreadScheduler.s_instance;

    public NewThreadScheduler(Func<ThreadStart, Thread> threadFactory) => this._threadFactory = threadFactory != null ? threadFactory : throw new ArgumentNullException(nameof (threadFactory));

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return new EventLoopScheduler(this._threadFactory)
      {
        ExitIfEmpty = true
      }.Schedule<TState>(state, dueTime, action);
    }

    public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      BooleanDisposable d = new BooleanDisposable();
      this._threadFactory((ThreadStart) (() => action(state, (ICancelable) d))).Start();
      return (IDisposable) d;
    }

    public IDisposable SchedulePeriodic<TState>(
      TState state,
      TimeSpan period,
      Func<TState, TState> action)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      NewThreadScheduler.Periodic<TState> periodic = new NewThreadScheduler.Periodic<TState>(state, period, action);
      this._threadFactory(new ThreadStart(periodic.Run)).Start();
      return (IDisposable) periodic;
    }

    public override IStopwatch StartStopwatch() => (IStopwatch) new StopwatchImpl();

    private class Periodic<TState> : IDisposable
    {
      private readonly IStopwatch _stopwatch;
      private readonly TimeSpan _period;
      private readonly Func<TState, TState> _action;
      private readonly object _cancel = new object();
      private volatile bool _done;
      private TState _state;
      private TimeSpan _next;

      public Periodic(TState state, TimeSpan period, Func<TState, TState> action)
      {
        this._stopwatch = ConcurrencyAbstractionLayer.Current.StartStopwatch();
        this._period = period;
        this._action = action;
        this._state = state;
        this._next = period;
      }

      public void Run()
      {
        while (!this._done)
        {
          TimeSpan timeout = Scheduler.Normalize(this._next - this._stopwatch.Elapsed);
          lock (this._cancel)
          {
            if (Monitor.Wait(this._cancel, timeout))
              break;
          }
          this._state = this._action(this._state);
          this._next += this._period;
        }
      }

      public void Dispose()
      {
        this._done = true;
        lock (this._cancel)
          Monitor.Pulse(this._cancel);
      }
    }
  }
}
