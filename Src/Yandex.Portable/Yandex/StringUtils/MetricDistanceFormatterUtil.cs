// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.MetricDistanceFormatterUtil
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System.Collections.Generic;
using Yandex.Properties;
using Yandex.StringUtils.Interfaces;

namespace Yandex.StringUtils
{
  public class MetricDistanceFormatterUtil : DistanceFormatterUtil, IDistanceFormatterUtil
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
