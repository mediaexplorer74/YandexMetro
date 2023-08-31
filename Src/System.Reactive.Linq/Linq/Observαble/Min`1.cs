// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Min`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class Min<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IComparer<TSource> _comparer;

    public Min(IObservable<TSource> source, IComparer<TSource> comparer)
    {
      this._source = source;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if ((object) default (TSource) == null)
      {
        Min<TSource>._ observer1 = new Min<TSource>._(this, observer, cancel);
        setSink((IDisposable) observer1);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
      }
      Min<TSource>.δ observer2 = new Min<TSource>.δ(this, observer, cancel);
      setSink((IDisposable) observer2);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer2);
    }

    private class δ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Min<TSource> _parent;
      private bool _hasValue;
      private TSource _lastValue;

      public δ(Min<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._hasValue = false;
        this._lastValue = default (TSource);
      }

      public void OnNext(TSource value)
      {
        if (this._hasValue)
        {
          int num;
          try
          {
            num = this._parent._comparer.Compare(value, this._lastValue);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          if (num >= 0)
            return;
          this._lastValue = value;
        }
        else
        {
          this._hasValue = true;
          this._lastValue = value;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._hasValue)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._lastValue);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Min<TSource> _parent;
      private TSource _lastValue;

      public _(Min<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._lastValue = default (TSource);
      }

      public void OnNext(TSource value)
      {
        if ((object) value == null)
          return;
        if ((object) this._lastValue == null)
        {
          this._lastValue = value;
        }
        else
        {
          int num;
          try
          {
            num = this._parent._comparer.Compare(value, this._lastValue);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          if (num >= 0)
            return;
          this._lastValue = value;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._lastValue);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
