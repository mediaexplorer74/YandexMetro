// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ToLookup`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq.Observαble
{
  internal class ToLookup<TSource, TKey, TElement> : Producer<ILookup<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public ToLookup(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._elementSelector = elementSelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<ILookup<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToLookup<TSource, TKey, TElement>._ observer1 = new ToLookup<TSource, TKey, TElement>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<ILookup<TKey, TElement>>, IObserver<TSource>
    {
      private readonly ToLookup<TSource, TKey, TElement> _parent;
      private System.Reactive.Lookup<TKey, TElement> _lookup;

      public _(
        ToLookup<TSource, TKey, TElement> parent,
        IObserver<ILookup<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._lookup = new System.Reactive.Lookup<TKey, TElement>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._lookup.Add(this._parent._keySelector(value), this._parent._elementSelector(value));
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
        this._observer.OnNext((ILookup<TKey, TElement>) this._lookup);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
