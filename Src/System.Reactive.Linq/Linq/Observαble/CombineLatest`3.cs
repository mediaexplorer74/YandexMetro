// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.CombineLatest`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class CombineLatest<TFirst, TSecond, TResult> : Producer<TResult>
  {
    private readonly IObservable<TFirst> _first;
    private readonly IObservable<TSecond> _second;
    private readonly Func<TFirst, TSecond, TResult> _resultSelector;

    public CombineLatest(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      this._first = first;
      this._second = second;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      CombineLatest<TFirst, TSecond, TResult>._ obj = new CombineLatest<TFirst, TSecond, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly CombineLatest<TFirst, TSecond, TResult> _parent;
      private object _gate;

      public _(
        CombineLatest<TFirst, TSecond, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable self1 = new SingleAssignmentDisposable();
        SingleAssignmentDisposable self2 = new SingleAssignmentDisposable();
        CombineLatest<TFirst, TSecond, TResult>._.F observer1 = new CombineLatest<TFirst, TSecond, TResult>._.F(this, (IDisposable) self1);
        CombineLatest<TFirst, TSecond, TResult>._.S observer2 = new CombineLatest<TFirst, TSecond, TResult>._.S(this, (IDisposable) self2);
        observer1.Other = observer2;
        observer2.Other = observer1;
        self1.Disposable = this._parent._first.SubscribeSafe<TFirst>((IObserver<TFirst>) observer1);
        self2.Disposable = this._parent._second.SubscribeSafe<TSecond>((IObserver<TSecond>) observer2);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) self1,
          (IDisposable) self2
        });
      }

      private class F : IObserver<TFirst>
      {
        private readonly CombineLatest<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;
        private CombineLatest<TFirst, TSecond, TResult>._.S _other;

        public F(CombineLatest<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public CombineLatest<TFirst, TSecond, TResult>._.S Other
        {
          set => this._other = value;
        }

        public bool HasValue { get; private set; }

        public TFirst Value { get; private set; }

        public bool Done { get; private set; }

        public void OnNext(TFirst value)
        {
          lock (this._parent._gate)
          {
            this.HasValue = true;
            this.Value = value;
            if (this._other.HasValue)
            {
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(value, this._other.Value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              this._parent._observer.OnNext(result2);
            }
            else
            {
              if (!this._other.Done)
                return;
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
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
            this.Done = true;
            if (this._other.Done)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }
      }

      private class S : IObserver<TSecond>
      {
        private readonly CombineLatest<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;
        private CombineLatest<TFirst, TSecond, TResult>._.F _other;

        public S(CombineLatest<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public CombineLatest<TFirst, TSecond, TResult>._.F Other
        {
          set => this._other = value;
        }

        public bool HasValue { get; private set; }

        public TSecond Value { get; private set; }

        public bool Done { get; private set; }

        public void OnNext(TSecond value)
        {
          lock (this._parent._gate)
          {
            this.HasValue = true;
            this.Value = value;
            if (this._other.HasValue)
            {
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(this._other.Value, value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              this._parent._observer.OnNext(result2);
            }
            else
            {
              if (!this._other.Done)
                return;
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
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
            this.Done = true;
            if (this._other.Done)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }
      }
    }
  }
}
