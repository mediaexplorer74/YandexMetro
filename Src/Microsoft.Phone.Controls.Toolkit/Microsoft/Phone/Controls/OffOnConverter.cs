// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.OffOnConverter
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Phone.Controls
{
  public class OffOnConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if ((object) targetType == null)
        throw new ArgumentNullException(nameof (targetType));
      if ((object) targetType != (object) typeof (object))
        throw new ArgumentException(Resources.UnexpectedType, nameof (targetType));
      switch (value)
      {
        case bool? _:
        case null:
          bool? nullable = (bool?) value;
          return (!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? (object) Resources.Off : (object) Resources.On;
        default:
          throw new ArgumentException(Resources.UnexpectedType, nameof (value));
      }
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
