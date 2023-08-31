// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.TakeUntil`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class TakeUntil<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly DateTimeOffset _endTime;
    internal readonly IScheduler _scheduler;

    public TakeUntil(IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler)
    {
      this._source = source;
      this._endTime = endTime;
      this._scheduler = scheduler;
    }

    public IObservable<TSource> Ω(DateTimeOffset endTime) => this._endTime <= endTime ? (IObservable<TSource>) this : (IObservable<TSource>) new TakeUntil<TSource>(this._source, endTime, this._scheduler);

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      TakeUntil<TSource>._ obj = new TakeUntil<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeUntil<TSource> _parent;
      private object _gate;

      public _(TakeUntil<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._parent._scheduler.Schedule(this._parent._endTime, new Action(this.Tick)),
          this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this)
        });
      }

      private void Tick()
      {
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
