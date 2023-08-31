// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.DrawManagerCommand
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Media.Imaging;

namespace Yandex.Maps
{
  internal class DrawManagerCommand : IDrawManagerCommand
  {
    public DrawManagerCommand(DrawManagerCommandTypes bitmapManagerCommandType) => this.Type = bitmapManagerCommandType;

    public DrawManagerCommandTypes Type { get; private set; }

    public IEnumerable<ITileInfo> TileInfos { get; set; }

    public IEnumerable<ITile> Tiles { get; set; }

    public RenderContentMode RenderContentMode { get; set; }
  }
}
