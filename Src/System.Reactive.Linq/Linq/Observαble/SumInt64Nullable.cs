// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumInt64Nullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumInt64Nullable : Producer<long?>
  {
    private readonly IObservable<long?> _source;

    public SumInt64Nullable(IObservable<long?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<long?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumInt64Nullable._ observer1 = new SumInt64Nullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<long?>((IObserver<long?>) observer1);
    }

    private class _ : Sink<long?>, IObserver<long?>
    {
      private long _sum;

      public _(IObserver<long?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0L;
      }

      public void OnNext(long? value)
      {
        try
        {
          if (!value.HasValue)
            return;
          checked { this._sum += value.Value; }
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
        this._observer.OnNext(new long?(this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
