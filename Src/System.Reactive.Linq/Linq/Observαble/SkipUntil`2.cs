// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SkipUntil`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class SkipUntil<TSource, TOther> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IObservable<TOther> _other;

    public SkipUntil(IObservable<TSource> source, IObservable<TOther> other)
    {
      this._source = source;
      this._other = other;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SkipUntil<TSource, TOther>._ obj = new SkipUntil<TSource, TOther>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>
    {
      private readonly SkipUntil<TSource, TOther> _parent;

      public _(SkipUntil<TSource, TOther> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        SkipUntil<TSource, TOther>._.T t = new SkipUntil<TSource, TOther>._.T(this);
        SkipUntil<TSource, TOther>._.O observer = new SkipUntil<TSource, TOther>._.O(this, t);
        IDisposable disposable1 = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) t);
        IDisposable disposable2 = this._parent._other.SubscribeSafe<TOther>((IObserver<TOther>) observer);
        t.Disposable = disposable1;
        observer.Disposable = disposable2;
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          disposable1,
          disposable2
        });
      }

      private class T : IObserver<TSource>
      {
        private readonly SkipUntil<TSource, TOther>._ _parent;
        public volatile IObserver<TSource> _observer;
        private readonly SingleAssignmentDisposable _subscription;

        public T(SkipUntil<TSource, TOther>._ parent)
        {
          this._parent = parent;
          this._observer = NopObserver<TSource>.Instance;
          this._subscription = new SingleAssignmentDisposable();
        }

        public IDisposable Disposable
        {
          set => this._subscription.Disposable = value;
        }

        public void OnNext(TSource value) => this._observer.OnNext(value);

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted()
        {
          this._observer.OnCompleted();
          this._subscription.Dispose();
        }
      }

      private class O : IObserver<TOther>
      {
        private readonly SkipUntil<TSource, TOther>._ _parent;
        private readonly SkipUntil<TSource, TOther>._.T _sourceObserver;
        private readonly SingleAssignmentDisposable _subscription;

        public O(SkipUntil<TSource, TOther>._ parent, SkipUntil<TSource, TOther>._.T sourceObserver)
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
          this._sourceObserver._observer = this._parent._observer;
          this._subscription.Dispose();
        }

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted() => this._subscription.Dispose();
      }
    }
  }
}
