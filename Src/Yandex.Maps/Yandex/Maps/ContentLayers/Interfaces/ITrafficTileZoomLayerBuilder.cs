﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.Interfaces.ITrafficTileZoomLayerBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Yandex.Maps.Traffic.Interfaces;

namespace Yandex.Maps.ContentLayers.Interfaces
{
  internal interface ITrafficTileZoomLayerBuilder : ITileZoomLayerBuilder<IJamTile>
  {
    [NotNull]
    ITileZoomLayer<IJamTile> CreateLayer(byte zoom, IJamRender jamRender);
  }
}
