// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousSafeObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive
{
  internal class AnonymousSafeObserver<T> : IObserver<T>
  {
    private readonly Action<T> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;
    private readonly IDisposable _disposable;
    private int isStopped;

    public AnonymousSafeObserver(
      Action<T> onNext,
      Action<Exception> onError,
      Action onCompleted,
      IDisposable disposable)
    {
      this._onNext = onNext;
      this._onError = onError;
      this._onCompleted = onCompleted;
      this._disposable = disposable;
    }

    public void OnNext(T value)
    {
      if (this.isStopped != 0)
        return;
      bool flag = false;
      try
      {
        this._onNext(value);
        flag = true;
      }
      finally
      {
        if (!flag)
          this._disposable.Dispose();
      }
    }

    public void OnError(Exception error)
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      try
      {
        this._onError(error);
      }
      finally
      {
        this._disposable.Dispose();
      }
    }

    public void OnCompleted()
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      try
      {
        this._onCompleted();
      }
      finally
      {
        this._disposable.Dispose();
      }
    }
  }
}
