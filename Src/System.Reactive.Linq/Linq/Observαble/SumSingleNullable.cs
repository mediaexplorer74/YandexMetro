// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumSingleNullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumSingleNullable : Producer<float?>
  {
    private readonly IObservable<float?> _source;

    public SumSingleNullable(IObservable<float?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumSingleNullable._ observer1 = new SumSingleNullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<float?>((IObserver<float?>) observer1);
    }

    private class _ : Sink<float?>, IObserver<float?>
    {
      private double _sum;

      public _(IObserver<float?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(float? value)
      {
        if (!value.HasValue)
          return;
        this._sum += (double) value.Value;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(new float?((float) this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
