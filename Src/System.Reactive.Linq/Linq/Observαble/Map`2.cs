// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Map`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq.Observαble
{
  internal class Map<TKey, TValue>
  {
    private readonly Dictionary<TKey, TValue> _map;

    public Map(IEqualityComparer<TKey> comparer) => this._map = new Dictionary<TKey, TValue>(comparer);

    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool added)
    {
      lock (this._map)
      {
        added = false;
        TValue orAdd = default (TValue);
        if (!this._map.TryGetValue(key, out orAdd))
        {
          orAdd = valueFactory();
          this._map.Add(key, orAdd);
          added = true;
        }
        return orAdd;
      }
    }

    public IEnumerable<TValue> Values
    {
      get
      {
        lock (this._map)
          return (IEnumerable<TValue>) this._map.Values.ToArray<TValue>();
      }
    }

    public bool Remove(TKey key)
    {
      lock (this._map)
        return this._map.Remove(key);
    }
  }
}
