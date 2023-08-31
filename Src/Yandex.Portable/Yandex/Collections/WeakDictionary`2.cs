// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.WeakDictionary`2
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Collections
{
  public class WeakDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
    where TValue : class
  {
    private readonly IDictionary<TKey, WeakReference> _dict = (IDictionary<TKey, WeakReference>) new Dictionary<TKey, WeakReference>();

    public void Put(TKey key, TValue value) => this._dict[key] = new WeakReference((object) value);

    public bool ContainsKey(TKey key) => this._dict.ContainsKey(key) && this._dict[key].IsAlive;

    public void Add(TKey key, TValue value)
    {
      WeakReference weakReference;
      if (this._dict.TryGetValue(key, out weakReference) && weakReference.Target == null)
        this._dict.Remove(key);
      this._dict.Add(key, new WeakReference((object) value));
    }

    public bool Remove(TKey key) => this._dict.Remove(key);

    public TValue this[TKey key]
    {
      get
      {
        WeakReference weakReference = this._dict[key];
        return weakReference.IsAlive ? (TValue) weakReference.Target : throw new KeyNotFoundException();
      }
      set => this._dict[key] = new WeakReference((object) value);
    }

    public ICollection<TKey> Keys => this._dict.Keys;

    public ICollection<TValue> Values => (ICollection<TValue>) this._dict.Where<KeyValuePair<TKey, WeakReference>>((Func<KeyValuePair<TKey, WeakReference>, bool>) (item => item.Value.IsAlive)).Select<KeyValuePair<TKey, WeakReference>, TValue>((Func<KeyValuePair<TKey, WeakReference>, TValue>) (item => (TValue) item.Value.Target)).ToList<TValue>();

    public bool TryGetValue(TKey key, out TValue value)
    {
      WeakReference weakReference;
      if (!this._dict.TryGetValue(key, out weakReference))
      {
        value = default (TValue);
        return false;
      }
      value = (TValue) weakReference.Target;
      return (object) value != null;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this._dict.Where<KeyValuePair<TKey, WeakReference>>((Func<KeyValuePair<TKey, WeakReference>, bool>) (item => item.Value.IsAlive)).Select<KeyValuePair<TKey, WeakReference>, KeyValuePair<TKey, TValue>>((Func<KeyValuePair<TKey, WeakReference>, KeyValuePair<TKey, TValue>>) (item => new KeyValuePair<TKey, TValue>(item.Key, (TValue) item.Value.Target))).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._dict.Where<KeyValuePair<TKey, WeakReference>>((Func<KeyValuePair<TKey, WeakReference>, bool>) (item => item.Value.IsAlive)).Select<KeyValuePair<TKey, WeakReference>, KeyValuePair<TKey, TValue>>((Func<KeyValuePair<TKey, WeakReference>, KeyValuePair<TKey, TValue>>) (item => new KeyValuePair<TKey, TValue>(item.Key, (TValue) item.Value.Target))).GetEnumerator();

    public void Add(KeyValuePair<TKey, TValue> item) => this._dict.Add(item.Key, new WeakReference((object) item.Value));

    public void Clear() => this._dict.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) => this._dict.ContainsKey(item.Key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

    public bool Remove(KeyValuePair<TKey, TValue> item) => this._dict.Remove(item.Key);

    public int Count => this._dict.Count;

    public bool IsReadOnly => false;
  }
}
