// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.DateTimeWrapper
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Globalization;

namespace Microsoft.Phone.Controls.Primitives
{
  public class DateTimeWrapper
  {
    public DateTime DateTime { get; private set; }

    public string YearNumber => this.DateTime.ToString("yyyy", (IFormatProvider) CultureInfo.CurrentCulture);

    public string MonthNumber => this.DateTime.ToString("MM", (IFormatProvider) CultureInfo.CurrentCulture);

    public string MonthName => this.DateTime.ToString("MMMM", (IFormatProvider) CultureInfo.CurrentCulture);

    public string DayNumber => this.DateTime.ToString("dd", (IFormatProvider) CultureInfo.CurrentCulture);

    public string DayName => this.DateTime.ToString("dddd", (IFormatProvider) CultureInfo.CurrentCulture);

    public string HourNumber => this.DateTime.ToString(DateTimeWrapper.CurrentCultureUsesTwentyFourHourClock() ? "%H" : "%h", (IFormatProvider) CultureInfo.CurrentCulture);

    public string MinuteNumber => this.DateTime.ToString("mm", (IFormatProvider) CultureInfo.CurrentCulture);

    public string AmPmString => this.DateTime.ToString("tt", (IFormatProvider) CultureInfo.CurrentCulture);

    public DateTimeWrapper(DateTime dateTime) => this.DateTime = dateTime;

    public static bool CurrentCultureUsesTwentyFourHourClock() => !CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("t");
  }
}
