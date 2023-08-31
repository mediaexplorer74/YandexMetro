// Decompiled with JetBrains decompiler
// Type: System.Reactive.Sink`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive
{
  internal abstract class Sink<TSource> : IDisposable
  {
    protected internal volatile IObserver<TSource> _observer;
    private IDisposable _cancel;

    public Sink(IObserver<TSource> observer, IDisposable cancel)
    {
      this._observer = observer;
      this._cancel = cancel;
    }

    public virtual void Dispose()
    {
      this._observer = NopObserver<TSource>.Instance;
      Interlocked.Exchange<IDisposable>(ref this._cancel, (IDisposable) null)?.Dispose();
    }

    public IObserver<TSource> GetForwarder() => (IObserver<TSource>) new Sink<TSource>._(this);

    private class _ : IObserver<TSource>
    {
      private readonly Sink<TSource> _forward;

      public _(Sink<TSource> forward) => this._forward = forward;

      public void OnNext(TSource value) => this._forward._observer.OnNext(value);

      public void OnError(Exception error)
      {
        this._forward._observer.OnError(error);
        this._forward.Dispose();
      }

      public void OnCompleted()
      {
        this._forward._observer.OnCompleted();
        this._forward.Dispose();
      }
    }
  }
}
