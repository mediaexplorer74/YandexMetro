// Decompiled with JetBrains decompiler
// Type: System.Reactive.Producer`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
  internal abstract class Producer<TSource> : IProducer<TSource>, IObservable<TSource>
  {
    public IDisposable Subscribe(IObserver<TSource> observer) => observer != null ? this.SubscribeRaw(observer, true) : throw new ArgumentNullException(nameof (observer));

    public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
    {
      Producer<TSource>.State state = new Producer<TSource>.State();
      state.observer = observer;
      state.sink = new SingleAssignmentDisposable();
      state.subscription = new SingleAssignmentDisposable();
      CompositeDisposable compositeDisposable = new CompositeDisposable(2)
      {
        (IDisposable) state.sink,
        (IDisposable) state.subscription
      };
      if (enableSafeguard)
        state.observer = SafeObserver<TSource>.Create(state.observer, (IDisposable) compositeDisposable);
      if (CurrentThreadScheduler.IsScheduleRequired)
        CurrentThreadScheduler.Instance.Schedule<Producer<TSource>.State>(state, new Func<IScheduler, Producer<TSource>.State, IDisposable>(this.Run));
      else
        state.subscription.Disposable = this.Run(state.observer, (IDisposable) state.subscription, new Action<IDisposable>(state.Assign));
      return (IDisposable) compositeDisposable;
    }

    private IDisposable Run(IScheduler _, Producer<TSource>.State x)
    {
      x.subscription.Disposable = this.Run(x.observer, (IDisposable) x.subscription, new Action<IDisposable>(x.Assign));
      return Disposable.Empty;
    }

    protected abstract IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink);

    private struct State
    {
      public SingleAssignmentDisposable sink;
      public SingleAssignmentDisposable subscription;
      public IObserver<TSource> observer;

      public void Assign(IDisposable s) => this.sink.Disposable = s;
    }
  }
}
