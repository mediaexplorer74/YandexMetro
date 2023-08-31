// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IsoDateTimeConverter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class IsoDateTimeConverter : DateTimeConverterBase
  {
    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
    private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private string _dateTimeFormat;
    private CultureInfo _culture;

    public DateTimeStyles DateTimeStyles
    {
      get => this._dateTimeStyles;
      set => this._dateTimeStyles = value;
    }

    public string DateTimeFormat
    {
      get => this._dateTimeFormat ?? string.Empty;
      set => this._dateTimeFormat = StringUtils.NullEmptyString(value);
    }

    public CultureInfo Culture
    {
      get => this._culture ?? CultureInfo.CurrentCulture;
      set => this._culture = value;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      string str;
      switch (value)
      {
        case DateTime universalTime1:
          if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            universalTime1 = universalTime1.ToUniversalTime();
          str = universalTime1.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", (IFormatProvider) this.Culture);
          break;
        case DateTimeOffset universalTime2:
          if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            universalTime2 = universalTime2.ToUniversalTime();
          str = universalTime2.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", (IFormatProvider) this.Culture);
          break;
        default:
          throw new Exception("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) ReflectionUtils.GetObjectType(value)));
      }
      writer.WriteValue(str);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      bool flag = ReflectionUtils.IsNullableType(objectType);
      Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw new Exception("Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      string str = reader.TokenType == JsonToken.String ? reader.Value.ToString() : throw new Exception("Unexpected token parsing date. Expected String, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      if (string.IsNullOrEmpty(str) && flag)
        return (object) null;
      return (object) type == (object) typeof (DateTimeOffset) ? (!string.IsNullOrEmpty(this._dateTimeFormat) ? (object) DateTimeOffset.ParseExact(str, this._dateTimeFormat, (IFormatProvider) this.Culture, this._dateTimeStyles) : (object) DateTimeOffset.Parse(str, (IFormatProvider) this.Culture, this._dateTimeStyles)) : (!string.IsNullOrEmpty(this._dateTimeFormat) ? (object) DateTime.ParseExact(str, this._dateTimeFormat, (IFormatProvider) this.Culture, this._dateTimeStyles) : (object) DateTime.Parse(str, (IFormatProvider) this.Culture, this._dateTimeStyles));
    }
  }
}
