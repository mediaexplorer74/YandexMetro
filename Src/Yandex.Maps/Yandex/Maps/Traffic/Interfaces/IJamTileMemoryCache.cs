// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamTileMemoryCache
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Traffic.Interfaces
{
  [ComVisible(false)]
  public interface IJamTileMemoryCache : 
    ITileCache<IJamTile>,
    IAsyncTileStorage<IJamTile>,
    ITileStorage<IJamTile>,
    IInitializable,
    IFlusheable
  {
  }
}
