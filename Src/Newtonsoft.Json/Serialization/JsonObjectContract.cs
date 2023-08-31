// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonObjectContract
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class JsonObjectContract : JsonContract
  {
    public MemberSerialization MemberSerialization { get; set; }

    public JsonPropertyCollection Properties { get; private set; }

    public JsonPropertyCollection ConstructorParameters { get; private set; }

    public ConstructorInfo OverrideConstructor { get; set; }

    public ConstructorInfo ParametrizedConstructor { get; set; }

    public JsonObjectContract(Type underlyingType)
      : base(underlyingType)
    {
      this.Properties = new JsonPropertyCollection(this.UnderlyingType);
      this.ConstructorParameters = new JsonPropertyCollection(this.UnderlyingType);
    }
  }
}
