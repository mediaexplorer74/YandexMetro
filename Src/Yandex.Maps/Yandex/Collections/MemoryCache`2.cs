// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.MemoryCache`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Yandex.Collections.Interfaces;

namespace Yandex.Collections
{
  internal class MemoryCache<TKey, TValue> : 
    IMemoryCache<TKey, TValue>,
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    protected readonly object _sync = new object();
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _dict;
    private readonly LinkedList<KeyValuePair<TKey, TValue>> _list = new LinkedList<KeyValuePair<TKey, TValue>>();

    public MemoryCache(int capacity)
    {
      this._capacity = capacity > 0 ? capacity : throw new ArgumentOutOfRangeException("capacity should be positive");
      this._dict = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(capacity);
    }

    protected LinkedListNode<KeyValuePair<TKey, TValue>> GetAndProlong(TKey key)
    {
      LinkedListNode<KeyValuePair<TKey, TValue>> node;
      if (!this._dict.TryGetValue(key, out node))
        return (LinkedListNode<KeyValuePair<TKey, TValue>>) null;
      this._list.Remove(node);
      this._list.AddFirst(node);
      return node;
    }

    protected void AddInternal(KeyValuePair<TKey, TValue> item)
    {
      if (this._list.Count >= this._capacity)
      {
        this._dict.Remove(this._list.Last.Value.Key);
        this._list.RemoveLast();
      }
      this._dict.Add(item.Key, this._list.AddFirst(item));
    }

    public int Capacity
    {
      get => this._capacity;
      set => throw new NotSupportedException();
    }

    public void Prolong(TKey key)
    {
      lock (this._sync)
      {
        if (this.GetAndProlong(key) == null)
          throw new KeyNotFoundException();
      }
    }

    public void Add(TKey key, TValue value) => this.Add(new KeyValuePair<TKey, TValue>(key, value));

    public bool ContainsKey(TKey key)
    {
      lock (this._sync)
        return this._dict.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
      lock (this._sync)
      {
        LinkedListNode<KeyValuePair<TKey, TValue>> node;
        if (!this._dict.TryGetValue(key, out node))
          return false;
        this._dict.Remove(key);
        this._list.Remove(node);
        return true;
      }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      lock (this._sync)
      {
        LinkedListNode<KeyValuePair<TKey, TValue>> andProlong = this.GetAndProlong(key);
        if (andProlong != null)
        {
          value = andProlong.Value.Value;
          return true;
        }
        value = default (TValue);
        return false;
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        lock (this._sync)
          return (this.GetAndProlong(key) ?? throw new KeyNotFoundException()).Value.Value;
      }
      set
      {
        lock (this._sync)
        {
          KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
          LinkedListNode<KeyValuePair<TKey, TValue>> andProlong = this.GetAndProlong(key);
          if (andProlong == null)
            this.AddInternal(keyValuePair);
          else
            andProlong.Value = keyValuePair;
        }
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      lock (this._sync)
      {
        if (this._dict.ContainsKey(item.Key))
          throw new ArgumentException("duplicate key");
        this.AddInternal(item);
      }
    }

    public void Clear()
    {
      lock (this._sync)
      {
        this._dict.Clear();
        this._list.Clear();
      }
    }

    public int Count
    {
      get
      {
        lock (this._sync)
          return this._list.Count;
      }
    }

    public bool IsReadOnly => false;

    public ICollection<TKey> Keys
    {
      get
      {
        lock (this._sync)
          return (ICollection<TKey>) this._dict.Keys.ToArray<TKey>();
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        lock (this._sync)
          return (ICollection<TValue>) this._list.Select<KeyValuePair<TKey, TValue>, TValue>((Func<KeyValuePair<TKey, TValue>, TValue>) (item => item.Value)).ToArray<TValue>();
      }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

    public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
  }
}
