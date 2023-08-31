// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.DistinctUntilChanged`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class DistinctUntilChanged<TSource, TKey> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public DistinctUntilChanged(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DistinctUntilChanged<TSource, TKey>._ observer1 = new DistinctUntilChanged<TSource, TKey>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly DistinctUntilChanged<TSource, TKey> _parent;
      private TKey _currentKey;
      private bool _hasCurrentKey;

      public _(
        DistinctUntilChanged<TSource, TKey> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._currentKey = default (TKey);
        this._hasCurrentKey = false;
      }

      public void OnNext(TSource value)
      {
        TKey key = default (TKey);
        TKey y;
        try
        {
          y = this._parent._keySelector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        bool flag = false;
        if (this._hasCurrentKey)
        {
          try
          {
            flag = this._parent._comparer.Equals(this._currentKey, y);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        if (this._hasCurrentKey && flag)
          return;
        this._hasCurrentKey = true;
        this._currentKey = y;
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
