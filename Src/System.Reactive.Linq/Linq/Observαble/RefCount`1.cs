// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.RefCount`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
  internal class RefCount<TSource> : Producer<TSource>
  {
    private readonly IConnectableObservable<TSource> _source;
    private readonly object _gate;
    private int _count;
    private IDisposable _connectableSubscription;

    public RefCount(IConnectableObservable<TSource> source)
    {
      this._source = source;
      this._gate = new object();
      this._count = 0;
      this._connectableSubscription = (IDisposable) null;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      RefCount<TSource>._ obj = new RefCount<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly RefCount<TSource> _parent;

      public _(RefCount<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable subscription = ((IObservable<TSource>) this._parent._source).SubscribeSafe<TSource>((IObserver<TSource>) this);
        lock (this._parent._gate)
        {
          if (++this._parent._count == 1)
            this._parent._connectableSubscription = this._parent._source.Connect();
        }
        return Disposable.Create((Action) (() =>
        {
          subscription.Dispose();
          lock (this._parent._gate)
          {
            if (--this._parent._count != 0)
              return;
            this._parent._connectableSubscription.Dispose();
          }
        }));
      }

      public void OnNext(TSource value) => this._observer.OnNext(value);

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
