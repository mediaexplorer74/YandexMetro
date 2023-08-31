// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.VirtualTimeScheduler`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
  public abstract class VirtualTimeScheduler<TAbsolute, TRelative> : 
    VirtualTimeSchedulerBase<TAbsolute, TRelative>
    where TAbsolute : IComparable<TAbsolute>
  {
    private readonly SchedulerQueue<TAbsolute> queue = new SchedulerQueue<TAbsolute>();

    protected VirtualTimeScheduler()
    {
    }

    protected VirtualTimeScheduler(TAbsolute initialClock, IComparer<TAbsolute> comparer)
      : base(initialClock, comparer)
    {
    }

    protected override IScheduledItem<TAbsolute> GetNext()
    {
      while (this.queue.Count > 0)
      {
        ScheduledItem<TAbsolute> next = this.queue.Peek();
        if (!next.IsCanceled)
          return (IScheduledItem<TAbsolute>) next;
        this.queue.Dequeue();
      }
      return (IScheduledItem<TAbsolute>) null;
    }

    public override IDisposable ScheduleAbsolute<TState>(
      TState state,
      TAbsolute dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      ScheduledItem<TAbsolute, TState> si = (ScheduledItem<TAbsolute, TState>) null;
      Func<IScheduler, TState, IDisposable> action1 = (Func<IScheduler, TState, IDisposable>) ((scheduler, state1) =>
      {
        this.queue.Remove((ScheduledItem<TAbsolute>) si);
        return action(scheduler, state1);
      });
      si = new ScheduledItem<TAbsolute, TState>((IScheduler) this, state, action1, dueTime, this.Comparer);
      this.queue.Enqueue((ScheduledItem<TAbsolute>) si);
      return Disposable.Create(new Action(((ScheduledItem<TAbsolute>) si).Cancel));
    }
  }
}
