// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.IObservableEnumerable`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Yandex.Collections
{
  internal interface IObservableEnumerable<T> : INotifyCollectionChanged, IEnumerable<T>, IEnumerable
  {
    void Clear();
  }
}
