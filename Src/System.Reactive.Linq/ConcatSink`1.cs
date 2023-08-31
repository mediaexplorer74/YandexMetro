// Decompiled with JetBrains decompiler
// Type: System.Reactive.ConcatSink`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive
{
  internal abstract class ConcatSink<TSource> : TailRecursiveSink<TSource>
  {
    public ConcatSink(IObserver<TSource> observer, IDisposable cancel)
      : base(observer, cancel)
    {
    }

    protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source) => source is IConcatenatable<TSource> concatenatable ? concatenatable.GetSources() : (IEnumerable<IObservable<TSource>>) null;

    public override void OnCompleted() => this._recurse();
  }
}
