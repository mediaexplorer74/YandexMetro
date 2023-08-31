// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IMapViewBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.ContentLayers;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.Interfaces
{
  internal interface IMapViewBase : IMapState, ICompositeTransformation
  {
    event EventHandler ViewAreaChanged;

    void UpdateProjection();

    Size ActualSize { get; }

    Rect Viewport { get; }

    IMapContentLayer MapContentLayer { get; }

    LayerManager LayerManager { get; }
  }
}
