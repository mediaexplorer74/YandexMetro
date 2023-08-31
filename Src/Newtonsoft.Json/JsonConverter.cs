// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Schema;
using System;

namespace Newtonsoft.Json
{
  public abstract class JsonConverter
  {
    public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

    public abstract object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer);

    public abstract bool CanConvert(Type objectType);

    public virtual JsonSchema GetSchema() => (JsonSchema) null;

    public virtual bool CanRead => true;

    public virtual bool CanWrite => true;
  }
}
