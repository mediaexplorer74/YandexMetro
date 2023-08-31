// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TileZoomLayerBuilder`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Ioc;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.IoC;

namespace Yandex.Maps.ContentLayers
{
  internal class TileZoomLayerBuilder<T> : ITileZoomLayerBuilder<T> where T : ITile
  {
    public ITileZoomLayer<T> CreateLayer(byte zoom) => IocSingleton<ControlsIocInitializer>.Container.Resolve<ITileZoomLayer<T>>((IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        nameof (zoom),
        (object) zoom
      }
    });
  }
}
