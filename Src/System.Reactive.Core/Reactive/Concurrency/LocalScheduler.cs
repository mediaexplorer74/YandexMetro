// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.LocalScheduler
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Threading;

namespace System.Reactive.Concurrency
{
  public abstract class LocalScheduler : IScheduler, IStopwatchProvider, IServiceProvider
  {
    private const int MAXERRORRATIO = 1000;
    private static readonly object s_gate = new object();
    private static readonly PriorityQueue<LocalScheduler.WorkItem> s_longTerm = new PriorityQueue<LocalScheduler.WorkItem>();
    private static readonly SerialDisposable s_nextLongTermTimer = new SerialDisposable();
    private static LocalScheduler.WorkItem s_nextLongTermWorkItem = (LocalScheduler.WorkItem) null;
    private static readonly PriorityQueue<LocalScheduler.WorkItem> s_shortTerm = new PriorityQueue<LocalScheduler.WorkItem>();
    private static readonly Dictionary<IDisposable, object> s_shortTermWork = new Dictionary<IDisposable, object>();
    private static readonly TimeSpan SHORTTERM = TimeSpan.FromSeconds(10.0);
    private static readonly TimeSpan LONGTOSHORT = TimeSpan.FromSeconds(5.0);
    private static readonly TimeSpan RETRYSHORT = TimeSpan.FromMilliseconds(50.0);

    static LocalScheduler() => SystemClock.SystemClockChanged += new EventHandler<SystemClockChangedEventArgs>(LocalScheduler.SystemClockChanged);

    private static IDisposable Enqueue<TState>(
      IScheduler scheduler,
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      TimeSpan timeSpan = Scheduler.Normalize(dueTime - scheduler.Now);
      if (timeSpan == TimeSpan.Zero)
        return scheduler.Schedule<TState>(state, TimeSpan.Zero, action);
      SystemClock.AddRef();
      LocalScheduler.WorkItem<TState> workItem = new LocalScheduler.WorkItem<TState>(scheduler, state, dueTime, action);
      if (timeSpan <= LocalScheduler.SHORTTERM)
        LocalScheduler.ScheduleShortTermWork((LocalScheduler.WorkItem) workItem);
      else
        LocalScheduler.ScheduleLongTermWork((LocalScheduler.WorkItem) workItem);
      return (IDisposable) workItem;
    }

    private static void ScheduleShortTermWork(LocalScheduler.WorkItem item)
    {
      lock (LocalScheduler.s_gate)
      {
        LocalScheduler.s_shortTerm.Enqueue(item);
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        LocalScheduler.s_shortTermWork.Add((IDisposable) assignmentDisposable, (object) null);
        TimeSpan dueTime = Scheduler.Normalize(item.DueTime - item.Scheduler.Now);
        assignmentDisposable.Disposable = item.Scheduler.Schedule<SingleAssignmentDisposable>(assignmentDisposable, dueTime, new Func<IScheduler, SingleAssignmentDisposable, IDisposable>(LocalScheduler.ExecuteNextShortTermWorkItem));
      }
    }

    private static IDisposable ExecuteNextShortTermWorkItem(
      IScheduler scheduler,
      IDisposable cancel)
    {
      LocalScheduler.WorkItem workItem = (LocalScheduler.WorkItem) null;
      lock (LocalScheduler.s_gate)
      {
        if (LocalScheduler.s_shortTermWork.Remove(cancel))
        {
          if (LocalScheduler.s_shortTerm.Count > 0)
            workItem = LocalScheduler.s_shortTerm.Dequeue();
        }
      }
      if (workItem != null)
      {
        if (workItem.DueTime - workItem.Scheduler.Now >= LocalScheduler.RETRYSHORT)
          LocalScheduler.ScheduleShortTermWork(workItem);
        else
          workItem.Invoke(scheduler);
      }
      return Disposable.Empty;
    }

    private static void ScheduleLongTermWork(LocalScheduler.WorkItem item)
    {
      lock (LocalScheduler.s_gate)
      {
        LocalScheduler.s_longTerm.Enqueue(item);
        LocalScheduler.UpdateLongTermProcessingTimer();
      }
    }

    private static void UpdateLongTermProcessingTimer()
    {
      if (LocalScheduler.s_longTerm.Count == 0)
        return;
      LocalScheduler.WorkItem workItem = LocalScheduler.s_longTerm.Peek();
      if (workItem == LocalScheduler.s_nextLongTermWorkItem)
        return;
      TimeSpan timeSpan1 = Scheduler.Normalize(workItem.DueTime - workItem.Scheduler.Now);
      TimeSpan timeSpan2 = TimeSpan.FromTicks(Math.Max(timeSpan1.Ticks / 1000L, LocalScheduler.LONGTOSHORT.Ticks));
      TimeSpan dueTime = timeSpan1 - timeSpan2;
      LocalScheduler.s_nextLongTermWorkItem = workItem;
      LocalScheduler.s_nextLongTermTimer.Disposable = ConcurrencyAbstractionLayer.Current.StartTimer(new Action<object>(LocalScheduler.EvaluateLongTermQueue), (object) null, dueTime);
    }

