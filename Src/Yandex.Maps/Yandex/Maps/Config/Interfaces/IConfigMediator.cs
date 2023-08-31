// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.Interfaces.IConfigMediator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Positioning;

namespace Yandex.Maps.Config.Interfaces
{
  internal interface IConfigMediator : INotifyPropertyChanged
  {
    string Uuid { get; set; }

    [NotNull]
    QueryHosts QueryHosts { get; set; }

    IDictionary<BaseLayers, Yandex.Maps.Config.MapLayer> MapLayers { get; set; }

    bool EnableLocationService { get; set; }

    GeoCoordinate DefaultPosition { get; set; }

    TimeSpan TileReloadTimeout { get; }

    JamCollectConfig JamCollectConfig { get; set; }

    OperatorConfig OperatorConfig { get; set; }

    bool CollectJamInformation { get; set; }

    double TilesStretchFactor { get; set; }

    IDisplayTileSize DisplayTileSize { get; }

    string ApiKey { get; set; }

    bool TwilightModeEnabled { get; set; }

    TileType TileType { get; }

    ObjectShowInterval ScaleLineShowInterval { get; set; }

    byte ZoomLevelsToDisplaySimultaneouslyDefault { get; set; }

    bool IsUserPoiEnabled { get; set; }
  }
}
