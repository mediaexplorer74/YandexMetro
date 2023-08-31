// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.Synchronize`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Concurrency
{
  internal class Synchronize<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly object _gate;

    public Synchronize(IObservable<TSource> source, object gate)
    {
      this._source = source;
      this._gate = gate;
    }

    public Synchronize(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Synchronize<TSource>._ observer1 = new Synchronize<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Synchronize<TSource> _parent;
      private readonly object _gate;

      public _(Synchronize<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._gate = this._parent._gate ?? new object();
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
