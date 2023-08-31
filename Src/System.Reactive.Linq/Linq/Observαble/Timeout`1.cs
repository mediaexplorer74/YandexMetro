// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Timeout`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Timeout<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TimeSpan? _dueTimeR;
    private readonly DateTimeOffset? _dueTimeA;
    private readonly IObservable<TSource> _other;
    private readonly IScheduler _scheduler;

    public Timeout(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeR = new TimeSpan?(dueTime);
      this._other = other;
      this._scheduler = scheduler;
    }

    public Timeout(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeA = new DateTimeOffset?(dueTime);
      this._other = other;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._dueTimeA.HasValue)
      {
        Timeout<TSource>.α α = new Timeout<TSource>.α(this, observer, cancel);
        setSink((IDisposable) α);
        return α.Run();
      }
      Timeout<TSource>.ρ ρ = new Timeout<TSource>.ρ(this, observer, cancel);
      setSink((IDisposable) ρ);
      return ρ.Run();
    }

    private class α : Sink<TSource>, IObserver<TSource>
    {
      private readonly Timeout<TSource> _parent;
      private SerialDisposable _subscription;
      private object _gate;
      private bool _switched;

      public α(Timeout<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._subscription = new SerialDisposable();
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._subscription.Disposable = (IDisposable) assignmentDisposable;
        this._gate = new object();
        this._switched = false;
        IDisposable disposable = this._parent._scheduler.Schedule(this._parent._dueTimeA.Value, new Action(this.Timeout));
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) this._subscription,
          disposable
        });
      }

      private void Timeout()
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          this._switched = true;
        }
        if (!flag)
          return;
        this._subscription.Disposable = this._parent._other.SubscribeSafe<TSource>(this.GetForwarder());
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
        {
          if (this._switched)
            return;
          this._observer.OnNext(value);
        }
      }

      public void OnError(Exception error)
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          this._switched = true;
        }
        if (!flag)
          return;
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          this._switched = true;
        }
        if (!flag)
          return;
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class ρ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Timeout<TSource> _parent;
      private SerialDisposable _subscription;
      private SerialDisposable _timer;
      private object _gate;
      private ulong _id;
      private bool _switched;

      public ρ(Timeout<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._subscription = new SerialDisposable();
        this._timer = new SerialDisposable();
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._subscription.Disposable = (IDisposable) assignmentDisposable;
        this._gate = new object();
        this._id = 0UL;
        this._switched = false;
        this.CreateTimer();
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) this._subscription,
          (IDisposable) this._timer
        });
      }

      private void CreateTimer() => this._timer.Disposable = this._parent._scheduler.Schedule<ulong>(this._id, this._parent._dueTimeR.Value, new Func<IScheduler, ulong, IDisposable>(this.Timeout));

      private IDisposable Timeout(IScheduler _, ulong myid)
      {
        bool flag = false;
        lock (this._gate)
        {
          this._switched = (long) this._id == (long) myid;
          flag = this._switched;
        }
        if (flag)
          this._subscription.Disposable = this._parent._other.SubscribeSafe<TSource>(this.GetForwarder());
        return Disposable.Empty;
      }

      public void OnNext(TSource value)
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          if (flag)
            ++this._id;
        }
        if (!flag)
          return;
        this._observer.OnNext(value);
        this.CreateTimer();
      }

      public void OnError(Exception error)
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          if (flag)
            ++this._id;
        }
        if (!flag)
          return;
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          if (flag)
            ++this._id;
        }
        if (!flag)
          return;
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
