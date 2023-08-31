// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.IViewportPointConveter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface IViewportPointConveter
  {
    Point ViewportPointToRelativePoint(ViewportPoint viewportPoint);

    Point RelativePointToViewportPoint(Point relativePoint, double zoomLevel);

    Rect RelativeRectToViewportRect(Rect relativeRect, double zoomLevel);

    Point CoordinatesToViewportPoint(GeoCoordinate value, double zoomLevel);

    GeoCoordinate ViewportPointToCoordinates(Point point, double zoomLevel);
  }
}
