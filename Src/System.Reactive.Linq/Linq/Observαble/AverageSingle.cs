// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.AverageSingle
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class AverageSingle : Producer<float>
  {
    private readonly IObservable<float> _source;

    public AverageSingle(IObservable<float> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageSingle._ observer1 = new AverageSingle._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<float>((IObserver<float>) observer1);
    }

    private class _ : Sink<float>, IObserver<float>
    {
      private double _sum;
      private long _count;

      public _(IObserver<float> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
        this._count = 0L;
      }

      public void OnNext(float value)
      {
        try
        {
          this._sum += (double) value;
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
          this._observer.OnNext((float) this._sum / (float) this._count);
          this._observer.OnCompleted();
        }
        else
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        this.Dispose();
      }
    }
  }
}
