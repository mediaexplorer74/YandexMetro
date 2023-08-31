﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.SumDouble
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class SumDouble : Producer<double>
  {
    private readonly IObservable<double> _source;

    public SumDouble(IObservable<double> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumDouble._ observer1 = new SumDouble._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<double>((IObserver<double>) observer1);
    }

    private class _ : Sink<double>, IObserver<double>
    {
      private double _sum;

      public _(IObserver<double> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(double value) => this._sum += value;

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._sum);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
