// Decompiled with JetBrains decompiler
// Type: System.Reactive.Threading.Semaphore
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive.Threading
{
  internal sealed class Semaphore : IDisposable
  {
    private int m_currentCount;
    private readonly int m_maximumCount;
    private readonly object m_lockObject;
    private bool m_disposed;

    public Semaphore(int initialCount, int maximumCount)
    {
      if (initialCount < 0)
        throw new ArgumentOutOfRangeException(nameof (initialCount), "Non-negative number required.");
      if (maximumCount < 1)
        throw new ArgumentOutOfRangeException(nameof (maximumCount), "Positive number required.");
      this.m_currentCount = initialCount <= maximumCount ? initialCount : throw new ArgumentException("Initial count must be smaller than maximum");
      this.m_maximumCount = maximumCount;
      this.m_lockObject = new object();
    }

    public int Release() => this.Release(1);

    public int Release(int releaseCount)
    {
      if (releaseCount < 1)
        throw new ArgumentOutOfRangeException(nameof (releaseCount), "Positive number required.");
      if (this.m_disposed)
        throw new ObjectDisposedException(nameof (Semaphore));
      int num = 0;
      lock (this.m_lockObject)
      {
        num = this.m_currentCount;
        if (releaseCount + this.m_currentCount > this.m_maximumCount)
          throw new ArgumentOutOfRangeException(nameof (releaseCount), "Amount of releases would overflow maximum");
        this.m_currentCount += releaseCount;
        Monitor.PulseAll(this.m_lockObject);
      }
      return num;
    }

    public bool WaitOne() => this.WaitOne(-1);

    public bool WaitOne(int millisecondsTimeout)
    {
      if (this.m_disposed)
        throw new ObjectDisposedException(nameof (Semaphore));
      lock (this.m_lockObject)
      {
        while (this.m_currentCount == 0)
        {
          if (!Monitor.Wait(this.m_lockObject, millisecondsTimeout))
            return false;
        }
        --this.m_currentCount;
        return true;
      }
    }

    public bool WaitOne(TimeSpan timeout) => this.WaitOne((int) timeout.TotalMilliseconds);

    public void Close() => this.Dispose();

    public void Dispose() => this.m_disposed = true;
  }
}
