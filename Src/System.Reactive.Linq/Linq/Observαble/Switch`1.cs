// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Switch`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Switch<TSource> : Producer<TSource>
  {
    private readonly IObservable<IObservable<TSource>> _sources;

    public Switch(IObservable<IObservable<TSource>> sources) => this._sources = sources;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Switch<TSource>._ obj = new Switch<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<IObservable<TSource>>
    {
      private readonly Switch<TSource> _parent;
      private object _gate;
      private IDisposable _subscription;
      private SerialDisposable _innerSubscription;
      private bool _isStopped;
      private ulong _latest;
      private bool _hasLatest;

      public _(Switch<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._innerSubscription = new SerialDisposable();
        this._isStopped = false;
        this._latest = 0UL;
        this._hasLatest = false;
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._subscription = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._sources.SubscribeSafe<IObservable<TSource>>((IObserver<IObservable<TSource>>) this);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._subscription,
          (IDisposable) this._innerSubscription
        });
      }

      public void OnNext(IObservable<TSource> value)
      {
        ulong id = 0;
        lock (this._gate)
        {
          id = ++this._latest;
          this._hasLatest = true;
        }
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._innerSubscription.Disposable = (IDisposable) self;
        self.Disposable = value.SubscribeSafe<TSource>((IObserver<TSource>) new Switch<TSource>._.ι(this, id, (IDisposable) self));
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
          this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._subscription.Dispose();
          this._isStopped = true;
          if (this._hasLatest)
            return;
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private class ι : IObserver<TSource>
      {
        private readonly Switch<TSource>._ _parent;
        private readonly ulong _id;
        private readonly IDisposable _self;

        public ι(Switch<TSource>._ parent, ulong id, IDisposable self)
        {
          this._parent = parent;
          this._id = id;
          this._self = self;
        }

        public void OnNext(TSource value)
        {
          lock (this._parent._gate)
          {
            if ((long) this._parent._latest != (long) this._id)
              return;
            this._parent._observer.OnNext(value);
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent._gate)
          {
            this._self.Dispose();
            if ((long) this._parent._latest != (long) this._id)
              return;
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            this._self.Dispose();
            if ((long) this._parent._latest != (long) this._id)
              return;
            this._parent._hasLatest = false;
            if (!this._parent._isStopped)
              return;
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }
    }
  }
}
