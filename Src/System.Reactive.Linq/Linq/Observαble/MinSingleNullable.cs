// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.MinSingleNullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class MinSingleNullable : Producer<float?>
  {
    private readonly IObservable<float?> _source;

    public MinSingleNullable(IObservable<float?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinSingleNullable._ observer1 = new MinSingleNullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<float?>((IObserver<float?>) observer1);
    }

    private class _ : Sink<float?>, IObserver<float?>
    {
      private float? _lastValue;

      public _(IObserver<float?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new float?();
      }

      public void OnNext(float? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          float? nullable = value;
          float? lastValue = this._lastValue;
          if (((double) nullable.GetValueOrDefault() >= (double) lastValue.GetValueOrDefault() ? 0 : (nullable.HasValue & lastValue.HasValue ? 1 : 0)) == 0 && !float.IsNaN(value.Value))
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
