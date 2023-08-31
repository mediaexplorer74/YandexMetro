// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalWriter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
  {
    private JsonSerializerProxy _internalSerializer;
    private List<object> _serializeStack;

    private List<object> SerializeStack
    {
      get
      {
        if (this._serializeStack == null)
          this._serializeStack = new List<object>();
        return this._serializeStack;
      }
    }

    public JsonSerializerInternalWriter(JsonSerializer serializer)
      : base(serializer)
    {
    }

    public void Serialize(JsonWriter jsonWriter, object value)
    {
      if (jsonWriter == null)
        throw new ArgumentNullException(nameof (jsonWriter));
      this.SerializeValue(jsonWriter, value, this.GetContractSafe(value), (JsonProperty) null, (JsonContract) null);
    }

    private JsonSerializerProxy GetInternalSerializer()
    {
      if (this._internalSerializer == null)
        this._internalSerializer = new JsonSerializerProxy(this);
      return this._internalSerializer;
    }

    private JsonContract GetContractSafe(object value) => value == null ? (JsonContract) null : this.Serializer.ContractResolver.ResolveContract(value.GetType());

    private void SerializePrimitive(
      JsonWriter writer,
      object value,
      JsonPrimitiveContract contract,
      JsonProperty member,
      JsonContract collectionValueContract)
    {
      if ((object) contract.UnderlyingType == (object) typeof (byte[]) && this.ShouldWriteType(TypeNameHandling.Objects, (JsonContract) contract, member, collectionValueContract))
      {
        writer.WriteStartObject();
        this.WriteTypeProperty(writer, contract.CreatedType);
        writer.WritePropertyName("$value");
        writer.WriteValue(value);
        writer.WriteEndObject();
      }
      else
        writer.WriteValue(value);
    }

    private void SerializeValue(
      JsonWriter writer,
      object value,
      JsonContract valueContract,
      JsonProperty member,
      JsonContract collectionValueContract)
    {
      JsonConverter converter = member?.Converter;
      if (value == null)
        writer.WriteNull();
      else if ((converter != null || (converter = valueContract.Converter) != null || (converter = this.Serializer.GetMatchingConverter(valueContract.UnderlyingType)) != null || (converter = valueContract.InternalConverter) != null) && converter.CanWrite)
      {
        this.SerializeConvertable(writer, converter, value, valueContract);
      }
      else
      {
        switch (valueContract)
        {
          case JsonPrimitiveContract _:
            this.SerializePrimitive(writer, value, (JsonPrimitiveContract) valueContract, member, collectionValueContract);
            break;
          case JsonStringContract _:
            this.SerializeString(writer, value, (JsonStringContract) valueContract);
            break;
          case JsonObjectContract _:
            this.SerializeObject(writer, value, (JsonObjectContract) valueContract, member, collectionValueContract);
            break;
          case JsonDictionaryContract _:
            JsonDictionaryContract contract1 = (JsonDictionaryContract) valueContract;
            this.SerializeDictionary(writer, contract1.CreateWrapper(value), contract1, member, collectionValueContract);
            break;
          case JsonArrayContract _:
            JsonArrayContract contract2 = (JsonArrayContract) valueContract;
            this.SerializeList(writer, contract2.CreateWrapper(value), contract2, member, collectionValueContract);
            break;
          case JsonLinqContract _:
            ((JToken) value).WriteTo(writer, this.Serializer.Converters != null ? this.Serializer.Converters.ToArray<JsonConverter>() : (JsonConverter[]) null);
            break;
        }
      }
    }

    private bool ShouldWriteReference(object value, JsonProperty property, JsonContract contract)
    {
      if (value == null || contract is JsonPrimitiveContract)
        return false;
      bool? nullable = new bool?();
      if (property != null)
        nullable = property.IsReference;
      if (!nullable.HasValue)
        nullable = contract.IsReference;
      if (!nullable.HasValue)
        nullable = !(contract is JsonArrayContract) ? new bool?(this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects)) : new bool?(this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays));
      return nullable.Value && this.Serializer.ReferenceResolver.IsReferenced((object) this, value);
    }

    private void WriteMemberInfoProperty(
      JsonWriter writer,
      object memberValue,
      JsonProperty property,
      JsonContract contract)
    {
      string propertyName = property.PropertyName;
      object defaultValue = property.DefaultValue;
      if (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) == NullValueHandling.Ignore && memberValue == null || this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) && MiscellaneousUtils.ValueEquals(memberValue, defaultValue))
        return;
      if (this.ShouldWriteReference(memberValue, property, contract))
      {
        writer.WritePropertyName(propertyName);
        this.WriteReference(writer, memberValue);
      }
      else
      {
        if (!this.CheckForCircularReference(memberValue, property.ReferenceLoopHandling, contract))
          return;
        if (memberValue == null && property.Required == Required.Always)
          throw new JsonSerializationException("Cannot write a null value for property '{0}'. Property requires a value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName));
        writer.WritePropertyName(propertyName);
        this.SerializeValue(writer, memberValue, contract, property, (JsonContract) null);
      }
    }

    private bool CheckForCircularReference(
      object value,
      ReferenceLoopHandling? referenceLoopHandling,
      JsonContract contract)
    {
      if (value == null || contract is JsonPrimitiveContract || this.SerializeStack.IndexOf(value) == -1)
        return true;
      switch (referenceLoopHandling.GetValueOrDefault(this.Serializer.ReferenceLoopHandling))
      {
        case ReferenceLoopHandling.Error:
          throw new JsonSerializationException("Self referencing loop detected for type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
        case ReferenceLoopHandling.Ignore:
          return false;
        case ReferenceLoopHandling.Serialize:
          return true;
        default:
          throw new InvalidOperationException("Unexpected ReferenceLoopHandling value: '{0}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.Serializer.ReferenceLoopHandling));
      }
    }

    private void WriteReference(JsonWriter writer, object value)
    {
      writer.WriteStartObject();
      writer.WritePropertyName("$ref");
      writer.WriteValue(this.Serializer.ReferenceResolver.GetReference((object) this, value));
      writer.WriteEndObject();
    }

    internal static bool TryConvertToString(object value, Type type, out string s)
    {
      TypeConverter converter = ConvertUtils.GetConverter(type);
      if (converter != null && (object) converter.GetType() != (object) typeof (TypeConverter) && converter.CanConvertTo(typeof (string)))
      {
        s = converter.ConvertToString(value);
        return true;
      }
      if (value is Guid || (object) (value as Uri) != null || value is TimeSpan)
      {
        s = value.ToString();
        return true;
      }
      if ((object) (value as Type) != null)
      {
        s = ((Type) value).AssemblyQualifiedName;
        return true;
      }
      s = (string) null;
      return false;
    }

    private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
    {
      contract.InvokeOnSerializing(value, this.Serializer.Context);
      string s;
      JsonSerializerInternalWriter.TryConvertToString(value, contract.UnderlyingType, out s);
      writer.WriteValue(s);
      contract.InvokeOnSerialized(value, this.Serializer.Context);
    }

    private void SerializeObject(
      JsonWriter writer,
      object value,
      JsonObjectContract contract,
      JsonProperty member,
      JsonContract collectionValueContract)
    {
      contract.InvokeOnSerializing(value, this.Serializer.Context);
      this.SerializeStack.Add(value);
      writer.WriteStartObject();
      if (((int) contract.IsReference ?? (this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects) ? 1 : 0)) != 0)
      {
        writer.WritePropertyName("$id");
        writer.WriteValue(this.Serializer.ReferenceResolver.GetReference((object) this, value));
      }
      if (this.ShouldWriteType(TypeNameHandling.Objects, (JsonContract) contract, member, collectionValueContract))
        this.WriteTypeProperty(writer, contract.UnderlyingType);
      int top = writer.Top;
      foreach (JsonProperty property in (Collection<JsonProperty>) contract.Properties)
      {
        try
        {
          if (!property.Ignored)
          {
            if (property.Readable)
            {
              if (this.ShouldSerialize(property, value))
              {
                if (this.IsSpecified(property, value))
                {
                  object memberValue = property.ValueProvider.GetValue(value);
                  JsonContract contractSafe = this.GetContractSafe(memberValue);
                  this.WriteMemberInfoProperty(writer, memberValue, property, contractSafe);
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(value, (JsonContract) contract, (object) property.PropertyName, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
      }
      writer.WriteEndObject();
      this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
      contract.InvokeOnSerialized(value, this.Serializer.Context);
    }

    private void WriteTypeProperty(JsonWriter writer, Type type)
    {
      writer.WritePropertyName("$type");
      writer.WriteValue(ReflectionUtils.GetTypeName(type, this.Serializer.TypeNameAssemblyFormat, this.Serializer.Binder));
    }

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag) => (value & flag) == flag;

    private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag) => (value & flag) == flag;

    private bool HasFlag(TypeNameHandling value, TypeNameHandling flag) => (value & flag) == flag;

    private void SerializeConvertable(
      JsonWriter writer,
      JsonConverter converter,
      object value,
      JsonContract contract)
    {
      if (this.ShouldWriteReference(value, (JsonProperty) null, contract))
      {
        this.WriteReference(writer, value);
      }
      else
      {
        if (!this.CheckForCircularReference(value, new ReferenceLoopHandling?(), contract))
          return;
        this.SerializeStack.Add(value);
        converter.WriteJson(writer, value, (JsonSerializer) this.GetInternalSerializer());
        this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
      }
    }

    private void SerializeList(
      JsonWriter writer,
      IWrappedCollection values,
      JsonArrayContract contract,
      JsonProperty member,
      JsonContract collectionValueContract)
    {
      contract.InvokeOnSerializing(values.UnderlyingCollection, this.Serializer.Context);
      this.SerializeStack.Add(values.UnderlyingCollection);
      bool flag1 = ((int) contract.IsReference ?? (this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays) ? 1 : 0)) != 0;
      bool flag2 = this.ShouldWriteType(TypeNameHandling.Arrays, (JsonContract) contract, member, collectionValueContract);
      if (flag1 || flag2)
      {
        writer.WriteStartObject();
        if (flag1)
        {
          writer.WritePropertyName("$id");
          writer.WriteValue(this.Serializer.ReferenceResolver.GetReference((object) this, values.UnderlyingCollection));
        }
        if (flag2)
          this.WriteTypeProperty(writer, values.UnderlyingCollection.GetType());
        writer.WritePropertyName("$values");
      }
      IContractResolver contractResolver = this.Serializer.ContractResolver;
      Type type = contract.CollectionItemType;
      if ((object) type == null)
        type = typeof (object);
      JsonContract collectionValueContract1 = contractResolver.ResolveContract(type);
      writer.WriteStartArray();
      int top = writer.Top;
      int keyValue = 0;
      foreach (object obj in (IEnumerable) values)
      {
        try
        {
          JsonContract contractSafe = this.GetContractSafe(obj);
          if (this.ShouldWriteReference(obj, (JsonProperty) null, contractSafe))
            this.WriteReference(writer, obj);
          else if (this.CheckForCircularReference(obj, new ReferenceLoopHandling?(), (JsonContract) contract))
            this.SerializeValue(writer, obj, contractSafe, (JsonProperty) null, collectionValueContract1);
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(values.UnderlyingCollection, (JsonContract) contract, (object) keyValue, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
        finally
        {
          ++keyValue;
        }
      }
      writer.WriteEndArray();
      if (flag1 || flag2)
        writer.WriteEndObject();
      this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
      contract.InvokeOnSerialized(values.UnderlyingCollection, this.Serializer.Context);
    }

    private bool ShouldWriteType(
      TypeNameHandling typeNameHandlingFlag,
      JsonContract contract,
      JsonProperty member,
      JsonContract collectionValueContract)
    {
      if (this.HasFlag((TypeNameHandling) ((int) member?.TypeNameHandling ?? (int) this.Serializer.TypeNameHandling), typeNameHandlingFlag))
        return true;
      if (member != null)
      {
        if (((int) member.TypeNameHandling ?? (int) this.Serializer.TypeNameHandling) == 4 && (object) contract.UnderlyingType != (object) member.PropertyType)
        {
          JsonContract jsonContract = this.Serializer.ContractResolver.ResolveContract(member.PropertyType);
          if ((object) contract.UnderlyingType != (object) jsonContract.CreatedType)
            return true;
        }
      }
      else if (collectionValueContract != null && this.Serializer.TypeNameHandling == TypeNameHandling.Auto && (object) contract.UnderlyingType != (object) collectionValueContract.UnderlyingType)
        return true;
      return false;
    }

    private void SerializeDictionary(
      JsonWriter writer,
      IWrappedDictionary values,
      JsonDictionaryContract contract,
      JsonProperty member,
      JsonContract collectionValueContract)
    {
      contract.InvokeOnSerializing(values.UnderlyingDictionary, this.Serializer.Context);
      this.SerializeStack.Add(values.UnderlyingDictionary);
      writer.WriteStartObject();
      if (((int) contract.IsReference ?? (this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects) ? 1 : 0)) != 0)
      {
        writer.WritePropertyName("$id");
        writer.WriteValue(this.Serializer.ReferenceResolver.GetReference((object) this, values.UnderlyingDictionary));
      }
      if (this.ShouldWriteType(TypeNameHandling.Objects, (JsonContract) contract, member, collectionValueContract))
        this.WriteTypeProperty(writer, values.UnderlyingDictionary.GetType());
      IContractResolver contractResolver = this.Serializer.ContractResolver;
      Type type = contract.DictionaryValueType;
      if ((object) type == null)
        type = typeof (object);
      JsonContract collectionValueContract1 = contractResolver.ResolveContract(type);
      int top = writer.Top;
      foreach (DictionaryEntry entry in (IDictionary) values)
      {
        string propertyName = this.GetPropertyName(entry);
        string str = contract.PropertyNameResolver != null ? contract.PropertyNameResolver(propertyName) : propertyName;
        try
        {
          object obj = entry.Value;
          JsonContract contractSafe = this.GetContractSafe(obj);
          if (this.ShouldWriteReference(obj, (JsonProperty) null, contractSafe))
          {
            writer.WritePropertyName(str);
            this.WriteReference(writer, obj);
          }
          else if (this.CheckForCircularReference(obj, new ReferenceLoopHandling?(), (JsonContract) contract))
          {
            writer.WritePropertyName(str);
            this.SerializeValue(writer, obj, contractSafe, (JsonProperty) null, collectionValueContract1);
          }
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(values.UnderlyingDictionary, (JsonContract) contract, (object) str, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
      }
      writer.WriteEndObject();
      this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
      contract.InvokeOnSerialized(values.UnderlyingDictionary, this.Serializer.Context);
    }

    private string GetPropertyName(DictionaryEntry entry)
    {
      if (entry.Key is IConvertible)
        return Convert.ToString(entry.Key, (IFormatProvider) CultureInfo.InvariantCulture);
      string s;
      return JsonSerializerInternalWriter.TryConvertToString(entry.Key, entry.Key.GetType(), out s) ? s : entry.Key.ToString();
    }

    private void HandleError(JsonWriter writer, int initialDepth)
    {
      this.ClearErrorContext();
      while (writer.Top > initialDepth)
        writer.WriteEnd();
    }

    private bool ShouldSerialize(JsonProperty property, object target) => property.ShouldSerialize == null || property.ShouldSerialize(target);

    private bool IsSpecified(JsonProperty property, object target) => property.GetIsSpecified == null || property.GetIsSpecified(target);
  }
}
