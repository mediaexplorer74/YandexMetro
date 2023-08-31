// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumInt32Nullable
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumInt32Nullable : Producer<int?>
  {
    private readonly IObservable<int?> _source;

    public SumInt32Nullable(IObservable<int?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<int?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumInt32Nullable._ observer1 = new SumInt32Nullable._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<int?>((IObserver<int?>) observer1);
    }

    private class _ : Sink<int?>, IObserver<int?>
    {
      private int _sum;

      public _(IObserver<int?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0;
      }

      public void OnNext(int? value)
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
        this._observer.OnNext(new int?(this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
