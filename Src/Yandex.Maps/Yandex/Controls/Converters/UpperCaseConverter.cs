// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Converters.UpperCaseConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Yandex.Controls.Converters
{
  internal class UpperCaseConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, [NotNull] CultureInfo culture)
    {
      if (targetType != typeof (string))
        throw new ArgumentOutOfRangeException(nameof (targetType));
      if (culture == null)
        throw new ArgumentNullException(nameof (culture));
      if (value == null)
        return (object) null;
      return value is string ? (object) ((string) value).ToUpper(culture) : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
