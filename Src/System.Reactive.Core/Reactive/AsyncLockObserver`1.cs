// Decompiled with JetBrains decompiler
// Type: System.Reactive.AsyncLockObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;

namespace System.Reactive
{
  internal class AsyncLockObserver<T> : ObserverBase<T>
  {
    private readonly AsyncLock _gate;
    private readonly IObserver<T> _observer;

    public AsyncLockObserver(IObserver<T> observer, AsyncLock gate)
    {
      this._gate = gate;
      this._observer = observer;
    }

    protected override void OnNextCore(T value) => this._gate.Wait((Action) (() => this._observer.OnNext(value)));

    protected override void OnErrorCore(Exception exception) => this._gate.Wait((Action) (() => this._observer.OnError(exception)));

    protected override void OnCompletedCore() => this._gate.Wait((Action) (() => this._observer.OnCompleted()));
  }
}
