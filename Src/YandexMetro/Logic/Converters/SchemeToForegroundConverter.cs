// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Converters.SchemeToForegroundConverter
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Y.Metro.ServiceLayer.Entities;
using Y.UI.Common.Utility;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Logic.Converters
{
  public class SchemeToForegroundConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Scheme scheme = (Scheme) value;
      if (scheme == null || Locator.SettingsStatic.SelectedScheme == null)
        return (object) ResourcesHelper.Get<SolidColorBrush>("PhoneForegroundBrush");
      return Locator.SettingsStatic.SelectedScheme.Id == scheme.Id ? (object) new SolidColorBrush(ResourcesHelper.Get<Color>("PhoneAccentColor")) : (object) ResourcesHelper.Get<SolidColorBrush>("PhoneForegroundBrush");
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
