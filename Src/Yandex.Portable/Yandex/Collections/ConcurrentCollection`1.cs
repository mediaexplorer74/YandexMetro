// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.ConcurrentCollection`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Yandex.Collections.Interfaces;

namespace Yandex.Collections
{
  public class ConcurrentCollection<T> : 
    IConcurrentCollection<T>,
    ICollection<T>,
    IEnumerable<T>,
    ICollection,
    IEnumerable
  {
    private readonly object _sync = new object();
    private readonly List<T> _innerList;

    public ConcurrentCollection() => this._innerList = new List<T>();

    public ConcurrentCollection(int capacity) => this._innerList = new List<T>(capacity);

    public ConcurrentCollection(IEnumerable<T> collection) => this._innerList = new List<T>(collection);

    public bool TryAdd(T item)
    {
      if (!Monitor.TryEnter(this._sync))
        return false;
      try
      {
        this._innerList.Add(item);
        return true;
      }
      finally
      {
        Monitor.Exit(this._sync);
      }
    }

    public void Add(T item)
    {
      lock (this._sync)
        this._innerList.Add(item);
    }

    public void AddRange(IEnumerable<T> collection)
    {
      lock (this._sync)
        this._innerList.AddRange(collection);
    }

    public void Clear()
    {
      lock (this._sync)
        this._innerList.Clear();
    }

    public bool Contains(T item)
    {
      lock (this._sync)
        return this._innerList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      lock (this._sync)
        this._innerList.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get
      {
        lock (this._sync)
          return this._innerList.Count;
      }
    }

    public bool IsReadOnly => false;

    public bool Remove(T item)
    {
      lock (this._sync)
        return this._innerList.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      lock (this._sync)
        return (IEnumerator<T>) this._innerList.ToList<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool IsSynchronized => true;

    public object SyncRoot => this._sync;

    public void CopyTo(Array array, int index) => this.CopyTo((T[]) array, index);
  }
}
