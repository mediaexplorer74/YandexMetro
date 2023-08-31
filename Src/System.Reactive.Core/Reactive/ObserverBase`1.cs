// Decompiled with JetBrains decompiler
// Type: System.Reactive.ObserverBase`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive
{
  public abstract class ObserverBase<T> : IObserver<T>, IDisposable
  {
    private int isStopped;

    protected ObserverBase() => this.isStopped = 0;

    public void OnNext(T value)
    {
      if (this.isStopped != 0)
        return;
      this.OnNextCore(value);
    }

    protected abstract void OnNextCore(T value);

    public void OnError(Exception error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      this.OnErrorCore(error);
    }

    protected abstract void OnErrorCore(Exception error);

    public void OnCompleted()
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      this.OnCompletedCore();
    }

    protected abstract void OnCompletedCore();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.isStopped = 1;
    }

    internal bool Fail(Exception error)
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return false;
      this.OnErrorCore(error);
      return true;
    }
  }
}
