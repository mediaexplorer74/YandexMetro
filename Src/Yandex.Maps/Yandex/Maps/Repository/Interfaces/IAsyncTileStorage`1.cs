// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.IAsyncTileStorage`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.EventArgs;
using Yandex.Maps.Repository.Events;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface IAsyncTileStorage<T> where T : ITile
  {
    event EventHandler<TilesReadyEventArgs<T>> GetTilesComplete;

    event EventHandler<AddTilesCompleteEventArgs<T>> AddTilesComplete;

    void BeginAddTiles(IList<T> tiles);

    void BeginGetTiles(IList<ITileInfo> tileInfos);
  }
}
