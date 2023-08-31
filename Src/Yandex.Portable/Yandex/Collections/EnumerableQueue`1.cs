// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.EnumerableQueue`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Yandex.Collections.Interfaces;

namespace Yandex.Collections
{
  public class EnumerableQueue<T> : IEnumerableQueue<T>, IQueue<T>, IEnumerable<T>, IEnumerable
  {
    private readonly List<T> _innerList = new List<T>();

    public bool Remove(T item) => this._innerList.Remove(item);

    public T Dequeue()
    {
      T obj = this._innerList.Count != 0 ? this._innerList[0] : throw new InvalidOperationException("Queue is empty.");
      this._innerList.RemoveAt(0);
      return obj;
    }

    public void Enqueue(T item) => this._innerList.Add(item);

    public int Count => this._innerList.Count;

    public void Clear() => this._innerList.Clear();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._innerList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
