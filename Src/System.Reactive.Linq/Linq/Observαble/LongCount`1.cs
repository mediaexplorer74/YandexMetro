// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.LongCount`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class LongCount<TSource> : Producer<long>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;

    public LongCount(IObservable<TSource> source) => this._source = source;

    public LongCount(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    protected override IDisposable Run(
      IObserver<long> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate == null)
      {
        LongCount<TSource>._ observer1 = new LongCount<TSource>._(observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      LongCount<TSource>.π observer2 = new LongCount<TSource>.π(this, observer, cancel);
      setSink((IDisposable) observer2);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
    }

    private class _ : Sink<long>, IObserver<TSource>
    {
      private long _count;

      public _(IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._count = 0L;
      }

      public void OnNext(TSource value)
      {
        try
        {
          checked { ++this._count; }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._count);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class π : Sink<long>, IObserver<TSource>
    {
      private readonly LongCount<TSource> _parent;
      private long _count;

      public π(LongCount<TSource> parent, IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._count = 0L;
      }

      public void OnNext(TSource value)
      {
        try
        {
          if (!this._parent._predicate(value))
            return;
          checked { ++this._count; }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._count);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
