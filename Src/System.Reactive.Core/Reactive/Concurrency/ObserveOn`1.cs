// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ObserveOn`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  internal class ObserveOn<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IScheduler _scheduler;
    private readonly SynchronizationContext _context;

    public ObserveOn(IObservable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    public ObserveOn(IObservable<TSource> source, SynchronizationContext context)
    {
      this._source = source;
      this._context = context;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._context != null)
      {
        ObserveOn<TSource>.ς ς = new ObserveOn<TSource>.ς(this, observer, cancel);
        setSink((IDisposable) ς);
        return ς.Run();
      }
      ObserveOnObserver<TSource> observer1 = new ObserveOnObserver<TSource>(this._scheduler, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class ς : Sink<TSource>, IObserver<TSource>
    {
      private readonly ObserveOn<TSource> _parent;

      public ς(ObserveOn<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._parent._context.OperationStarted();
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this),
          Disposable.Create((Action) (() => this._parent._context.OperationCompleted()))
        });
      }

      public void OnNext(TSource value) => this._parent._context.Post(new SendOrPostCallback(this.OnNextPosted), (object) value);

      public void OnError(Exception error) => this._parent._context.Post(new SendOrPostCallback(this.OnErrorPosted), (object) error);

      public void OnCompleted() => this._parent._context.Post(new SendOrPostCallback(this.OnCompletedPosted), (object) null);

      private void OnNextPosted(object value) => this._observer.OnNext((TSource) value);

      private void OnErrorPosted(object error)
      {
        this._observer.OnError((Exception) error);
        this.Dispose();
      }

      private void OnCompletedPosted(object ignored)
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
