// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.StringEnumConverter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Converters
{
  public class StringEnumConverter : JsonConverter
  {
    private readonly Dictionary<Type, BidirectionalDictionary<string, string>> _enumMemberNamesPerType = new Dictionary<Type, BidirectionalDictionary<string, string>>();

    public bool CamelCaseText { get; set; }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Enum @enum = (Enum) value;
        string first = @enum.ToString("G");
        if (char.IsNumber(first[0]) || first[0] == '-')
        {
          writer.WriteValue(value);
        }
        else
        {
          string second;
          this.GetEnumNameMap(@enum.GetType()).TryGetByFirst(first, out second);
          second = second ?? first;
          if (this.CamelCaseText)
            second = StringUtils.ToCamelCase(second);
          writer.WriteValue(second);
        }
      }
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw new Exception("Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      if (reader.TokenType == JsonToken.String)
      {
        string first;
        this.GetEnumNameMap(type).TryGetBySecond(reader.Value.ToString(), out first);
        string str = first ?? reader.Value.ToString();
        return Enum.Parse(type, str, true);
      }
      if (reader.TokenType == JsonToken.Integer)
        return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
      throw new Exception("Unexpected token when parsing enum. Expected String or Integer, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    private BidirectionalDictionary<string, string> GetEnumNameMap(Type t)
    {
      BidirectionalDictionary<string, string> enumNameMap;
      if (!this._enumMemberNamesPerType.TryGetValue(t, out enumNameMap))
      {
        lock (this._enumMemberNamesPerType)
        {
          if (this._enumMemberNamesPerType.TryGetValue(t, out enumNameMap))
            return enumNameMap;
          enumNameMap = new BidirectionalDictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          foreach (FieldInfo field in t.GetFields())
          {
            string name = field.Name;
            string second = field.GetCustomAttributes(typeof (EnumMemberAttribute), true).Cast<EnumMemberAttribute>().Select<EnumMemberAttribute, string>((Func<EnumMemberAttribute, string>) (a => a.Value)).SingleOrDefault<string>() ?? field.Name;
            if (enumNameMap.TryGetBySecond(second, out string _))
              throw new Exception("Enum name '{0}' already exists on enum '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) second, (object) t.Name));
            enumNameMap.Add(name, second);
          }
          this._enumMemberNamesPerType[t] = enumNameMap;
        }
      }
      return enumNameMap;
    }

    public override bool CanConvert(Type objectType) => (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType).IsEnum;
  }
}
