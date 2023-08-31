// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.LastAsync`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class LastAsync<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly bool _throwOnEmpty;

    public LastAsync(IObservable<TSource> source, Func<TSource, bool> predicate, bool throwOnEmpty)
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
        LastAsync<TSource>.π observer1 = new LastAsync<TSource>.π(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      LastAsync<TSource>._ observer2 = new LastAsync<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer2);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly LastAsync<TSource> _parent;
      private TSource _value;
      private bool _seenValue;

      public _(LastAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._value = default (TSource);
        this._seenValue = false;
      }

      public void OnNext(TSource value)
      {
        this._value = value;
        this._seenValue = true;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._seenValue && this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._value);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }

    private class π : Sink<TSource>, IObserver<TSource>
    {
      private readonly LastAsync<TSource> _parent;
      private TSource _value;
      private bool _seenValue;

      public π(LastAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._value = default (TSource);
        this._seenValue = false;
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
        this._value = value;
        this._seenValue = true;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._seenValue && this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._value);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
