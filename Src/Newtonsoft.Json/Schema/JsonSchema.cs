// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchema
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Schema
{
  public class JsonSchema
  {
    private readonly string _internalId = Guid.NewGuid().ToString("N");

    public string Id { get; set; }

    public string Title { get; set; }

    public bool? Required { get; set; }

    public bool? ReadOnly { get; set; }

    public bool? Hidden { get; set; }

    public bool? Transient { get; set; }

    public string Description { get; set; }

    public JsonSchemaType? Type { get; set; }

    public string Pattern { get; set; }

    public int? MinimumLength { get; set; }

    public int? MaximumLength { get; set; }

    public double? DivisibleBy { get; set; }

    public double? Minimum { get; set; }

    public double? Maximum { get; set; }

    public bool? ExclusiveMinimum { get; set; }

    public bool? ExclusiveMaximum { get; set; }

    public int? MinimumItems { get; set; }

    public int? MaximumItems { get; set; }

    public IList<JsonSchema> Items { get; set; }

    public IDictionary<string, JsonSchema> Properties { get; set; }

    public JsonSchema AdditionalProperties { get; set; }

    public IDictionary<string, JsonSchema> PatternProperties { get; set; }

    public bool AllowAdditionalProperties { get; set; }

    public string Requires { get; set; }

    public IList<string> Identity { get; set; }

    public IList<JToken> Enum { get; set; }

    public IDictionary<JToken, string> Options { get; set; }

    public JsonSchemaType? Disallow { get; set; }

    public JToken Default { get; set; }

    public JsonSchema Extends { get; set; }

    public string Format { get; set; }

    internal string InternalId => this._internalId;

    public JsonSchema() => this.AllowAdditionalProperties = true;

    public static JsonSchema Read(JsonReader reader) => JsonSchema.Read(reader, new JsonSchemaResolver());

    public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      ValidationUtils.ArgumentNotNull((object) resolver, nameof (resolver));
      return new JsonSchemaBuilder(resolver).Parse(reader);
    }

    public static JsonSchema Parse(string json) => JsonSchema.Parse(json, new JsonSchemaResolver());

    public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) json, nameof (json));
      return JsonSchema.Read((JsonReader) new JsonTextReader((TextReader) new StringReader(json)), resolver);
    }

    public void WriteTo(JsonWriter writer) => this.WriteTo(writer, new JsonSchemaResolver());

    public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) writer, nameof (writer));
      ValidationUtils.ArgumentNotNull((object) resolver, nameof (resolver));
      new JsonSchemaWriter(writer, resolver).WriteSchema(this);
    }

    public override string ToString()
    {
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter);
      writer.Formatting = Formatting.Indented;
      this.WriteTo((JsonWriter) writer);
      return stringWriter.ToString();
    }
  }
}
