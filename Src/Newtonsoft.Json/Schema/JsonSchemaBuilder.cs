// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaBuilder
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaBuilder
  {
    private JsonReader _reader;
    private readonly IList<JsonSchema> _stack;
    private readonly JsonSchemaResolver _resolver;
    private JsonSchema _currentSchema;

    private void Push(JsonSchema value)
    {
      this._currentSchema = value;
      this._stack.Add(value);
      this._resolver.LoadedSchemas.Add(value);
    }

    private JsonSchema Pop()
    {
      JsonSchema currentSchema = this._currentSchema;
      this._stack.RemoveAt(this._stack.Count - 1);
      this._currentSchema = this._stack.LastOrDefault<JsonSchema>();
      return currentSchema;
    }

    private JsonSchema CurrentSchema => this._currentSchema;

    public JsonSchemaBuilder(JsonSchemaResolver resolver)
    {
      this._stack = (IList<JsonSchema>) new List<JsonSchema>();
      this._resolver = resolver;
    }

    internal JsonSchema Parse(JsonReader reader)
    {
      this._reader = reader;
      if (reader.TokenType == JsonToken.None)
        this._reader.Read();
      return this.BuildSchema();
    }

    private JsonSchema BuildSchema()
    {
      if (this._reader.TokenType != JsonToken.StartObject)
        throw new Exception("Expected StartObject while parsing schema object, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      this._reader.Read();
      if (this._reader.TokenType == JsonToken.EndObject)
      {
        this.Push(new JsonSchema());
        return this.Pop();
      }
      string propertyName1 = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      this._reader.Read();
      if (propertyName1 == "$ref")
      {
        string id = (string) this._reader.Value;
        while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
        {
          if (this._reader.TokenType == JsonToken.StartObject)
            throw new Exception("Found StartObject within the schema reference with the Id '{0}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) id));
        }
        return this._resolver.GetSchema(id) ?? throw new Exception("Could not resolve schema reference for Id '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) id));
      }
      this.Push(new JsonSchema());
      this.ProcessSchemaProperty(propertyName1);
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
      {
        string propertyName2 = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._reader.Read();
        this.ProcessSchemaProperty(propertyName2);
      }
      return this.Pop();
    }

    private void ProcessSchemaProperty(string propertyName)
    {
      switch (propertyName)
      {
        case "type":
          this.CurrentSchema.Type = this.ProcessType();
          break;
        case "id":
          this.CurrentSchema.Id = (string) this._reader.Value;
          break;
        case "title":
          this.CurrentSchema.Title = (string) this._reader.Value;
          break;
        case "description":
          this.CurrentSchema.Description = (string) this._reader.Value;
          break;
        case "properties":
          this.ProcessProperties();
          break;
        case "items":
          this.ProcessItems();
          break;
        case "additionalProperties":
          this.ProcessAdditionalProperties();
          break;
        case "patternProperties":
          this.ProcessPatternProperties();
          break;
        case "required":
          this.CurrentSchema.Required = new bool?((bool) this._reader.Value);
          break;
        case "requires":
          this.CurrentSchema.Requires = (string) this._reader.Value;
          break;
        case "identity":
          this.ProcessIdentity();
          break;
        case "minimum":
          this.CurrentSchema.Minimum = new double?(Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "maximum":
          this.CurrentSchema.Maximum = new double?(Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "exclusiveMinimum":
          this.CurrentSchema.ExclusiveMinimum = new bool?((bool) this._reader.Value);
          break;
        case "exclusiveMaximum":
          this.CurrentSchema.ExclusiveMaximum = new bool?((bool) this._reader.Value);
          break;
        case "maxLength":
          this.CurrentSchema.MaximumLength = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "minLength":
          this.CurrentSchema.MinimumLength = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "maxItems":
          this.CurrentSchema.MaximumItems = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "minItems":
          this.CurrentSchema.MinimumItems = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "divisibleBy":
          this.CurrentSchema.DivisibleBy = new double?(Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "disallow":
          this.CurrentSchema.Disallow = this.ProcessType();
          break;
        case "default":
          this.ProcessDefault();
          break;
        case "hidden":
          this.CurrentSchema.Hidden = new bool?((bool) this._reader.Value);
          break;
        case "readonly":
          this.CurrentSchema.ReadOnly = new bool?((bool) this._reader.Value);
          break;
        case "format":
          this.CurrentSchema.Format = (string) this._reader.Value;
          break;
        case "pattern":
          this.CurrentSchema.Pattern = (string) this._reader.Value;
          break;
        case "options":
          this.ProcessOptions();
          break;
        case "enum":
          this.ProcessEnum();
          break;
        case "extends":
          this.ProcessExtends();
          break;
        default:
          this._reader.Skip();
          break;
      }
    }

    private void ProcessExtends() => this.CurrentSchema.Extends = this.BuildSchema();

    private void ProcessEnum()
    {
      if (this._reader.TokenType != JsonToken.StartArray)
        throw new Exception("Expected StartArray token while parsing enum values, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      this.CurrentSchema.Enum = (IList<JToken>) new List<JToken>();
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
        this.CurrentSchema.Enum.Add(JToken.ReadFrom(this._reader));
    }

    private void ProcessOptions()
    {
      this.CurrentSchema.Options = (IDictionary<JToken, string>) new Dictionary<JToken, string>((IEqualityComparer<JToken>) new JTokenEqualityComparer());
      if (this._reader.TokenType == JsonToken.StartArray)
      {
        while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
        {
          if (this._reader.TokenType != JsonToken.StartObject)
            throw new Exception("Expect object token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
          string str1 = (string) null;
          JToken key = (JToken) null;
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
          {
            string str2 = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
            this._reader.Read();
            switch (str2)
            {
              case "value":
                key = JToken.ReadFrom(this._reader);
                continue;
              case "label":
                str1 = (string) this._reader.Value;
                continue;
              default:
                throw new Exception("Unexpected property in JSON schema option: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str2));
            }
          }
          if (key == null)
            throw new Exception("No value specified for JSON schema option.");
          if (this.CurrentSchema.Options.ContainsKey(key))
            throw new Exception("Duplicate value in JSON schema option collection: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) key));
          this.CurrentSchema.Options.Add(key, str1);
        }
      }
      else
        throw new Exception("Expected array token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
    }

    private void ProcessDefault() => this.CurrentSchema.Default = JToken.ReadFrom(this._reader);

    private void ProcessIdentity()
    {
      this.CurrentSchema.Identity = (IList<string>) new List<string>();
      switch (this._reader.TokenType)
      {
        case JsonToken.StartArray:
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
          {
            if (this._reader.TokenType != JsonToken.String)
              throw new Exception("Exception JSON property name string token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
            this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
          }
          break;
        case JsonToken.String:
          this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
          break;
        default:
          throw new Exception("Expected array or JSON property name string token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      }
    }

    private void ProcessAdditionalProperties()
    {
      if (this._reader.TokenType == JsonToken.Boolean)
        this.CurrentSchema.AllowAdditionalProperties = (bool) this._reader.Value;
      else
        this.CurrentSchema.AdditionalProperties = this.BuildSchema();
    }

    private void ProcessPatternProperties()
    {
      Dictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
      if (this._reader.TokenType != JsonToken.StartObject)
        throw new Exception("Expected start object token.");
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
      {
        string key = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._reader.Read();
        if (dictionary.ContainsKey(key))
          throw new Exception("Property {0} has already been defined in schema.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) key));
        dictionary.Add(key, this.BuildSchema());
      }
      this.CurrentSchema.PatternProperties = (IDictionary<string, JsonSchema>) dictionary;
    }

    private void ProcessItems()
    {
      this.CurrentSchema.Items = (IList<JsonSchema>) new List<JsonSchema>();
      switch (this._reader.TokenType)
      {
        case JsonToken.StartObject:
          this.CurrentSchema.Items.Add(this.BuildSchema());
          break;
        case JsonToken.StartArray:
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
            this.CurrentSchema.Items.Add(this.BuildSchema());
          break;
        default:
          throw new Exception("Expected array or JSON schema object token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      }
    }

    private void ProcessProperties()
    {
      IDictionary<string, JsonSchema> dictionary = (IDictionary<string, JsonSchema>) new Dictionary<string, JsonSchema>();
      if (this._reader.TokenType != JsonToken.StartObject)
        throw new Exception("Expected StartObject token while parsing schema properties, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
      {
        string key = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._reader.Read();
        if (dictionary.ContainsKey(key))
          throw new Exception("Property {0} has already been defined in schema.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) key));
        dictionary.Add(key, this.BuildSchema());
      }
      this.CurrentSchema.Properties = dictionary;
    }

    private JsonSchemaType? ProcessType()
    {
      switch (this._reader.TokenType)
      {
        case JsonToken.StartArray:
          JsonSchemaType? nullable1 = new JsonSchemaType?(JsonSchemaType.None);
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
          {
            if (this._reader.TokenType != JsonToken.String)
              throw new Exception("Exception JSON schema type string token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
            JsonSchemaType? nullable2 = nullable1;
            JsonSchemaType jsonSchemaType = JsonSchemaBuilder.MapType(this._reader.Value.ToString());
            nullable1 = nullable2.HasValue ? new JsonSchemaType?(nullable2.GetValueOrDefault() | jsonSchemaType) : new JsonSchemaType?();
          }
          return nullable1;
        case JsonToken.String:
          return new JsonSchemaType?(JsonSchemaBuilder.MapType(this._reader.Value.ToString()));
        default:
          throw new Exception("Expected array or JSON schema type string token, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      }
    }

    internal static JsonSchemaType MapType(string type)
    {
      JsonSchemaType jsonSchemaType;
      if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out jsonSchemaType))
        throw new Exception("Invalid JSON schema type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      return jsonSchemaType;
    }

    internal static string MapType(JsonSchemaType type) => JsonSchemaConstants.JsonSchemaTypeMapping.Single<KeyValuePair<string, JsonSchemaType>>((Func<KeyValuePair<string, JsonSchemaType>, bool>) (kv => kv.Value == type)).Key;
  }
}
