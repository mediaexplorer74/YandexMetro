// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Delay`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Delay<TSource, TDelay> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IObservable<TDelay> _subscriptionDelay;
    private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

    public Delay(
      IObservable<TSource> source,
      IObservable<TDelay> subscriptionDelay,
      Func<TSource, IObservable<TDelay>> delaySelector)
    {
      this._source = source;
      this._subscriptionDelay = subscriptionDelay;
      this._delaySelector = delaySelector;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Delay<TSource, TDelay>._ obj = new Delay<TSource, TDelay>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Delay<TSource, TDelay> _parent;
      private CompositeDisposable _delays;
      private object _gate;
      private bool _atEnd;
      private SerialDisposable _subscription;

      public _(Delay<TSource, TDelay> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._delays = new CompositeDisposable();
        this._gate = new object();
        this._atEnd = false;
        this._subscription = new SerialDisposable();
        if (this._parent._subscriptionDelay == null)
          this.Start();
        else
          this._subscription.Disposable = this._parent._subscriptionDelay.SubscribeSafe<TDelay>((IObserver<TDelay>) new Delay<TSource, TDelay>._.σ(this));
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) this._subscription,
          (IDisposable) this._delays
        });
      }

      private void Start() => this._subscription.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);

      public void OnNext(TSource value)
      {
        IObservable<TDelay> source;
        try
        {
          source = this._parent._delaySelector(value);
        }
        catch (Exception ex)
        {
          lock (this._gate)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._delays.Add((IDisposable) self);
        self.Disposable = source.SubscribeSafe<TDelay>((IObserver<TDelay>) new Delay<TSource, TDelay>._.δ(this, value, (IDisposable) self));
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
          this._subscription.Dispose();
          this.CheckDone();
        }
      }

      private void CheckDone()
      {
        if (!this._atEnd || this._delays.Count != 0)
          return;
        this._observer.OnCompleted();
        this.Dispose();
      }

      private class σ : IObserver<TDelay>
      {
        private readonly Delay<TSource, TDelay>._ _parent;

        public σ(Delay<TSource, TDelay>._ parent) => this._parent = parent;

        public void OnNext(TDelay value) => this._parent.Start();

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted() => this._parent.Start();
      }

      private class δ : IObserver<TDelay>
      {
        private readonly Delay<TSource, TDelay>._ _parent;
        private readonly TSource _value;
        private readonly IDisposable _self;

        public δ(Delay<TSource, TDelay>._ parent, TSource value, IDisposable self)
        {
          this._parent = parent;
          this._value = value;
          this._self = self;
        }

        public void OnNext(TDelay value)
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnNext(this._value);
            this._parent._delays.Remove(this._self);
            this._parent.CheckDone();
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnNext(this._value);
            this._parent._delays.Remove(this._self);
            this._parent.CheckDone();
          }
        }
      }
    }
  }
}
