// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Join`5
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> : Producer<TResult>
  {
    private readonly IObservable<TLeft> _left;
    private readonly IObservable<TRight> _right;
    private readonly Func<TLeft, IObservable<TLeftDuration>> _leftDurationSelector;
    private readonly Func<TRight, IObservable<TRightDuration>> _rightDurationSelector;
    private readonly Func<TLeft, TRight, TResult> _resultSelector;

    public Join(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      this._left = left;
      this._right = right;
      this._leftDurationSelector = leftDurationSelector;
      this._rightDurationSelector = rightDurationSelector;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ obj = new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> _parent;
      private object _gate;
      private CompositeDisposable _group;
      private bool _leftDone;
      private int _leftID;
      private Dictionary<int, TLeft> _leftMap;
      private bool _rightDone;
      private int _rightID;
      private Dictionary<int, TRight> _rightMap;

      public _(
        Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._group = new CompositeDisposable();
        SingleAssignmentDisposable self1 = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self1);
        this._leftDone = false;
        this._leftID = 0;
        this._leftMap = new Dictionary<int, TLeft>();
        SingleAssignmentDisposable self2 = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self2);
        this._rightDone = false;
        this._rightID = 0;
        this._rightMap = new Dictionary<int, TRight>();
        self1.Disposable = this._parent._left.SubscribeSafe<TLeft>((IObserver<TLeft>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.λ(this, (IDisposable) self1));
        self2.Disposable = this._parent._right.SubscribeSafe<TRight>((IObserver<TRight>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.ρ(this, (IDisposable) self2));
        return (IDisposable) this._group;
      }

      private class λ : IObserver<TLeft>
      {
        private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ _parent;
        private readonly IDisposable _self;

        public λ(
          Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ parent,
          IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        private void Expire(int id, IDisposable resource)
        {
          lock (this._parent._gate)
          {
            if (this._parent._leftMap.Remove(id))
            {
              if (this._parent._leftMap.Count == 0)
              {
                if (this._parent._leftDone)
                {
                  this._parent._observer.OnCompleted();
                  this._parent.Dispose();
                }
              }
            }
          }
          this._parent._group.Remove(resource);
        }

        public void OnNext(TLeft value)
        {
          int num = 0;
          lock (this._parent._gate)
          {
            num = this._parent._leftID++;
            this._parent._leftMap.Add(num, value);
          }
          SingleAssignmentDisposable self = new SingleAssignmentDisposable();
          this._parent._group.Add((IDisposable) self);
          IObservable<TLeftDuration> source;
          try
          {
            source = this._parent._parent._leftDurationSelector(value);
          }
          catch (Exception ex)
          {
            this._parent._observer.OnError(ex);
            this._parent.Dispose();
            return;
          }
          self.Disposable = source.SubscribeSafe<TLeftDuration>((IObserver<TLeftDuration>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.λ.δ(this, num, (IDisposable) self));
          lock (this._parent._gate)
          {
            foreach (TRight right in this._parent._rightMap.Values)
            {
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(value, right);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                break;
              }
              this._parent._observer.OnNext(result2);
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
            this._parent._leftDone = true;
            if (this._parent._rightDone || this._parent._leftMap.Count == 0)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }

        private class δ : IObserver<TLeftDuration>
        {
          private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.λ _parent;
          private readonly int _id;
          private readonly IDisposable _self;

          public δ(
            Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.λ parent,
            int id,
            IDisposable self)
          {
            this._parent = parent;
            this._id = id;
            this._self = self;
          }

          public void OnNext(TLeftDuration value) => this._parent.Expire(this._id, this._self);

          public void OnError(Exception error) => this._parent.OnError(error);

          public void OnCompleted() => this._parent.Expire(this._id, this._self);
        }
      }

      private class ρ : IObserver<TRight>
      {
        private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ _parent;
        private readonly IDisposable _self;

        public ρ(
          Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ parent,
          IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        private void Expire(int id, IDisposable resource)
        {
          lock (this._parent._gate)
          {
            if (this._parent._rightMap.Remove(id))
            {
              if (this._parent._rightMap.Count == 0)
              {
                if (this._parent._rightDone)
                {
                  this._parent._observer.OnCompleted();
                  this._parent.Dispose();
                }
              }
            }
          }
          this._parent._group.Remove(resource);
        }

        public void OnNext(TRight value)
        {
          int num = 0;
          lock (this._parent._gate)
          {
            num = this._parent._rightID++;
            this._parent._rightMap.Add(num, value);
          }
          SingleAssignmentDisposable self = new SingleAssignmentDisposable();
          this._parent._group.Add((IDisposable) self);
          IObservable<TRightDuration> source;
          try
          {
            source = this._parent._parent._rightDurationSelector(value);
          }
          catch (Exception ex)
          {
            this._parent._observer.OnError(ex);
            this._parent.Dispose();
            return;
          }
          self.Disposable = source.SubscribeSafe<TRightDuration>((IObserver<TRightDuration>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.ρ.δ(this, num, (IDisposable) self));
          lock (this._parent._gate)
          {
            foreach (TLeft left in this._parent._leftMap.Values)
            {
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(left, value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                break;
              }
              this._parent._observer.OnNext(result2);
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
            this._parent._rightDone = true;
            if (this._parent._leftDone || this._parent._rightMap.Count == 0)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }

        private class δ : IObserver<TRightDuration>
        {
          private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.ρ _parent;
          private readonly int _id;
          private readonly IDisposable _self;

          public δ(
            Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.ρ parent,
            int id,
            IDisposable self)
          {
            this._parent = parent;
            this._id = id;
            this._self = self;
          }

          public void OnNext(TRightDuration value) => this._parent.Expire(this._id, this._self);

          public void OnError(Exception error) => this._parent.OnError(error);

          public void OnCompleted() => this._parent.Expire(this._id, this._self);
        }
      }
    }
  }
}
