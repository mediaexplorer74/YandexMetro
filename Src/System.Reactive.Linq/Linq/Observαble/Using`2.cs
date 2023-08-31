// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Using`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Using<TSource, TResource> : Producer<TSource> where TResource : IDisposable
  {
    private readonly Func<TResource> _resourceFactory;
    private readonly Func<TResource, IObservable<TSource>> _observableFactory;

    public Using(
      Func<TResource> resourceFactory,
      Func<TResource, IObservable<TSource>> observableFactory)
    {
      this._resourceFactory = resourceFactory;
      this._observableFactory = observableFactory;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Using<TSource, TResource>._ obj = new Using<TSource, TResource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Using<TSource, TResource> _parent;

      public _(Using<TSource, TResource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable disposable = Disposable.Empty;
        IObservable<TSource> source;
        try
        {
          TResource resource = this._parent._resourceFactory();
          if ((object) resource != null)
            disposable = (IDisposable) resource;
          source = this._parent._observableFactory(resource);
        }
        catch (Exception ex)
        {
          return (IDisposable) new CompositeDisposable(new IDisposable[2]
          {
            Observable.Throw<TSource>(ex).SubscribeSafe<TSource>((IObserver<TSource>) this),
            disposable
          });
        }
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          source.SubscribeSafe<TSource>((IObserver<TSource>) this),
          disposable
        });
      }

      public void OnNext(TSource value) => this._observer.OnNext(value);

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
