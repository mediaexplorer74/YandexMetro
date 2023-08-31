// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Aggregate`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Aggregate<TSource, TAccumulate, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly TAccumulate _seed;
    private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
    private readonly Func<TAccumulate, TResult> _resultSelector;

    public Aggregate(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator,
      Func<TAccumulate, TResult> resultSelector)
    {
      this._source = source;
      this._seed = seed;
      this._accumulator = accumulator;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Aggregate<TSource, TAccumulate, TResult>._ observer1 = new Aggregate<TSource, TAccumulate, TResult>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly Aggregate<TSource, TAccumulate, TResult> _parent;
      private TAccumulate _accumulation;

      public _(
        Aggregate<TSource, TAccumulate, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._accumulation = this._parent._seed;
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._accumulation = this._parent._accumulator(this._accumulation, value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this._parent._resultSelector(this._accumulation);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
