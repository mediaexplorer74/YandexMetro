// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumSingle
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumSingle : Producer<float>
  {
    private readonly IObservable<float> _source;

    public SumSingle(IObservable<float> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumSingle._ observer1 = new SumSingle._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<float>((IObserver<float>) observer1);
    }

    private class _ : Sink<float>, IObserver<float>
    {
      private double _sum;

      public _(IObserver<float> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(float value) => this._sum += (double) value;

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext((float) this._sum);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
