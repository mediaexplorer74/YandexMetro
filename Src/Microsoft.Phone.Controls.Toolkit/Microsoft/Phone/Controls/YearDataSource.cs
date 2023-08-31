// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.YearDataSource
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;

namespace Microsoft.Phone.Controls
{
  internal class YearDataSource : DataSource
  {
    protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
    {
      if (1601 == relativeDate.Year || 3000 == relativeDate.Year)
        return new DateTime?();
      int year = relativeDate.Year + delta;
      int day = Math.Min(relativeDate.Day, DateTime.DaysInMonth(year, relativeDate.Month));
      return new DateTime?(new DateTime(year, relativeDate.Month, day, relativeDate.Hour, relativeDate.Minute, relativeDate.Second));
    }
  }
}
