// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Converters.LineToShortNameConverter
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Yandex.Metro.Logic.Converters
{
  public class LineToShortNameConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) value;
      if (string.IsNullOrWhiteSpace(str))
        return (object) string.Empty;
      return (object) string.Format(" ({0})", (object) ((IEnumerable<string>) str.Split(' ')).Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, s) => current + s.Substring(0, 1).ToUpper())));
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
