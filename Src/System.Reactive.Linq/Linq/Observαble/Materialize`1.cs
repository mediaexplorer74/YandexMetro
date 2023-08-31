// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Materialize`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Materialize<TSource> : Producer<Notification<TSource>>
  {
    private readonly IObservable<TSource> _source;

    public Materialize(IObservable<TSource> source) => this._source = source;

    public IObservable<TSource> Dematerialize() => this._source.AsObservable<TSource>();

    protected override IDisposable Run(
      IObserver<Notification<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Materialize<TSource>._ observer1 = new Materialize<TSource>._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<Notification<TSource>>, IObserver<TSource>
    {
      public _(IObserver<Notification<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public void OnNext(TSource value) => this._observer.OnNext(Notification.CreateOnNext<TSource>(value));

      public void OnError(Exception error)
      {
        this._observer.OnNext(Notification.CreateOnError<TSource>(error));
        this._observer.OnCompleted();
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(Notification.CreateOnCompleted<TSource>());
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
