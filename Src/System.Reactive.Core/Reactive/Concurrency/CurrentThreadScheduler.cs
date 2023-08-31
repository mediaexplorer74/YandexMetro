// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.CurrentThreadScheduler
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  public sealed class CurrentThreadScheduler : LocalScheduler
  {
    private static readonly CurrentThreadScheduler s_instance = new CurrentThreadScheduler();
    private static readonly Dictionary<int, SchedulerQueue<TimeSpan>> s_queues = new Dictionary<int, SchedulerQueue<TimeSpan>>();
    private static readonly Dictionary<int, IStopwatch> s_clocks = new Dictionary<int, IStopwatch>();

    private CurrentThreadScheduler()
    {
    }

    public static CurrentThreadScheduler Instance => CurrentThreadScheduler.s_instance;

    private static SchedulerQueue<TimeSpan> GetQueue()
    {
      lock (CurrentThreadScheduler.s_queues)
      {
        SchedulerQueue<TimeSpan> schedulerQueue = (SchedulerQueue<TimeSpan>) null;
        return CurrentThreadScheduler.s_queues.TryGetValue(Thread.CurrentThread.ManagedThreadId, out schedulerQueue) ? schedulerQueue : (SchedulerQueue<TimeSpan>) null;
      }
    }

    private static void SetQueue(SchedulerQueue<TimeSpan> newQueue)
    {
      lock (CurrentThreadScheduler.s_queues)
      {
        if (newQueue == null)
          CurrentThreadScheduler.s_queues.Remove(Thread.CurrentThread.ManagedThreadId);
        else
          CurrentThreadScheduler.s_queues[Thread.CurrentThread.ManagedThreadId] = newQueue;
      }
    }

    private static TimeSpan Time
    {
      get
      {
        lock (CurrentThreadScheduler.s_clocks)
        {
          IStopwatch stopwatch = (IStopwatch) null;
          if (!CurrentThreadScheduler.s_clocks.TryGetValue(Thread.CurrentThread.ManagedThreadId, out stopwatch))
            CurrentThreadScheduler.s_clocks[Thread.CurrentThread.ManagedThreadId] = stopwatch = ConcurrencyAbstractionLayer.Current.StartStopwatch();
          return stopwatch.Elapsed;
        }
      }
    }

    [Obsolete("This instance property is no longer supported. Use CurrentThreadScheduler.IsScheduleRequired instead. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ScheduleRequired => CurrentThreadScheduler.IsScheduleRequired;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static bool IsScheduleRequired => CurrentThreadScheduler.GetQueue() == null;

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan dueTime1 = CurrentThreadScheduler.Time + Scheduler.Normalize(dueTime);
      ScheduledItem<TimeSpan, TState> scheduledItem = new ScheduledItem<TimeSpan, TState>((IScheduler) this, state, action, dueTime1);
      SchedulerQueue<TimeSpan> queue = CurrentThreadScheduler.GetQueue();
      if (queue == null)
      {
        SchedulerQueue<TimeSpan> schedulerQueue = new SchedulerQueue<TimeSpan>(4);
        schedulerQueue.Enqueue((ScheduledItem<TimeSpan>) scheduledItem);
        CurrentThreadScheduler.SetQueue(schedulerQueue);
        try
        {
          CurrentThreadScheduler.Trampoline.Run(schedulerQueue);
        }
        finally
        {
          CurrentThreadScheduler.SetQueue((SchedulerQueue<TimeSpan>) null);
        }
      }
      else
        queue.Enqueue((ScheduledItem<TimeSpan>) scheduledItem);
      return Disposable.Create(new Action(((ScheduledItem<TimeSpan>) scheduledItem).Cancel));
    }

    private static class Trampoline
    {
      public static void Run(SchedulerQueue<TimeSpan> queue)
      {
        while (queue.Count > 0)
        {
          ScheduledItem<TimeSpan> scheduledItem = queue.Dequeue();
          if (!scheduledItem.IsCanceled)
          {
            TimeSpan timeout = scheduledItem.DueTime - CurrentThreadScheduler.Time;
            if (timeout.Ticks > 0L)
              ConcurrencyAbstractionLayer.Current.Sleep(timeout);
            if (!scheduledItem.IsCanceled)
              scheduledItem.Invoke();
          }
        }
      }
    }
  }
}
