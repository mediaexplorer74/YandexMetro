// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SelectMany`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class SelectMany<TSource, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, IObservable<TResult>> _selector;
    private readonly Func<Exception, IObservable<TResult>> _selectorOnError;
    private readonly Func<IObservable<TResult>> _selectorOnCompleted;
    private readonly Func<TSource, IEnumerable<TResult>> _selectorE;

    public SelectMany(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
    {
      this._source = source;
      this._selector = selector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector,
      Func<Exception, IObservable<TResult>> selectorOnError,
      Func<IObservable<TResult>> selectorOnCompleted)
    {
      this._source = source;
      this._selector = selector;
      this._selectorOnError = selectorOnError;
      this._selectorOnCompleted = selectorOnCompleted;
    }

    public SelectMany(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
      this._source = source;
      this._selectorE = selector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._selector != null)
      {
        SelectMany<TSource, TResult>._ obj = new SelectMany<TSource, TResult>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      SelectMany<TSource, TResult>.ε observer1 = new SelectMany<TSource, TResult>.ε(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TResult> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;

      public _(
        SelectMany<TSource, TResult> parent,
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
        IObservable<TResult> inner;
        try
        {
          inner = this._parent._selector(value);
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
        this.SubscribeInner(inner);
      }

      public void OnError(Exception error)
      {
        if (this._parent._selectorOnError != null)
        {
          IObservable<TResult> inner;
          try
          {
            inner = this._parent._selectorOnError(error);
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
          this.SubscribeInner(inner);
          this.Final();
        }
        else
        {
          lock (this._gate)
          {
            this._observer.OnError(error);
            this.Dispose();
          }
        }
      }

      public void OnCompleted()
      {
        if (this._parent._selectorOnCompleted != null)
        {
          IObservable<TResult> inner;
          try
          {
            inner = this._parent._selectorOnCompleted();
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
          this.SubscribeInner(inner);
        }
        this.Final();
      }

      private void Final()
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

      private void SubscribeInner(IObservable<TResult> inner)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = inner.SubscribeSafe<TResult>((IObserver<TResult>) new SelectMany<TSource, TResult>._.ι(this, (IDisposable) self));
      }

      private class ι : IObserver<TResult>
      {
        private readonly SelectMany<TSource, TResult>._ _parent;
        private readonly IDisposable _self;

        public ι(SelectMany<TSource, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TResult value)
        {
          lock (this._parent._gate)
            this._parent._observer.OnNext(value);
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
      private readonly SelectMany<TSource, TResult> _parent;

      public ε(
        SelectMany<TSource, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        IEnumerable<TResult> results;
        try
        {
          results = this._parent._selectorE(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        IEnumerator<TResult> enumerator;
        try
        {
          enumerator = results.GetEnumerator();
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
                result = enumerator.Current;
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
