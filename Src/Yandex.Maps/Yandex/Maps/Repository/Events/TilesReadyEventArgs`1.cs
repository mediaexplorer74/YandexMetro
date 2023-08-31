// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Events.TilesReadyEventArgs`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository.Events
{
  internal class TilesReadyEventArgs<T> : EventArgs where T : ITile
  {
    public TilesReadyEventArgs(IList<ITileInfo> requestedTiles, IList<T> tiles)
      : this(requestedTiles, tiles, (object) null)
    {
    }

    public TilesReadyEventArgs(IList<ITileInfo> requestedTiles, IList<T> tiles, object status)
    {
      if (tiles == null)
        throw new ArgumentNullException(nameof (tiles));
      if (requestedTiles == null)
        throw new ArgumentNullException(nameof (requestedTiles));
      this.Tiles = tiles;
      this.RequestedTiles = requestedTiles;
      this.Status = status;
    }

    public object Status { get; private set; }

    public IList<T> Tiles { get; private set; }

    public IList<ITileInfo> RequestedTiles { get; private set; }
  }
}
