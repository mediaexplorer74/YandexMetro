// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Timeout`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Timeout<TSource, TTimeout> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IObservable<TTimeout> _firstTimeout;
    private readonly Func<TSource, IObservable<TTimeout>> _timeoutSelector;
    private readonly IObservable<TSource> _other;

    public Timeout(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutSelector,
      IObservable<TSource> other)
    {
      this._source = source;
      this._firstTimeout = firstTimeout;
      this._timeoutSelector = timeoutSelector;
      this._other = other;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Timeout<TSource, TTimeout>._ obj = new Timeout<TSource, TTimeout>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Timeout<TSource, TTimeout> _parent;
      private SerialDisposable _subscription;
      private SerialDisposable _timer;
      private object _gate;
      private ulong _id;
      private bool _switched;

      public _(Timeout<TSource, TTimeout> parent, IObserver<TSource> observer, IDisposable cancel)
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
        this.SetTimer(this._parent._firstTimeout);
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) this._subscription,
          (IDisposable) this._timer
        });
      }

      public void OnNext(TSource value)
      {
        if (!this.ObserverWins())
          return;
        this._observer.OnNext(value);
        IObservable<TTimeout> timeout;
        try
        {
          timeout = this._parent._timeoutSelector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this.SetTimer(timeout);
      }

      public void OnError(Exception error)
      {
        if (!this.ObserverWins())
          return;
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this.ObserverWins())
          return;
        this._observer.OnCompleted();
        this.Dispose();
      }

      private void SetTimer(IObservable<TTimeout> timeout)
      {
        ulong id = this._id;
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._timer.Disposable = (IDisposable) self;
        self.Disposable = timeout.SubscribeSafe<TTimeout>((IObserver<TTimeout>) new Timeout<TSource, TTimeout>._.τ(this, id, (IDisposable) self));
      }

      private bool ObserverWins()
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._switched;
          if (flag)
            ++this._id;
        }
        return flag;
      }

      private class τ : IObserver<TTimeout>
      {
        private readonly Timeout<TSource, TTimeout>._ _parent;
        private readonly ulong _id;
        private readonly IDisposable _self;

        public τ(Timeout<TSource, TTimeout>._ parent, ulong id, IDisposable self)
        {
          this._parent = parent;
          this._id = id;
          this._self = self;
        }

        public void OnNext(TTimeout value)
        {
          if (this.TimerWins())
            this._parent._subscription.Disposable = this._parent._parent._other.SubscribeSafe<TSource>(this._parent.GetForwarder());
          this._self.Dispose();
        }

        public void OnError(Exception error)
        {
          if (!this.TimerWins())
            return;
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted()
        {
          if (!this.TimerWins())
            return;
          this._parent._subscription.Disposable = this._parent._parent._other.SubscribeSafe<TSource>(this._parent.GetForwarder());
        }

        private bool TimerWins()
        {
          lock (this._parent._gate)
          {
            this._parent._switched = (long) this._parent._id == (long) this._id;
            return this._parent._switched;
          }
        }
      }
    }
  }
}
