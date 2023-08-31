// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.TimeInterval`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
  internal class TimeInterval<TSource> : Producer<System.Reactive.TimeInterval<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly IScheduler _scheduler;

    public TimeInterval(IObservable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<System.Reactive.TimeInterval<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      TimeInterval<TSource>._ obj = new TimeInterval<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<System.Reactive.TimeInterval<TSource>>, IObserver<TSource>
    {
      private readonly TimeInterval<TSource> _parent;
      private IStopwatch _watch;
      private TimeSpan _last;

      public _(
        TimeInterval<TSource> parent,
        IObserver<System.Reactive.TimeInterval<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._watch = this._parent._scheduler.StartStopwatch();
        this._last = TimeSpan.Zero;
        return this._parent._source.Subscribe((IObserver<TSource>) this);
      }

      public void OnNext(TSource value)
      {
        TimeSpan elapsed = this._watch.Elapsed;
        TimeSpan interval = elapsed.Subtract(this._last);
        this._last = elapsed;
        this._observer.OnNext(new System.Reactive.TimeInterval<TSource>(value, interval));
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
