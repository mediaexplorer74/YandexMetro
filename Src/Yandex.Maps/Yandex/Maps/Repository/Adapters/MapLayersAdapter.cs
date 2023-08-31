// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Adapters.MapLayersAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API;
using Yandex.Maps.API.Adapters;

namespace Yandex.Maps.Repository.Adapters
{
  internal class MapLayersAdapter
  {
    public static Dictionary<BaseLayers, Yandex.Maps.Repository.Config.MapLayer> Convert(
      IEnumerable<Yandex.Maps.Config.MapLayer> mapLayers)
    {
      return mapLayers.ToDictionary<Yandex.Maps.Config.MapLayer, BaseLayers, Yandex.Maps.Repository.Config.MapLayer>((Func<Yandex.Maps.Config.MapLayer, BaseLayers>) (item => item.Layer), (Func<Yandex.Maps.Config.MapLayer, Yandex.Maps.Repository.Config.MapLayer>) (item => new Yandex.Maps.Repository.Config.MapLayer()
      {
        Id = item.Id,
        Name = item.Name,
        Service = item.IsService ? 1 : 0,
        Version = (int) item.MapVersion,
        TilePixelSize = item.SizeInPixels,
        Request = LayerAdapter.LayerToString(item.Layer)
      }));
    }
  }
}
