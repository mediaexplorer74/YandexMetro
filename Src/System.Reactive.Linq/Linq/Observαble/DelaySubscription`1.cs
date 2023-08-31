// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.DelaySubscription`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
  internal class DelaySubscription<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly DateTimeOffset? _dueTimeA;
    private readonly TimeSpan? _dueTimeR;
    private readonly IScheduler _scheduler;

    public DelaySubscription(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeA = new DateTimeOffset?(dueTime);
      this._scheduler = scheduler;
    }

    public DelaySubscription(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeR = new TimeSpan?(dueTime);
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DelaySubscription<TSource>._ state = new DelaySubscription<TSource>._(observer, cancel);
      setSink((IDisposable) state);
      return this._dueTimeA.HasValue ? this._scheduler.Schedule<DelaySubscription<TSource>._>(state, this._dueTimeA.Value, new Func<IScheduler, DelaySubscription<TSource>._, IDisposable>(this.Subscribe)) : this._scheduler.Schedule<DelaySubscription<TSource>._>(state, this._dueTimeR.Value, new Func<IScheduler, DelaySubscription<TSource>._, IDisposable>(this.Subscribe));
    }

    private IDisposable Subscribe(IScheduler _, DelaySubscription<TSource>._ sink) => this._source.SubscribeSafe<TSource>((IObserver<TSource>) sink);

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      public _(IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public void OnNext(TSource value) => this._observer.OnNext(value);

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
