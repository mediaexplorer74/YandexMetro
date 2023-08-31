// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.RegexConverter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Bson;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Converters
{
  public class RegexConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Regex regex = (Regex) value;
      if (writer is BsonWriter writer1)
        this.WriteBson(writer1, regex);
      else
        this.WriteJson(writer, regex);
    }

    private bool HasFlag(RegexOptions options, RegexOptions flag) => (options & flag) == flag;

    private void WriteBson(BsonWriter writer, Regex regex)
    {
      string str = (string) null;
      if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
        str += "i";
      if (this.HasFlag(regex.Options, RegexOptions.Multiline))
        str += "m";
      if (this.HasFlag(regex.Options, RegexOptions.Singleline))
        str += "s";
      string options = str + "u";
      if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
        options += "x";
      writer.WriteRegex(regex.ToString(), options);
    }

    private void WriteJson(JsonWriter writer, Regex regex)
    {
      writer.WriteStartObject();
      writer.WritePropertyName("Pattern");
      writer.WriteValue(regex.ToString());
      writer.WritePropertyName("Options");
      writer.WriteValue((object) regex.Options);
      writer.WriteEndObject();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      return reader is BsonReader reader1 ? this.ReadBson(reader1) : (object) this.ReadJson(reader);
    }

    private object ReadBson(BsonReader reader)
    {
      string str1 = (string) reader.Value;
      int num = str1.LastIndexOf("/");
      string pattern = str1.Substring(1, num - 1);
      string str2 = str1.Substring(num + 1);
      RegexOptions options = RegexOptions.None;
      foreach (char ch in str2)
      {
        switch (ch)
        {
          case 'i':
            options |= RegexOptions.IgnoreCase;
            break;
          case 'm':
            options |= RegexOptions.Multiline;
            break;
          case 's':
            options |= RegexOptions.Singleline;
            break;
          case 'x':
            options |= RegexOptions.ExplicitCapture;
            break;
        }
      }
      return (object) new Regex(pattern, options);
    }

    private Regex ReadJson(JsonReader reader)
    {
      reader.Read();
      reader.Read();
      string pattern = (string) reader.Value;
      reader.Read();
      reader.Read();
      int int32 = Convert.ToInt32(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      reader.Read();
      return new Regex(pattern, (RegexOptions) int32);
    }

    public override bool CanConvert(Type objectType) => (object) objectType == (object) typeof (Regex);
  }
}
