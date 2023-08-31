// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.BidirectionalDictionary`2
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
  internal class BidirectionalDictionary<TFirst, TSecond>
  {
    private readonly IDictionary<TFirst, TSecond> _firstToSecond;
    private readonly IDictionary<TSecond, TFirst> _secondToFirst;

    public BidirectionalDictionary()
      : this((IEqualityComparer<TFirst>) EqualityComparer<TFirst>.Default, (IEqualityComparer<TSecond>) EqualityComparer<TSecond>.Default)
    {
    }

    public BidirectionalDictionary(
      IEqualityComparer<TFirst> firstEqualityComparer,
      IEqualityComparer<TSecond> secondEqualityComparer)
    {
      this._firstToSecond = (IDictionary<TFirst, TSecond>) new Dictionary<TFirst, TSecond>(firstEqualityComparer);
      this._secondToFirst = (IDictionary<TSecond, TFirst>) new Dictionary<TSecond, TFirst>(secondEqualityComparer);
    }

    public void Add(TFirst first, TSecond second)
    {
      if (this._firstToSecond.ContainsKey(first) || this._secondToFirst.ContainsKey(second))
        throw new ArgumentException("Duplicate first or second");
      this._firstToSecond.Add(first, second);
      this._secondToFirst.Add(second, first);
    }

    public bool TryGetByFirst(TFirst first, out TSecond second) => this._firstToSecond.TryGetValue(first, out second);

    public bool TryGetBySecond(TSecond second, out TFirst first) => this._secondToFirst.TryGetValue(second, out first);
  }
}
