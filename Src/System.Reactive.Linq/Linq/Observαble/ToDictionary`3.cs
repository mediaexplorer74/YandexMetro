// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ToDictionary`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class ToDictionary<TSource, TKey, TElement> : Producer<IDictionary<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public ToDictionary(
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
      IObserver<IDictionary<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToDictionary<TSource, TKey, TElement>._ observer1 = new ToDictionary<TSource, TKey, TElement>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<IDictionary<TKey, TElement>>, IObserver<TSource>
    {
      private readonly ToDictionary<TSource, TKey, TElement> _parent;
      private Dictionary<TKey, TElement> _dictionary;

      public _(
        ToDictionary<TSource, TKey, TElement> parent,
        IObserver<IDictionary<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._dictionary = new Dictionary<TKey, TElement>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._dictionary.Add(this._parent._keySelector(value), this._parent._elementSelector(value));
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
        this._observer.OnNext((IDictionary<TKey, TElement>) this._dictionary);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
