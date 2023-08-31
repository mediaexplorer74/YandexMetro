// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ListViewDateTimeConverter
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Phone.Controls
{
  public class ListViewDateTimeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is DateTime dateTime))
        throw new ArgumentException(Resources.InvalidDateTimeArgument);
      DateTime now = DateTime.Now;
      if (DateTimeFormatHelper.IsFutureDateTime(now, dateTime))
        throw new NotSupportedException(Resources.NonSupportedDateTime);
      return !DateTimeFormatHelper.IsAnOlderWeek(now, dateTime) ? (!DateTimeFormatHelper.IsPastDayOfWeek(now, dateTime) ? (object) DateTimeFormatHelper.GetSuperShortTime(dateTime) : (object) DateTimeFormatHelper.GetAbbreviatedDay(dateTime)) : (object) DateTimeFormatHelper.GetMonthAndDay(dateTime);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
