// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.PushToPullAdapter`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal abstract class PushToPullAdapter<TSource, TResult> : IEnumerable<TResult>, IEnumerable
  {
    private readonly IObservable<TSource> _source;

    public PushToPullAdapter(IObservable<TSource> source) => this._source = source;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public IEnumerator<TResult> GetEnumerator()
    {
      SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
      PushToPullSink<TSource, TResult> observer = this.Run((IDisposable) subscription);
      subscription.Disposable = this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer);
      return (IEnumerator<TResult>) observer;
    }

    protected abstract PushToPullSink<TSource, TResult> Run(IDisposable subscription);
  }
}
