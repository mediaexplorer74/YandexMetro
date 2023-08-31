// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Finally`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Finally<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Action _finallyAction;

    public Finally(IObservable<TSource> source, Action finallyAction)
    {
      this._source = source;
      this._finallyAction = finallyAction;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Finally<TSource>._ obj = new Finally<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Finally<TSource> _parent;

      public _(Finally<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable subscription = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return Disposable.Create((Action) (() =>
        {
          try
          {
            subscription.Dispose();
          }
          finally
          {
            this._parent._finallyAction();
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
