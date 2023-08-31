// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerWrapper
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Concurrency
{
  internal abstract class SchedulerWrapper : IScheduler, IServiceProvider
  {
    protected readonly IScheduler _scheduler;
    private readonly object _gate = new object();
    private IScheduler _recursiveOriginal;
    private IScheduler _recursiveWrapper;

    public SchedulerWrapper(IScheduler scheduler) => this._scheduler = scheduler;

    public DateTimeOffset Now => this._scheduler.Now;

    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this._scheduler.Schedule<TState>(state, this.Wrap<TState>(action));
    }

    public IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this._scheduler.Schedule<TState>(state, dueTime, this.Wrap<TState>(action));
    }

    public IDisposable Schedule<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this._scheduler.Schedule<TState>(state, dueTime, this.Wrap<TState>(action));
    }

    protected virtual Func<IScheduler, TState, IDisposable> Wrap<TState>(
      Func<IScheduler, TState, IDisposable> action)
    {
      return (Func<IScheduler, TState, IDisposable>) ((self, state) => action(this.GetRecursiveWrapper(self), state));
    }

    protected IScheduler GetRecursiveWrapper(IScheduler scheduler)
    {
      lock (this._gate)
      {
        if (!object.ReferenceEquals((object) scheduler, (object) this._recursiveOriginal))
        {
          this._recursiveOriginal = scheduler;
          SchedulerWrapper schedulerWrapper = this.Clone(scheduler);
          schedulerWrapper._recursiveOriginal = scheduler;
          schedulerWrapper._recursiveWrapper = (IScheduler) schedulerWrapper;
          this._recursiveWrapper = (IScheduler) schedulerWrapper;
        }
        return this._recursiveWrapper;
      }
    }

    protected abstract SchedulerWrapper Clone(IScheduler scheduler);

    public object GetService(Type serviceType)
    {
      if (!(this._scheduler is IServiceProvider scheduler))
        return (object) null;
      object service = (object) null;
      return this.TryGetService(scheduler, serviceType, out service) ? service : scheduler.GetService(serviceType);
    }

    protected abstract bool TryGetService(
      IServiceProvider provider,
      Type serviceType,
      out object service);
  }
}
