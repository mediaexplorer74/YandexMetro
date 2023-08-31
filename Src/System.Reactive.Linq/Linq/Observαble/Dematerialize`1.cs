// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Dematerialize`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Dematerialize<TSource> : Producer<TSource>
  {
    private readonly IObservable<Notification<TSource>> _source;

    public Dematerialize(IObservable<Notification<TSource>> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Dematerialize<TSource>._ observer1 = new Dematerialize<TSource>._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<Notification<TSource>>((IObserver<Notification<TSource>>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<Notification<TSource>>
    {
      public _(IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public void OnNext(Notification<TSource> value)
      {
        switch (value.Kind)
        {
          case NotificationKind.OnNext:
            this._observer.OnNext(value.Value);
            break;
          case NotificationKind.OnError:
            this._observer.OnError(value.Exception);
            this.Dispose();
            break;
          case NotificationKind.OnCompleted:
            this._observer.OnCompleted();
            this.Dispose();
            break;
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
