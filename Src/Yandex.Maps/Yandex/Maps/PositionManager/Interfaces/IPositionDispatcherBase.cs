// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.Interfaces.IPositionDispatcherBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Media;

namespace Yandex.Maps.PositionManager.Interfaces
{
  internal interface IPositionDispatcherBase : IDisposable
  {
    bool IsUserInteractionEnabled { get; set; }

    AnimationLevel AnimationLevel { get; set; }

    event EventHandler<PositionChangedEventArgs> PositionChanged;

    event EventHandler<OperationStatusChangedEventArgs> OperationStatusChanged;

    void ResetPositionFollowing();

    void MoveTo(Point relativeTargetPoint);

    void ZoomTo(double newZoomLevel, Point? screenScaleCenter = null);

    void MapPositionChanged(double zoomLevel, Rect viewport);

    void ResendPositionChangedEvent();

    void ZoomIn(Point screenScaleCenter);

    void ZoomInWithCurrentPositionAsScaleCenter();

    void ZoomOut(Point screenScaleCenter);

    void ZoomOutWithCurrentPositionAsScaleCenter();

    Position TargetPosition { get; }
  }
}
