// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Scan`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Scan<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TSource, TSource> _accumulator;

    public Scan(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
    {
      this._source = source;
      this._accumulator = accumulator;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Scan<TSource>._ observer1 = new Scan<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Scan<TSource> _parent;
      private TSource _accumulation;
      private bool _hasAccumulation;

      public _(Scan<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._accumulation = default (TSource);
        this._hasAccumulation = false;
      }

      public void OnNext(TSource value)
      {
        try
        {
          if (this._hasAccumulation)
          {
            this._accumulation = this._parent._accumulator(this._accumulation, value);
          }
          else
          {
            this._accumulation = value;
            this._hasAccumulation = true;
          }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(this._accumulation);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
