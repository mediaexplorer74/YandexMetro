﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.AverageInt64
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class AverageInt64 : Producer<double>
  {
    private readonly IObservable<long> _source;

    public AverageInt64(IObservable<long> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageInt64._ observer1 = new AverageInt64._(observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<long>((IObserver<long>) observer1);
    }

    private class _ : Sink<double>, IObserver<long>
    {
      private long _sum;
      private long _count;

      public _(IObserver<double> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0L;
        this._count = 0L;
      }

      public void OnNext(long value)
      {
        try
        {
          checked { this._sum += value; }
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
          this._observer.OnNext((double) this._sum / (double) this._count);
          this._observer.OnCompleted();
        }
        else
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        this.Dispose();
      }
    }
  }
}
