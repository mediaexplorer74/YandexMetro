// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DispatcherScheduler
// Assembly: System.Reactive.Windows.Threading, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: D0078FA9-FF17-4C3C-823C-96AD815797E4
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Windows.Threading.dll

using System.Reactive.Disposables;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace System.Reactive.Concurrency
{
  public class DispatcherScheduler : LocalScheduler, ISchedulerPeriodic
  {
    private Dispatcher _dispatcher;

    [Obsolete("Use the Current property to retrieve the DispatcherScheduler instance for the current thread's Dispatcher object. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.")]
    public static DispatcherScheduler Instance => new DispatcherScheduler(((DependencyObject) Deployment.Current).Dispatcher);

    public static DispatcherScheduler Current => new DispatcherScheduler(((DependencyObject) Deployment.Current).Dispatcher);

    public DispatcherScheduler(Dispatcher dispatcher) => this._dispatcher = dispatcher != null ? dispatcher : throw new ArgumentNullException(nameof (dispatcher));

    public Dispatcher Dispatcher => this._dispatcher;

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      this._dispatcher.BeginInvoke((Action) (() =>
      {
        if (d.IsDisposed)
          return;
        d.Disposable = action((IScheduler) this, state);
      }));
      return (IDisposable) d;
    }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan timeSpan = Scheduler.Normalize(dueTime);
      if (timeSpan.Ticks == 0L)
        return this.Schedule<TState>(state, action);
      MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
      DispatcherTimer timer = new DispatcherTimer();
      timer.Tick += (EventHandler) ((s, e) =>
      {
        DispatcherTimer dispatcherTimer = Interlocked.Exchange<DispatcherTimer>(ref timer, (DispatcherTimer) null);
        if (dispatcherTimer == null)
          return;
        try
        {
          d.Disposable = action((IScheduler) this, state);
        }
        finally
        {
          dispatcherTimer.Stop();
          action = (Func<IScheduler, TState, IDisposable>) null;
        }
      });
      timer.Interval = timeSpan;
      timer.Start();
      d.Disposable = Disposable.Create((Action) (() =>
      {
        DispatcherTimer dispatcherTimer = Interlocked.Exchange<DispatcherTimer>(ref timer, (DispatcherTimer) null);
        if (dispatcherTimer == null)
          return;
        dispatcherTimer.Stop();
        action = (Func<IScheduler, TState, IDisposable>) ((_, __) => Disposable.Empty);
      }));
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
      DispatcherTimer timer = new DispatcherTimer();
      TState state1 = state;
      timer.Tick += (EventHandler) ((s, e) => state1 = action(state1));
      timer.Interval = period;
      timer.Start();
      return Disposable.Create((Action) (() =>
      {
        DispatcherTimer dispatcherTimer = Interlocked.Exchange<DispatcherTimer>(ref timer, (DispatcherTimer) null);
        if (dispatcherTimer == null)
          return;
        dispatcherTimer.Stop();
        action = (Func<TState, TState>) (_ => _);
      }));
    }
  }
}
