// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamTileMemoryCache
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Collections;
using Yandex.Collections.Interfaces;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.EventArgs;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Traffic
{
  internal class JamTileMemoryCache : 
    IJamTileMemoryCache,
    ITileCache<IJamTile>,
    IAsyncTileStorage<IJamTile>,
    ITileStorage<IJamTile>,
    IInitializable,
    IFlusheable
  {
    private const int Capacity = 30;
    private readonly IMemoryCache<ITileInfo, WeakTileHolder<IJamTile>> _cache = (IMemoryCache<ITileInfo, WeakTileHolder<IJamTile>>) new MemoryCache<ITileInfo, WeakTileHolder<IJamTile>>(30);

    public IJamTile this[ITileInfo tileInfo]
    {
      get => this._cache[tileInfo].GetStrongTile();
      set => this._cache[tileInfo] = new WeakTileHolder<IJamTile>(value);
    }

    public bool ContainsKey(ITileInfo tileInfo) => this._cache.ContainsKey(tileInfo);

    public void Clear() => this._cache.Clear();

    public void RemoveRange([NotNull] IList<ITileInfo> tileInfos)
    {
      if (tileInfos == null)
        throw new ArgumentNullException(nameof (tileInfos));
      foreach (ITileInfo tileInfo in (IEnumerable<ITileInfo>) tileInfos)
        ((IDictionary<ITileInfo, WeakTileHolder<IJamTile>>) this._cache).Remove(tileInfo);
    }

    public void Remove(Func<IJamTile, bool> removeCondition)
    {
      foreach (WeakTileHolder<IJamTile> weakTileHolder in this._cache.Values.Where<WeakTileHolder<IJamTile>>((Func<WeakTileHolder<IJamTile>, bool>) (value => removeCondition(value.GetStrongTile()))))
        ((IDictionary<ITileInfo, WeakTileHolder<IJamTile>>) this._cache).Remove(weakTileHolder.Key);
    }

    public event EventHandler<TilesReadyEventArgs<IJamTile>> GetTilesComplete;

    public event EventHandler<AddTilesCompleteEventArgs<IJamTile>> AddTilesComplete;

    public void BeginAddTiles(IList<IJamTile> tiles) => throw new NotImplementedException();

    public void BeginGetTiles(IList<ITileInfo> tileInfos) => throw new NotImplementedException();

    public void WriteTiles(IList<IJamTile> saveTiles)
    {
      foreach (IJamTile saveTile in (IEnumerable<IJamTile>) saveTiles)
        this._cache[saveTile.TileInfo] = new WeakTileHolder<IJamTile>(saveTile);
    }

    public IList<IJamTile> ReadTiles(IList<ITileInfo> tiles)
    {
      WeakTileHolder<IJamTile> value = (WeakTileHolder<IJamTile>) null;
      return (IList<IJamTile>) tiles.Where<ITileInfo>((Func<ITileInfo, bool>) (tile => this._cache.TryGetValue(tile, out value))).Select<ITileInfo, IJamTile>((Func<ITileInfo, IJamTile>) (tile => value.GetStrongTile())).ToList<IJamTile>();
    }

    public void Flush()
    {
    }

    public void Initialize()
    {
    }
  }
}
