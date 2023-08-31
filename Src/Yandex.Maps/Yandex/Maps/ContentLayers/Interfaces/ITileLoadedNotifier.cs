// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.Interfaces.ITileLoadedNotifier
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Events;

namespace Yandex.Maps.ContentLayers.Interfaces
{
  internal interface ITileLoadedNotifier
  {
    event EventHandler<TileLoadedEventArgs> Loaded;

    ITile Tile { get; }

    ITileInfo TileInfo { get; }
  }
}
