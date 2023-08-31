// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.TakeLastBuffer`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
  internal class TakeLastBuffer<TSource> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly TimeSpan _duration;
    private readonly IScheduler _scheduler;

    public TakeLastBuffer(IObservable<TSource> source, int count)
    {
      this._source = source;
      this._count = count;
    }

    public TakeLastBuffer(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
    {
      this._source = source;
      this._duration = duration;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        TakeLastBuffer<TSource>._ observer1 = new TakeLastBuffer<TSource>._(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      TakeLastBuffer<TSource>.τ τ = new TakeLastBuffer<TSource>.τ(this, observer, cancel);
      setSink((IDisposable) τ);
      return τ.Run();
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly TakeLastBuffer<TSource> _parent;
      private Queue<TSource> _queue;

      public _(
        TakeLastBuffer<TSource> parent,
        IObserver<IList<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._queue = new Queue<TSource>();
      }

      public void OnNext(TSource value)
      {
        this._queue.Enqueue(value);
        if (this._queue.Count <= this._parent._count)
          return;
        this._queue.Dequeue();
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        List<TSource> sourceList = new List<TSource>(this._queue.Count);
        while (this._queue.Count > 0)
          sourceList.Add(this._queue.Dequeue());
        this._observer.OnNext((IList<TSource>) sourceList);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class τ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly TakeLastBuffer<TSource> _parent;
      private Queue<System.Reactive.TimeInterval<TSource>> _queue;
      private IStopwatch _watch;

      public τ(
        TakeLastBuffer<TSource> parent,
        IObserver<IList<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
      }

      public IDisposable Run()
      {
        this._watch = this._parent._scheduler.StartStopwatch();
        return this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
      }

      public void OnNext(TSource value)
      {
        TimeSpan elapsed = this._watch.Elapsed;
        this._queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, elapsed));
        this.Trim(elapsed);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this.Trim(this._watch.Elapsed);
        List<TSource> sourceList = new List<TSource>(this._queue.Count);
        while (this._queue.Count > 0)
          sourceList.Add(this._queue.Dequeue().Value);
        this._observer.OnNext((IList<TSource>) sourceList);
        this._observer.OnCompleted();
        this.Dispose();
      }

      private void Trim(TimeSpan now)
      {
        while (this._queue.Count > 0 && now - this._queue.Peek().Interval >= this._parent._duration)
          this._queue.Dequeue();
      }
    }
  }
}
