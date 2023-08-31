// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.IStateQueue`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Collections.Generic;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface IStateQueue<TKey, TValue> where TValue : class
  {
    bool TryEnqueue(TKey key, out TValue entry);

    bool TryGetEntry(TKey key, out TValue entry);

    [CanBeNull]
    TValue Dequeue(TKey key);

    [NotNull]
    IEnumerable<TKey> Keys { get; }

    [NotNull]
    IEnumerable<TValue> Values { get; }

    bool Contains(TKey tileInfo);

    void Clear();
  }
}
