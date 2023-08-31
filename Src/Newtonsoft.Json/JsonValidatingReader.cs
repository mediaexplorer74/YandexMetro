// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonValidatingReader
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json
{
  public class JsonValidatingReader : JsonReader, IJsonLineInfo
  {
    private readonly JsonReader _reader;
    private readonly Stack<JsonValidatingReader.SchemaScope> _stack;
    private JsonSchema _schema;
    private JsonSchemaModel _model;
    private JsonValidatingReader.SchemaScope _currentScope;

    public event Newtonsoft.Json.Schema.ValidationEventHandler ValidationEventHandler;

    public override object Value => this._reader.Value;

    public override int Depth => this._reader.Depth;

    public override char QuoteChar
    {
      get => this._reader.QuoteChar;
      protected internal set
      {
      }
    }

    public override JsonToken TokenType => this._reader.TokenType;

    public override Type ValueType => this._reader.ValueType;

    private void Push(JsonValidatingReader.SchemaScope scope)
    {
      this._stack.Push(scope);
      this._currentScope = scope;
    }

    private JsonValidatingReader.SchemaScope Pop()
    {
      JsonValidatingReader.SchemaScope schemaScope = this._stack.Pop();
      this._currentScope = this._stack.Count != 0 ? this._stack.Peek() : (JsonValidatingReader.SchemaScope) null;
      return schemaScope;
    }

    private IEnumerable<JsonSchemaModel> CurrentSchemas => (IEnumerable<JsonSchemaModel>) this._currentScope.Schemas;

    private IEnumerable<JsonSchemaModel> CurrentMemberSchemas
    {
      get
      {
        if (this._currentScope == null)
          return (IEnumerable<JsonSchemaModel>) new List<JsonSchemaModel>((IEnumerable<JsonSchemaModel>) new JsonSchemaModel[1]
          {
            this._model
          });
        if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
          return Enumerable.Empty<JsonSchemaModel>();
        switch (this._currentScope.TokenType)
        {
          case JTokenType.None:
            return (IEnumerable<JsonSchemaModel>) this._currentScope.Schemas;
          case JTokenType.Object:
            if (this._currentScope.CurrentPropertyName == null)
              throw new Exception("CurrentPropertyName has not been set on scope.");
            IList<JsonSchemaModel> currentMemberSchemas1 = (IList<JsonSchemaModel>) new List<JsonSchemaModel>();
            foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
            {
              JsonSchemaModel jsonSchemaModel;
              if (currentSchema.Properties != null && currentSchema.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out jsonSchemaModel))
                currentMemberSchemas1.Add(jsonSchemaModel);
              if (currentSchema.PatternProperties != null)
              {
                foreach (KeyValuePair<string, JsonSchemaModel> patternProperty in (IEnumerable<KeyValuePair<string, JsonSchemaModel>>) currentSchema.PatternProperties)
                {
                  if (Regex.IsMatch(this._currentScope.CurrentPropertyName, patternProperty.Key))
                    currentMemberSchemas1.Add(patternProperty.Value);
                }
              }
              if (currentMemberSchemas1.Count == 0 && currentSchema.AllowAdditionalProperties && currentSchema.AdditionalProperties != null)
                currentMemberSchemas1.Add(currentSchema.AdditionalProperties);
            }
            return (IEnumerable<JsonSchemaModel>) currentMemberSchemas1;
          case JTokenType.Array:
            IList<JsonSchemaModel> currentMemberSchemas2 = (IList<JsonSchemaModel>) new List<JsonSchemaModel>();
            foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
            {
              if (!CollectionUtils.IsNullOrEmpty<JsonSchemaModel>((ICollection<JsonSchemaModel>) currentSchema.Items))
              {
                if (currentSchema.Items.Count == 1)
                  currentMemberSchemas2.Add(currentSchema.Items[0]);
                if (currentSchema.Items.Count > this._currentScope.ArrayItemCount - 1)
                  currentMemberSchemas2.Add(currentSchema.Items[this._currentScope.ArrayItemCount - 1]);
              }
              if (currentSchema.AllowAdditionalProperties && currentSchema.AdditionalProperties != null)
                currentMemberSchemas2.Add(currentSchema.AdditionalProperties);
            }
            return (IEnumerable<JsonSchemaModel>) currentMemberSchemas2;
          case JTokenType.Constructor:
            return Enumerable.Empty<JsonSchemaModel>();
          default:
            throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._currentScope.TokenType));
        }
      }
    }

    private void RaiseError(string message, JsonSchemaModel schema)
    {
      IJsonLineInfo jsonLineInfo = (IJsonLineInfo) this;
      string message1;
      if (!jsonLineInfo.HasLineInfo())
        message1 = message;
      else
        message1 = message + " Line {0}, position {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jsonLineInfo.LineNumber, (object) jsonLineInfo.LinePosition);
      this.OnValidationEvent(new JsonSchemaException(message1, (Exception) null, jsonLineInfo.LineNumber, jsonLineInfo.LinePosition));
    }

    private void OnValidationEvent(JsonSchemaException exception)
    {
      Newtonsoft.Json.Schema.ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
      if (validationEventHandler == null)
        throw exception;
      validationEventHandler((object) this, new ValidationEventArgs(exception));
    }

    public JsonValidatingReader(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      this._reader = reader;
      this._stack = new Stack<JsonValidatingReader.SchemaScope>();
    }

    public JsonSchema Schema
    {
      get => this._schema;
      set
      {
        if (this.TokenType != JsonToken.None)
          throw new Exception("Cannot change schema while validating JSON.");
        this._schema = value;
        this._model = (JsonSchemaModel) null;
      }
    }

    public JsonReader Reader => this._reader;

    private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      JToken jtoken = (JToken) new JValue(this._reader.Value);
      if (schema.Enum != null)
      {
        StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        jtoken.WriteTo((JsonWriter) new JsonTextWriter((TextWriter) stringWriter));
        if (!schema.Enum.ContainsValue<JToken>(jtoken, (IEqualityComparer<JToken>) new JTokenEqualityComparer()))
          this.RaiseError("Value {0} is not defined in enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) stringWriter.ToString()), schema);
      }
      JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
      if (!currentNodeSchemaType.HasValue || !JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.Value))
        return;
      this.RaiseError("Type {0} is disallowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) currentNodeSchemaType), schema);
    }

    private JsonSchemaType? GetCurrentNodeSchemaType()
    {
      switch (this._reader.TokenType)
      {
        case JsonToken.StartObject:
          return new JsonSchemaType?(JsonSchemaType.Object);
        case JsonToken.StartArray:
          return new JsonSchemaType?(JsonSchemaType.Array);
        case JsonToken.Integer:
          return new JsonSchemaType?(JsonSchemaType.Integer);
        case JsonToken.Float:
          return new JsonSchemaType?(JsonSchemaType.Float);
        case JsonToken.String:
          return new JsonSchemaType?(JsonSchemaType.String);
        case JsonToken.Boolean:
          return new JsonSchemaType?(JsonSchemaType.Boolean);
        case JsonToken.Null:
          return new JsonSchemaType?(JsonSchemaType.Null);
        default:
          return new JsonSchemaType?();
      }
    }

    public override byte[] ReadAsBytes()
    {
      byte[] numArray = this._reader.ReadAsBytes();
      this.ValidateCurrentToken();
      return numArray;
    }

    public override Decimal? ReadAsDecimal()
    {
      Decimal? nullable = this._reader.ReadAsDecimal();
      this.ValidateCurrentToken();
      return nullable;
    }

    public override DateTimeOffset? ReadAsDateTimeOffset()
    {
      DateTimeOffset? nullable = this._reader.ReadAsDateTimeOffset();
      this.ValidateCurrentToken();
      return nullable;
    }

    public override bool Read()
    {
      if (!this._reader.Read())
        return false;
      if (this._reader.TokenType == JsonToken.Comment)
        return true;
      this.ValidateCurrentToken();
      return true;
    }

    private void ValidateCurrentToken()
    {
      if (this._model == null)
        this._model = new JsonSchemaModelBuilder().Build(this._schema);
      switch (this._reader.TokenType)
      {
        case JsonToken.StartObject:
          this.ProcessValue();
          this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, (IList<JsonSchemaModel>) this.CurrentMemberSchemas.Where<JsonSchemaModel>(new Func<JsonSchemaModel, bool>(this.ValidateObject)).ToList<JsonSchemaModel>()));
          break;
        case JsonToken.StartArray:
          this.ProcessValue();
          this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, (IList<JsonSchemaModel>) this.CurrentMemberSchemas.Where<JsonSchemaModel>(new Func<JsonSchemaModel, bool>(this.ValidateArray)).ToList<JsonSchemaModel>()));
          break;
        case JsonToken.StartConstructor:
          this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, (IList<JsonSchemaModel>) null));
          break;
        case JsonToken.PropertyName:
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidatePropertyName(enumerator.Current);
            break;
          }
        case JsonToken.Raw:
          break;
        case JsonToken.Integer:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateInteger(enumerator.Current);
            break;
          }
        case JsonToken.Float:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateFloat(enumerator.Current);
            break;
          }
        case JsonToken.String:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateString(enumerator.Current);
            break;
          }
        case JsonToken.Boolean:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateBoolean(enumerator.Current);
            break;
          }
        case JsonToken.Null:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateNull(enumerator.Current);
            break;
          }
        case JsonToken.Undefined:
          break;
        case JsonToken.EndObject:
          foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
            this.ValidateEndObject(currentSchema);
          this.Pop();
          break;
        case JsonToken.EndArray:
          foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
            this.ValidateEndArray(currentSchema);
          this.Pop();
          break;
        case JsonToken.EndConstructor:
          this.Pop();
          break;
        case JsonToken.Date:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void ValidateEndObject(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
      if (requiredProperties == null)
        return;
      List<string> list = requiredProperties.Where<KeyValuePair<string, bool>>((Func<KeyValuePair<string, bool>, bool>) (kv => !kv.Value)).Select<KeyValuePair<string, bool>, string>((Func<KeyValuePair<string, bool>, string>) (kv => kv.Key)).ToList<string>();
      if (list.Count <= 0)
        return;
      this.RaiseError("Required properties are missing from object: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) string.Join(", ", list.ToArray())), schema);
    }

    private void ValidateEndArray(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      int arrayItemCount = this._currentScope.ArrayItemCount;
      if (schema.MaximumItems.HasValue)
      {
        int num = arrayItemCount;
        int? maximumItems = schema.MaximumItems;
        if ((num <= maximumItems.GetValueOrDefault() ? 0 : (maximumItems.HasValue ? 1 : 0)) != 0)
          this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) arrayItemCount, (object) schema.MaximumItems), schema);
      }
      if (!schema.MinimumItems.HasValue)
        return;
      int num1 = arrayItemCount;
      int? minimumItems = schema.MinimumItems;
      if ((num1 >= minimumItems.GetValueOrDefault() ? 0 : (minimumItems.HasValue ? 1 : 0)) == 0)
        return;
      this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) arrayItemCount, (object) schema.MinimumItems), schema);
    }

    private void ValidateNull(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Null))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
    }

    private void ValidateBoolean(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Boolean))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
    }

    private void ValidateString(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.String))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
      string input = this._reader.Value.ToString();
      if (schema.MaximumLength.HasValue)
      {
        int length = input.Length;
        int? maximumLength = schema.MaximumLength;
        if ((length <= maximumLength.GetValueOrDefault() ? 0 : (maximumLength.HasValue ? 1 : 0)) != 0)
          this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) input, (object) schema.MaximumLength), schema);
      }
      if (schema.MinimumLength.HasValue)
      {
        int length = input.Length;
        int? minimumLength = schema.MinimumLength;
        if ((length >= minimumLength.GetValueOrDefault() ? 0 : (minimumLength.HasValue ? 1 : 0)) != 0)
          this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) input, (object) schema.MinimumLength), schema);
      }
      if (schema.Patterns == null)
        return;
      foreach (string pattern in (IEnumerable<string>) schema.Patterns)
      {
        if (!Regex.IsMatch(input, pattern))
          this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) input, (object) pattern), schema);
      }
    }

    private void ValidateInteger(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Integer))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
      long int64 = Convert.ToInt64(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (schema.Maximum.HasValue)
      {
        double num1 = (double) int64;
        double? maximum1 = schema.Maximum;
        if ((num1 <= maximum1.GetValueOrDefault() ? 0 : (maximum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) int64, (object) schema.Maximum), schema);
        if (schema.ExclusiveMaximum)
        {
          double num2 = (double) int64;
          double? maximum2 = schema.Maximum;
          if ((num2 != maximum2.GetValueOrDefault() ? 0 : (maximum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) int64, (object) schema.Maximum), schema);
        }
      }
      if (schema.Minimum.HasValue)
      {
        double num3 = (double) int64;
        double? minimum1 = schema.Minimum;
        if ((num3 >= minimum1.GetValueOrDefault() ? 0 : (minimum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) int64, (object) schema.Minimum), schema);
        if (schema.ExclusiveMinimum)
        {
          double num4 = (double) int64;
          double? minimum2 = schema.Minimum;
          if ((num4 != minimum2.GetValueOrDefault() ? 0 : (minimum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) int64, (object) schema.Minimum), schema);
        }
      }
      if (!schema.DivisibleBy.HasValue || JsonValidatingReader.IsZero((double) int64 % schema.DivisibleBy.Value))
        return;
      this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(int64), (object) schema.DivisibleBy), schema);
    }

    private void ProcessValue()
    {
      if (this._currentScope == null || this._currentScope.TokenType != JTokenType.Array)
        return;
      ++this._currentScope.ArrayItemCount;
      foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
      {
        if (currentSchema != null && currentSchema.Items != null && currentSchema.Items.Count > 1 && this._currentScope.ArrayItemCount >= currentSchema.Items.Count)
          this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._currentScope.ArrayItemCount), currentSchema);
      }
    }

    private void ValidateFloat(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Float))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
      double num1 = Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (schema.Maximum.HasValue)
      {
        double num2 = num1;
        double? maximum1 = schema.Maximum;
        if ((num2 <= maximum1.GetValueOrDefault() ? 0 : (maximum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Maximum), schema);
        if (schema.ExclusiveMaximum)
        {
          double num3 = num1;
          double? maximum2 = schema.Maximum;
          if ((num3 != maximum2.GetValueOrDefault() ? 0 : (maximum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Maximum), schema);
        }
      }
      if (schema.Minimum.HasValue)
      {
        double num4 = num1;
        double? minimum1 = schema.Minimum;
        if ((num4 >= minimum1.GetValueOrDefault() ? 0 : (minimum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Minimum), schema);
        if (schema.ExclusiveMinimum)
        {
          double num5 = num1;
          double? minimum2 = schema.Minimum;
          if ((num5 != minimum2.GetValueOrDefault() ? 0 : (minimum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Minimum), schema);
        }
      }
      if (!schema.DivisibleBy.HasValue || JsonValidatingReader.IsZero(num1 % schema.DivisibleBy.Value))
        return;
      this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.DivisibleBy), schema);
    }

    private static bool IsZero(double value)
    {
      double num = 2.2204460492503131E-16;
      return Math.Abs(value) < 10.0 * num;
    }

    private void ValidatePropertyName(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      string str = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (this._currentScope.RequiredProperties.ContainsKey(str))
        this._currentScope.RequiredProperties[str] = true;
      if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, str))
        this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), schema);
      this._currentScope.CurrentPropertyName = str;
    }

    private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
    {
      if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
        return true;
      if (schema.PatternProperties != null)
      {
        foreach (string key in (IEnumerable<string>) schema.PatternProperties.Keys)
        {
          if (Regex.IsMatch(propertyName, key))
            return true;
        }
      }
      return false;
    }

    private bool ValidateArray(JsonSchemaModel schema) => schema == null || this.TestType(schema, JsonSchemaType.Array);

    private bool ValidateObject(JsonSchemaModel schema) => schema == null || this.TestType(schema, JsonSchemaType.Object);

    private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
    {
      if (JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
        return true;
      this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) currentSchema.Type, (object) currentType), currentSchema);
      return false;
    }

    bool IJsonLineInfo.HasLineInfo() => this._reader is IJsonLineInfo reader && reader.HasLineInfo();

    int IJsonLineInfo.LineNumber => !(this._reader is IJsonLineInfo reader) ? 0 : reader.LineNumber;

    int IJsonLineInfo.LinePosition => !(this._reader is IJsonLineInfo reader) ? 0 : reader.LinePosition;

    private class SchemaScope
    {
      private readonly JTokenType _tokenType;
      private readonly IList<JsonSchemaModel> _schemas;
      private readonly Dictionary<string, bool> _requiredProperties;

      public string CurrentPropertyName { get; set; }

      public int ArrayItemCount { get; set; }

      public IList<JsonSchemaModel> Schemas => this._schemas;

      public Dictionary<string, bool> RequiredProperties => this._requiredProperties;

      public JTokenType TokenType => this._tokenType;

      public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
      {
        this._tokenType = tokenType;
        this._schemas = schemas;
        this._requiredProperties = schemas.SelectMany<JsonSchemaModel, string>(new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties)).Distinct<string>().ToDictionary<string, string, bool>((Func<string, string>) (p => p), (Func<string, bool>) (p => false));
      }

      private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema) => schema == null || schema.Properties == null ? Enumerable.Empty<string>() : schema.Properties.Where<KeyValuePair<string, JsonSchemaModel>>((Func<KeyValuePair<string, JsonSchemaModel>, bool>) (p => p.Value.Required)).Select<KeyValuePair<string, JsonSchemaModel>, string>((Func<KeyValuePair<string, JsonSchemaModel>, string>) (p => p.Key));
    }
  }
}
