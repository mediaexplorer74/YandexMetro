// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.DayDataSource
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;

namespace Microsoft.Phone.Controls
{
  internal class DayDataSource : DataSource
  {
    protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
    {
      int num = DateTime.DaysInMonth(relativeDate.Year, relativeDate.Month);
      int day = (num + relativeDate.Day - 1 + delta) % num + 1;
      return new DateTime?(new DateTime(relativeDate.Year, relativeDate.Month, day, relativeDate.Hour, relativeDate.Minute, relativeDate.Second));
    }
  }
}
