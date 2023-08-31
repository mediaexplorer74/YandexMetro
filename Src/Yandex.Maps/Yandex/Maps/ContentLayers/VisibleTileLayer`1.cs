// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.VisibleTileLayer`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  internal class VisibleTileLayer<TTile> : IVisibleTileLayer<TTile> where TTile : ITile
  {
    private readonly IDictionary<ITileInfo, TTile> _visibleTiles;
    private readonly object _visibleTilesLock = new object();

    public VisibleTileLayer() => this._visibleTiles = (IDictionary<ITileInfo, TTile>) new Dictionary<ITileInfo, TTile>();

    public IList<ITileInfo> VisibleTileInfos
    {
      get
      {
        lock (this._visibleTilesLock)
          return (IList<ITileInfo>) this._visibleTiles.Keys.ToList<ITileInfo>();
      }
    }

    public virtual void DisposeTiles() => this.OnDisposeTiles();

    protected virtual void OnDisposeTiles()
    {
      lock (this._visibleTilesLock)
        this._visibleTiles.Clear();
      this.OnVisibleTilesChanged();
    }

    public virtual IList<ITileInfo> DisposeTilesExcept(IList<ITileInfo> tiles)
    {
      IList<ITileInfo> list;
      lock (this._visibleTilesLock)
      {
        list = (IList<ITileInfo>) this._visibleTiles.Keys.Where<ITileInfo>((Func<ITileInfo, bool>) (item => !tiles.Any<ITileInfo>((Func<ITileInfo, bool>) (tile => tile.EqualsCoordinates(item))))).ToList<ITileInfo>();
        foreach (ITileInfo key in (IEnumerable<ITileInfo>) list)
          this._visibleTiles.Remove(key);
      }
      this.OnVisibleTilesChanged();
      return list;
    }

    public void DisplayTiles(IList<TTile> tiles) => this.OnDisplayTiles(tiles);

    protected virtual void OnDisplayTiles([NotNull] IList<TTile> tiles)
    {
      lock (this._visibleTilesLock)
      {
        foreach (TTile tile in (IEnumerable<TTile>) tiles)
          this._visibleTiles[tile.TileInfo] = tile;
      }
      this.OnVisibleTilesChanged();
    }

    public bool TryGetValue(ITileInfo keyTileInfo, out TTile tile)
    {
      lock (this._visibleTilesLock)
        return this._visibleTiles.TryGetValue(keyTileInfo, out tile);
    }

    public IList<TTile> VisibleTiles
    {
      get
      {
        lock (this._visibleTilesLock)
          return (IList<TTile>) this._visibleTiles.Values.ToList<TTile>();
      }
    }

    public void DisposeTiles(IList<ITileInfo> tiles) => this.OnDisposeTiles(tiles);

    protected virtual void OnDisposeTiles(IList<ITileInfo> tiles)
    {
      lock (this._visibleTilesLock)
      {
        foreach (ITileInfo key in this._visibleTiles.Keys.Where<ITileInfo>((Func<ITileInfo, bool>) (item => tiles.Any<ITileInfo>((Func<ITileInfo, bool>) (tile => tile.EqualsCoordinates(item))))).ToList<ITileInfo>())
          this._visibleTiles.Remove(key);
      }
      this.OnVisibleTilesChanged();
    }

    public event EventHandler VisibleTilesChanged;

    protected void OnVisibleTilesChanged()
    {
      EventHandler visibleTilesChanged = this.VisibleTilesChanged;
      if (visibleTilesChanged == null)
        return;
      visibleTilesChanged((object) this, EventArgs.Empty);
    }
  }
}
