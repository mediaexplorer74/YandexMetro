// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.GroupBy`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
  internal class GroupBy<TSource, TKey, TElement> : Producer<IGroupedObservable<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly IEqualityComparer<TKey> _comparer;
    private CompositeDisposable _groupDisposable;
    private RefCountDisposable _refCountDisposable;

    public GroupBy(
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
      IObserver<IGroupedObservable<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      this._groupDisposable = new CompositeDisposable();
      this._refCountDisposable = new RefCountDisposable((IDisposable) this._groupDisposable);
      GroupBy<TSource, TKey, TElement>._ observer1 = new GroupBy<TSource, TKey, TElement>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      this._groupDisposable.Add(this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1));
      return (IDisposable) this._refCountDisposable;
    }

    private class _ : Sink<IGroupedObservable<TKey, TElement>>, IObserver<TSource>
    {
      private readonly GroupBy<TSource, TKey, TElement> _parent;
      private readonly Dictionary<TKey, ISubject<TElement>> _map;
      private ISubject<TElement> _null;

      public _(
        GroupBy<TSource, TKey, TElement> parent,
        IObserver<IGroupedObservable<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._map = new Dictionary<TKey, ISubject<TElement>>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        TKey key1 = default (TKey);
        TKey key2;
        try
        {
          key2 = this._parent._keySelector(value);
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        bool flag = false;
        ISubject<TElement> subject = (ISubject<TElement>) null;
        try
        {
          if ((object) key2 == null)
          {
            if (this._null == null)
            {
              this._null = (ISubject<TElement>) new Subject<TElement>();
              flag = true;
            }
            subject = this._null;
          }
          else if (!this._map.TryGetValue(key2, out subject))
          {
            subject = (ISubject<TElement>) new Subject<TElement>();
            this._map.Add(key2, subject);
            flag = true;
          }
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        if (flag)
          this._observer.OnNext((IGroupedObservable<TKey, TElement>) new GroupedObservable<TKey, TElement>(key2, subject, this._parent._refCountDisposable));
        TElement element1 = default (TElement);
        TElement element2;
        try
        {
          element2 = this._parent._elementSelector(value);
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        ((IObserver<TElement>) subject).OnNext(element2);
      }

      public void OnError(Exception error) => this.Error(error);

      public void OnCompleted()
      {
        if (this._null != null)
          ((IObserver<TElement>) this._null).OnCompleted();
        foreach (IObserver<TElement> iobserver in this._map.Values)
          iobserver.OnCompleted();
        this._observer.OnCompleted();
        this.Dispose();
      }

      private void Error(Exception exception)
      {
        if (this._null != null)
          ((IObserver<TElement>) this._null).OnError(exception);
        foreach (IObserver<TElement> iobserver in this._map.Values)
          iobserver.OnError(exception);
        this._observer.OnError(exception);
        this.Dispose();
      }
    }
  }
}
