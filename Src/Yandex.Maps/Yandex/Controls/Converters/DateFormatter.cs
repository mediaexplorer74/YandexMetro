// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Converters.DateFormatter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace Yandex.Controls.Converters
{
  internal class DateFormatter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string format = parameter as string;
      if (string.IsNullOrEmpty(format))
        return (object) value.ToString();
      return (object) string.Format((IFormatProvider) Thread.CurrentThread.CurrentCulture, format, value);
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
