// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.Interfaces.IMemoryCache`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;

namespace Yandex.Collections.Interfaces
{
  internal interface IMemoryCache<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    int Capacity { get; set; }

    void Prolong(TKey key);
  }
}
