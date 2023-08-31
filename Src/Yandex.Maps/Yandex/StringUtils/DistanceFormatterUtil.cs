// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.DistanceFormatterUtil
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Yandex.Properties;

namespace Yandex.StringUtils
{
  internal abstract class DistanceFormatterUtil
  {
    private const string AlmostEqualsSimbol = "~";
    private const int MaxDecimals = 2;

    public abstract string GetDistanceString(double distance, bool almostEquals);

    public abstract AnnotatedValue<double> GetDistance(double distance);

    protected static AnnotatedValue<double> GetDistance(
      double distance,
      Dictionary<string, double> dividers)
    {
      foreach (KeyValuePair<string, double> divider in dividers)
      {
        distance /= divider.Value;
        double? nextDivider = DistanceFormatterUtil.GetNextDivider((IEnumerable<KeyValuePair<string, double>>) dividers, divider.Key);
        if (nextDivider.HasValue)
        {
          double num = distance;
          double? nullable = nextDivider;
          if ((num >= nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
            continue;
        }
        return new AnnotatedValue<double>(DistanceFormatterUtil.RoundDistance(distance), divider.Key);
      }
      return new AnnotatedValue<double>(DistanceFormatterUtil.RoundDistance(distance), dividers.Last<KeyValuePair<string, double>>().Key);
    }

    private static double RoundDistance(double distance)
    {
      int num1 = (int) Math.Log10(distance);
      int digits = 2 - num1;
      if (digits > 0)
        return Math.Round(distance, digits);
      double num2 = Math.Round(distance);
      if (num1 <= 1)
        return num2;
      return num1 == 2 ? Math.Round(num2 * 0.1) * 10.0 : Math.Round(num2 * 0.01) * 100.0;
    }

    protected static string GetDistanceString(
      double distance,
      Dictionary<string, double> dividers,
      bool almostEquals)
    {
      AnnotatedValue<double> distance1 = DistanceFormatterUtil.GetDistance(distance, dividers);
      return DistanceFormatterUtil.GetStringRepresentation(distance1.Value, distance1.Annotation, almostEquals);
    }

    private static double? GetNextDivider(
      IEnumerable<KeyValuePair<string, double>> dividers,
      string key)
    {
      if (dividers == null)
        throw new ArgumentNullException(nameof (dividers));
      bool flag = false;
      foreach (KeyValuePair<string, double> divider in dividers)
      {
        if (flag)
          return new double?(divider.Value);
        if (divider.Key == key)
          flag = true;
      }
      return new double?();
    }

    private static string GetStringRepresentation(
      double distance,
      string mesure,
      bool almostEquals)
    {
      string stringRepresentation = string.Format(Resources.PairFormat, (object) distance.ToString((IFormatProvider) CultureInfo.CurrentUICulture), (object) mesure);
      if (almostEquals)
        stringRepresentation = "~" + stringRepresentation;
      return stringRepresentation;
    }
  }
}
