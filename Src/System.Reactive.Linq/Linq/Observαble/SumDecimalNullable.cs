// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumDecimalNullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumDecimalNullable : Producer<Decimal?>
  {
    private readonly IObservable<Decimal?> _source;

    public SumDecimalNullable(IObservable<Decimal?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<Decimal?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumDecimalNullable._ observer1 = new SumDecimalNullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<Decimal?>((IObserver<Decimal?>) observer1);
    }

    private class _ : Sink<Decimal?>, IObserver<Decimal?>
    {
      private Decimal _sum;

      public _(IObserver<Decimal?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0M;
      }

      public void OnNext(Decimal? value)
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
        this._observer.OnNext(new Decimal?(this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
