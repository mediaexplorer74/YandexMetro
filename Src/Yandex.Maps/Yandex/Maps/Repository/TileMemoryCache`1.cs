// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.TileMemoryCache`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.EventArgs;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Repository
{
  internal class TileMemoryCache<T> : 
    ITileMemoryCache<T>,
    ITileCache<T>,
    IAsyncTileStorage<T>,
    ITileStorage<T>,
    IInitializable,
    IFlusheable
    where T : class, ITile
  {
    private readonly IDictionary<ITileInfo, T> _cache;
    private readonly ITileInfoNormalizer _tileInfoConverter;

    public event EventHandler<TilesReadyEventArgs<T>> GetTilesComplete;

    public event EventHandler<AddTilesCompleteEventArgs<T>> AddTilesComplete;

    public TileMemoryCache(IDictionary<ITileInfo, T> cache, ITileInfoNormalizer tileInfoConverter)
    {
      if (cache == null)
        throw new ArgumentNullException(nameof (cache));
      if (tileInfoConverter == null)
        throw new ArgumentNullException(nameof (tileInfoConverter));
      this._cache = cache;
      this._tileInfoConverter = tileInfoConverter;
    }

    public T this[ITileInfo tileInfo]
    {
      set
      {
        lock (this._cache)
          this._cache[this._tileInfoConverter.ConvertToFiniteCoordinates(tileInfo)] = value;
      }
      get
      {
        T obj;
        if (!this._cache.TryGetValue(tileInfo, out obj))
          obj = default (T);
        return obj;
      }
    }

    public bool ContainsKey(ITileInfo tileInfo) => this._cache.ContainsKey(tileInfo);

    public void BeginAddTiles(IList<T> tiles) => throw new NotSupportedException();

    public void BeginGetTiles(IList<ITileInfo> tileInfos) => throw new NotSupportedException();

    public void Clear()
    {
      lock (this._cache)
        this._cache.Clear();
    }

    public void RemoveRange([NotNull] IList<ITileInfo> tileInfos)
    {
      if (tileInfos == null)
        throw new ArgumentNullException(nameof (tileInfos));
      foreach (ITileInfo tileInfo in (IEnumerable<ITileInfo>) tileInfos)
        this._cache.Remove(tileInfo);
    }

    public void Remove([NotNull] Func<T, bool> removeCondition)
    {
      lock (this._cache)
      {
        foreach (T obj in this._cache.Values.Where<T>(removeCondition).ToList<T>())
          this._cache.Remove(obj.TileInfo);
      }
    }

    public void WriteTiles(IList<T> tiles)
    {
      foreach (T tile in (IEnumerable<T>) tiles)
        this[tile.TileInfo] = tile;
    }

    public IList<T> ReadTiles(IList<ITileInfo> tileInfos)
    {
      List<T> objList = new List<T>();
      foreach (ITileInfo tileInfo in (IEnumerable<ITileInfo>) tileInfos)
      {
        T obj;
        if (this._cache.TryGetValue(tileInfo, out obj))
          objList.Add(obj);
      }
      return (IList<T>) objList;
    }

    public void Flush()
    {
    }

    public void Initialize()
    {
    }

    public IList<T> GetAllTiles()
    {
      lock (this._cache)
        return (IList<T>) this._cache.Values.ToList<T>();
    }
  }
}
