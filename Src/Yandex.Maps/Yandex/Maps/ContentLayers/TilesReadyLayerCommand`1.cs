// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TilesReadyLayerCommand`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls;

namespace Yandex.Maps.ContentLayers
{
  internal class TilesReadyLayerCommand<T> : LayerCommand where T : ITile
  {
    public IList<T> ReadyTiles { get; private set; }

    public TilesReadyLayerCommand(IList<T> readyTiles)
      : base(LayerCommandTypes.Custom)
    {
      this.ReadyTiles = readyTiles;
    }
  }
}
