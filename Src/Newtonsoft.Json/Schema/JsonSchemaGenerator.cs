// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaGenerator
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Schema
{
  public class JsonSchemaGenerator
  {
    private IContractResolver _contractResolver;
    private JsonSchemaResolver _resolver;
    private IList<JsonSchemaGenerator.TypeSchema> _stack = (IList<JsonSchemaGenerator.TypeSchema>) new List<JsonSchemaGenerator.TypeSchema>();
    private JsonSchema _currentSchema;

    public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

    public IContractResolver ContractResolver
    {
      get => this._contractResolver == null ? DefaultContractResolver.Instance : this._contractResolver;
      set => this._contractResolver = value;
    }

    private JsonSchema CurrentSchema => this._currentSchema;

    private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
    {
      this._currentSchema = typeSchema.Schema;
      this._stack.Add(typeSchema);
      this._resolver.LoadedSchemas.Add(typeSchema.Schema);
    }

    private JsonSchemaGenerator.TypeSchema Pop()
    {
      JsonSchemaGenerator.TypeSchema typeSchema1 = this._stack[this._stack.Count - 1];
      this._stack.RemoveAt(this._stack.Count - 1);
      JsonSchemaGenerator.TypeSchema typeSchema2 = this._stack.LastOrDefault<JsonSchemaGenerator.TypeSchema>();
      this._currentSchema = typeSchema2 == null ? (JsonSchema) null : typeSchema2.Schema;
      return typeSchema1;
    }

    public JsonSchema Generate(Type type) => this.Generate(type, new JsonSchemaResolver(), false);

    public JsonSchema Generate(Type type, JsonSchemaResolver resolver) => this.Generate(type, resolver, false);

    public JsonSchema Generate(Type type, bool rootSchemaNullable) => this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);

    public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      ValidationUtils.ArgumentNotNull((object) resolver, nameof (resolver));
      this._resolver = resolver;
      return this.GenerateInternal(type, !rootSchemaNullable ? Required.Always : Required.Default, false);
    }

    private string GetTitle(Type type)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
      return containerAttribute != null && !string.IsNullOrEmpty(containerAttribute.Title) ? containerAttribute.Title : (string) null;
    }

    private string GetDescription(Type type)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
      if (containerAttribute != null && !string.IsNullOrEmpty(containerAttribute.Description))
        return containerAttribute.Description;
      return ReflectionUtils.GetAttribute<DescriptionAttribute>((ICustomAttributeProvider) type)?.Description;
    }

    private string GetTypeId(Type type, bool explicitOnly)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
      if (containerAttribute != null && !string.IsNullOrEmpty(containerAttribute.Id))
        return containerAttribute.Id;
      if (explicitOnly)
        return (string) null;
      switch (this.UndefinedSchemaIdHandling)
      {
        case UndefinedSchemaIdHandling.UseTypeName:
          return type.FullName;
        case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
          return type.AssemblyQualifiedName;
        default:
          return (string) null;
      }
    }

    private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      string typeId1 = this.GetTypeId(type, false);
      string typeId2 = this.GetTypeId(type, true);
      if (!string.IsNullOrEmpty(typeId1))
      {
        JsonSchema schema = this._resolver.GetSchema(typeId1);
        if (schema != null)
        {
          if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
          {
            JsonSchema jsonSchema = schema;
            JsonSchemaType? type1 = jsonSchema.Type;
            jsonSchema.Type = type1.HasValue ? new JsonSchemaType?(type1.GetValueOrDefault() | JsonSchemaType.Null) : new JsonSchemaType?();
          }
          if (required)
          {
            bool? required1 = schema.Required;
            if ((!required1.GetValueOrDefault() ? 1 : (!required1.HasValue ? 1 : 0)) != 0)
              schema.Required = new bool?(true);
          }
          return schema;
        }
      }
      if (this._stack.Any<JsonSchemaGenerator.TypeSchema>((Func<JsonSchemaGenerator.TypeSchema, bool>) (tc => (object) tc.Type == (object) type)))
        throw new Exception("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      JsonContract contract = this.ContractResolver.ResolveContract(type);
      JsonConverter jsonConverter;
      if ((jsonConverter = contract.Converter) != null || (jsonConverter = contract.InternalConverter) != null)
      {
        JsonSchema schema = jsonConverter.GetSchema();
        if (schema != null)
          return schema;
      }
      this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
      if (typeId2 != null)
        this.CurrentSchema.Id = typeId2;
      if (required)
        this.CurrentSchema.Required = new bool?(true);
      this.CurrentSchema.Title = this.GetTitle(type);
      this.CurrentSchema.Description = this.GetDescription(type);
      if (jsonConverter != null)
        this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
      else if (contract is JsonDictionaryContract)
      {
        this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
        Type keyType;
        Type valueType;
        ReflectionUtils.GetDictionaryKeyValueTypes(type, out keyType, out valueType);
        if ((object) keyType != null && typeof (IConvertible).IsAssignableFrom(keyType))
          this.CurrentSchema.AdditionalProperties = this.GenerateInternal(valueType, Required.Default, false);
      }
      else if (contract is JsonArrayContract)
      {
        this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
        this.CurrentSchema.Id = this.GetTypeId(type, false);
        bool flag = !(JsonTypeReflector.GetJsonContainerAttribute(type) is JsonArrayAttribute containerAttribute) || containerAttribute.AllowNullItems;
        Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
        if ((object) collectionItemType != null)
        {
          this.CurrentSchema.Items = (IList<JsonSchema>) new List<JsonSchema>();
          this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, !flag ? Required.Always : Required.Default, false));
        }
      }
      else if ((object) (contract as JsonPrimitiveContract) != null)
      {
        this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
        JsonSchemaType? type2 = this.CurrentSchema.Type;
        if ((type2.GetValueOrDefault() != JsonSchemaType.Integer ? 0 : (type2.HasValue ? 1 : 0)) != 0 && type.IsEnum && !type.IsDefined(typeof (FlagsAttribute), true))
        {
          this.CurrentSchema.Enum = (IList<JToken>) new List<JToken>();
          this.CurrentSchema.Options = (IDictionary<JToken, string>) new Dictionary<JToken, string>();
          foreach (EnumValue<long> namesAndValue in (Collection<EnumValue<long>>) EnumUtils.GetNamesAndValues<long>(type))
          {
            JToken key = JToken.FromObject((object) namesAndValue.Value);
            this.CurrentSchema.Enum.Add(key);
            this.CurrentSchema.Options.Add(key, namesAndValue.Name);
          }
        }
      }
      else
      {
        switch (contract)
        {
          case JsonObjectContract _:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            this.GenerateObjectSchema(type, (JsonObjectContract) contract);
            break;
          case JsonStringContract _:
            this.CurrentSchema.Type = new JsonSchemaType?(!ReflectionUtils.IsNullable(contract.UnderlyingType) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired));
            break;
          case JsonLinqContract _:
            this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
            break;
          default:
            throw new Exception("Unexpected contract type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract));
        }
      }
      return this.Pop().Schema;
    }

    private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired) => valueRequired != Required.Always ? type | JsonSchemaType.Null : type;

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag) => (value & flag) == flag;

    private void GenerateObjectSchema(Type type, JsonObjectContract contract)
    {
      this.CurrentSchema.Properties = (IDictionary<string, JsonSchema>) new Dictionary<string, JsonSchema>();
      foreach (JsonProperty property in (Collection<JsonProperty>) contract.Properties)
      {
        if (!property.Ignored)
        {
          NullValueHandling? nullValueHandling = property.NullValueHandling;
          bool flag = (nullValueHandling.GetValueOrDefault() != NullValueHandling.Ignore ? 0 : (nullValueHandling.HasValue ? 1 : 0)) != 0 || this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || property.ShouldSerialize != null || property.GetIsSpecified != null;
          JsonSchema jsonSchema = this.GenerateInternal(property.PropertyType, property.Required, !flag);
          if (property.DefaultValue != null)
            jsonSchema.Default = JToken.FromObject(property.DefaultValue);
          this.CurrentSchema.Properties.Add(property.PropertyName, jsonSchema);
        }
      }
      if (!type.IsSealed)
        return;
      this.CurrentSchema.AllowAdditionalProperties = false;
    }

    internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
    {
      if (!value.HasValue)
        return true;
      JsonSchemaType? nullable1 = value;
      JsonSchemaType jsonSchemaType1 = flag;
      JsonSchemaType? nullable2 = nullable1.HasValue ? new JsonSchemaType?(nullable1.GetValueOrDefault() & jsonSchemaType1) : new JsonSchemaType?();
      JsonSchemaType jsonSchemaType2 = flag;
      return nullable2.GetValueOrDefault() == jsonSchemaType2 && nullable2.HasValue;
    }

    private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
    {
      JsonSchemaType jsonSchemaType = JsonSchemaType.None;
      if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
      {
        jsonSchemaType = JsonSchemaType.Null;
        if (ReflectionUtils.IsNullableType(type))
          type = Nullable.GetUnderlyingType(type);
      }
      TypeCode typeCode = Type.GetTypeCode(type);
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Object:
          return jsonSchemaType | JsonSchemaType.String;
        case TypeCode.DBNull:
          return jsonSchemaType | JsonSchemaType.Null;
        case TypeCode.Boolean:
          return jsonSchemaType | JsonSchemaType.Boolean;
        case TypeCode.Char:
          return jsonSchemaType | JsonSchemaType.String;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return jsonSchemaType | JsonSchemaType.Integer;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return jsonSchemaType | JsonSchemaType.Float;
        case TypeCode.DateTime:
          return jsonSchemaType | JsonSchemaType.String;
        case TypeCode.String:
          return jsonSchemaType | JsonSchemaType.String;
        default:
          throw new Exception("Unexpected type code '{0}' for type '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeCode, (object) type));
      }
    }

    private class TypeSchema
    {
      public Type Type { get; private set; }

      public JsonSchema Schema { get; private set; }

      public TypeSchema(Type type, JsonSchema schema)
      {
        ValidationUtils.ArgumentNotNull((object) type, nameof (type));
        ValidationUtils.ArgumentNotNull((object) schema, nameof (schema));
        this.Type = type;
        this.Schema = schema;
      }
    }
  }
}
