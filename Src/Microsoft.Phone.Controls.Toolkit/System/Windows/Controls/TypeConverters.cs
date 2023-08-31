// Decompiled with JetBrains decompiler
// Type: System.Windows.Controls.TypeConverters
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Controls
{
  internal static class TypeConverters
  {
    internal static bool CanConvertTo<T>(Type destinationType)
    {
      if ((object) destinationType == null)
        throw new ArgumentNullException(nameof (destinationType));
      return (object) destinationType == (object) typeof (string) || destinationType.IsAssignableFrom(typeof (T));
    }

    internal static object ConvertTo(TypeConverter converter, object value, Type destinationType)
    {
      if ((object) destinationType == null)
        throw new ArgumentNullException(nameof (destinationType));
      if (value == null && !destinationType.IsValueType)
        return (object) null;
      return value != null && destinationType.IsAssignableFrom(value.GetType()) ? value : throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TypeConverters_Convert_CannotConvert, new object[3]
      {
        (object) converter.GetType().Name,
        value != null ? (object) value.GetType().FullName : (object) "(null)",
        (object) destinationType.GetType().Name
      }));
    }
  }
}
