// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Select`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Select<TSource, TResult> : Select<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TResult> _selector;
    private readonly Func<TSource, int, TResult> _selectorI;

    public Select(IObservable<TSource> source, Func<TSource, TResult> selector)
    {
      this._source = source;
      this._selector = selector;
    }

    public Select(IObservable<TSource> source, Func<TSource, int, TResult> selector)
    {
      this._source = source;
      this._selectorI = selector;
    }

    public override IObservable<TResult2> Ω<TResult2>(Func<TResult, TResult2> selector) => this._selector != null ? (IObservable<TResult2>) new Select<TSource, TResult2>(this._source, (Func<TSource, TResult2>) (x => selector(this._selector(x)))) : (IObservable<TResult2>) new Select<TResult, TResult2>((IObservable<TResult>) this, selector);

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._selector != null)
      {
        Select<TSource, TResult>._ observer1 = new Select<TSource, TResult>._(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      Select<TSource, TResult>.τ observer2 = new Select<TSource, TResult>.τ(this, observer, cancel);
      setSink((IDisposable) observer2);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly Select<TSource, TResult> _parent;

      public _(Select<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this._parent._selector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
      }

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

    private class τ : Sink<TResult>, IObserver<TSource>
    {
      private readonly Select<TSource, TResult> _parent;
      private int _index;

      public τ(Select<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._index = 0;
      }

      public void OnNext(TSource value)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this._parent._selectorI(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
      }

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
