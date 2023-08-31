// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.HistoricalScheduler
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
  public class HistoricalScheduler : HistoricalSchedulerBase
  {
    private readonly SchedulerQueue<DateTimeOffset> queue = new SchedulerQueue<DateTimeOffset>();

    public HistoricalScheduler()
    {
    }

    public HistoricalScheduler(DateTimeOffset initialClock)
      : base(initialClock)
    {
    }

    public HistoricalScheduler(DateTimeOffset initialClock, IComparer<DateTimeOffset> comparer)
      : base(initialClock, comparer)
    {
    }

    protected override IScheduledItem<DateTimeOffset> GetNext()
    {
      while (this.queue.Count > 0)
      {
        ScheduledItem<DateTimeOffset> next = this.queue.Peek();
        if (!next.IsCanceled)
          return (IScheduledItem<DateTimeOffset>) next;
        this.queue.Dequeue();
      }
      return (IScheduledItem<DateTimeOffset>) null;
    }

    public override IDisposable ScheduleAbsolute<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      ScheduledItem<DateTimeOffset, TState> si = (ScheduledItem<DateTimeOffset, TState>) null;
      Func<IScheduler, TState, IDisposable> action1 = (Func<IScheduler, TState, IDisposable>) ((scheduler, state1) =>
      {
        this.queue.Remove((ScheduledItem<DateTimeOffset>) si);
        return action(scheduler, state1);
      });
      si = new ScheduledItem<DateTimeOffset, TState>((IScheduler) this, state, action1, dueTime, this.Comparer);
      this.queue.Enqueue((ScheduledItem<DateTimeOffset>) si);
      return Disposable.Create(new Action(((ScheduledItem<DateTimeOffset>) si).Cancel));
    }
  }
}
