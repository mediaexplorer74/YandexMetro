// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.TakeLast`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class TakeLast<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly TimeSpan _duration;
    private readonly IScheduler _scheduler;
    private readonly IScheduler _loopScheduler;

    public TakeLast(IObservable<TSource> source, int count, IScheduler loopScheduler)
    {
      this._source = source;
      this._count = count;
      this._loopScheduler = loopScheduler;
    }

    public TakeLast(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler,
      IScheduler loopScheduler)
    {
      this._source = source;
      this._duration = duration;
      this._scheduler = scheduler;
      this._loopScheduler = loopScheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        TakeLast<TSource>._ obj = new TakeLast<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      TakeLast<TSource>.τ τ = new TakeLast<TSource>.τ(this, observer, cancel);
      setSink((IDisposable) τ);
      return τ.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeLast<TSource> _parent;
      private Queue<TSource> _queue;
      private SingleAssignmentDisposable _subscription;
      private SingleAssignmentDisposable _loop;

      public _(TakeLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._queue = new Queue<TSource>();
      }

      public IDisposable Run()
      {
        this._subscription = new SingleAssignmentDisposable();
        this._loop = new SingleAssignmentDisposable();
        this._subscription.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) this._subscription,
          (IDisposable) this._loop
        });
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
        this._subscription.Dispose();
        ISchedulerLongRunning scheduler = this._parent._loopScheduler.AsLongRunning();
        if (scheduler != null)
          this._loop.Disposable = scheduler.ScheduleLongRunning(new Action<ICancelable>(this.Loop));
        else
          this._loop.Disposable = this._parent._loopScheduler.Schedule(new Action<Action>(this.LoopRec));
      }

      private void LoopRec(Action recurse)
      {
        if (this._queue.Count > 0)
        {
          this._observer.OnNext(this._queue.Dequeue());
          recurse();
        }
        else
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private void Loop(ICancelable cancel)
      {
        int count = this._queue.Count;
        while (!cancel.IsDisposed)
        {
          if (count == 0)
          {
            this._observer.OnCompleted();
            break;
          }
          this._observer.OnNext(this._queue.Dequeue());
          --count;
        }
        this.Dispose();
      }
    }

    private class τ : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeLast<TSource> _parent;
      private Queue<System.Reactive.TimeInterval<TSource>> _queue;
      private SingleAssignmentDisposable _subscription;
      private SingleAssignmentDisposable _loop;
      private IStopwatch _watch;

      public τ(TakeLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
      }

      public IDisposable Run()
      {
        this._subscription = new SingleAssignmentDisposable();
        this._loop = new SingleAssignmentDisposable();
        this._watch = this._parent._scheduler.StartStopwatch();
        this._subscription.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) this._subscription,
          (IDisposable) this._loop
        });
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
        this._subscription.Dispose();
        this.Trim(this._watch.Elapsed);
        ISchedulerLongRunning scheduler = this._parent._loopScheduler.AsLongRunning();
        if (scheduler != null)
          this._loop.Disposable = scheduler.ScheduleLongRunning(new Action<ICancelable>(this.Loop));
        else
          this._loop.Disposable = this._parent._loopScheduler.Schedule(new Action<Action>(this.LoopRec));
      }

      private void LoopRec(Action recurse)
      {
        if (this._queue.Count > 0)
        {
          this._observer.OnNext(this._queue.Dequeue().Value);
          recurse();
        }
        else
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private void Loop(ICancelable cancel)
      {
        int count = this._queue.Count;
        while (!cancel.IsDisposed)
        {
          if (count == 0)
          {
            this._observer.OnCompleted();
            break;
          }
          this._observer.OnNext(this._queue.Dequeue().Value);
          --count;
        }
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
