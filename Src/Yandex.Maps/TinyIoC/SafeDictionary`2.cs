// Decompiled with JetBrains decompiler
// Type: TinyIoC.SafeDictionary`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyIoC
{
  internal class SafeDictionary<TKey, TValue> : IDisposable
  {
    private readonly object _Padlock = new object();
    private readonly Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
      set
      {
        lock (this._Padlock)
        {
          TValue obj;
          if (this._Dictionary.TryGetValue(key, out obj) && obj is IDisposable disposable)
            disposable.Dispose();
          this._Dictionary[key] = value;
        }
      }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      lock (this._Padlock)
        return this._Dictionary.TryGetValue(key, out value);
    }

    public bool Remove(TKey key)
    {
      lock (this._Padlock)
        return this._Dictionary.Remove(key);
    }

    public void Clear()
    {
      lock (this._Padlock)
        this._Dictionary.Clear();
    }

    public IEnumerable<TKey> Keys => (IEnumerable<TKey>) this._Dictionary.Keys;

    public void Dispose()
    {
      lock (this._Padlock)
      {
        foreach (IDisposable disposable in this._Dictionary.Values.Where<TValue>((Func<TValue, bool>) (item => (object) item is IDisposable)).Select<TValue, IDisposable>((Func<TValue, IDisposable>) (item => (object) item as IDisposable)))
          disposable.Dispose();
      }
      GC.SuppressFinalize((object) this);
    }
  }
}
