// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.Interfaces.IMetaContentLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using Yandex.Maps.Controls.ContentLayers;
using Yandex.Maps.Controls.Events;
using Yandex.Positioning;

namespace Yandex.Maps.ContentLayers.Interfaces
{
  [ComVisible(false)]
  public interface IMetaContentLayer : IContentLayer, IContentContainer
  {
    bool PanoramaAvailable { get; }

    bool TrafficAvailable { get; set; }

    event EventHandler<TrafficAvailableChangedArgs> TrafficAvailbleChanged;

    bool IsRoutingEnabledInCoordinates(GeoCoordinate coordinates);

    [NotNull]
    ITileLoadedNotifier GetTileByCoordinates(GeoCoordinate coordinates);
  }
}
