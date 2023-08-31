// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.OnErrorResumeNext`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class OnErrorResumeNext<TSource> : Producer<TSource>
  {
    private readonly IEnumerable<IObservable<TSource>> _sources;

    public OnErrorResumeNext(IEnumerable<IObservable<TSource>> sources) => this._sources = sources;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      OnErrorResumeNext<TSource>._ obj = new OnErrorResumeNext<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this._sources);
    }

    private class _ : TailRecursiveSink<TSource>
    {
      public _(IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source) => source is OnErrorResumeNext<TSource> onErrorResumeNext ? onErrorResumeNext._sources : (IEnumerable<IObservable<TSource>>) null;

      public override void OnNext(TSource value) => this._observer.OnNext(value);

      public override void OnError(Exception error) => this._recurse();

      public override void OnCompleted() => this._recurse();
    }
  }
}
