// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.Interfaces.IVisibleTileLayer`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.ContentLayers.Interfaces
{
  internal interface IVisibleTileLayer<TTile> where TTile : ITile
  {
    void DisposeTiles();

    IList<ITileInfo> DisposeTilesExcept(IList<ITileInfo> tiles);

    IList<ITileInfo> VisibleTileInfos { get; }

    IList<TTile> VisibleTiles { get; }

    void DisplayTiles(IList<TTile> tiles);

    bool TryGetValue(ITileInfo keyTileInfo, out TTile tile);

    event EventHandler VisibleTilesChanged;
  }
}
