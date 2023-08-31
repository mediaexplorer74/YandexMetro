// Decompiled with JetBrains decompiler
// Type: Yandex.Input.WeightedSumVelocityCalculator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Yandex.Input.Interfaces;
using Yandex.Media;

namespace Yandex.Input
{
  internal class WeightedSumVelocityCalculator : IVelocityCalculator
  {
    private const double TwoPointsLastPointWeight = 0.2;
    private const double TwoPoints0PointWeight = 0.8;
    private const double ThreeOrMorePointsLastPointWeight = 0.2;
    private const double ThreeOrMorePointsLastBut1PointWeight = 0.7;
    private const double ThreeOrMorePointsLastBut2PointWeight = 0.10000000000000009;

    public Point CalculateVelocity([NotNull] IList<Point> manipulationVelocities)
    {
      int num = manipulationVelocities != null ? manipulationVelocities.Count : throw new ArgumentNullException(nameof (manipulationVelocities));
      switch (num)
      {
        case 0:
          return new Point();
        case 1:
          return manipulationVelocities[0];
        case 2:
          return new Point(manipulationVelocities[1].X * 0.2 + manipulationVelocities[0].X * 0.8, manipulationVelocities[1].Y * 0.2 + manipulationVelocities[0].Y * 0.8);
        default:
          return new Point(manipulationVelocities[num - 1].X * 0.2 + manipulationVelocities[num - 2].X * 0.7 + manipulationVelocities[num - 3].X * 0.10000000000000009, manipulationVelocities[num - 1].Y * 0.2 + manipulationVelocities[num - 2].Y * 0.7 + manipulationVelocities[num - 3].Y * 0.10000000000000009);
      }
    }
  }
}
