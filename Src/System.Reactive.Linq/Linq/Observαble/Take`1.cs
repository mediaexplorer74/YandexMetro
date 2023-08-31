// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Take`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Take<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly TimeSpan _duration;
    internal readonly IScheduler _scheduler;

    public Take(IObservable<TSource> source, int count)
    {
      this._source = source;
      this._count = count;
    }

    public Take(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
    {
      this._source = source;
      this._duration = duration;
      this._scheduler = scheduler;
    }

    public IObservable<TSource> Ω(int count) => this._count <= count ? (IObservable<TSource>) this : (IObservable<TSource>) new Take<TSource>(this._source, count);

    public IObservable<TSource> Ω(TimeSpan duration) => this._duration <= duration ? (IObservable<TSource>) this : (IObservable<TSource>) new Take<TSource>(this._source, duration, this._scheduler);

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        Take<TSource>._ observer1 = new Take<TSource>._(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      Take<TSource>.τ τ = new Take<TSource>.τ(this, observer, cancel);
      setSink((IDisposable) τ);
      return τ.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Take<TSource> _parent;
      private int _remaining;

      public _(Take<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._remaining = this._parent._count;
      }

      public void OnNext(TSource value)
      {
        if (this._remaining <= 0)
          return;
        --this._remaining;
        this._observer.OnNext(value);
        if (this._remaining != 0)
          return;
        this._observer.OnCompleted();
        this.Dispose();
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class τ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Take<TSource> _parent;
      private object _gate;

      public τ(Take<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._parent._scheduler.Schedule(this._parent._duration, new Action(this.Tick)),
          this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this)
        });
      }

      private void Tick()
      {
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._observer.OnNext(value);
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
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
