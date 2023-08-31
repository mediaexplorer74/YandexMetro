// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.MinBy`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class MinBy<TSource, TKey> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly IComparer<TKey> _comparer;

    public MinBy(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinBy<TSource, TKey>._ observer1 = new MinBy<TSource, TKey>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly MinBy<TSource, TKey> _parent;
      private bool _hasValue;
      private TKey _lastKey;
      private List<TSource> _list;

      public _(MinBy<TSource, TKey> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._hasValue = false;
        this._lastKey = default (TKey);
        this._list = new List<TSource>();
      }

      public void OnNext(TSource value)
      {
        TKey key = default (TKey);
        TKey x;
        try
        {
          x = this._parent._keySelector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        int num = 0;
        if (!this._hasValue)
        {
          this._hasValue = true;
          this._lastKey = x;
        }
        else
        {
          try
          {
            num = this._parent._comparer.Compare(x, this._lastKey);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        if (num < 0)
        {
          this._lastKey = x;
          this._list.Clear();
        }
        if (num > 0)
          return;
        this._list.Add(value);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext((IList<TSource>) this._list);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
