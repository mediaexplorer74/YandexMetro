// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.TileQueueEntry
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository
{
  internal class TileQueueEntry : QueueEntry<TileQueueState, ITileInfo>
  {
    public TileQueueEntry(ITileInfo key)
      : base(key)
    {
      this.MappedTileInfos = (IList<ITileInfo>) new List<ITileInfo>();
    }

    public uint ReloadAttempts { get; set; }

    public IList<ITileInfo> MappedTileInfos { get; private set; }
  }
}
