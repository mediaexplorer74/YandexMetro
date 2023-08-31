// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SelectMany`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class SelectMany<TSource, TCollection, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, IObservable<TCollection>> _collectionSelector;
    private readonly Func<TSource, IEnumerable<TCollection>> _collectionSelectorE;
    private readonly Func<TSource, TCollection, TResult> _resultSelector;

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelector = collectionSelector;
      this._resultSelector = resultSelector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelectorE = collectionSelector;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._collectionSelector != null)
      {
        SelectMany<TSource, TCollection, TResult>._ obj = new SelectMany<TSource, TCollection, TResult>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      SelectMany<TSource, TCollection, TResult>.ε observer1 = new SelectMany<TSource, TCollection, TResult>.ε(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;

      public _(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._isStopped = false;
        this._group = new CompositeDisposable();
        this._sourceSubscription = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) this._sourceSubscription);
        this._sourceSubscription.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) this._group;
      }

      public void OnNext(TSource value)
      {
        IObservable<TCollection> source;
        try
        {
          source = this._parent._collectionSelector(value);
        }
        catch (Exception ex)
        {
          lock (this._gate)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = source.SubscribeSafe<TCollection>((IObserver<TCollection>) new SelectMany<TSource, TCollection, TResult>._.ι(this, value, (IDisposable) self));
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        this._isStopped = true;
        if (this._group.Count == 1)
        {
          lock (this._gate)
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
        }
        else
          this._sourceSubscription.Dispose();
      }

      private class ι : IObserver<TCollection>
      {
        private readonly SelectMany<TSource, TCollection, TResult>._ _parent;
        private readonly TSource _value;
        private readonly IDisposable _self;

        public ι(
          SelectMany<TSource, TCollection, TResult>._ parent,
          TSource value,
          IDisposable self)
        {
          this._parent = parent;
          this._value = value;
          this._self = self;
        }

        public void OnNext(TCollection value)
        {
          TResult result1 = default (TResult);
          TResult result2;
          try
          {
            result2 = this._parent._parent._resultSelector(this._value, value);
          }
          catch (Exception ex)
          {
            lock (this._parent._gate)
            {
              this._parent._observer.OnError(ex);
              this._parent.Dispose();
              return;
            }
          }
          lock (this._parent._gate)
            this._parent._observer.OnNext(result2);
        }

        public void OnError(Exception error)
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          this._parent._group.Remove(this._self);
          if (!this._parent._isStopped || this._parent._group.Count != 1)
            return;
          lock (this._parent._gate)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }
    }

    private class ε : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;

      public ε(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        IEnumerable<TCollection> collections;
        try
        {
          collections = this._parent._collectionSelectorE(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        IEnumerator<TCollection> enumerator;
        try
        {
          enumerator = collections.GetEnumerator();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        try
        {
          bool flag = true;
          while (flag)
          {
            TResult result = default (TResult);
            try
            {
              flag = enumerator.MoveNext();
              if (flag)
                result = this._parent._resultSelector(value, enumerator.Current);
            }
            catch (Exception ex)
            {
              this._observer.OnError(ex);
              this.Dispose();
              break;
            }
            if (flag)
              this._observer.OnNext(result);
          }
        }
        finally
        {
          enumerator?.Dispose();
        }
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
