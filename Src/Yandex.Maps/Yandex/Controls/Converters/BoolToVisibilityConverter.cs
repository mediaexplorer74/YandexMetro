// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Converters.BoolToVisibilityConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Yandex.Controls.Converters
{
  internal class BoolToVisibilityConverter : IValueConverter
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
