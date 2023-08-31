// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConvert
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Newtonsoft.Json
{
  public static class JsonConvert
  {
    public static readonly string True = "true";
    public static readonly string False = "false";
    public static readonly string Null = "null";
    public static readonly string Undefined = "undefined";
    public static readonly string PositiveInfinity = "Infinity";
    public static readonly string NegativeInfinity = "-Infinity";
    public static readonly string NaN = nameof (NaN);
    internal static readonly long InitialJavaScriptDateTicks = 621355968000000000;

    public static string ToString(DateTime value)
    {
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
      {
        JsonConvert.WriteDateTimeString((TextWriter) stringWriter, value, JsonConvert.GetUtcOffset(value), value.Kind);
        return stringWriter.ToString();
      }
    }

    public static string ToString(DateTimeOffset value)
    {
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
      {
        JsonConvert.WriteDateTimeString((TextWriter) stringWriter, value.UtcDateTime, value.Offset, DateTimeKind.Local);
        return stringWriter.ToString();
      }
    }

    private static TimeSpan GetUtcOffset(DateTime dateTime) => TimeZoneInfo.Local.GetUtcOffset(dateTime);

    internal static void WriteDateTimeString(TextWriter writer, DateTime value) => JsonConvert.WriteDateTimeString(writer, value, JsonConvert.GetUtcOffset(value), value.Kind);

    internal static void WriteDateTimeString(
      TextWriter writer,
      DateTime value,
      TimeSpan offset,
      DateTimeKind kind)
    {
      long javaScriptTicks = JsonConvert.ConvertDateTimeToJavaScriptTicks(value, offset);
      writer.Write("\"\\/Date(");
      writer.Write(javaScriptTicks);
      switch (kind)
      {
        case DateTimeKind.Unspecified:
        case DateTimeKind.Local:
          writer.Write(offset.Ticks >= 0L ? "+" : "-");
          int num1 = Math.Abs(offset.Hours);
          if (num1 < 10)
            writer.Write(0);
          writer.Write(num1);
          int num2 = Math.Abs(offset.Minutes);
          if (num2 < 10)
            writer.Write(0);
          writer.Write(num2);
          break;
      }
      writer.Write(")\\/\"");
    }

    private static long ToUniversalTicks(DateTime dateTime) => dateTime.Kind == DateTimeKind.Utc ? dateTime.Ticks : JsonConvert.ToUniversalTicks(dateTime, JsonConvert.GetUtcOffset(dateTime));

    private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
    {
      if (dateTime.Kind == DateTimeKind.Utc)
        return dateTime.Ticks;
      long num = dateTime.Ticks - offset.Ticks;
      if (num > 3155378975999999999L)
        return 3155378975999999999;
      return num < 0L ? 0L : num;
    }

    internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset) => JsonConvert.UniversialTicksToJavaScriptTicks(JsonConvert.ToUniversalTicks(dateTime, offset));

    internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime) => JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, true);

    internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc) => JsonConvert.UniversialTicksToJavaScriptTicks(convertToUtc ? JsonConvert.ToUniversalTicks(dateTime) : dateTime.Ticks);

    private static long UniversialTicksToJavaScriptTicks(long universialTicks) => (universialTicks - JsonConvert.InitialJavaScriptDateTicks) / 10000L;

    internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks) => new DateTime(javaScriptTicks * 10000L + JsonConvert.InitialJavaScriptDateTicks, DateTimeKind.Utc);

    public static string ToString(bool value) => !value ? JsonConvert.False : JsonConvert.True;

    public static string ToString(char value) => JsonConvert.ToString(char.ToString(value));

    public static string ToString(Enum value) => value.ToString("D");

    public static string ToString(int value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(short value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(ushort value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(uint value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(long value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(ulong value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(float value) => JsonConvert.EnsureDecimalPlace((double) value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    public static string ToString(double value) => JsonConvert.EnsureDecimalPlace(value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    private static string EnsureDecimalPlace(double value, string text) => double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1 ? text : text + ".0";

    private static string EnsureDecimalPlace(string text) => text.IndexOf('.') != -1 ? text : text + ".0";

    public static string ToString(byte value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(sbyte value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(Decimal value) => JsonConvert.EnsureDecimalPlace(value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture));

    public static string ToString(Guid value) => '"'.ToString() + value.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture) + (object) '"';

    public static string ToString(TimeSpan value) => '"'.ToString() + value.ToString() + (object) '"';

    public static string ToString(Uri value) => '"'.ToString() + value.ToString() + (object) '"';

    public static string ToString(string value) => JsonConvert.ToString(value, '"');

    public static string ToString(string value, char delimter) => JavaScriptUtils.ToEscapedJavaScriptString(value, delimter, true);

    public static string ToString(object value)
    {
      switch (value)
      {
        case null:
          return JsonConvert.Null;
        case IConvertible convertible:
          switch (convertible.GetTypeCode())
          {
            case TypeCode.DBNull:
              return JsonConvert.Null;
            case TypeCode.Boolean:
              return JsonConvert.ToString(convertible.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Char:
              return JsonConvert.ToString(convertible.ToChar((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.SByte:
              return JsonConvert.ToString(convertible.ToSByte((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Byte:
              return JsonConvert.ToString(convertible.ToByte((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Int16:
              return JsonConvert.ToString(convertible.ToInt16((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.UInt16:
              return JsonConvert.ToString(convertible.ToUInt16((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Int32:
              return JsonConvert.ToString(convertible.ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.UInt32:
              return JsonConvert.ToString(convertible.ToUInt32((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Int64:
              return JsonConvert.ToString(convertible.ToInt64((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.UInt64:
              return JsonConvert.ToString(convertible.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Single:
              return JsonConvert.ToString(convertible.ToSingle((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Double:
              return JsonConvert.ToString(convertible.ToDouble((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.Decimal:
              return JsonConvert.ToString(convertible.ToDecimal((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.DateTime:
              return JsonConvert.ToString(convertible.ToDateTime((IFormatProvider) CultureInfo.InvariantCulture));
            case TypeCode.String:
              return JsonConvert.ToString(convertible.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          }
          break;
        case DateTimeOffset dateTimeOffset:
          return JsonConvert.ToString(dateTimeOffset);
        case Guid guid:
          return JsonConvert.ToString(guid);
        case Uri _:
          return JsonConvert.ToString((Uri) value);
        case TimeSpan timeSpan:
          return JsonConvert.ToString(timeSpan);
      }
      throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    private static bool IsJsonPrimitiveTypeCode(TypeCode typeCode)
    {
      switch (typeCode)
      {
        case TypeCode.DBNull:
        case TypeCode.Boolean:
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
        case TypeCode.DateTime:
        case TypeCode.String:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsJsonPrimitiveType(Type type)
    {
      if (ReflectionUtils.IsNullableType(type))
        type = Nullable.GetUnderlyingType(type);
      return (object) type == (object) typeof (DateTimeOffset) || (object) type == (object) typeof (byte[]) || (object) type == (object) typeof (Uri) || (object) type == (object) typeof (TimeSpan) || (object) type == (object) typeof (Guid) || JsonConvert.IsJsonPrimitiveTypeCode(Type.GetTypeCode(type));
    }

    internal static bool IsJsonPrimitive(object value)
    {
      switch (value)
      {
        case null:
          return true;
        case IConvertible convertible:
          return JsonConvert.IsJsonPrimitiveTypeCode(convertible.GetTypeCode());
        case DateTimeOffset _:
          return true;
        case byte[] _:
          return true;
        case Uri _:
          return true;
        case TimeSpan _:
          return true;
        case Guid _:
          return true;
        default:
          return false;
      }
    }

    public static string SerializeObject(object value) => JsonConvert.SerializeObject(value, Formatting.None, (JsonSerializerSettings) null);

    public static string SerializeObject(object value, Formatting formatting) => JsonConvert.SerializeObject(value, formatting, (JsonSerializerSettings) null);

    public static string SerializeObject(object value, params JsonConverter[] converters) => JsonConvert.SerializeObject(value, Formatting.None, converters);

    public static string SerializeObject(
      object value,
      Formatting formatting,
      params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length <= 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.SerializeObject(value, formatting, settings);
    }

    public static string SerializeObject(
      object value,
      Formatting formatting,
      JsonSerializerSettings settings)
    {
      JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
      StringWriter stringWriter = new StringWriter(new StringBuilder(128), (IFormatProvider) CultureInfo.InvariantCulture);
      using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
      {
        jsonTextWriter.Formatting = formatting;
        jsonSerializer.Serialize((JsonWriter) jsonTextWriter, value);
      }
      return stringWriter.ToString();
    }

    public static object DeserializeObject(string value) => JsonConvert.DeserializeObject(value, (Type) null, (JsonSerializerSettings) null);

    public static object DeserializeObject(string value, JsonSerializerSettings settings) => JsonConvert.DeserializeObject(value, (Type) null, settings);

    public static object DeserializeObject(string value, Type type) => JsonConvert.DeserializeObject(value, type, (JsonSerializerSettings) null);

    public static T DeserializeObject<T>(string value) => JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings) null);

    public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject) => JsonConvert.DeserializeObject<T>(value);

    public static T DeserializeObject<T>(string value, params JsonConverter[] converters) => (T) JsonConvert.DeserializeObject(value, typeof (T), converters);

    public static T DeserializeObject<T>(string value, JsonSerializerSettings settings) => (T) JsonConvert.DeserializeObject(value, typeof (T), settings);

    public static object DeserializeObject(
      string value,
      Type type,
      params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length <= 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.DeserializeObject(value, type, settings);
    }

    public static object DeserializeObject(
      string value,
      Type type,
      JsonSerializerSettings settings)
    {
      StringReader reader1 = new StringReader(value);
      object obj;
      using (JsonReader reader2 = (JsonReader) new JsonTextReader((TextReader) reader1))
      {
        obj = JsonSerializer.Create(settings).Deserialize(reader2, type);
        if (reader2.Read())
        {
          if (reader2.TokenType != JsonToken.Comment)
            throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
        }
      }
      return obj;
    }

    public static void PopulateObject(string value, object target) => JsonConvert.PopulateObject(value, target, (JsonSerializerSettings) null);

    public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
    {
      StringReader reader1 = new StringReader(value);
      using (JsonReader reader2 = (JsonReader) new JsonTextReader((TextReader) reader1))
      {
        JsonSerializer.Create(settings).Populate(reader2, target);
        if (reader2.Read() && reader2.TokenType != JsonToken.Comment)
          throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
      }
    }

    public static string SerializeXNode(XObject node) => JsonConvert.SerializeXNode(node, Formatting.None);

    public static string SerializeXNode(XObject node, Formatting formatting) => JsonConvert.SerializeXNode(node, formatting, false);

    public static string SerializeXNode(XObject node, Formatting formatting, bool omitRootObject)
    {
      XmlNodeConverter xmlNodeConverter = new XmlNodeConverter()
      {
        OmitRootObject = omitRootObject
      };
      return JsonConvert.SerializeObject((object) node, formatting, (JsonConverter) xmlNodeConverter);
    }

    public static XDocument DeserializeXNode(string value) => JsonConvert.DeserializeXNode(value, (string) null);

    public static XDocument DeserializeXNode(string value, string deserializeRootElementName) => JsonConvert.DeserializeXNode(value, deserializeRootElementName, false);

    public static XDocument DeserializeXNode(
      string value,
      string deserializeRootElementName,
      bool writeArrayAttribute)
    {
      return (XDocument) JsonConvert.DeserializeObject(value, typeof (XDocument), (JsonConverter) new XmlNodeConverter()
      {
        DeserializeRootElementName = deserializeRootElementName,
        WriteArrayAttribute = writeArrayAttribute
      });
    }
  }
}
