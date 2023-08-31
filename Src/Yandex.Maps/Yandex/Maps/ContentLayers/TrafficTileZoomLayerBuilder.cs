// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TrafficTileZoomLayerBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Collections.Generic;
using Yandex.Ioc;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.Traffic.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [UsedImplicitly]
  internal class TrafficTileZoomLayerBuilder : 
    ITrafficTileZoomLayerBuilder,
    ITileZoomLayerBuilder<IJamTile>
  {
    public ITileZoomLayer<IJamTile> CreateLayer(byte zoom) => IocSingleton<ControlsIocInitializer>.Container.Resolve<ITileZoomLayer<IJamTile>>((IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        nameof (zoom),
        (object) zoom
      }
    });

    public ITileZoomLayer<IJamTile> CreateLayer(byte zoom, IJamRender jamRender) => IocSingleton<ControlsIocInitializer>.Container.Resolve<ITileZoomLayer<IJamTile>>((IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        nameof (zoom),
        (object) zoom
      },
      {
        nameof (jamRender),
        (object) jamRender
      }
    });
  }
}
