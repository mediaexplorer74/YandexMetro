// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.AsyncLock
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;

namespace System.Reactive.Concurrency
{
  public sealed class AsyncLock : IDisposable
  {
    private readonly Queue<Action> queue = new Queue<Action>();
    private bool isAcquired;
    private bool hasFaulted;

    public void Wait(Action action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      bool flag = false;
      lock (this.queue)
      {
        if (!this.hasFaulted)
        {
          this.queue.Enqueue(action);
          flag = !this.isAcquired;
          this.isAcquired = true;
        }
      }
      if (!flag)
        return;
      while (true)
      {
        Action action1 = (Action) null;
        lock (this.queue)
        {
          if (this.queue.Count > 0)
          {
            action1 = this.queue.Dequeue();
          }
          else
          {
            this.isAcquired = false;
            break;
          }
        }
        try
        {
          action1();
        }
        catch
        {
          lock (this.queue)
          {
            this.queue.Clear();
            this.hasFaulted = true;
          }
          throw;
        }
      }
    }

    public void Dispose()
    {
      lock (this.queue)
      {
        this.queue.Clear();
        this.hasFaulted = true;
      }
    }
  }
}
