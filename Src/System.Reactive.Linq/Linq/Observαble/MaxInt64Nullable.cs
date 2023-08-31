// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.MaxInt64Nullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class MaxInt64Nullable : Producer<long?>
  {
    private readonly IObservable<long?> _source;

    public MaxInt64Nullable(IObservable<long?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<long?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MaxInt64Nullable._ observer1 = new MaxInt64Nullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<long?>((IObserver<long?>) observer1);
    }

    private class _ : Sink<long?>, IObserver<long?>
    {
      private long? _lastValue;

      public _(IObserver<long?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new long?();
      }

      public void OnNext(long? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          long? nullable = value;
          long? lastValue = this._lastValue;
          if ((nullable.GetValueOrDefault() <= lastValue.GetValueOrDefault() ? 0 : (nullable.HasValue & lastValue.HasValue ? 1 : 0)) == 0)
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
