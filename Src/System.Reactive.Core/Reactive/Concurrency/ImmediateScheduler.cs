// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ImmediateScheduler
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
  public sealed class ImmediateScheduler : LocalScheduler
  {
    private static readonly ImmediateScheduler s_instance = new ImmediateScheduler();

    private ImmediateScheduler()
    {
    }

    public static ImmediateScheduler Instance => ImmediateScheduler.s_instance;

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return action((IScheduler) new ImmediateScheduler.AsyncLockScheduler(), state);
    }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan timeout = Scheduler.Normalize(dueTime);
      if (timeout.Ticks > 0L)
        ConcurrencyAbstractionLayer.Current.Sleep(timeout);
      return action((IScheduler) new ImmediateScheduler.AsyncLockScheduler(), state);
    }

    private class AsyncLockScheduler : LocalScheduler
    {
      private AsyncLock asyncLock;

      public override IDisposable Schedule<TState>(
        TState state,
        Func<IScheduler, TState, IDisposable> action)
      {
        if (action == null)
          throw new ArgumentNullException(nameof (action));
        SingleAssignmentDisposable m = new SingleAssignmentDisposable();
        if (this.asyncLock == null)
          this.asyncLock = new AsyncLock();
        this.asyncLock.Wait((Action) (() =>
        {
          if (m.IsDisposed)
            return;
          m.Disposable = action((IScheduler) this, state);
        }));
        return (IDisposable) m;
      }

      public override IDisposable Schedule<TState>(
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
      {
        if (action == null)
          throw new ArgumentNullException(nameof (action));
        if (dueTime.Ticks <= 0L)
          return this.Schedule<TState>(state, action);
        IStopwatch timer = ConcurrencyAbstractionLayer.Current.StartStopwatch();
        SingleAssignmentDisposable m = new SingleAssignmentDisposable();
        if (this.asyncLock == null)
          this.asyncLock = new AsyncLock();
        this.asyncLock.Wait((Action) (() =>
        {
          if (m.IsDisposed)
            return;
          TimeSpan timeout = dueTime - timer.Elapsed;
          if (timeout.Ticks > 0L)
            ConcurrencyAbstractionLayer.Current.Sleep(timeout);
          if (m.IsDisposed)
            return;
          m.Disposable = action((IScheduler) this, state);
        }));
        return (IDisposable) m;
      }
    }
  }
}
