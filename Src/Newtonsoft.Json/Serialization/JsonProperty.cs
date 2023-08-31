// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonProperty
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  public class JsonProperty
  {
    public string PropertyName { get; set; }

    public int? Order { get; set; }

    public string UnderlyingName { get; set; }

    public IValueProvider ValueProvider { get; set; }

    public Type PropertyType { get; set; }

    public JsonConverter Converter { get; set; }

    public JsonConverter MemberConverter { get; set; }

    public bool Ignored { get; set; }

    public bool Readable { get; set; }

    public bool Writable { get; set; }

    public object DefaultValue { get; set; }

    public Required Required { get; set; }

    public bool? IsReference { get; set; }

    public Newtonsoft.Json.NullValueHandling? NullValueHandling { get; set; }

    public Newtonsoft.Json.DefaultValueHandling? DefaultValueHandling { get; set; }

    public Newtonsoft.Json.ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

    public Newtonsoft.Json.ObjectCreationHandling? ObjectCreationHandling { get; set; }

    public Newtonsoft.Json.TypeNameHandling? TypeNameHandling { get; set; }

    public Predicate<object> ShouldSerialize { get; set; }

    public Predicate<object> GetIsSpecified { get; set; }

    public Action<object, object> SetIsSpecified { get; set; }

    public override string ToString() => this.PropertyName;
  }
}
