﻿// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Converters.ToLowCaseConverter
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Globalization;
using System.Windows.Data;

namespace Y.UI.Common.Converters
{
  public class ToLowCaseConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? (object) null : (object) value.ToString().ToLower();

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
