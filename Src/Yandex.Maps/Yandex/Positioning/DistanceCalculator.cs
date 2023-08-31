// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.DistanceCalculator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Positioning.Interfaces;
using Yandex.StringUtils.Interfaces;

namespace Yandex.Positioning
{
  internal class DistanceCalculator : IDistanceCalculator
  {
    private readonly IDistanceFormatterUtil _distanceFormatterUtil;
    private readonly IPositionWatcher _positionWatcher;

    public DistanceCalculator(
      [NotNull] IDistanceFormatterUtil distanceFormatterUtil,
      [NotNull] IPositionWatcher positionWatcher)
    {
      if (distanceFormatterUtil == null)
        throw new ArgumentNullException(nameof (distanceFormatterUtil));
      if (positionWatcher == null)
        throw new ArgumentNullException(nameof (positionWatcher));
      this._distanceFormatterUtil = distanceFormatterUtil;
      this._positionWatcher = positionWatcher;
    }

    public string GetStringDistanceTo(GeoCoordinate point) => this.GetStringDistanceTo(point, true);

    public string GetStringDistanceTo(GeoCoordinate point, bool almostEquals) => point == null || this._positionWatcher.Status != GeoPositionStatus.Ready ? (string) null : this._distanceFormatterUtil.GetDistanceString(point.GetDistanceTo(this._positionWatcher.LastPosition.GeoCoordinate), almostEquals);
  }
}