    private static void EvaluateLongTermQueue(object state)
    {
      lock (LocalScheduler.s_gate)
      {
        while (LocalScheduler.s_longTerm.Count > 0)
        {
          LocalScheduler.WorkItem workItem = LocalScheduler.s_longTerm.Peek();
          if (!(Scheduler.Normalize(workItem.DueTime - workItem.Scheduler.Now) >= LocalScheduler.SHORTTERM))
            LocalScheduler.ScheduleShortTermWork(LocalScheduler.s_longTerm.Dequeue());
          else
            break;
        }
        LocalScheduler.s_nextLongTermWorkItem = (LocalScheduler.WorkItem) null;
        LocalScheduler.UpdateLongTermProcessingTimer();
      }
    }

    private static void SystemClockChanged(object sender, SystemClockChangedEventArgs args)
    {
      lock (LocalScheduler.s_gate)
      {
        foreach (IDisposable key in LocalScheduler.s_shortTermWork.Keys)
          key.Dispose();
        LocalScheduler.s_shortTermWork.Clear();
        while (LocalScheduler.s_shortTerm.Count > 0)
        {
          LocalScheduler.WorkItem workItem = LocalScheduler.s_shortTerm.Dequeue();
          LocalScheduler.s_longTerm.Enqueue(workItem);
        }
        LocalScheduler.s_nextLongTermWorkItem = (LocalScheduler.WorkItem) null;
        LocalScheduler.EvaluateLongTermQueue((object) null);
      }
    }

    public virtual DateTimeOffset Now => Scheduler.Now;

    public virtual IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this.Schedule<TState>(state, TimeSpan.Zero, action);
    }

    public abstract IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action);

    public virtual IDisposable Schedule<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return LocalScheduler.Enqueue<TState>((IScheduler) this, state, dueTime, action);
    }

    public virtual IStopwatch StartStopwatch() => ConcurrencyAbstractionLayer.Current.StartStopwatch();

    object IServiceProvider.GetService(Type serviceType) => this.GetService(serviceType);

    protected virtual object GetService(Type serviceType)
    {
      if ((object) serviceType == (object) typeof (IStopwatchProvider))
        return (object) this;
      if ((object) serviceType == (object) typeof (ISchedulerLongRunning))
        return (object) (this as ISchedulerLongRunning);
      return (object) serviceType == (object) typeof (ISchedulerPeriodic) ? (object) (this as ISchedulerPeriodic) : (object) null;
    }

    private abstract class WorkItem : IComparable<LocalScheduler.WorkItem>, IDisposable
    {
      private readonly IScheduler _scheduler;
      private readonly DateTimeOffset _dueTime;
      private readonly SingleAssignmentDisposable _disposable;
      private int _hasRun;

      public WorkItem(IScheduler scheduler, DateTimeOffset dueTime)
      {
        this._scheduler = scheduler;
        this._dueTime = dueTime;
        this._disposable = new SingleAssignmentDisposable();
        this._hasRun = 0;
      }

      public IScheduler Scheduler => this._scheduler;

      public DateTimeOffset DueTime => this._dueTime;

      public void Invoke(IScheduler scheduler)
      {
        if (Interlocked.Exchange(ref this._hasRun, 1) != 0)
          return;
        try
        {
          if (this._disposable.IsDisposed)
            return;
          this._disposable.Disposable = this.InvokeCore(scheduler);
        }
        finally
        {
          SystemClock.Release();
        }
      }

      protected abstract IDisposable InvokeCore(IScheduler scheduler);

      public int CompareTo(LocalScheduler.WorkItem other) => Comparer<DateTimeOffset>.Default.Compare(this._dueTime, other._dueTime);

      public void Dispose() => this._disposable.Dispose();
    }

    private sealed class WorkItem<TState> : LocalScheduler.WorkItem
    {
      private readonly TState _state;
      private readonly Func<IScheduler, TState, IDisposable> _action;

      public WorkItem(
        IScheduler scheduler,
        TState state,
        DateTimeOffset dueTime,
        Func<IScheduler, TState, IDisposable> action)
        : base(scheduler, dueTime)
      {
        this._state = state;
        this._action = action;
      }

      protected override IDisposable InvokeCore(IScheduler scheduler) => this._action(scheduler, this._state);
    }
  }
}
