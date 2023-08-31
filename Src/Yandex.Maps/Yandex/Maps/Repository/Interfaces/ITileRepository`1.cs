// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.ITileRepository`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface ITileRepository<T> : IAsyncTileStorage<T>, IFlusheable where T : ITile
  {
    bool ContainsKey(ITileInfo tileInfo, BaseLayers layers);

    T this[ITileInfo tileInfo] { get; set; }

    IAsyncResult ClearPersistentCache(AsyncCallback callback, object state);

    void RemoveRange(IList<ITileInfo> tileInfos);

    void Remove(Func<T, bool> removeCondition);

    void ResetCachedTileBitmaps();
  }
}
