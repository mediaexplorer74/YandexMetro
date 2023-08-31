// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.DummyPositionWatcherWraper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.Controls.PositionManager.Event;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.PositionManager
{
  internal class DummyPositionWatcherWraper : IPositionWatcherWrapper
  {
    public Point? PinnedMarkerPointCenterScreenOffset => new Point?();

    public GeoCoordinate LastCoordinates => new GeoCoordinate(55.733837, 37.588088);

    public GeoPositionStatus Status => GeoPositionStatus.Ready;

    public void ResetPositionFollowing()
    {
    }

    public void DisablePositionFollowing()
    {
    }

    public event EventHandler PositionChangedWithoutMapMoving;

    public void PositionChanged(double zoomLevel, Rect viewport)
    {
    }

    public event EventHandler<PositionMoveEventArgs> PositionMove;

    public void ScheduleJumpToCurrentLocation()
    {
    }
  }
}
