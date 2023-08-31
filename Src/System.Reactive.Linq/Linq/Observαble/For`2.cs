// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.For`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class For<TSource, TResult> : Producer<TResult>, IConcatenatable<TResult>
  {
    private readonly IEnumerable<TSource> _source;
    private readonly Func<TSource, IObservable<TResult>> _resultSelector;

    public For(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
    {
      this._source = source;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      For<TSource, TResult>._ obj = new For<TSource, TResult>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this.GetSources());
    }

    public IEnumerable<IObservable<TResult>> GetSources()
    {
      foreach (TSource item in this._source)
        yield return this._resultSelector(item);
    }

    private class _ : ConcatSink<TResult>
    {
      public _(IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public override void OnNext(TResult value) => this._observer.OnNext(value);

      public override void OnError(Exception error)
      {
        this._observer.OnError(error);
        base.Dispose();
      }
    }
  }
}
