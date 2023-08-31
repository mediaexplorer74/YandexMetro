// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.Interfaces.IPositionManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Media;

namespace Yandex.Maps.PositionManager.Interfaces
{
  internal interface IPositionManager : IDisposable
  {
    [NotNull]
    Position TargetPosition { get; }

    Position CurrentPosition { get; }

    AnimationLevel AnimationLevel { get; set; }

    Rect Viewport { get; set; }

    bool IsEnumerationInProcess { get; }

    bool MoveTo(Point relativeTargetPoint, bool withAnimation = true);

    bool ZoomTo(double newZoomLevel, Point? screenScaleCenter = null, bool withAnimation = true);

    void MoveByVelocity(Point velocityInPixelsPerMillisecond);

    event EventHandler<PositionChangedEventArgs> PositionChanged;

    void FinalizeLastQueueIfExists();

    event EventHandler<PositionChangedEventArgs> PointsEnumerationFinished;
  }
}
