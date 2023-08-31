// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.Interfaces.IConcurrentCollection`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System.Collections;
using System.Collections.Generic;

namespace Yandex.Collections.Interfaces
{
  public interface IConcurrentCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable
  {
    bool TryAdd(T item);

    bool IsSynchronized { get; }

    object SyncRoot { get; }
  }
}
