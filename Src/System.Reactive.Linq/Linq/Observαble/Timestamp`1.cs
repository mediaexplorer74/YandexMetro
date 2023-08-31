// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Timestamp`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
  internal class Timestamp<TSource> : Producer<Timestamped<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly IScheduler _scheduler;

    public Timestamp(IObservable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<Timestamped<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Timestamp<TSource>._ observer1 = new Timestamp<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<Timestamped<TSource>>, IObserver<TSource>
    {
      private readonly Timestamp<TSource> _parent;

      public _(
        Timestamp<TSource> parent,
        IObserver<Timestamped<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value) => this._observer.OnNext(new Timestamped<TSource>(value, this._parent._scheduler.Now));

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
