// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TypeConverters.GeoCoordinatesConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.ComponentModel;
using System.Globalization;
using Yandex.Positioning;

namespace Yandex.Maps.TypeConverters
{
  internal class GeoCoordinatesConverter : TypeConverter
  {
    public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);

    public virtual object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      return value is string ? (object) new GeoCoordinate((string) value) : base.ConvertFrom(context, culture, value);
    }

    public virtual object ConvertTo(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      return destinationType == typeof (string) && value is GeoCoordinate geoCoordinate ? (object) geoCoordinate.ToString() : base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
