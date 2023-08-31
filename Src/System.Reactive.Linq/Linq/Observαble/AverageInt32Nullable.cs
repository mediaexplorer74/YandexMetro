// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.AverageInt32Nullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class AverageInt32Nullable : Producer<double?>
  {
    private readonly IObservable<int?> _source;

    public AverageInt32Nullable(IObservable<int?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageInt32Nullable._ observer1 = new AverageInt32Nullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<int?>((IObserver<int?>) observer1);
    }

    private class _ : Sink<double?>, IObserver<int?>
    {
      private long _sum;
      private long _count;

      public _(IObserver<double?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0L;
        this._count = 0L;
      }

      public void OnNext(int? value)
      {
        try
        {
          if (!value.HasValue)
            return;
          checked { this._sum += (long) value.Value; }
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
          this._observer.OnNext(new double?((double) this._sum / (double) this._count));
        else
          this._observer.OnNext(new double?());
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
