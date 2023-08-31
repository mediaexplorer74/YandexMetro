// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Catch`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class Catch<TSource> : Producer<TSource>
  {
    private readonly IEnumerable<IObservable<TSource>> _sources;

    public Catch(IEnumerable<IObservable<TSource>> sources) => this._sources = sources;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Catch<TSource>._ obj = new Catch<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this._sources);
    }

    private class _ : TailRecursiveSink<TSource>
    {
      private Exception _lastException;

      public _(IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source) => source is Catch<TSource> @catch ? @catch._sources : (IEnumerable<IObservable<TSource>>) null;

      public override void OnNext(TSource value) => this._observer.OnNext(value);

      public override void OnError(Exception error)
      {
        this._lastException = error;
        this._recurse();
      }

      public override void OnCompleted()
      {
        this._observer.OnCompleted();
        base.Dispose();
      }

      protected override void Done()
      {
        if (this._lastException != null)
          this._observer.OnError(this._lastException);
        else
          this._observer.OnCompleted();
        base.Dispose();
      }
    }
  }
}
