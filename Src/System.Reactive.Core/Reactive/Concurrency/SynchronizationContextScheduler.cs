// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SynchronizationContextScheduler
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  public class SynchronizationContextScheduler : LocalScheduler
  {
    private readonly SynchronizationContext _context;
    private readonly bool _alwaysPost;

    public SynchronizationContextScheduler(SynchronizationContext context)
    {
      this._context = context != null ? context : throw new ArgumentNullException(nameof (context));
      this._alwaysPost = true;
    }

    public SynchronizationContextScheduler(SynchronizationContext context, bool alwaysPost)
    {
      this._context = context != null ? context : throw new ArgumentNullException(nameof (context));
      this._alwaysPost = alwaysPost;
    }

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      if (!this._alwaysPost && this._context == SynchronizationContext.Current)
        d.Disposable = action((IScheduler) this, state);
      else
        this._context.PostWithStartComplete((Action) (() =>
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
      TimeSpan dueTime1 = Scheduler.Normalize(dueTime);
      return dueTime1.Ticks == 0L ? this.Schedule<TState>(state, action) : DefaultScheduler.Instance.Schedule<TState>(state, dueTime1, (Func<IScheduler, TState, IDisposable>) ((_, state1) => this.Schedule<TState>(state1, action)));
    }
  }
}
