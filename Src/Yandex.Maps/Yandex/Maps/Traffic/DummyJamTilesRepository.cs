// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DummyJamTilesRepository
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.EventArgs;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Traffic
{
  internal class DummyJamTilesRepository : 
    ITileRepository<IJamTile>,
    IAsyncTileStorage<IJamTile>,
    IFlusheable
  {
    public bool ContainsKey(ITileInfo tileInfo, BaseLayers layers) => false;

    public IJamTile this[ITileInfo tileInfo]
    {
      get => throw new NotSupportedException();
      set
      {
      }
    }

    public void Flush()
    {
    }

    public IAsyncResult ClearPersistentCache(AsyncCallback callback, object state) => throw new NotSupportedException();

    public void RemoveRange(IList<ITileInfo> tileInfos)
    {
    }

    public void Remove(Func<IJamTile, bool> removeCondition)
    {
    }

    public void ResetCachedTileBitmaps()
    {
    }

    public event EventHandler<TilesReadyEventArgs<IJamTile>> GetTilesComplete;

    protected virtual void OnGetTilesComplete(
      IList<ITileInfo> requestedTiles,
      IList<IJamTile> tiles)
    {
      EventHandler<TilesReadyEventArgs<IJamTile>> getTilesComplete = this.GetTilesComplete;
      if (getTilesComplete == null)
        return;
      getTilesComplete((object) this, new TilesReadyEventArgs<IJamTile>(requestedTiles, tiles));
    }

    public event EventHandler<AddTilesCompleteEventArgs<IJamTile>> AddTilesComplete;

    protected virtual void OnAddTilesComplete(IEnumerable<IJamTile> tiles)
    {
      EventHandler<AddTilesCompleteEventArgs<IJamTile>> addTilesComplete = this.AddTilesComplete;
      if (addTilesComplete == null)
        return;
      addTilesComplete((object) this, new AddTilesCompleteEventArgs<IJamTile>(tiles));
    }

    public void BeginAddTiles(IList<IJamTile> tiles) => this.OnAddTilesComplete((IEnumerable<IJamTile>) tiles);

    public void BeginGetTiles(IList<ITileInfo> tileInfos) => this.OnGetTilesComplete(tileInfos, (IList<IJamTile>) new IJamTile[0]);
  }
}
