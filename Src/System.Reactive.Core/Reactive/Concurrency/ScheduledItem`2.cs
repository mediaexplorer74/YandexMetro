// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ScheduledItem`2
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;

namespace System.Reactive.Concurrency
{
  public sealed class ScheduledItem<TAbsolute, TValue> : ScheduledItem<TAbsolute> where TAbsolute : IComparable<TAbsolute>
  {
    private readonly IScheduler _scheduler;
    private readonly TValue _state;
    private readonly Func<IScheduler, TValue, IDisposable> _action;

    public ScheduledItem(
      IScheduler scheduler,
      TValue state,
      Func<IScheduler, TValue, IDisposable> action,
      TAbsolute dueTime,
      IComparer<TAbsolute> comparer)
      : base(dueTime, comparer)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      this._scheduler = scheduler;
      this._state = state;
      this._action = action;
    }

    public ScheduledItem(
      IScheduler scheduler,
      TValue state,
      Func<IScheduler, TValue, IDisposable> action,
      TAbsolute dueTime)
      : this(scheduler, state, action, dueTime, (IComparer<TAbsolute>) Comparer<TAbsolute>.Default)
    {
    }

    protected override IDisposable InvokeCore() => this._action(this._scheduler, this._state);
  }
}
