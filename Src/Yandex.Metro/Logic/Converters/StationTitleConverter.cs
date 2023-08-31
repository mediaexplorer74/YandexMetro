// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Converters.StationTitleConverter
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;
using Yandex.Metro.Resources;

namespace Yandex.Metro.Logic.Converters
{
  public class StationTitleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) string.Format(Localization.Station_Title, (bool) value ? (object) Localization.Station_Dispatch : (object) Localization.Station_Destination).ToUpper();

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
