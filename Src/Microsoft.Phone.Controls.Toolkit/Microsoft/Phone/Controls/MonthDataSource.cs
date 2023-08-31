// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.MonthDataSource
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;

namespace Microsoft.Phone.Controls
{
  internal class MonthDataSource : DataSource
  {
    protected override DateTime? GetRelativeTo(DateTime relativeDate, int delta)
    {
      int num = 12;
      int month = (num + relativeDate.Month - 1 + delta) % num + 1;
      int day = Math.Min(relativeDate.Day, DateTime.DaysInMonth(relativeDate.Year, month));
      return new DateTime?(new DateTime(relativeDate.Year, month, day, relativeDate.Hour, relativeDate.Minute, relativeDate.Second));
    }
  }
}
