// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.MinDouble
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class MinDouble : Producer<double>
  {
    private readonly IObservable<double> _source;

    public MinDouble(IObservable<double> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinDouble._ observer1 = new MinDouble._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<double>((IObserver<double>) observer1);
    }

    private class _ : Sink<double>, IObserver<double>
    {
      private bool _hasValue;
      private double _lastValue;

      public _(IObserver<double> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._hasValue = false;
        this._lastValue = 0.0;
      }

      public void OnNext(double value)
      {
        if (this._hasValue)
        {
          if (value >= this._lastValue && !double.IsNaN(value))
            return;
          this._lastValue = value;
        }
        else
        {
          this._lastValue = value;
          this._hasValue = true;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._hasValue)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._lastValue);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
