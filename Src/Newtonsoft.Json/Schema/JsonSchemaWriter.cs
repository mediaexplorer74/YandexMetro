// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaWriter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaWriter
  {
    private readonly JsonWriter _writer;
    private readonly JsonSchemaResolver _resolver;

    public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) writer, nameof (writer));
      this._writer = writer;
      this._resolver = resolver;
    }

    private void ReferenceOrWriteSchema(JsonSchema schema)
    {
      if (schema.Id != null && this._resolver.GetSchema(schema.Id) != null)
      {
        this._writer.WriteStartObject();
        this._writer.WritePropertyName("$ref");
        this._writer.WriteValue(schema.Id);
        this._writer.WriteEndObject();
      }
      else
        this.WriteSchema(schema);
    }

    public void WriteSchema(JsonSchema schema)
    {
      ValidationUtils.ArgumentNotNull((object) schema, nameof (schema));
      if (!this._resolver.LoadedSchemas.Contains(schema))
        this._resolver.LoadedSchemas.Add(schema);
      this._writer.WriteStartObject();
      this.WritePropertyIfNotNull(this._writer, "id", (object) schema.Id);
      this.WritePropertyIfNotNull(this._writer, "title", (object) schema.Title);
      this.WritePropertyIfNotNull(this._writer, "description", (object) schema.Description);
      this.WritePropertyIfNotNull(this._writer, "required", (object) schema.Required);
      this.WritePropertyIfNotNull(this._writer, "readonly", (object) schema.ReadOnly);
      this.WritePropertyIfNotNull(this._writer, "hidden", (object) schema.Hidden);
      this.WritePropertyIfNotNull(this._writer, "transient", (object) schema.Transient);
      if (schema.Type.HasValue)
        this.WriteType("type", this._writer, schema.Type.Value);
      if (!schema.AllowAdditionalProperties)
      {
        this._writer.WritePropertyName("additionalProperties");
        this._writer.WriteValue(schema.AllowAdditionalProperties);
      }
      else if (schema.AdditionalProperties != null)
      {
        this._writer.WritePropertyName("additionalProperties");
        this.ReferenceOrWriteSchema(schema.AdditionalProperties);
      }
      this.WriteSchemaDictionaryIfNotNull(this._writer, "properties", schema.Properties);
      this.WriteSchemaDictionaryIfNotNull(this._writer, "patternProperties", schema.PatternProperties);
      this.WriteItems(schema);
      this.WritePropertyIfNotNull(this._writer, "minimum", (object) schema.Minimum);
      this.WritePropertyIfNotNull(this._writer, "maximum", (object) schema.Maximum);
      this.WritePropertyIfNotNull(this._writer, "exclusiveMinimum", (object) schema.ExclusiveMinimum);
      this.WritePropertyIfNotNull(this._writer, "exclusiveMaximum", (object) schema.ExclusiveMaximum);
      this.WritePropertyIfNotNull(this._writer, "minLength", (object) schema.MinimumLength);
      this.WritePropertyIfNotNull(this._writer, "maxLength", (object) schema.MaximumLength);
      this.WritePropertyIfNotNull(this._writer, "minItems", (object) schema.MinimumItems);
      this.WritePropertyIfNotNull(this._writer, "maxItems", (object) schema.MaximumItems);
      this.WritePropertyIfNotNull(this._writer, "divisibleBy", (object) schema.DivisibleBy);
      this.WritePropertyIfNotNull(this._writer, "format", (object) schema.Format);
      this.WritePropertyIfNotNull(this._writer, "pattern", (object) schema.Pattern);
      if (schema.Enum != null)
      {
        this._writer.WritePropertyName("enum");
        this._writer.WriteStartArray();
        foreach (JToken jtoken in (IEnumerable<JToken>) schema.Enum)
          jtoken.WriteTo(this._writer);
        this._writer.WriteEndArray();
      }
      if (schema.Default != null)
      {
        this._writer.WritePropertyName("default");
        schema.Default.WriteTo(this._writer);
      }
      if (schema.Options != null)
      {
        this._writer.WritePropertyName("options");
        this._writer.WriteStartArray();
        foreach (KeyValuePair<JToken, string> option in (IEnumerable<KeyValuePair<JToken, string>>) schema.Options)
        {
          this._writer.WriteStartObject();
          this._writer.WritePropertyName("value");
          option.Key.WriteTo(this._writer);
          if (option.Value != null)
          {
            this._writer.WritePropertyName("label");
            this._writer.WriteValue(option.Value);
          }
          this._writer.WriteEndObject();
        }
        this._writer.WriteEndArray();
      }
      if (schema.Disallow.HasValue)
        this.WriteType("disallow", this._writer, schema.Disallow.Value);
      if (schema.Extends != null)
      {
        this._writer.WritePropertyName("extends");
        this.ReferenceOrWriteSchema(schema.Extends);
      }
      this._writer.WriteEndObject();
    }

    private void WriteSchemaDictionaryIfNotNull(
      JsonWriter writer,
      string propertyName,
      IDictionary<string, JsonSchema> properties)
    {
      if (properties == null)
        return;
      writer.WritePropertyName(propertyName);
      writer.WriteStartObject();
      foreach (KeyValuePair<string, JsonSchema> property in (IEnumerable<KeyValuePair<string, JsonSchema>>) properties)
      {
        writer.WritePropertyName(property.Key);
        this.ReferenceOrWriteSchema(property.Value);
      }
      writer.WriteEndObject();
    }

    private void WriteItems(JsonSchema schema)
    {
      if (CollectionUtils.IsNullOrEmpty<JsonSchema>((ICollection<JsonSchema>) schema.Items))
        return;
      this._writer.WritePropertyName("items");
      if (schema.Items.Count == 1)
      {
        this.ReferenceOrWriteSchema(schema.Items[0]);
      }
      else
      {
        this._writer.WriteStartArray();
        foreach (JsonSchema schema1 in (IEnumerable<JsonSchema>) schema.Items)
          this.ReferenceOrWriteSchema(schema1);
        this._writer.WriteEndArray();
      }
    }

    private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
    {
      IList<JsonSchemaType> jsonSchemaTypeList;
      if (Enum.IsDefined(typeof (JsonSchemaType), (object) type))
        jsonSchemaTypeList = (IList<JsonSchemaType>) new List<JsonSchemaType>()
        {
          type
        };
      else
        jsonSchemaTypeList = (IList<JsonSchemaType>) EnumUtils.GetFlagsValues<JsonSchemaType>(type).Where<JsonSchemaType>((Func<JsonSchemaType, bool>) (v => v != JsonSchemaType.None)).ToList<JsonSchemaType>();
      if (jsonSchemaTypeList.Count == 0)
        return;
      writer.WritePropertyName(propertyName);
      if (jsonSchemaTypeList.Count == 1)
      {
        writer.WriteValue(JsonSchemaBuilder.MapType(jsonSchemaTypeList[0]));
      }
      else
      {
        writer.WriteStartArray();
        foreach (JsonSchemaType type1 in (IEnumerable<JsonSchemaType>) jsonSchemaTypeList)
          writer.WriteValue(JsonSchemaBuilder.MapType(type1));
        writer.WriteEndArray();
      }
    }

    private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
    {
      if (value == null)
        return;
      writer.WritePropertyName(propertyName);
      writer.WriteValue(value);
    }
  }
}
