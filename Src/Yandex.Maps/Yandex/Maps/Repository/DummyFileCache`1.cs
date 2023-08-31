// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.DummyFileCache`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.EventArgs;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Repository
{
  internal class DummyFileCache<T> : 
    ITileFileCache<T>,
    ITileCache<T>,
    IAsyncTileStorage<T>,
    ITileStorage<T>,
    IInitializable,
    IFlusheable
    where T : class, ITile
  {
    public event EventHandler<TilesReadyEventArgs<T>> GetTilesComplete;

    public void OnGetTilesComplete(TilesReadyEventArgs<T> e)
    {
      EventHandler<TilesReadyEventArgs<T>> getTilesComplete = this.GetTilesComplete;
      if (getTilesComplete == null)
        return;
      getTilesComplete((object) this, e);
    }

    public event EventHandler<AddTilesCompleteEventArgs<T>> AddTilesComplete;

    public void BeginAddTiles(IList<T> tiles)
    {
    }

    public void BeginGetTiles(IList<ITileInfo> tileInfos) => this.OnGetTilesComplete(new TilesReadyEventArgs<T>(tileInfos, (IList<T>) new T[0]));

    public void WriteTiles(IList<T> saveTiles)
    {
    }

    public IList<T> ReadTiles(IList<ITileInfo> tiles) => (IList<T>) new T[0];

    public void Flush()
    {
    }

    public void Initialize()
    {
    }

    public T this[ITileInfo tileInfo]
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public bool ContainsKey(ITileInfo tileInfo) => false;

    public void Clear()
    {
    }

    public void RemoveRange(IList<ITileInfo> tileInfos)
    {
    }

    public void Remove(Func<T, bool> removeCondition)
    {
    }
  }
}
