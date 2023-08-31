// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.Queue`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using Yandex.Collections.Interfaces;

namespace Yandex.Collections
{
  public class Queue<T> : IQueue<T>
  {
    private readonly object _sync;
    private readonly System.Collections.Generic.Queue<T> _queue;

    public Queue()
    {
      this._sync = new object();
      this._queue = new System.Collections.Generic.Queue<T>();
    }

    public void Enqueue(T ti)
    {
      lock (this._sync)
        this._queue.Enqueue(ti);
    }

    public T Dequeue()
    {
      lock (this._sync)
        return this._queue.Dequeue();
    }

    public int Count
    {
      get
      {
        lock (this._sync)
          return this._queue.Count;
      }
    }

    public void Clear()
    {
      lock (this._sync)
        this._queue.Clear();
    }
  }
}
