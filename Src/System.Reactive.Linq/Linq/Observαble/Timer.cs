// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Timer
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
  internal class Timer : Producer<long>
  {
    private readonly DateTimeOffset? _dueTimeA;
    private readonly TimeSpan? _dueTimeR;
    private readonly TimeSpan? _period;
    private readonly IScheduler _scheduler;

    public Timer(DateTimeOffset dueTime, TimeSpan? period, IScheduler scheduler)
    {
      this._dueTimeA = new DateTimeOffset?(dueTime);
      this._period = period;
      this._scheduler = scheduler;
    }

    public Timer(TimeSpan dueTime, TimeSpan? period, IScheduler scheduler)
    {
      this._dueTimeR = new TimeSpan?(dueTime);
      this._period = period;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<long> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._period.HasValue)
      {
        Timer.π π = new Timer.π(this, observer, cancel);
        setSink((IDisposable) π);
        return π.Run();
      }
      Timer._ obj = new Timer._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<long>
    {
      private readonly Timer _parent;

      public _(Timer parent, IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run() => this._parent._dueTimeA.HasValue ? this._parent._scheduler.Schedule(this._parent._dueTimeA.Value, new Action(this.Invoke)) : this._parent._scheduler.Schedule(this._parent._dueTimeR.Value, new Action(this.Invoke));

      private void Invoke()
      {
        this._observer.OnNext(0L);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class π : Sink<long>
    {
      private readonly Timer _parent;
      private readonly TimeSpan _period;
      private int _pendingTickCount;
      private IDisposable _periodic;

      public π(Timer parent, IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._period = this._parent._period.Value;
      }

      public IDisposable Run()
      {
        if (this._parent._dueTimeA.HasValue)
          return this._parent._scheduler.Schedule<object>((object) null, this._parent._dueTimeA.Value, new Func<IScheduler, object, IDisposable>(this.InvokeStart));
        TimeSpan dueTime = this._parent._dueTimeR.Value;
        return dueTime == this._period ? this._parent._scheduler.SchedulePeriodic<long>(0L, this._period, new Func<long, long>(this.Tick)) : this._parent._scheduler.Schedule<object>((object) null, dueTime, new Func<IScheduler, object, IDisposable>(this.InvokeStart));
      }

      private long Tick(long count)
      {
        this._observer.OnNext(count);
        return count + 1L;
      }

      private IDisposable InvokeStart(IScheduler self, object state)
      {
        this._pendingTickCount = 1;
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._periodic = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = self.SchedulePeriodic<long>(1L, this._period, new Func<long, long>(this.Tock));
        try
        {
          this._observer.OnNext(0L);
        }
        catch (Exception ex)
        {
          assignmentDisposable.Dispose();
          ex.Throw();
        }
        if (Interlocked.Decrement(ref this._pendingTickCount) <= 0)
          return (IDisposable) assignmentDisposable;
        return (IDisposable) new CompositeDisposable(2)
        {
          (IDisposable) assignmentDisposable,
          (IDisposable) new SingleAssignmentDisposable()
          {
            Disposable = self.Schedule<long>(1L, new Action<long, Action<long>>(this.CatchUp))
          }
        };
      }

      private long Tock(long count)
      {
        if (Interlocked.Increment(ref this._pendingTickCount) == 1)
        {
          this._observer.OnNext(count);
          Interlocked.Decrement(ref this._pendingTickCount);
        }
        return count + 1L;
      }

      private void CatchUp(long count, Action<long> recurse)
      {
        try
        {
          this._observer.OnNext(count);
        }
        catch (Exception ex)
        {
          this._periodic.Dispose();
          ex.Throw();
        }
        if (Interlocked.Decrement(ref this._pendingTickCount) <= 0)
          return;
        recurse(count + 1L);
      }
    }
  }
}
