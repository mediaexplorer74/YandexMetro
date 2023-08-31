// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Merge`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Merge<TSource> : Producer<TSource>
  {
    private readonly IObservable<IObservable<TSource>> _sources;
    private readonly int _maxConcurrent;

    public Merge(IObservable<IObservable<TSource>> sources) => this._sources = sources;

    public Merge(IObservable<IObservable<TSource>> sources, int maxConcurrent)
    {
      this._sources = sources;
      this._maxConcurrent = maxConcurrent;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._maxConcurrent > 0)
      {
        Merge<TSource>.μ μ = new Merge<TSource>.μ(this, observer, cancel);
        setSink((IDisposable) μ);
        return μ.Run();
      }
      Merge<TSource>._ obj = new Merge<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<IObservable<TSource>>
    {
      private readonly Merge<TSource> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;

      public _(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._isStopped = false;
        this._group = new CompositeDisposable();
        this._sourceSubscription = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) this._sourceSubscription);
        this._sourceSubscription.Disposable = this._parent._sources.SubscribeSafe<IObservable<TSource>>((IObserver<IObservable<TSource>>) this);
        return (IDisposable) this._group;
      }

      public void OnNext(IObservable<TSource> value)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = value.SubscribeSafe<TSource>((IObserver<TSource>) new Merge<TSource>._.ι(this, (IDisposable) self));
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
        this._isStopped = true;
        if (this._group.Count == 1)
        {
          lock (this._gate)
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
        }
        else
          this._sourceSubscription.Dispose();
      }

      private class ι : IObserver<TSource>
      {
        private readonly Merge<TSource>._ _parent;
        private readonly IDisposable _self;

        public ι(Merge<TSource>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TSource value)
        {
          lock (this._parent._gate)
            this._parent._observer.OnNext(value);
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
          this._parent._group.Remove(this._self);
          if (!this._parent._isStopped || this._parent._group.Count != 1)
            return;
          lock (this._parent._gate)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }
    }

    private class μ : Sink<TSource>, IObserver<IObservable<TSource>>
    {
      private readonly Merge<TSource> _parent;
      private object _gate;
      private Queue<IObservable<TSource>> _q;
      private bool _isStopped;
      private SingleAssignmentDisposable _sourceSubscription;
      private CompositeDisposable _group;
      private int _activeCount;

      public μ(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._q = new Queue<IObservable<TSource>>();
        this._isStopped = false;
        this._activeCount = 0;
        this._group = new CompositeDisposable();
        this._sourceSubscription = new SingleAssignmentDisposable();
        this._sourceSubscription.Disposable = this._parent._sources.SubscribeSafe<IObservable<TSource>>((IObserver<IObservable<TSource>>) this);
        this._group.Add((IDisposable) this._sourceSubscription);
        return (IDisposable) this._group;
      }

      public void OnNext(IObservable<TSource> value)
      {
        lock (this._gate)
        {
          if (this._activeCount < this._parent._maxConcurrent)
          {
            ++this._activeCount;
            this.Subscribe(value);
          }
          else
            this._q.Enqueue(value);
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
          this._isStopped = true;
          if (this._activeCount == 0)
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
          else
            this._sourceSubscription.Dispose();
        }
      }

      private void Subscribe(IObservable<TSource> innerSource)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = innerSource.SubscribeSafe<TSource>((IObserver<TSource>) new Merge<TSource>.μ.ι(this, (IDisposable) self));
      }

      private class ι : IObserver<TSource>
      {
        private readonly Merge<TSource>.μ _parent;
        private readonly IDisposable _self;

        public ι(Merge<TSource>.μ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TSource value)
        {
          lock (this._parent._gate)
            this._parent._observer.OnNext(value);
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
          this._parent._group.Remove(this._self);
          lock (this._parent._gate)
          {
            if (this._parent._q.Count > 0)
            {
              this._parent.Subscribe(this._parent._q.Dequeue());
            }
            else
            {
              --this._parent._activeCount;
              if (!this._parent._isStopped || this._parent._activeCount != 0)
                return;
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
          }
        }
      }
    }
  }
}
