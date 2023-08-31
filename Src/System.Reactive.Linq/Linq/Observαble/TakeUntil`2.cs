// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.TakeUntil`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class TakeUntil<TSource, TOther> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IObservable<TOther> _other;

    public TakeUntil(IObservable<TSource> source, IObservable<TOther> other)
    {
      this._source = source;
      this._other = other;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      TakeUntil<TSource, TOther>._ obj = new TakeUntil<TSource, TOther>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>
    {
      private readonly TakeUntil<TSource, TOther> _parent;

      public _(TakeUntil<TSource, TOther> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        TakeUntil<TSource, TOther>._.T t = new TakeUntil<TSource, TOther>._.T(this);
        TakeUntil<TSource, TOther>._.O observer = new TakeUntil<TSource, TOther>._.O(this, t);
        IDisposable disposable1 = this._parent._other.SubscribeSafe<TOther>((IObserver<TOther>) observer);
        observer.Disposable = disposable1;
        IDisposable disposable2 = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) t);
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          disposable1,
          disposable2
        });
      }

      private class T : IObserver<TSource>
      {
        private readonly TakeUntil<TSource, TOther>._ _parent;
        public volatile bool _open;

        public T(TakeUntil<TSource, TOther>._ parent)
        {
          this._parent = parent;
          this._open = false;
        }

        public void OnNext(TSource value)
        {
          if (this._open)
          {
            this._parent._observer.OnNext(value);
          }
          else
          {
            lock (this._parent)
              this._parent._observer.OnNext(value);
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }

      private class O : IObserver<TOther>
      {
        private readonly TakeUntil<TSource, TOther>._ _parent;
        private readonly TakeUntil<TSource, TOther>._.T _sourceObserver;
        private readonly SingleAssignmentDisposable _subscription;

        public O(TakeUntil<TSource, TOther>._ parent, TakeUntil<TSource, TOther>._.T sourceObserver)
        {
          this._parent = parent;
          this._sourceObserver = sourceObserver;
          this._subscription = new SingleAssignmentDisposable();
        }

        public IDisposable Disposable
        {
          set => this._subscription.Disposable = value;
        }

        public void OnNext(TOther value)
        {
          lock (this._parent)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent)
          {
            this._sourceObserver._open = true;
            this._subscription.Dispose();
          }
        }
      }
    }
  }
}
