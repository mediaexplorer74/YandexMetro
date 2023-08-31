// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Cast`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Cast<TSource, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;

    public Cast(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Cast<TSource, TResult>._ observer1 = new Cast<TSource, TResult>._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      public _(IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public void OnNext(TSource value)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = (TResult) (object) value;
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
