// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Delay`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
  internal class Delay<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TimeSpan? _dueTimeR;
    private readonly DateTimeOffset? _dueTimeA;
    private readonly IScheduler _scheduler;

    public Delay(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeR = new TimeSpan?(dueTime);
      this._scheduler = scheduler;
    }

    public Delay(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeA = new DateTimeOffset?(dueTime);
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler.AsLongRunning() != null)
      {
        Delay<TSource>.λ λ = new Delay<TSource>.λ(this, observer, cancel);
        setSink((IDisposable) λ);
        return λ.Run();
      }
      Delay<TSource>._ obj = new Delay<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Delay<TSource> _parent;
      private IScheduler _scheduler;
      private IDisposable _sourceSubscription;
      private SerialDisposable _cancelable;
      private TimeSpan _delay;
      private IStopwatch _watch;
      private object _gate;
      private bool _ready;
      private bool _active;
      private bool _running;
      private Queue<System.Reactive.TimeInterval<TSource>> _queue;
      private bool _hasCompleted;
      private TimeSpan _completeAt;
      private bool _hasFailed;
      private Exception _exception;

      public _(Delay<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._scheduler = this._parent._scheduler;
        this._cancelable = new SerialDisposable();
        this._gate = new object();
        this._active = false;
        this._running = false;
        this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
        this._hasCompleted = false;
        this._completeAt = new TimeSpan();
        this._hasFailed = false;
        this._exception = (Exception) null;
        this._watch = this._scheduler.StartStopwatch();
        if (this._parent._dueTimeA.HasValue)
        {
          this._ready = false;
          this._cancelable.Disposable = this._scheduler.Schedule(this._parent._dueTimeA.Value, new Action(this.Start));
        }
        else
        {
          this._ready = true;
          this._delay = Scheduler.Normalize(this._parent._dueTimeR.Value);
        }
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._sourceSubscription = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._sourceSubscription,
          (IDisposable) this._cancelable
        });
      }

      private void Start()
      {
        TimeSpan dueTime = new TimeSpan();
        bool flag = false;
        lock (this._gate)
        {
          this._delay = this._watch.Elapsed;
          Queue<System.Reactive.TimeInterval<TSource>> queue = this._queue;
          this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
          if (queue.Count > 0)
          {
            dueTime = queue.Peek().Interval;
            while (queue.Count > 0)
            {
              System.Reactive.TimeInterval<TSource> timeInterval = queue.Dequeue();
              this._queue.Enqueue(new System.Reactive.TimeInterval<TSource>(timeInterval.Value, timeInterval.Interval.Add(this._delay)));
            }
            flag = true;
            this._active = true;
          }
          this._ready = true;
        }
        if (!flag)
          return;
        this._cancelable.Disposable = this._scheduler.Schedule(dueTime, new Action<Action<TimeSpan>>(this.DrainQueue));
      }

      public void OnNext(TSource value)
      {
        TimeSpan interval = this._watch.Elapsed.Add(this._delay);
        bool flag = false;
        lock (this._gate)
        {
          this._queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, interval));
          flag = this._ready && !this._active;
          this._active = true;
        }
        if (!flag)
          return;
        this._cancelable.Disposable = this._scheduler.Schedule(this._delay, new Action<Action<TimeSpan>>(this.DrainQueue));
      }

      public void OnError(Exception error)
      {
        this._sourceSubscription.Dispose();
        bool flag = false;
        lock (this._gate)
        {
          this._queue.Clear();
          this._exception = error;
          this._hasFailed = true;
          flag = !this._running;
        }
        if (!flag)
          return;
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._sourceSubscription.Dispose();
        TimeSpan timeSpan = this._watch.Elapsed.Add(this._delay);
        bool flag = false;
        lock (this._gate)
        {
          this._completeAt = timeSpan;
          this._hasCompleted = true;
          flag = this._ready && !this._active;
          this._active = true;
        }
        if (!flag)
          return;
        this._cancelable.Disposable = this._scheduler.Schedule(this._delay, new Action<Action<TimeSpan>>(this.DrainQueue));
      }

      private void DrainQueue(Action<TimeSpan> recurse)
      {
        lock (this._gate)
        {
          if (this._hasFailed)
            return;
          this._running = true;
        }
        bool flag1 = false;
        bool flag2;
        Exception exception;
        bool flag3;
        bool flag4;
        TimeSpan timeSpan;
        while (true)
        {
          flag2 = false;
          exception = (Exception) null;
          bool flag5 = false;
          TSource source = default (TSource);
          flag3 = false;
          flag4 = false;
          timeSpan = new TimeSpan();
          lock (this._gate)
          {
            if (this._hasFailed)
            {
              exception = this._exception;
              flag2 = true;
              this._running = false;
            }
            else
            {
              TimeSpan elapsed = this._watch.Elapsed;
              if (this._queue.Count > 0)
              {
                TimeSpan interval = this._queue.Peek().Interval;
                if (interval.CompareTo(elapsed) <= 0 && !flag1)
                {
                  source = this._queue.Dequeue().Value;
                  flag5 = true;
                }
                else
                {
                  flag4 = true;
                  timeSpan = Scheduler.Normalize(interval.Subtract(elapsed));
                  this._running = false;
                }
              }
              else if (this._hasCompleted)
              {
                if (this._completeAt.CompareTo(elapsed) <= 0 && !flag1)
                {
                  flag3 = true;
                }
                else
                {
                  flag4 = true;
                  timeSpan = Scheduler.Normalize(this._completeAt.Subtract(elapsed));
                  this._running = false;
                }
              }
              else
              {
                this._running = false;
                this._active = false;
              }
            }
          }
          if (flag5)
          {
            this._observer.OnNext(source);
            flag1 = true;
          }
          else
            break;
        }
        if (flag3)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
        else if (flag2)
        {
          this._observer.OnError(exception);
          this.Dispose();
        }
        else
        {
          if (!flag4)
            return;
          recurse(timeSpan);
        }
      }
    }

    private class λ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Delay<TSource> _parent;
      private IDisposable _sourceSubscription;
      private SerialDisposable _cancelable;
      private TimeSpan _delay;
      private IStopwatch _watch;
      private object _gate;
      private System.Reactive.Threading.Semaphore _evt;
      private bool _stopped;
      private ManualResetEvent _stop;
      private Queue<System.Reactive.TimeInterval<TSource>> _queue;
      private bool _hasCompleted;
      private TimeSpan _completeAt;
      private bool _hasFailed;
      private Exception _exception;

      public λ(Delay<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._cancelable = new SerialDisposable();
        this._gate = new object();
        this._evt = new System.Reactive.Threading.Semaphore(0, int.MaxValue);
        this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
        this._hasCompleted = false;
        this._completeAt = new TimeSpan();
        this._hasFailed = false;
        this._exception = (Exception) null;
        this._watch = this._parent._scheduler.StartStopwatch();
        if (this._parent._dueTimeA.HasValue)
        {
          this._cancelable.Disposable = this._parent._scheduler.Schedule(this._parent._dueTimeA.Value, new Action(this.Start));
        }
        else
        {
          this._delay = Scheduler.Normalize(this._parent._dueTimeR.Value);
          this.ScheduleDrain();
        }
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._sourceSubscription = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._sourceSubscription,
          (IDisposable) this._cancelable
        });
      }

      private void Start()
      {
        lock (this._gate)
        {
          this._delay = this._watch.Elapsed;
          Queue<System.Reactive.TimeInterval<TSource>> queue = this._queue;
          this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
          while (queue.Count > 0)
          {
            System.Reactive.TimeInterval<TSource> timeInterval = queue.Dequeue();
            this._queue.Enqueue(new System.Reactive.TimeInterval<TSource>(timeInterval.Value, timeInterval.Interval.Add(this._delay)));
          }
        }
        this.ScheduleDrain();
      }

      private void ScheduleDrain()
      {
        this._stop = new ManualResetEvent(false);
        this._cancelable.Disposable = Disposable.Create((Action) (() =>
        {
          this._stopped = true;
          this._stop.Set();
          this._evt.Release();
        }));
        this._parent._scheduler.AsLongRunning().ScheduleLongRunning(new Action<ICancelable>(this.DrainQueue));
      }

      public void OnNext(TSource value)
      {
        TimeSpan interval = this._watch.Elapsed.Add(this._delay);
        lock (this._gate)
        {
          this._queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, interval));
          this._evt.Release();
        }
      }

      public void OnError(Exception error)
      {
        this._sourceSubscription.Dispose();
        lock (this._gate)
        {
          this._queue.Clear();
          this._exception = error;
          this._hasFailed = true;
          this._evt.Release();
        }
      }

      public void OnCompleted()
      {
        this._sourceSubscription.Dispose();
        TimeSpan timeSpan = this._watch.Elapsed.Add(this._delay);
        lock (this._gate)
        {
          this._completeAt = timeSpan;
          this._hasCompleted = true;
          this._evt.Release();
        }
      }

      private void DrainQueue(ICancelable cancel)
      {
        bool flag1;
        Exception exception;
        bool flag2;
        while (true)
        {
          this._evt.WaitOne();
          if (!this._stopped)
          {
            flag1 = false;
            exception = (Exception) null;
            bool flag3 = false;
            TSource source = default (TSource);
            flag2 = false;
            bool flag4 = false;
            TimeSpan dueTime = new TimeSpan();
            lock (this._gate)
            {
              if (this._hasFailed)
              {
                exception = this._exception;
                flag1 = true;
              }
              else
              {
                TimeSpan elapsed = this._watch.Elapsed;
                if (this._queue.Count > 0)
                {
                  System.Reactive.TimeInterval<TSource> timeInterval = this._queue.Dequeue();
                  flag3 = true;
                  source = timeInterval.Value;
                  TimeSpan interval = timeInterval.Interval;
                  if (interval.CompareTo(elapsed) > 0)
                  {
                    flag4 = true;
                    dueTime = Scheduler.Normalize(interval.Subtract(elapsed));
                  }
                }
                else if (this._hasCompleted)
                {
                  flag2 = true;
                  if (this._completeAt.CompareTo(elapsed) > 0)
                  {
                    flag4 = true;
                    dueTime = Scheduler.Normalize(this._completeAt.Subtract(elapsed));
                  }
                }
              }
            }
            if (flag4)
            {
              ManualResetEvent timer = new ManualResetEvent(false);
              this._parent._scheduler.Schedule(dueTime, (Action) (() => timer.Set()));
              if (WaitHandle.WaitAny((WaitHandle[]) new ManualResetEvent[2]
              {
                timer,
                this._stop
              }) == 1)
                goto label_1;
            }
            if (flag3)
              this._observer.OnNext(source);
            else
              goto label_20;
          }
          else
            break;
        }
        return;
label_1:
        return;
label_20:
        if (flag2)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
        else
        {
          if (!flag1)
            return;
          this._observer.OnError(exception);
          this.Dispose();
        }
      }
    }
  }
}
