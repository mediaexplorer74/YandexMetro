// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.DoWhile`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class DoWhile<TSource> : Producer<TSource>, IConcatenatable<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<bool> _condition;

    public DoWhile(IObservable<TSource> source, Func<bool> condition)
    {
      this._condition = condition;
      this._source = source;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DoWhile<TSource>._ obj = new DoWhile<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this.GetSources());
    }

    public IEnumerable<IObservable<TSource>> GetSources()
    {
      yield return this._source;
      while (this._condition())
        yield return this._source;
    }

    private class _ : ConcatSink<TSource>
    {
      public _(IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public override void OnNext(TSource value) => this._observer.OnNext(value);

      public override void OnError(Exception error)
      {
        this._observer.OnError(error);
        base.Dispose();
      }
    }
  }
}
