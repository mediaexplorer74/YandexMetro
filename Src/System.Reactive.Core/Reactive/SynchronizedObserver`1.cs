// Decompiled with JetBrains decompiler
// Type: System.Reactive.SynchronizedObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  internal class SynchronizedObserver<T> : ObserverBase<T>
  {
    private readonly object _gate;
    private readonly IObserver<T> _observer;

    public SynchronizedObserver(IObserver<T> observer, object gate)
    {
      this._gate = gate;
      this._observer = observer;
    }

    protected override void OnNextCore(T value)
    {
      lock (this._gate)
        this._observer.OnNext(value);
    }

    protected override void OnErrorCore(Exception exception)
    {
      lock (this._gate)
        this._observer.OnError(exception);
    }

    protected override void OnCompletedCore()
    {
      lock (this._gate)
        this._observer.OnCompleted();
    }
  }
}
