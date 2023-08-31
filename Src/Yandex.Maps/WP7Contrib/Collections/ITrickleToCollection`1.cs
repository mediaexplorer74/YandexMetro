// Decompiled with JetBrains decompiler
// Type: WP7Contrib.Collections.ITrickleToCollection`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;

namespace WP7Contrib.Collections
{
  internal interface ITrickleToCollection<T>
  {
    Queue<T> Source { get; }

    bool Pending { get; }

    bool IsTrickling { get; }

    void Start(int trickleDelay, IEnumerable<T> sourceCollection, IList<T> destinationCollection);

    void Stop();

    void Suspend();

    void Resume();
  }
}
