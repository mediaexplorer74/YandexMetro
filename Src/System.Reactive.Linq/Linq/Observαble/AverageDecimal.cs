// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.AverageDecimal
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class AverageDecimal : Producer<Decimal>
  {
    private readonly IObservable<Decimal> _source;

    public AverageDecimal(IObservable<Decimal> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<Decimal> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageDecimal._ observer1 = new AverageDecimal._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<Decimal>((IObserver<Decimal>) observer1);
    }

    private class _ : Sink<Decimal>, IObserver<Decimal>
    {
      private Decimal _sum;
      private long _count;

      public _(IObserver<Decimal> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0M;
        this._count = 0L;
      }

      public void OnNext(Decimal value)
      {
        try
        {
          this._sum += value;
          checked { ++this._count; }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (this._count > 0L)
        {
          this._observer.OnNext(this._sum / (Decimal) this._count);
          this._observer.OnCompleted();
        }
        else
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        this.Dispose();
      }
    }
  }
}
