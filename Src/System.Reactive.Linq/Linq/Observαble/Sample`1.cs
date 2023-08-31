// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Sample`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Sample<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TimeSpan _interval;
    private readonly IScheduler _scheduler;

    public Sample(IObservable<TSource> source, TimeSpan interval, IScheduler scheduler)
    {
      this._source = source;
      this._interval = interval;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Sample<TSource>._ obj = new Sample<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Sample<TSource> _parent;
      private object _gate;
      private IDisposable _sourceSubscription;
      private bool _hasValue;
      private TSource _value;
      private bool _atEnd;

      public _(Sample<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._sourceSubscription = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) assignmentDisposable,
          this._parent._scheduler.SchedulePeriodic(this._parent._interval, new Action(this.Tick))
        });
      }

      private void Tick()
      {
        lock (this._gate)
        {
          if (this._hasValue)
          {
            this._hasValue = false;
            this._observer.OnNext(this._value);
          }
          if (!this._atEnd)
            return;
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
        {
          this._hasValue = true;
          this._value = value;
        }
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
          this._atEnd = true;
          this._sourceSubscription.Dispose();
        }
      }
    }
  }
}
