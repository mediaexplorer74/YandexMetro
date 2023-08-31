// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.GroupKeysConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace Yandex.Maps
{
  internal class GroupKeysConverter : TypeConverter
  {
    public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);

    public virtual object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      if (!(value is string str))
        return base.ConvertFrom(context, culture, value);
      char[] separator = new char[1]{ ',' };
      return (object) new Collection<string>((IList<string>) str.Split(separator, StringSplitOptions.RemoveEmptyEntries));
    }
  }
}
