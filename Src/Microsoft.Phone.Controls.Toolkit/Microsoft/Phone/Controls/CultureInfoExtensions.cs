// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.CultureInfoExtensions
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Microsoft.Phone.Controls
{
  public static class CultureInfoExtensions
  {
    private static string[] CulturesWithTFWeekends = new string[1]
    {
      "ar-SA"
    };
    private static string[] CulturesWithFSWeekends = new string[2]
    {
      "he-IL",
      "ar-EG"
    };

    public static ReadOnlyCollection<string> Weekdays(this CultureInfo culture)
    {
      DayOfWeek[] dayOfWeekArray;
      if (((IEnumerable<string>) CultureInfoExtensions.CulturesWithTFWeekends).Contains<string>(culture.Name))
        dayOfWeekArray = new DayOfWeek[5]
        {
          DayOfWeek.Monday,
          DayOfWeek.Tuesday,
          DayOfWeek.Wednesday,
          DayOfWeek.Saturday,
          DayOfWeek.Sunday
        };
      else if (((IEnumerable<string>) CultureInfoExtensions.CulturesWithFSWeekends).Contains<string>(culture.Name))
        dayOfWeekArray = new DayOfWeek[5]
        {
          DayOfWeek.Monday,
          DayOfWeek.Tuesday,
          DayOfWeek.Wednesday,
          DayOfWeek.Thursday,
          DayOfWeek.Sunday
        };
      else
        dayOfWeekArray = new DayOfWeek[5]
        {
          DayOfWeek.Monday,
          DayOfWeek.Tuesday,
          DayOfWeek.Wednesday,
          DayOfWeek.Thursday,
          DayOfWeek.Friday
        };
      List<string> list = new List<string>();
      foreach (DayOfWeek dayofweek in dayOfWeekArray)
        list.Add(culture.DateTimeFormat.GetDayName(dayofweek));
      return new ReadOnlyCollection<string>((IList<string>) list);
    }

    public static ReadOnlyCollection<string> Weekends(this CultureInfo culture)
    {
      DayOfWeek[] dayOfWeekArray;
      if (((IEnumerable<string>) CultureInfoExtensions.CulturesWithTFWeekends).Contains<string>(culture.Name))
        dayOfWeekArray = new DayOfWeek[2]
        {
          DayOfWeek.Thursday,
          DayOfWeek.Friday
        };
      else if (((IEnumerable<string>) CultureInfoExtensions.CulturesWithFSWeekends).Contains<string>(culture.Name))
        dayOfWeekArray = new DayOfWeek[2]
        {
          DayOfWeek.Friday,
          DayOfWeek.Saturday
        };
      else
        dayOfWeekArray = new DayOfWeek[2]
        {
          DayOfWeek.Saturday,
          DayOfWeek.Sunday
        };
      List<string> list = new List<string>();
      foreach (DayOfWeek dayofweek in dayOfWeekArray)
        list.Add(culture.DateTimeFormat.GetDayName(dayofweek));
      return new ReadOnlyCollection<string>((IList<string>) list);
    }
  }
}
