// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.MetricDistanceFormatterUtil
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Properties;
using Yandex.StringUtils.Interfaces;

namespace Yandex.StringUtils
{
  internal class MetricDistanceFormatterUtil : DistanceFormatterUtil, IDistanceFormatterUtil
  {
    private readonly Dictionary<string, double> _mesures = new Dictionary<string, double>()
    {
      {
        Resources.MesureUnitMeterShort,
        1.0
      },
      {
        Resources.MesureUnitKiloMetreShort,
        1000.0
      }
    };

    public override string GetDistanceString(double distance, bool almostEquals) => DistanceFormatterUtil.GetDistanceString(distance, this._mesures, almostEquals);

    public override AnnotatedValue<double> GetDistance(double distance) => DistanceFormatterUtil.GetDistance(distance, this._mesures);
  }
}
