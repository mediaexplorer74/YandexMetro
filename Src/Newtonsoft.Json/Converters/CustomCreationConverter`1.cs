// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.CustomCreationConverter`1
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Converters
{
  public abstract class CustomCreationConverter<T> : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      T target = this.Create(objectType);
      if ((object) target == null)
        throw new JsonSerializationException("No object created.");
      serializer.Populate(reader, (object) target);
      return (object) target;
    }

    public abstract T Create(Type objectType);

    public override bool CanConvert(Type objectType) => typeof (T).IsAssignableFrom(objectType);

    public override bool CanWrite => false;
  }
}
