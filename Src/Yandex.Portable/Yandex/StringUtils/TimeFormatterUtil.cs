// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.TimeFormatterUtil
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Properties;
using Yandex.StringUtils.Interfaces;

namespace Yandex.StringUtils
{
  public class TimeFormatterUtil : ITimeFormatterUtil
  {
    private static readonly TimeSpan OneDayTimeSpan = new TimeSpan(23, 51, 0);

    public string GetTimeString(TimeSpan timeSpan) => string.Join(" ", this.GetTimeValue(timeSpan).Select<AnnotatedValue<int>, string>((Func<AnnotatedValue<int>, string>) (pair => string.Format(Resources.PairFormat, (object) pair.Value, (object) pair.Annotation))).ToArray<string>());

    public List<AnnotatedValue<int>> GetTimeValue(TimeSpan timeSpan)
    {
      if (timeSpan.TotalDays > 3.0)
        return new List<AnnotatedValue<int>>()
        {
          new AnnotatedValue<int>((int) timeSpan.TotalDays, Resources.DaysFormatString)
        };
      if (timeSpan >= TimeFormatterUtil.OneDayTimeSpan)
      {
        timeSpan = TimeSpan.FromHours(Math.Round(timeSpan.TotalHours));
        if (timeSpan.Hours == 0)
          return new List<AnnotatedValue<int>>()
          {
            new AnnotatedValue<int>((int) timeSpan.TotalDays, Resources.DaysFormatString)
          };
        return new List<AnnotatedValue<int>>()
        {
          new AnnotatedValue<int>((int) timeSpan.TotalDays, Resources.DaysFormatString),
          new AnnotatedValue<int>(timeSpan.Hours, Resources.HoursFormatString)
        };
      }
      if (timeSpan.TotalMinutes >= 56.0)
      {
        int num = (int) Math.Round((double) timeSpan.Minutes / 10.0);
        timeSpan = num != 6 ? TimeSpan.FromHours((double) timeSpan.Hours).Add(TimeSpan.FromMinutes((double) (num * 10))) : TimeSpan.FromHours((double) (timeSpan.Hours + 1));
        if (timeSpan.Minutes == 0)
          return new List<AnnotatedValue<int>>()
          {
            new AnnotatedValue<int>((int) timeSpan.TotalHours, Resources.HoursFormatString)
          };
        return new List<AnnotatedValue<int>>()
        {
          new AnnotatedValue<int>((int) timeSpan.TotalHours, Resources.HoursFormatString),
          new AnnotatedValue<int>(timeSpan.Minutes, Resources.MinutesFromatString)
        };
      }
      if (timeSpan.TotalMinutes >= 30.0)
        return new List<AnnotatedValue<int>>()
        {
          new AnnotatedValue<int>((int) Math.Round(timeSpan.TotalMinutes / 5.0) * 5, Resources.MinutesFromatString)
        };
      return new List<AnnotatedValue<int>>()
      {
        new AnnotatedValue<int>((int) timeSpan.TotalMinutes, Resources.MinutesFromatString)
      };
    }
  }
}
