// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Contains`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class Contains<TSource> : Producer<bool>
  {
    private readonly IObservable<TSource> _source;
    private readonly TSource _value;
    private readonly IEqualityComparer<TSource> _comparer;

    public Contains(
      IObservable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      this._source = source;
      this._value = value;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<bool> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Contains<TSource>._ observer1 = new Contains<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<bool>, IObserver<TSource>
    {
      private readonly Contains<TSource> _parent;

      public _(Contains<TSource> parent, IObserver<bool> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        bool flag;
        try
        {
          flag = this._parent._comparer.Equals(value, this._parent._value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (!flag)
          return;
        this._observer.OnNext(true);
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
        this._observer.OnNext(false);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
