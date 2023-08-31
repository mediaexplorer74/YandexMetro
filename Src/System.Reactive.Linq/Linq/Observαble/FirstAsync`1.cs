// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.FirstAsync`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class FirstAsync<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly bool _throwOnEmpty;

    public FirstAsync(
      IObservable<TSource> source,
      Func<TSource, bool> predicate,
      bool throwOnEmpty)
    {
      this._source = source;
      this._predicate = predicate;
      this._throwOnEmpty = throwOnEmpty;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate != null)
      {
        FirstAsync<TSource>.π observer1 = new FirstAsync<TSource>.π(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      FirstAsync<TSource>._ observer2 = new FirstAsync<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer2);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly FirstAsync<TSource> _parent;

      public _(FirstAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        this._observer.OnNext(value);
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
        if (this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(default (TSource));
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }

    private class π : Sink<TSource>, IObserver<TSource>
    {
      private readonly FirstAsync<TSource> _parent;

      public π(FirstAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        bool flag;
        try
        {
          flag = this._parent._predicate(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (!flag)
          return;
        this._observer.OnNext(value);
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
        if (this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(default (TSource));
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
