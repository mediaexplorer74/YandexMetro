// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.Interfaces.IDrawManagerCommand
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Media.Imaging;

namespace Yandex.Maps.ContentLayers.Interfaces
{
  internal interface IDrawManagerCommand
  {
    DrawManagerCommandTypes Type { get; }

    IEnumerable<ITileInfo> TileInfos { get; }

    IEnumerable<ITile> Tiles { get; }

    RenderContentMode RenderContentMode { get; }
  }
}
