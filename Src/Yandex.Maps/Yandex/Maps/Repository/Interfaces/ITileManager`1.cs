// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.ITileManager`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Patterns;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface ITileManager<T> : IFlusheable where T : ITile
  {
    event EventHandler<TilesReadyEventArgs<T>> TilesReady;

    void QueryTiles(IEnumerable<ITileInfo> infiniteTiles);

    void CancelQueryingTilesExcept(IList<ITileInfo> infiniteTiles);

    void RemoveTiles(Func<T, bool> removeCondition);

    IAsyncResult ClearPersistentCache(AsyncCallback callback, object state);

    OperationMode OperationMode { get; set; }

    void ResetCachedTileBitmaps();
  }
}
