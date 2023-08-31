// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumDoubleNullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumDoubleNullable : Producer<double?>
  {
    private readonly IObservable<double?> _source;

    public SumDoubleNullable(IObservable<double?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumDoubleNullable._ observer1 = new SumDoubleNullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<double?>((IObserver<double?>) observer1);
    }

    private class _ : Sink<double?>, IObserver<double?>
    {
      private double _sum;

      public _(IObserver<double?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(double? value)
      {
        if (!value.HasValue)
          return;
        this._sum += value.Value;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(new double?(this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
