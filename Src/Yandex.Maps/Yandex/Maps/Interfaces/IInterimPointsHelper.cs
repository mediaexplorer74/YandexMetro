// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IInterimPointsHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.PositionManager.InterimPointsEnumerators.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.Interfaces
{
  internal interface IInterimPointsHelper
  {
    IInterimPointsEnumerator GetInterimPointsEnumerator(
      Position currentPosition,
      Position targetPosition,
      Point? relativeScaleCenter);

    bool DistanceIsSuitableForAnimation(Position currentPosition, Position targetPosition);

    TimeSpan StepTimeout { get; }

    double[] GetInterimPoints(double start, double end, int pointsNumber);

    Point[] GetInterimPoints(Point start, Point end, int pointsNumber);

    IInterimPointsEnumerator GetVelocityInterimPointsEnumerator(
      Position currentPosition,
      Point velocityRelativePoint);
  }
}
