// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Converters.StringNullToVisibilityConverter
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Y.UI.Common.Converters
{
  public class StringNullToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool flag = value == null || string.IsNullOrWhiteSpace(value.ToString());
      if (parameter != null)
        flag = !flag;
      return flag ? (object) (Visibility) 1 : (object) (Visibility) 0;
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
