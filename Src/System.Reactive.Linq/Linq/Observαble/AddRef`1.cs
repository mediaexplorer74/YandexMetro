// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.AddRef`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class AddRef<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly RefCountDisposable _refCount;

    public AddRef(IObservable<TSource> source, RefCountDisposable refCount)
    {
      this._source = source;
      this._refCount = refCount;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      CompositeDisposable cancel1 = new CompositeDisposable(new IDisposable[2]
      {
        this._refCount.GetDisposable(),
        cancel
      });
      AddRef<TSource>._ observer1 = new AddRef<TSource>._(observer, (IDisposable) cancel1);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      public _(IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
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
