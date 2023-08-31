// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.Interfaces.IPositionWatcherWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Controls.PositionManager.Event;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.PositionManager.Interfaces
{
  internal interface IPositionWatcherWrapper
  {
    Point? PinnedMarkerPointCenterScreenOffset { get; }

    [CanBeNull]
    GeoCoordinate LastCoordinates { get; }

    GeoPositionStatus Status { get; }

    void ResetPositionFollowing();

    void DisablePositionFollowing();

    event EventHandler PositionChangedWithoutMapMoving;

    void PositionChanged(double zoomLevel, Rect viewport);

    event EventHandler<PositionMoveEventArgs> PositionMove;

    void ScheduleJumpToCurrentLocation();
  }
}
