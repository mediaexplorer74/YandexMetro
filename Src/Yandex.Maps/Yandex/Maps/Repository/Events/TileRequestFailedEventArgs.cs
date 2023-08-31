// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Events.TileRequestFailedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository.Events
{
  internal class TileRequestFailedEventArgs : EventArgs
  {
    public TileRequestFailedEventArgs(IList<ITileInfo> requestedTiles)
      : this(requestedTiles, (object) null)
    {
    }

    public TileRequestFailedEventArgs(IList<ITileInfo> requestedTiles, object status)
    {
      this.RequestedTiles = requestedTiles != null ? requestedTiles : throw new ArgumentNullException(nameof (requestedTiles));
      this.Status = status;
    }

    public object Status { get; private set; }

    public IList<ITileInfo> RequestedTiles { get; private set; }
  }
}
