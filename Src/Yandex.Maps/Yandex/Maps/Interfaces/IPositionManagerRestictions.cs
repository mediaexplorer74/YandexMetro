// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IPositionManagerRestictions
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Media;

namespace Yandex.Maps.Interfaces
{
  internal interface IPositionManagerRestictions
  {
    Rect Viewport { get; set; }

    void UpdateRelativePointIfViewportIsOutOfMap(ref Point relativePoint, double zoomLevel);

    bool MapIsGreaterThanViewport(byte zoomLevel);

    bool CheckRelativePointIfViewportIsOutOfMap(Point relativePoint, double zoomLevel);

    Point VelocityRestriction(Point velocityInPixelsPerMillisecond);
  }
}
