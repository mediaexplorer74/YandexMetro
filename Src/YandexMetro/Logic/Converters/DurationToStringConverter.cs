// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Converters.DurationToStringConverter
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;
using Yandex.Metro.Resources;

namespace Yandex.Metro.Logic.Converters
{
  public class DurationToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (parameter != null)
        return (object) MetroService.Instance.TimeForRoute.AddSeconds((double) (int) value).ToString("HH:mm");
      TimeSpan timeSpan = TimeSpan.FromSeconds((double) (int) value);
      int hours = timeSpan.Hours;
      int num = timeSpan.Minutes + (timeSpan.Seconds >= 30 ? 1 : 0);
      if (hours <= 0)
        return (object) string.Format("{0} {1}", (object) num, (object) Localization.Map_Minute);
      return (object) string.Format("{0} {1} {2} {3}", (object) hours, (object) Localization.Map_Hour, (object) num, (object) Localization.Map_Minute);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) null;
    }
  }
}
