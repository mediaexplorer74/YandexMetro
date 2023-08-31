// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DateTimeUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
  internal static class DateTimeUtils
  {
    public static string GetLocalOffset(this DateTime d)
    {
      TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(d);
      return utcOffset.Hours.ToString("+00;-00", (IFormatProvider) CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
    {
      switch (kind)
      {
        case DateTimeKind.Unspecified:
          return XmlDateTimeSerializationMode.Unspecified;
        case DateTimeKind.Utc:
          return XmlDateTimeSerializationMode.Utc;
        case DateTimeKind.Local:
          return XmlDateTimeSerializationMode.Local;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof (kind), (object) kind, "Unexpected DateTimeKind value.");
      }
    }
  }
}
