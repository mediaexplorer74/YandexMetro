// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SequenceEqual`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class SequenceEqual<TSource> : Producer<bool>
  {
    private readonly IObservable<TSource> _first;
    private readonly IObservable<TSource> _second;
    private readonly IEnumerable<TSource> _secondE;
    private readonly IEqualityComparer<TSource> _comparer;

    public SequenceEqual(
      IObservable<TSource> first,
      IObservable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      this._first = first;
      this._second = second;
      this._comparer = comparer;
    }

    public SequenceEqual(
      IObservable<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      this._first = first;
      this._secondE = second;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<bool> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._second != null)
      {
        SequenceEqual<TSource>._ obj = new SequenceEqual<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      SequenceEqual<TSource>.ε ε = new SequenceEqual<TSource>.ε(this, observer, cancel);
      setSink((IDisposable) ε);
      return ε.Run();
    }

    private class _ : Sink<bool>
    {
      private readonly SequenceEqual<TSource> _parent;
      private object _gate;
      private bool _donel;
      private bool _doner;
      private Queue<TSource> _ql;
      private Queue<TSource> _qr;

      public _(SequenceEqual<TSource> parent, IObserver<bool> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._donel = false;
        this._doner = false;
        this._ql = new Queue<TSource>();
        this._qr = new Queue<TSource>();
        return (IDisposable) new CompositeDisposable()
        {
          this._parent._first.SubscribeSafe<TSource>((IObserver<TSource>) new SequenceEqual<TSource>._.F(this)),
          this._parent._second.SubscribeSafe<TSource>((IObserver<TSource>) new SequenceEqual<TSource>._.S(this))
        };
      }

      private class F : IObserver<TSource>
      {
        private readonly SequenceEqual<TSource>._ _parent;

        public F(SequenceEqual<TSource>._ parent) => this._parent = parent;

        public void OnNext(TSource value)
        {
          lock (this._parent._gate)
          {
            if (this._parent._qr.Count > 0)
            {
              TSource y = this._parent._qr.Dequeue();
              bool flag;
              try
              {
                flag = this._parent._parent._comparer.Equals(value, y);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              if (flag)
                return;
              this._parent._observer.OnNext(false);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else if (this._parent._doner)
            {
              this._parent._observer.OnNext(false);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._parent._ql.Enqueue(value);
          }
        }

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            this._parent._donel = true;
            if (this._parent._ql.Count != 0)
              return;
            if (this._parent._qr.Count > 0)
            {
              this._parent._observer.OnNext(false);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
            {
              if (!this._parent._doner)
                return;
              this._parent._observer.OnNext(true);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
          }
        }
      }

      private class S : IObserver<TSource>
      {
        private readonly SequenceEqual<TSource>._ _parent;

        public S(SequenceEqual<TSource>._ parent) => this._parent = parent;

        public void OnNext(TSource value)
        {
          lock (this._parent._gate)
          {
            if (this._parent._ql.Count > 0)
            {
              TSource x = this._parent._ql.Dequeue();
              bool flag;
              try
              {
                flag = this._parent._parent._comparer.Equals(x, value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              if (flag)
                return;
              this._parent._observer.OnNext(false);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else if (this._parent._donel)
            {
              this._parent._observer.OnNext(false);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._parent._qr.Enqueue(value);
          }
        }

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            this._parent._doner = true;
            if (this._parent._qr.Count != 0)
              return;
            if (this._parent._ql.Count > 0)
            {
              this._parent._observer.OnNext(false);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
            {
              if (!this._parent._donel)
                return;
              this._parent._observer.OnNext(true);
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
          }
        }
      }
    }

    private class ε : Sink<bool>, IObserver<TSource>
    {
      private readonly SequenceEqual<TSource> _parent;
      private IEnumerator<TSource> _enumerator;

      public ε(SequenceEqual<TSource> parent, IObserver<bool> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        try
        {
          this._enumerator = this._parent._secondE.GetEnumerator();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          this._parent._first.SubscribeSafe<TSource>((IObserver<TSource>) this),
          (IDisposable) this._enumerator
        });
      }

      public void OnNext(TSource value)
      {
        bool flag = false;
        try
        {
          if (this._enumerator.MoveNext())
          {
            TSource current = this._enumerator.Current;
            flag = this._parent._comparer.Equals(value, current);
          }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (flag)
          return;
        this._observer.OnNext(false);
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
        bool flag;
        try
        {
          flag = this._enumerator.MoveNext();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(!flag);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
