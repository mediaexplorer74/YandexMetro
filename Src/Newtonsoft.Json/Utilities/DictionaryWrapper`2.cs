// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DictionaryWrapper`2
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
  internal class DictionaryWrapper<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IWrappedDictionary,
    IDictionary,
    ICollection,
    IEnumerable
  {
    private readonly IDictionary _dictionary;
    private readonly IDictionary<TKey, TValue> _genericDictionary;
    private object _syncRoot;

    public DictionaryWrapper(IDictionary dictionary)
    {
      ValidationUtils.ArgumentNotNull((object) dictionary, nameof (dictionary));
      this._dictionary = dictionary;
    }

    public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
    {
      ValidationUtils.ArgumentNotNull((object) dictionary, nameof (dictionary));
      this._genericDictionary = dictionary;
    }

    public void Add(TKey key, TValue value)
    {
      if (this._genericDictionary != null)
        this._genericDictionary.Add(key, value);
      else
        this._dictionary.Add((object) key, (object) value);
    }

    public bool ContainsKey(TKey key) => this._genericDictionary != null ? this._genericDictionary.ContainsKey(key) : this._dictionary.Contains((object) key);

    public ICollection<TKey> Keys => this._genericDictionary != null ? this._genericDictionary.Keys : (ICollection<TKey>) this._dictionary.Keys.Cast<TKey>().ToList<TKey>();

    public bool Remove(TKey key)
    {
      if (this._genericDictionary != null)
        return this._genericDictionary.Remove(key);
      if (!this._dictionary.Contains((object) key))
        return false;
      this._dictionary.Remove((object) key);
      return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      if (this._genericDictionary != null)
        return this._genericDictionary.TryGetValue(key, out value);
      if (!this._dictionary.Contains((object) key))
      {
        value = default (TValue);
        return false;
      }
      value = (TValue) this._dictionary[(object) key];
      return true;
    }

    public ICollection<TValue> Values => this._genericDictionary != null ? this._genericDictionary.Values : (ICollection<TValue>) this._dictionary.Values.Cast<TValue>().ToList<TValue>();

    public TValue this[TKey key]
    {
      get => this._genericDictionary != null ? this._genericDictionary[key] : (TValue) this._dictionary[(object) key];
      set
      {
        if (this._genericDictionary != null)
          this._genericDictionary[key] = value;
        else
          this._dictionary[(object) key] = (object) value;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      if (this._genericDictionary != null)
        this._genericDictionary.Add(item);
      else
        ((IList) this._dictionary).Add((object) item);
    }

    public void Clear()
    {
      if (this._genericDictionary != null)
        this._genericDictionary.Clear();
      else
        this._dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => this._genericDictionary != null ? this._genericDictionary.Contains(item) : ((IList) this._dictionary).Contains((object) item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      if (this._genericDictionary != null)
      {
        this._genericDictionary.CopyTo(array, arrayIndex);
      }
      else
      {
        foreach (DictionaryEntry dictionaryEntry in this._dictionary)
          array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey) dictionaryEntry.Key, (TValue) dictionaryEntry.Value);
      }
    }

    public int Count => this._genericDictionary != null ? this._genericDictionary.Count : this._dictionary.Count;

    public bool IsReadOnly => this._genericDictionary != null ? this._genericDictionary.IsReadOnly : this._dictionary.IsReadOnly;

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      if (this._genericDictionary != null)
        return ((ICollection<KeyValuePair<TKey, TValue>>) this._genericDictionary).Remove(item);
      if (!this._dictionary.Contains((object) item.Key))
        return true;
      if (!object.Equals(this._dictionary[(object) item.Key], (object) item.Value))
        return false;
      this._dictionary.Remove((object) item.Key);
      return true;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this._genericDictionary != null ? this._genericDictionary.GetEnumerator() : this._dictionary.Cast<DictionaryEntry>().Select<DictionaryEntry, KeyValuePair<TKey, TValue>>((Func<DictionaryEntry, KeyValuePair<TKey, TValue>>) (de => new KeyValuePair<TKey, TValue>((TKey) de.Key, (TValue) de.Value))).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    void IDictionary.Add(object key, object value)
    {
      if (this._genericDictionary != null)
        this._genericDictionary.Add((TKey) key, (TValue) value);
      else
        this._dictionary.Add(key, value);
    }

    bool IDictionary.Contains(object key) => this._genericDictionary != null ? this._genericDictionary.ContainsKey((TKey) key) : this._dictionary.Contains(key);

    IDictionaryEnumerator IDictionary.GetEnumerator() => this._genericDictionary != null ? (IDictionaryEnumerator) new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator()) : this._dictionary.GetEnumerator();

    bool IDictionary.IsFixedSize => this._genericDictionary == null && this._dictionary.IsFixedSize;

    ICollection IDictionary.Keys => this._genericDictionary != null ? (ICollection) this._genericDictionary.Keys.ToList<TKey>() : this._dictionary.Keys;

    public void Remove(object key)
    {
      if (this._genericDictionary != null)
        this._genericDictionary.Remove((TKey) key);
      else
        this._dictionary.Remove(key);
    }

    ICollection IDictionary.Values => this._genericDictionary != null ? (ICollection) this._genericDictionary.Values.ToList<TValue>() : this._dictionary.Values;

    object IDictionary.this[object key]
    {
      get => this._genericDictionary != null ? (object) this._genericDictionary[(TKey) key] : this._dictionary[key];
      set
      {
        if (this._genericDictionary != null)
          this._genericDictionary[(TKey) key] = (TValue) value;
        else
          this._dictionary[key] = value;
      }
    }

    void ICollection.CopyTo(Array array, int index)
    {
      if (this._genericDictionary != null)
        this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[]) array, index);
      else
        this._dictionary.CopyTo(array, index);
    }

    bool ICollection.IsSynchronized => this._genericDictionary == null && this._dictionary.IsSynchronized;

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    public object UnderlyingDictionary => this._genericDictionary != null ? (object) this._genericDictionary : (object) this._dictionary;

    private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : 
      IDictionaryEnumerator,
      IEnumerator
    {
      private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

      public DictionaryEnumerator(
        IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
      {
        ValidationUtils.ArgumentNotNull((object) e, nameof (e));
        this._e = e;
      }

      public DictionaryEntry Entry => (DictionaryEntry) this.Current;

      public object Key => this.Entry.Key;

      public object Value => this.Entry.Value;

      public object Current => (object) new DictionaryEntry((object) this._e.Current.Key, (object) this._e.Current.Value);

      public bool MoveNext() => this._e.MoveNext();

      public void Reset() => this._e.Reset();
    }
  }
}
