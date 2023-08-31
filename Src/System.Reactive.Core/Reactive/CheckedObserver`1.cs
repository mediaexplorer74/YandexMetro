// Decompiled with JetBrains decompiler
// Type: System.Reactive.CheckedObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive
{
  internal class CheckedObserver<T> : IObserver<T>
  {
    private const int IDLE = 0;
    private const int BUSY = 1;
    private const int DONE = 2;
    private readonly IObserver<T> _observer;
    private int _state;

    public CheckedObserver(IObserver<T> observer) => this._observer = observer;

    public void OnNext(T value)
    {
      this.CheckAccess();
      try
      {
        this._observer.OnNext(value);
      }
      finally
      {
        Interlocked.Exchange(ref this._state, 0);
      }
    }

    public void OnError(Exception error)
    {
      this.CheckAccess();
      try
      {
        this._observer.OnError(error);
      }
      finally
      {
        Interlocked.Exchange(ref this._state, 2);
      }
    }

    public void OnCompleted()
    {
      this.CheckAccess();
      try
      {
        this._observer.OnCompleted();
      }
      finally
      {
        Interlocked.Exchange(ref this._state, 2);
      }
    }

    private void CheckAccess()
    {
      switch (Interlocked.CompareExchange(ref this._state, 1, 0))
      {
        case 1:
          throw new InvalidOperationException(Strings_Core.REENTRANCY_DETECTED);
        case 2:
          throw new InvalidOperationException(Strings_Core.OBSERVER_TERMINATED);
      }
    }
  }
}
