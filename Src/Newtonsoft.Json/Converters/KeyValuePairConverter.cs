// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.KeyValuePairConverter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  public class KeyValuePairConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Type type = value.GetType();
      PropertyInfo property1 = type.GetProperty("Key");
      PropertyInfo property2 = type.GetProperty("Value");
      writer.WriteStartObject();
      writer.WritePropertyName("Key");
      serializer.Serialize(writer, ReflectionUtils.GetMemberValue((MemberInfo) property1, value));
      writer.WritePropertyName("Value");
      serializer.Serialize(writer, ReflectionUtils.GetMemberValue((MemberInfo) property2, value));
      writer.WriteEndObject();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      IList<Type> genericArguments = (IList<Type>) objectType.GetGenericArguments();
      Type objectType1 = genericArguments[0];
      Type objectType2 = genericArguments[1];
      reader.Read();
      reader.Read();
      object obj1 = serializer.Deserialize(reader, objectType1);
      reader.Read();
      reader.Read();
      object obj2 = serializer.Deserialize(reader, objectType2);
      reader.Read();
      return ReflectionUtils.CreateInstance(objectType, obj1, obj2);
    }

    public override bool CanConvert(Type objectType) => objectType.IsValueType && objectType.IsGenericType && (object) objectType.GetGenericTypeDefinition() == (object) typeof (KeyValuePair<,>);
  }
}
