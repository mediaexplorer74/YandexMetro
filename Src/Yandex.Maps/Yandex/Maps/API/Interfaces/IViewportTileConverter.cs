// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.IViewportTileConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface IViewportTileConverter
  {
    ITileInfo GetTileByPoint(Point point, byte zoom);

    Point GetTileTopLeftPoint(ITileInfo tileInfo);

    IEnumerable<ITileInfo> GetTilesByArea(Rect visibleArea, byte zoom);

    Point ZoomPoint(Point point, uint oldZoom, byte zoom);

    IEnumerable<ITileInfo> ViewportRectToTiles(ViewportRect viewportRect, byte zoom);

    Rect GetTilesArea(ViewportRect visibleArea, byte zoom);
  }
}
