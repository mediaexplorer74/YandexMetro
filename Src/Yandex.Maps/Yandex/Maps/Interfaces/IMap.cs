// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IMap
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.Interfaces
{
  internal interface IMap : IMapState
  {
    Rect PlainViewPort { get; }

    bool IsTrafficEnabled { get; set; }

    void ClearPersistentCache();

    GeoPositionStatus JumpToCurrentLocation();

    void ZoomIn();

    void ZoomOut();
  }
}
