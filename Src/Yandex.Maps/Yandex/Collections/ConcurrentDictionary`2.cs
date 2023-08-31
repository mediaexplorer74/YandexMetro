// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.ConcurrentDictionary`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Collections
{
  internal class ConcurrentDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    private readonly object _sync = new object();
    private readonly IDictionary<TKey, TValue> _innerDic;

    public ConcurrentDictionary() => this._innerDic = (IDictionary<TKey, TValue>) new Dictionary<TKey, TValue>();

    public ConcurrentDictionary(int capacity) => this._innerDic = (IDictionary<TKey, TValue>) new Dictionary<TKey, TValue>(capacity);

    public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary) => this._innerDic = (IDictionary<TKey, TValue>) new Dictionary<TKey, TValue>(dictionary);

    public void Add(TKey key, TValue value)
    {
      lock (this._sync)
        this._innerDic.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
      lock (this._sync)
        return this._innerDic.ContainsKey(key);
    }

    public ICollection<TKey> Keys
    {
      get
      {
        lock (this._sync)
          return (ICollection<TKey>) this._innerDic.Keys.ToArray<TKey>();
      }
    }

    public bool Remove(TKey key)
    {
      lock (this._sync)
        return this._innerDic.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      lock (this._sync)
        return this._innerDic.TryGetValue(key, out value);
    }

    public ICollection<TValue> Values
    {
      get
      {
        lock (this._sync)
          return (ICollection<TValue>) this._innerDic.Values.ToArray<TValue>();
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        lock (this._sync)
          return this._innerDic[key];
      }
      set
      {
        lock (this._sync)
          this._innerDic[key] = value;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      lock (this._sync)
        this._innerDic.Add(item);
    }

    public void Clear()
    {
      lock (this._sync)
        this._innerDic.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      lock (this._sync)
        return this._innerDic.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      lock (this._sync)
        this._innerDic.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get
      {
        lock (this._sync)
          return this._innerDic.Count;
      }
    }

    public bool IsReadOnly => false;

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      lock (this._sync)
        return ((ICollection<KeyValuePair<TKey, TValue>>) this._innerDic).Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      lock (this._sync)
        return (IEnumerator<KeyValuePair<TKey, TValue>>) new Dictionary<TKey, TValue>(this._innerDic).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
