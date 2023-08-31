// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.QueueEntry`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Maps.Repository
{
  internal class QueueEntry<TState, TKey>
  {
    public QueueEntry(TKey key) => this.Key = key;

    public TState State { get; set; }

    public TKey Key { get; private set; }
  }
}
