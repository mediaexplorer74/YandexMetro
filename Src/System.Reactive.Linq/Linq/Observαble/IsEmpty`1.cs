// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.IsEmpty`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class IsEmpty<TSource> : Producer<bool>
  {
    private readonly IObservable<TSource> _source;

    public IsEmpty(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<bool> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      IsEmpty<TSource>._ observer1 = new IsEmpty<TSource>._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<bool>, IObserver<TSource>
    {
      public _(IObserver<bool> observer, IDisposable cancel)
        : base(observer, cancel)
      {
      }

      public void OnNext(TSource value)
      {
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
        this._observer.OnNext(true);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
