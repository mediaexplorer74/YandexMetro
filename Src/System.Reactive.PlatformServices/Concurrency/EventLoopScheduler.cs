// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.EventLoopScheduler
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  public sealed class EventLoopScheduler : LocalScheduler, ISchedulerPeriodic, IDisposable
  {
    private static int s_counter;
    private readonly Func<ThreadStart, Thread> _threadFactory;
    private IStopwatch _stopwatch;
    private Thread _thread;
    private readonly object _gate;
    private readonly System.Reactive.Threading.Semaphore _evt;
    private readonly SchedulerQueue<TimeSpan> _queue;
    private readonly Queue<ScheduledItem<TimeSpan>> _readyList;
    private ScheduledItem<TimeSpan> _nextItem;
    private readonly SerialDisposable _nextTimer;
    private bool _disposed;

    public EventLoopScheduler()
      : this((Func<ThreadStart, Thread>) (a => new Thread(a)
      {
        Name = "Event Loop " + (object) Interlocked.Increment(ref EventLoopScheduler.s_counter),
        IsBackground = true
      }))
    {
    }

    public EventLoopScheduler(Func<ThreadStart, Thread> threadFactory)
    {
      this._threadFactory = threadFactory != null ? threadFactory : throw new ArgumentNullException(nameof (threadFactory));
      this._stopwatch = ConcurrencyAbstractionLayer.Current.StartStopwatch();
      this._gate = new object();
      this._evt = new System.Reactive.Threading.Semaphore(0, int.MaxValue);
      this._queue = new SchedulerQueue<TimeSpan>();
      this._readyList = new Queue<ScheduledItem<TimeSpan>>();
      this._nextTimer = new SerialDisposable();
      this.ExitIfEmpty = false;
    }

    internal bool ExitIfEmpty { get; set; }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan dueTime1 = this._stopwatch.Elapsed + dueTime;
      ScheduledItem<TimeSpan, TState> scheduledItem = new ScheduledItem<TimeSpan, TState>((IScheduler) this, state, action, dueTime1);
      lock (this._gate)
      {
        if (this._disposed)
          throw new ObjectDisposedException("");
        if (dueTime <= TimeSpan.Zero)
        {
          this._readyList.Enqueue((ScheduledItem<TimeSpan>) scheduledItem);
          this._evt.Release();
        }
        else
        {
          this._queue.Enqueue((ScheduledItem<TimeSpan>) scheduledItem);
          this._evt.Release();
        }
        this.EnsureThread();
      }
      return Disposable.Create(new Action(((ScheduledItem<TimeSpan>) scheduledItem).Cancel));
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
      TimeSpan next = this._stopwatch.Elapsed + period;
      TState state1 = state;
      MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
      AsyncLock gate = new AsyncLock();
      Func<IScheduler, object, IDisposable> tick = (Func<IScheduler, object, IDisposable>) null;
      tick = (Func<IScheduler, object, IDisposable>) ((self_, _) =>
      {
        next += period;
        d.Disposable = self_.Schedule<object>((object) null, next - this._stopwatch.Elapsed, tick);
        gate.Wait((Action) (() => state1 = action(state1)));
        return Disposable.Empty;
      });
      d.Disposable = this.Schedule<object>((object) null, next - this._stopwatch.Elapsed, tick);
      return (IDisposable) new CompositeDisposable(new IDisposable[2]
      {
        (IDisposable) d,
        (IDisposable) gate
      });
    }

    public override IStopwatch StartStopwatch() => (IStopwatch) new StopwatchImpl();

    public void Dispose()
    {
      lock (this._gate)
      {
        if (this._disposed)
          return;
        this._disposed = true;
        this._nextTimer.Dispose();
        this._evt.Release();
      }
    }

    private void EnsureThread()
    {
      if (this._thread != null)
        return;
      this._thread = this._threadFactory(new ThreadStart(this.Run));
      this._thread.Start();
    }

    private void Run()
    {
label_0:
      do
      {
        this._evt.WaitOne();
        ScheduledItem<TimeSpan>[] scheduledItemArray = (ScheduledItem<TimeSpan>[]) null;
        lock (this._gate)
        {
          if (this._disposed)
          {
            this._evt.Dispose();
            return;
          }
          while (this._queue.Count > 0 && this._queue.Peek().DueTime <= this._stopwatch.Elapsed)
            this._readyList.Enqueue(this._queue.Dequeue());
          if (this._queue.Count > 0)
          {
            ScheduledItem<TimeSpan> state = this._queue.Peek();
            if (state != this._nextItem)
            {
              this._nextItem = state;
              TimeSpan dueTime = state.DueTime - this._stopwatch.Elapsed;
              this._nextTimer.Disposable = ConcurrencyAbstractionLayer.Current.StartTimer(new Action<object>(this.Tick), (object) state, dueTime);
            }
          }
          if (this._readyList.Count > 0)
          {
            scheduledItemArray = this._readyList.ToArray();
            this._readyList.Clear();
          }
        }
        if (scheduledItemArray != null)
        {
          foreach (ScheduledItem<TimeSpan> scheduledItem in scheduledItemArray)
          {
            if (!scheduledItem.IsCanceled)
              scheduledItem.Invoke();
          }
        }
      }
      while (!this.ExitIfEmpty);
      lock (this._gate)
      {
        if (this._readyList.Count == 0)
        {
          if (this._queue.Count == 0)
            this._thread = (Thread) null;
          else
            goto label_0;
        }
        else
          goto label_0;
      }
    }

    private void Tick(object state)
    {
      lock (this._gate)
      {
        if (this._disposed)
          return;
        ScheduledItem<TimeSpan> scheduledItem = (ScheduledItem<TimeSpan>) state;
        if (this._queue.Remove(scheduledItem))
          this._readyList.Enqueue(scheduledItem);
        this._evt.Release();
      }
    }
  }
}
