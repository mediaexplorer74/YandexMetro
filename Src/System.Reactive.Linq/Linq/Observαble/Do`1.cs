// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Do`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Do<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Action<TSource> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;

    public Do(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      this._source = source;
      this._onNext = onNext;
      this._onError = onError;
      this._onCompleted = onCompleted;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Do<TSource>._ observer1 = new Do<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Do<TSource> _parent;

      public _(Do<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._parent._onNext(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        try
        {
          this._parent._onError(error);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        try
        {
          this._parent._onCompleted();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
