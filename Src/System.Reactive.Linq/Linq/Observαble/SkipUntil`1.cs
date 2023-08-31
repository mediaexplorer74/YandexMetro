// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SkipUntil`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class SkipUntil<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly DateTimeOffset _startTime;
    internal readonly IScheduler _scheduler;

    public SkipUntil(IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler)
    {
      this._source = source;
      this._startTime = startTime;
      this._scheduler = scheduler;
    }

    public IObservable<TSource> Ω(DateTimeOffset startTime) => startTime <= this._startTime ? (IObservable<TSource>) this : (IObservable<TSource>) new SkipUntil<TSource>(this._source, startTime, this._scheduler);

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SkipUntil<TSource>._ obj = new SkipUntil<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly SkipUntil<TSource> _parent;
      private volatile bool _open;

      public _(SkipUntil<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run() => (IDisposable) new CompositeDisposable(new IDisposable[2]
      {
        this._parent._scheduler.Schedule(this._parent._startTime, new Action(this.Tick)),
        this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this)
      });

      private void Tick() => this._open = true;

      public void OnNext(TSource value)
      {
        if (!this._open)
          return;
        this._observer.OnNext(value);
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
