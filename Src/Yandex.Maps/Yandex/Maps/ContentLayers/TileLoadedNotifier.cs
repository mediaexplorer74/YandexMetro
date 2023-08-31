// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TileLoadedNotifier
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Events;
using Yandex.Maps.ContentLayers.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  internal class TileLoadedNotifier : ITileLoadedNotifier
  {
    public event EventHandler<TileLoadedEventArgs> Loaded;

    public ITile Tile { get; internal set; }

    public ITileInfo TileInfo { get; internal set; }

    internal void OnLoaded(ITile tile)
    {
      EventHandler<TileLoadedEventArgs> loaded = this.Loaded;
      if (loaded == null)
        return;
      loaded((object) this, new TileLoadedEventArgs()
      {
        Tile = tile
      });
    }
  }
}
