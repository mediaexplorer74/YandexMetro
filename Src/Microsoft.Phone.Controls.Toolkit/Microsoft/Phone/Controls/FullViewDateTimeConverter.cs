// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.FullViewDateTimeConverter
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Microsoft.Phone.Controls
{
  public class FullViewDateTimeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is DateTime dt))
        throw new ArgumentException(Resources.InvalidDateTimeArgument);
      StringBuilder stringBuilder = new StringBuilder(string.Empty);
      if (DateTimeFormatHelper.IsCurrentCultureJapanese() || DateTimeFormatHelper.IsCurrentCultureKorean())
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1} {2}", new object[3]
        {
          (object) DateTimeFormatHelper.GetMonthAndDay(dt),
          (object) DateTimeFormatHelper.GetAbbreviatedDay(dt),
          (object) DateTimeFormatHelper.GetShortTime(dt)
        });
      else if (DateTimeFormatHelper.IsCurrentCultureTurkish())
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0}, {1} {2}", new object[3]
        {
          (object) DateTimeFormatHelper.GetMonthAndDay(dt),
          (object) DateTimeFormatHelper.GetAbbreviatedDay(dt),
          (object) DateTimeFormatHelper.GetShortTime(dt)
        });
      else
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1}, {2}", new object[3]
        {
          (object) DateTimeFormatHelper.GetAbbreviatedDay(dt),
          (object) DateTimeFormatHelper.GetMonthAndDay(dt),
          (object) DateTimeFormatHelper.GetShortTime(dt)
        });
      if (DateTimeFormatHelper.IsCurrentUICultureFrench())
        stringBuilder.Replace(",", string.Empty);
      return (object) stringBuilder.ToString();
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
