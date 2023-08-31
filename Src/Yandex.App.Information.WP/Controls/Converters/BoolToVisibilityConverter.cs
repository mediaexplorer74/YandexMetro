// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Converters.BoolToVisibilityConverter
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Yandex.Controls.Converters
{
  public class BoolToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value is bool flag) ? (object) (Visibility) 0 : (object) (Visibility) (flag ? 0 : 1);

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
