// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.EnumerableQueue`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Yandex.Collections.Interfaces;

namespace Yandex.Collections
{
  internal class EnumerableQueue<T> : IEnumerableQueue<T>, IQueue<T>, IEnumerable<T>, IEnumerable
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
