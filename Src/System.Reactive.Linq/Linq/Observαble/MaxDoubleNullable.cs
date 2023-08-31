// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.MaxDoubleNullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class MaxDoubleNullable : Producer<double?>
  {
    private readonly IObservable<double?> _source;

    public MaxDoubleNullable(IObservable<double?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MaxDoubleNullable._ observer1 = new MaxDoubleNullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<double?>((IObserver<double?>) observer1);
    }

    private class _ : Sink<double?>, IObserver<double?>
    {
      private double? _lastValue;

      public _(IObserver<double?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new double?();
      }

      public void OnNext(double? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          double? nullable = value;
          double? lastValue = this._lastValue;
          if ((nullable.GetValueOrDefault() <= lastValue.GetValueOrDefault() ? 0 : (nullable.HasValue & lastValue.HasValue ? 1 : 0)) == 0 && !double.IsNaN(value.Value))
            return;
          this._lastValue = value;
        }
        else
          this._lastValue = value;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._lastValue);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
