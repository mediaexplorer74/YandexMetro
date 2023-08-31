// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.VisibleTileInfoLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  internal class VisibleTileInfoLayer : IVisibleTileInfoLayer
  {
    private readonly IDictionary<ITileInfo, bool> _visibleTiles;

    public VisibleTileInfoLayer() => this._visibleTiles = (IDictionary<ITileInfo, bool>) new Dictionary<ITileInfo, bool>();

    public IEnumerable<ITileInfo> VisibleTileInfos => this._visibleTiles.Keys.AsEnumerable<ITileInfo>();

    public virtual void DisposeTiles() => this._visibleTiles.Clear();

    public virtual void DisposeTilesExcept(IEnumerable<ITileInfo> tiles) => this.RemoveTilesToDispose((IEnumerable<ITileInfo>) tiles.ToList<ITileInfo>());

    public virtual void DisplayTiles(IEnumerable<ITileInfo> tiles)
    {
      foreach (ITileInfo tile in tiles)
        this._visibleTiles[tile] = true;
    }

    protected IEnumerable<ITileInfo> RemoveTilesToDispose(IEnumerable<ITileInfo> tiles)
    {
      List<ITileInfo> list = this._visibleTiles.Keys.Where<ITileInfo>((Func<ITileInfo, bool>) (item => !tiles.Any<ITileInfo>((Func<ITileInfo, bool>) (tile => tile.EqualsCoordinates(item))))).ToList<ITileInfo>();
      foreach (ITileInfo key in list)
        this._visibleTiles.Remove(key);
      return (IEnumerable<ITileInfo>) list;
    }
  }
}
