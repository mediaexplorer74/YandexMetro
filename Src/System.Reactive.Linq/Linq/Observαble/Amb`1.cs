// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Amb`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Amb<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _left;
    private readonly IObservable<TSource> _right;

    public Amb(IObservable<TSource> left, IObservable<TSource> right)
    {
      this._left = left;
      this._right = right;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Amb<TSource>._ obj = new Amb<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>
    {
      private readonly Amb<TSource> _parent;
      private Amb<TSource>._.AmbState _choice;

      public _(Amb<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        SingleAssignmentDisposable assignmentDisposable1 = new SingleAssignmentDisposable();
        SingleAssignmentDisposable assignmentDisposable2 = new SingleAssignmentDisposable();
        CompositeDisposable compositeDisposable = new CompositeDisposable(new IDisposable[2]
        {
          (IDisposable) assignmentDisposable1,
          (IDisposable) assignmentDisposable2
        });
        object gate = new object();
        Amb<TSource>._.AmbObserver observer1 = new Amb<TSource>._.AmbObserver()
        {
          _disposable = (IDisposable) compositeDisposable
        };
        observer1._target = (IObserver<TSource>) new Amb<TSource>._.DecisionObserver(this, gate, Amb<TSource>._.AmbState.Left, (IDisposable) assignmentDisposable1, (IDisposable) assignmentDisposable2, observer1);
        Amb<TSource>._.AmbObserver observer2 = new Amb<TSource>._.AmbObserver()
        {
          _disposable = (IDisposable) compositeDisposable
        };
        observer2._target = (IObserver<TSource>) new Amb<TSource>._.DecisionObserver(this, gate, Amb<TSource>._.AmbState.Right, (IDisposable) assignmentDisposable2, (IDisposable) assignmentDisposable1, observer2);
        this._choice = Amb<TSource>._.AmbState.Neither;
        assignmentDisposable1.Disposable = this._parent._left.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
        assignmentDisposable2.Disposable = this._parent._right.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
        return (IDisposable) compositeDisposable;
      }

      private class DecisionObserver : IObserver<TSource>
      {
        private readonly Amb<TSource>._ _parent;
        private readonly Amb<TSource>._.AmbState _me;
        private readonly IDisposable _subscription;
        private readonly IDisposable _otherSubscription;
        private readonly object _gate;
        private readonly Amb<TSource>._.AmbObserver _observer;

        public DecisionObserver(
          Amb<TSource>._ parent,
          object gate,
          Amb<TSource>._.AmbState me,
          IDisposable subscription,
          IDisposable otherSubscription,
          Amb<TSource>._.AmbObserver observer)
        {
          this._parent = parent;
          this._gate = gate;
          this._me = me;
          this._subscription = subscription;
          this._otherSubscription = otherSubscription;
          this._observer = observer;
        }

        public void OnNext(TSource value)
        {
          lock (this._gate)
          {
            if (this._parent._choice == Amb<TSource>._.AmbState.Neither)
            {
              this._parent._choice = this._me;
              this._otherSubscription.Dispose();
              this._observer._disposable = this._subscription;
              this._observer._target = this._parent._observer;
            }
            if (this._parent._choice != this._me)
              return;
            this._parent._observer.OnNext(value);
          }
        }

        public void OnError(Exception error)
        {
          lock (this._gate)
          {
            if (this._parent._choice == Amb<TSource>._.AmbState.Neither)
            {
              this._parent._choice = this._me;
              this._otherSubscription.Dispose();
              this._observer._disposable = this._subscription;
              this._observer._target = this._parent._observer;
            }
            if (this._parent._choice != this._me)
              return;
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._gate)
          {
            if (this._parent._choice == Amb<TSource>._.AmbState.Neither)
            {
              this._parent._choice = this._me;
              this._otherSubscription.Dispose();
              this._observer._disposable = this._subscription;
              this._observer._target = this._parent._observer;
            }
            if (this._parent._choice != this._me)
              return;
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }

      private class AmbObserver : IObserver<TSource>
      {
        public IObserver<TSource> _target;
        public IDisposable _disposable;

        public void OnNext(TSource value) => this._target.OnNext(value);

        public void OnError(Exception error)
        {
          this._target.OnError(error);
          this._disposable.Dispose();
        }

        public void OnCompleted()
        {
          this._target.OnCompleted();
          this._disposable.Dispose();
        }
      }

      private enum AmbState
      {
        Left,
        Right,
        Neither,
      }
    }
  }
}
