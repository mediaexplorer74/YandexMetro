// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.GroupByUntil`4
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
  internal class GroupByUntil<TSource, TKey, TElement, TDuration> : 
    Producer<IGroupedObservable<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> _durationSelector;
    private readonly IEqualityComparer<TKey> _comparer;
    private CompositeDisposable _groupDisposable;
    private RefCountDisposable _refCountDisposable;

    public GroupByUntil(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._elementSelector = elementSelector;
      this._durationSelector = durationSelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<IGroupedObservable<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      this._groupDisposable = new CompositeDisposable();
      this._refCountDisposable = new RefCountDisposable((IDisposable) this._groupDisposable);
      GroupByUntil<TSource, TKey, TElement, TDuration>._ observer1 = new GroupByUntil<TSource, TKey, TElement, TDuration>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      this._groupDisposable.Add(this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1));
      return (IDisposable) this._refCountDisposable;
    }

    private class _ : Sink<IGroupedObservable<TKey, TElement>>, IObserver<TSource>
    {
      private readonly GroupByUntil<TSource, TKey, TElement, TDuration> _parent;
      private readonly Map<TKey, ISubject<TElement>> _map;
      private ISubject<TElement> _null;
      private object _nullGate;

      public _(
        GroupByUntil<TSource, TKey, TElement, TDuration> parent,
        IObserver<IGroupedObservable<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._map = new Map<TKey, ISubject<TElement>>(this._parent._comparer);
        this._nullGate = new object();
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
        bool added = false;
        ISubject<TElement> subject = (ISubject<TElement>) null;
        try
        {
          if ((object) key2 == null)
          {
            lock (this._nullGate)
            {
              if (this._null == null)
              {
                this._null = (ISubject<TElement>) new Subject<TElement>();
                added = true;
              }
              subject = this._null;
            }
          }
          else
            subject = this._map.GetOrAdd(key2, (Func<ISubject<TElement>>) (() => (ISubject<TElement>) new Subject<TElement>()), out added);
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        if (added)
        {
          GroupedObservable<TKey, TElement> groupedObservable1 = new GroupedObservable<TKey, TElement>(key2, subject, this._parent._refCountDisposable);
          GroupedObservable<TKey, TElement> groupedObservable2 = new GroupedObservable<TKey, TElement>(key2, subject);
          IObservable<TDuration> source;
          try
          {
            source = this._parent._durationSelector((IGroupedObservable<TKey, TElement>) groupedObservable2);
          }
          catch (Exception ex)
          {
            this.Error(ex);
            return;
          }
          lock (this._observer)
            this._observer.OnNext((IGroupedObservable<TKey, TElement>) groupedObservable1);
          SingleAssignmentDisposable self = new SingleAssignmentDisposable();
          this._parent._groupDisposable.Add((IDisposable) self);
          self.Disposable = source.SubscribeSafe<TDuration>((IObserver<TDuration>) new GroupByUntil<TSource, TKey, TElement, TDuration>._.δ(this, key2, subject, (IDisposable) self));
        }
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
        lock (subject)
          ((IObserver<TElement>) subject).OnNext(element2);
      }

      public void OnError(Exception error) => this.Error(error);

      public void OnCompleted()
      {
        ISubject<TElement> subject = (ISubject<TElement>) null;
        lock (this._nullGate)
          subject = this._null;
        ((IObserver<TElement>) subject)?.OnCompleted();
        foreach (IObserver<TElement> iobserver in this._map.Values)
          iobserver.OnCompleted();
        lock (this._observer)
          this._observer.OnCompleted();
        this.Dispose();
      }

      private void Error(Exception exception)
      {
        ISubject<TElement> subject = (ISubject<TElement>) null;
        lock (this._nullGate)
          subject = this._null;
        ((IObserver<TElement>) subject)?.OnError(exception);
        foreach (IObserver<TElement> iobserver in this._map.Values)
          iobserver.OnError(exception);
        lock (this._observer)
          this._observer.OnError(exception);
        this.Dispose();
      }

      private class δ : IObserver<TDuration>
      {
        private readonly GroupByUntil<TSource, TKey, TElement, TDuration>._ _parent;
        private readonly TKey _key;
        private readonly ISubject<TElement> _writer;
        private readonly IDisposable _self;

        public δ(
          GroupByUntil<TSource, TKey, TElement, TDuration>._ parent,
          TKey key,
          ISubject<TElement> writer,
          IDisposable self)
        {
          this._parent = parent;
          this._key = key;
          this._writer = writer;
          this._self = self;
        }

        public void OnNext(TDuration value) => this.OnCompleted();

        public void OnError(Exception error)
        {
          this._parent.Error(error);
          this._self.Dispose();
        }

        public void OnCompleted()
        {
          if ((object) this._key == null)
          {
            ISubject<TElement> subject = (ISubject<TElement>) null;
            lock (this._parent._nullGate)
            {
              subject = this._parent._null;
              this._parent._null = (ISubject<TElement>) null;
            }
            lock (subject)
              ((IObserver<TElement>) subject).OnCompleted();
          }
          else if (this._parent._map.Remove(this._key))
          {
            lock (this._writer)
              ((IObserver<TElement>) this._writer).OnCompleted();
          }
          this._parent._parent._groupDisposable.Remove(this._self);
        }
      }
    }
  }
}
