// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Converters.MetroColorToBrushConverter
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;
using Y.Metro.ServiceLayer.FastScheme;
using Yandex.Metro.Views;

namespace Yandex.Metro.Logic.Converters
{
  public class MetroColorToBrushConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) MapGenerator.GetBrush((MetroColor) value);

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
