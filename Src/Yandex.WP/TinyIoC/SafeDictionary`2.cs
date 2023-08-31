// Decompiled with JetBrains decompiler
// Type: TinyIoC.SafeDictionary`2
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyIoC
{
  public class SafeDictionary<TKey, TValue> : IDisposable
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
