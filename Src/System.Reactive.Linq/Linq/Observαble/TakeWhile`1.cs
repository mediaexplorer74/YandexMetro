// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.TakeWhile`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class TakeWhile<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly Func<TSource, int, bool> _predicateI;

    public TakeWhile(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    public TakeWhile(IObservable<TSource> source, Func<TSource, int, bool> predicate)
    {
      this._source = source;
      this._predicateI = predicate;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate != null)
      {
        TakeWhile<TSource>._ observer1 = new TakeWhile<TSource>._(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      TakeWhile<TSource>.τ observer2 = new TakeWhile<TSource>.τ(this, observer, cancel);
      setSink((IDisposable) observer2);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeWhile<TSource> _parent;
      private bool _running;

      public _(TakeWhile<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._running = true;
      }

      public void OnNext(TSource value)
      {
        if (!this._running)
          return;
        try
        {
          this._running = this._parent._predicate(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (this._running)
        {
          this._observer.OnNext(value);
        }
        else
        {
          this._observer.OnCompleted();
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
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class τ : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeWhile<TSource> _parent;
      private bool _running;
      private int _index;

      public τ(TakeWhile<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._running = true;
        this._index = 0;
      }

      public void OnNext(TSource value)
      {
        if (!this._running)
          return;
        try
        {
          this._running = this._parent._predicateI(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (this._running)
        {
          this._observer.OnNext(value);
        }
        else
        {
          this._observer.OnCompleted();
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
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
