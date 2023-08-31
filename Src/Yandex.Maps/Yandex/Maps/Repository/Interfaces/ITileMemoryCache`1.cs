// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.ITileMemoryCache`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface ITileMemoryCache<T> : 
    ITileCache<T>,
    IAsyncTileStorage<T>,
    ITileStorage<T>,
    IInitializable,
    IFlusheable
    where T : class, ITile
  {
    IList<T> GetAllTiles();
  }
}
