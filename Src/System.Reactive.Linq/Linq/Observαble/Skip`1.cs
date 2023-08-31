// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Skip`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Skip<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly TimeSpan _duration;
    internal readonly IScheduler _scheduler;

    public Skip(IObservable<TSource> source, int count)
    {
      this._source = source;
      this._count = count;
    }

    public Skip(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
    {
      this._source = source;
      this._duration = duration;
      this._scheduler = scheduler;
    }

    public IObservable<TSource> Ω(int count) => (IObservable<TSource>) new Skip<TSource>(this._source, this._count + count);

    public IObservable<TSource> Ω(TimeSpan duration) => duration <= this._duration ? (IObservable<TSource>) this : (IObservable<TSource>) new Skip<TSource>(this._source, duration, this._scheduler);

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        Skip<TSource>._ observer1 = new Skip<TSource>._(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      Skip<TSource>.τ τ = new Skip<TSource>.τ(this, observer, cancel);
      setSink((IDisposable) τ);
      return τ.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Skip<TSource> _parent;
      private int _remaining;

      public _(Skip<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._remaining = this._parent._count;
      }

      public void OnNext(TSource value)
      {
        if (this._remaining <= 0)
          this._observer.OnNext(value);
        else
          --this._remaining;
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
      private readonly Skip<TSource> _parent;
      private volatile bool _open;

      public τ(Skip<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run() => (IDisposable) new CompositeDisposable(new IDisposable[2]
      {
        this._parent._scheduler.Schedule(this._parent._duration, new Action(this.Tick)),
        this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this)
      });

      private void Tick() => this._open = true;

      public void OnNext(TSource value)
      {
        if (!this._open)
          return;
        this._observer.OnNext(value);
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
  }
}
