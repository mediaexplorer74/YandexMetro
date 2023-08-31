// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ToList`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class ToList<TSource> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;

    public ToList(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToList<TSource>._ observer1 = new ToList<TSource>._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private List<TSource> _list;

      public _(IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._list = new List<TSource>();
      }

      public void OnNext(TSource value) => this._list.Add(value);

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext((IList<TSource>) this._list);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
