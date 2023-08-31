// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalReader
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerInternalReader : JsonSerializerInternalBase
  {
    private JsonSerializerProxy _internalSerializer;

    public JsonSerializerInternalReader(JsonSerializer serializer)
      : base(serializer)
    {
    }

    public void Populate(JsonReader reader, object target)
    {
      ValidationUtils.ArgumentNotNull(target, nameof (target));
      Type type = target.GetType();
      JsonContract contract = this.Serializer.ContractResolver.ResolveContract(type);
      if (reader.TokenType == JsonToken.None)
        reader.Read();
      if (reader.TokenType == JsonToken.StartArray)
      {
        if (contract is JsonArrayContract)
          this.PopulateList(CollectionUtils.CreateCollectionWrapper(target), reader, (string) null, (JsonArrayContract) contract);
        else
          throw new JsonSerializationException("Cannot populate JSON array onto type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      }
      else if (reader.TokenType == JsonToken.StartObject)
      {
        this.CheckedRead(reader);
        string id = (string) null;
        if (reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", StringComparison.Ordinal))
        {
          this.CheckedRead(reader);
          id = reader.Value != null ? reader.Value.ToString() : (string) null;
          this.CheckedRead(reader);
        }
        switch (contract)
        {
          case JsonDictionaryContract _:
            this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(target), reader, (JsonDictionaryContract) contract, id);
            break;
          case JsonObjectContract _:
            this.PopulateObject(target, reader, (JsonObjectContract) contract, id);
            break;
          default:
            throw new JsonSerializationException("Cannot populate JSON object onto type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
        }
      }
      else
        throw new JsonSerializationException("Unexpected initial token '{0}' when populating object. Expected JSON object or array.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    private JsonContract GetContractSafe(Type type) => (object) type == null ? (JsonContract) null : this.Serializer.ContractResolver.ResolveContract(type);

    private JsonContract GetContractSafe(Type type, object value) => value == null ? this.GetContractSafe(type) : this.Serializer.ContractResolver.ResolveContract(value.GetType());

    public object Deserialize(JsonReader reader, Type objectType)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      return reader.TokenType == JsonToken.None && !this.ReadForType(reader, objectType, (JsonConverter) null) ? (object) null : this.CreateValueNonProperty(reader, objectType, this.GetContractSafe(objectType));
    }

    private JsonSerializerProxy GetInternalSerializer()
    {
      if (this._internalSerializer == null)
        this._internalSerializer = new JsonSerializerProxy(this);
      return this._internalSerializer;
    }

    private JToken CreateJToken(JsonReader reader, JsonContract contract)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (contract != null && (object) contract.UnderlyingType == (object) typeof (JRaw))
        return (JToken) JRaw.Create(reader);
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jtokenWriter.WriteToken(reader);
        return jtokenWriter.Token;
      }
    }

    private JToken CreateJObject(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jtokenWriter.WriteStartObject();
        if (reader.TokenType == JsonToken.PropertyName)
          jtokenWriter.WriteToken(reader, reader.Depth - 1);
        else
          jtokenWriter.WriteEndObject();
        return jtokenWriter.Token;
      }
    }

    private object CreateValueProperty(
      JsonReader reader,
      JsonProperty property,
      object target,
      bool gottenCurrentValue,
      object currentValue)
    {
      JsonContract contractSafe = this.GetContractSafe(property.PropertyType, currentValue);
      Type propertyType = property.PropertyType;
      JsonConverter converter = this.GetConverter(contractSafe, property.MemberConverter);
      if (converter == null || !converter.CanRead)
        return this.CreateValueInternal(reader, propertyType, contractSafe, property, currentValue);
      if (!gottenCurrentValue && target != null && property.Readable)
        currentValue = property.ValueProvider.GetValue(target);
      return converter.ReadJson(reader, propertyType, currentValue, (JsonSerializer) this.GetInternalSerializer());
    }

    private object CreateValueNonProperty(
      JsonReader reader,
      Type objectType,
      JsonContract contract)
    {
      JsonConverter converter = this.GetConverter(contract, (JsonConverter) null);
      return converter != null && converter.CanRead ? converter.ReadJson(reader, objectType, (object) null, (JsonSerializer) this.GetInternalSerializer()) : this.CreateValueInternal(reader, objectType, contract, (JsonProperty) null, (object) null);
    }

    private object CreateValueInternal(
      JsonReader reader,
      Type objectType,
      JsonContract contract,
      JsonProperty member,
      object existingValue)
    {
      if (contract is JsonLinqContract)
        return (object) this.CreateJToken(reader, contract);
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.StartObject:
            return this.CreateObject(reader, objectType, contract, member, existingValue);
          case JsonToken.StartArray:
            return this.CreateList(reader, objectType, contract, member, existingValue, (string) null);
          case JsonToken.StartConstructor:
          case JsonToken.EndConstructor:
            return (object) reader.Value.ToString();
          case JsonToken.Comment:
            continue;
          case JsonToken.Raw:
            return (object) new JRaw((object) (string) reader.Value);
          case JsonToken.Integer:
          case JsonToken.Float:
          case JsonToken.Boolean:
          case JsonToken.Date:
          case JsonToken.Bytes:
            return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
          case JsonToken.String:
            if (string.IsNullOrEmpty((string) reader.Value) && (object) objectType != null && ReflectionUtils.IsNullableType(objectType))
              return (object) null;
            return (object) objectType == (object) typeof (byte[]) ? (object) Convert.FromBase64String((string) reader.Value) : this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
          case JsonToken.Null:
          case JsonToken.Undefined:
            return (object) objectType == (object) typeof (DBNull) ? (object) DBNull.Value : this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
          default:
            throw new JsonSerializationException("Unexpected token while deserializing object: " + (object) reader.TokenType);
        }
      }
      while (reader.Read());
      throw new JsonSerializationException("Unexpected end when deserializing object.");
    }

    private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter)
    {
      JsonConverter converter = (JsonConverter) null;
      if (memberConverter != null)
        converter = memberConverter;
      else if (contract != null)
      {
        if (contract.Converter != null)
        {
          converter = contract.Converter;
        }
        else
        {
          JsonConverter matchingConverter;
          if ((matchingConverter = this.Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
            converter = matchingConverter;
          else if (contract.InternalConverter != null)
            converter = contract.InternalConverter;
        }
      }
      return converter;
    }

    private object CreateObject(
      JsonReader reader,
      Type objectType,
      JsonContract contract,
      JsonProperty member,
      object existingValue)
    {
      this.CheckedRead(reader);
      string str = (string) null;
      if (reader.TokenType == JsonToken.PropertyName)
      {
        bool flag;
        do
        {
          string a = reader.Value.ToString();
          if (string.Equals(a, "$ref", StringComparison.Ordinal))
          {
            this.CheckedRead(reader);
            if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null)
              throw new JsonSerializationException("JSON reference {0} property must have a string or null value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) "$ref"));
            string reference = reader.Value != null ? reader.Value.ToString() : (string) null;
            this.CheckedRead(reader);
            if (reference != null)
            {
              if (reader.TokenType == JsonToken.PropertyName)
                throw new JsonSerializationException("Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) "$ref"));
              return this.Serializer.ReferenceResolver.ResolveReference((object) this, reference);
            }
            flag = true;
          }
          else if (string.Equals(a, "$type", StringComparison.Ordinal))
          {
            this.CheckedRead(reader);
            string fullyQualifiedTypeName = reader.Value.ToString();
            this.CheckedRead(reader);
            if (((int) member?.TypeNameHandling ?? (int) this.Serializer.TypeNameHandling) != 0)
            {
              string typeName;
              string assemblyName;
              ReflectionUtils.SplitFullyQualifiedTypeName(fullyQualifiedTypeName, out typeName, out assemblyName);
              Type type;
              try
              {
                type = this.Serializer.Binder.BindToType(assemblyName, typeName);
              }
              catch (Exception ex)
              {
                throw new JsonSerializationException("Error resolving type specified in JSON '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) fullyQualifiedTypeName), ex);
              }
              if ((object) type == null)
                throw new JsonSerializationException("Type specified in JSON '{0}' was not resolved.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) fullyQualifiedTypeName));
              objectType = (object) objectType == null || objectType.IsAssignableFrom(type) ? type : throw new JsonSerializationException("Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type.AssemblyQualifiedName, (object) objectType.AssemblyQualifiedName));
              contract = this.GetContractSafe(type);
            }
            flag = true;
          }
          else if (string.Equals(a, "$id", StringComparison.Ordinal))
          {
            this.CheckedRead(reader);
            str = reader.Value != null ? reader.Value.ToString() : (string) null;
            this.CheckedRead(reader);
            flag = true;
          }
          else
          {
            if (string.Equals(a, "$values", StringComparison.Ordinal))
            {
              this.CheckedRead(reader);
              object list = this.CreateList(reader, objectType, contract, member, existingValue, str);
              this.CheckedRead(reader);
              return list;
            }
            flag = false;
          }
        }
        while (flag && reader.TokenType == JsonToken.PropertyName);
      }
      if (!this.HasDefinedType(objectType))
        return (object) this.CreateJObject(reader);
      switch (contract)
      {
        case null:
          throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        case JsonDictionaryContract contract2:
          return existingValue == null ? this.CreateAndPopulateDictionary(reader, contract2, str) : this.PopulateDictionary(contract2.CreateWrapper(existingValue), reader, contract2, str);
        case JsonObjectContract contract3:
          return existingValue == null ? this.CreateAndPopulateObject(reader, contract3, str) : this.PopulateObject(existingValue, reader, contract3, str);
        default:
          JsonPrimitiveContract contract1 = contract as JsonPrimitiveContract;
          if ((object) contract1 != null && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", StringComparison.Ordinal))
          {
            this.CheckedRead(reader);
            object valueInternal = this.CreateValueInternal(reader, objectType, (JsonContract) contract1, member, existingValue);
            this.CheckedRead(reader);
            return valueInternal;
          }
          throw new JsonSerializationException("Cannot deserialize JSON object into type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      }
    }

    private JsonArrayContract EnsureArrayContract(Type objectType, JsonContract contract)
    {
      if (contract == null)
        throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      return contract is JsonArrayContract jsonArrayContract ? jsonArrayContract : throw new JsonSerializationException("Cannot deserialize JSON array into type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
    }

    private void CheckedRead(JsonReader reader)
    {
      if (!reader.Read())
        throw new JsonSerializationException("Unexpected end when deserializing object.");
    }

    private object CreateList(
      JsonReader reader,
      Type objectType,
      JsonContract contract,
      JsonProperty member,
      object existingValue,
      string reference)
    {
      object list;
      if (this.HasDefinedType(objectType))
      {
        JsonArrayContract contract1 = this.EnsureArrayContract(objectType, contract);
        list = existingValue != null ? this.PopulateList(contract1.CreateWrapper(existingValue), reader, reference, contract1) : this.CreateAndPopulateList(reader, reference, contract1);
      }
      else
        list = (object) this.CreateJToken(reader, contract);
      return list;
    }

    private bool HasDefinedType(Type type) => (object) type != null && (object) type != (object) typeof (object) && !typeof (JToken).IsAssignableFrom(type);

    private object EnsureType(object value, CultureInfo culture, Type targetType)
    {
      if ((object) targetType == null)
        return value;
      if ((object) ReflectionUtils.GetObjectType(value) == (object) targetType)
        return value;
      try
      {
        return ConvertUtils.ConvertOrCast(value, culture, targetType);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error converting value {0} to type '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.FormatValueForPrint(value), (object) targetType), ex);
      }
    }

    private string FormatValueForPrint(object value)
    {
      if (value == null)
        return "{null}";
      return value is string ? "\"" + value + "\"" : value.ToString();
    }

    private void SetPropertyValue(JsonProperty property, JsonReader reader, object target)
    {
      if (property.Ignored)
      {
        reader.Skip();
      }
      else
      {
        object obj = (object) null;
        bool flag = false;
        bool gottenCurrentValue = false;
        switch (property.ObjectCreationHandling.GetValueOrDefault(this.Serializer.ObjectCreationHandling))
        {
          case ObjectCreationHandling.Auto:
          case ObjectCreationHandling.Reuse:
            if ((reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject) && property.Readable)
            {
              obj = property.ValueProvider.GetValue(target);
              gottenCurrentValue = true;
              flag = obj != null && !property.PropertyType.IsArray && !ReflectionUtils.InheritsGenericDefinition(property.PropertyType, typeof (ReadOnlyCollection<>)) && !property.PropertyType.IsValueType;
              break;
            }
            break;
        }
        if (!property.Writable && !flag)
          reader.Skip();
        else if (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) == NullValueHandling.Ignore && reader.TokenType == JsonToken.Null)
          reader.Skip();
        else if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) && JsonReader.IsPrimitiveToken(reader.TokenType) && MiscellaneousUtils.ValueEquals(reader.Value, property.DefaultValue))
        {
          reader.Skip();
        }
        else
        {
          object currentValue = flag ? obj : (object) null;
          object valueProperty = this.CreateValueProperty(reader, property, target, gottenCurrentValue, currentValue);
          if (flag && valueProperty == obj || !this.ShouldSetPropertyValue(property, valueProperty))
            return;
          property.ValueProvider.SetValue(target, valueProperty);
          if (property.SetIsSpecified == null)
            return;
          property.SetIsSpecified(target, (object) true);
        }
      }
    }

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag) => (value & flag) == flag;

    private bool ShouldSetPropertyValue(JsonProperty property, object value) => (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) != NullValueHandling.Ignore || value != null) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(value, property.DefaultValue)) && property.Writable;

    private object CreateAndPopulateDictionary(
      JsonReader reader,
      JsonDictionaryContract contract,
      string id)
    {
      if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || this.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
      {
        object dictionary = contract.DefaultCreator();
        IWrappedDictionary wrapper = contract.CreateWrapper(dictionary);
        this.PopulateDictionary(wrapper, reader, contract, id);
        return wrapper.UnderlyingDictionary;
      }
      throw new JsonSerializationException("Unable to find a default constructor to use for type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
    }

    private object PopulateDictionary(
      IWrappedDictionary dictionary,
      JsonReader reader,
      JsonDictionaryContract contract,
      string id)
    {
      if (id != null)
        this.Serializer.ReferenceResolver.AddReference((object) this, id, dictionary.UnderlyingDictionary);
      contract.InvokeOnDeserializing(dictionary.UnderlyingDictionary, this.Serializer.Context);
      int depth = reader.Depth;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            object obj = reader.Value;
            try
            {
              try
              {
                obj = this.EnsureType(obj, CultureInfo.InvariantCulture, contract.DictionaryKeyType);
              }
              catch (Exception ex)
              {
                throw new JsonSerializationException("Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, reader.Value, (object) contract.DictionaryKeyType), ex);
              }
              if (!this.ReadForType(reader, contract.DictionaryValueType, (JsonConverter) null))
                throw new JsonSerializationException("Unexpected end when deserializing object.");
              dictionary[obj] = this.CreateValueNonProperty(reader, contract.DictionaryValueType, this.GetContractSafe(contract.DictionaryValueType));
              goto case JsonToken.Comment;
            }
            catch (Exception ex)
            {
              if (this.IsErrorHandled((object) dictionary, (JsonContract) contract, obj, ex))
              {
                this.HandleError(reader, depth);
                goto case JsonToken.Comment;
              }
              else
                throw;
            }
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            contract.InvokeOnDeserialized(dictionary.UnderlyingDictionary, this.Serializer.Context);
            return dictionary.UnderlyingDictionary;
          default:
            throw new JsonSerializationException("Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (reader.Read());
      throw new JsonSerializationException("Unexpected end when deserializing object.");
    }

    private object CreateAndPopulateList(
      JsonReader reader,
      string reference,
      JsonArrayContract contract)
    {
      return CollectionUtils.CreateAndPopulateList(contract.CreatedType, (Action<IList, bool>) ((l, isTemporaryListReference) =>
      {
        if (reference != null && isTemporaryListReference)
          throw new JsonSerializationException("Cannot preserve reference to array or readonly list: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
        if ((object) contract.OnSerializing != null && isTemporaryListReference)
          throw new JsonSerializationException("Cannot call OnSerializing on an array or readonly list: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
        if ((object) contract.OnError != null && isTemporaryListReference)
          throw new JsonSerializationException("Cannot call OnError on an array or readonly list: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
        this.PopulateList(contract.CreateWrapper((object) l), reader, reference, contract);
      }));
    }

    private bool ReadForTypeArrayHack(JsonReader reader, Type t)
    {
      try
      {
        return this.ReadForType(reader, t, (JsonConverter) null);
      }
      catch (JsonReaderException ex)
      {
        if (reader.TokenType == JsonToken.EndArray)
          return true;
        throw;
      }
    }

    private object PopulateList(
      IWrappedCollection wrappedList,
      JsonReader reader,
      string reference,
      JsonArrayContract contract)
    {
      object underlyingCollection = wrappedList.UnderlyingCollection;
      if (wrappedList.IsFixedSize)
      {
        reader.Skip();
        return wrappedList.UnderlyingCollection;
      }
      if (reference != null)
        this.Serializer.ReferenceResolver.AddReference((object) this, reference, underlyingCollection);
      contract.InvokeOnDeserializing(underlyingCollection, this.Serializer.Context);
      int depth = reader.Depth;
      while (this.ReadForTypeArrayHack(reader, contract.CollectionItemType))
      {
        switch (reader.TokenType)
        {
          case JsonToken.Comment:
            continue;
          case JsonToken.EndArray:
            contract.InvokeOnDeserialized(underlyingCollection, this.Serializer.Context);
            return wrappedList.UnderlyingCollection;
          default:
            try
            {
              object valueNonProperty = this.CreateValueNonProperty(reader, contract.CollectionItemType, this.GetContractSafe(contract.CollectionItemType));
              wrappedList.Add(valueNonProperty);
              continue;
            }
            catch (Exception ex)
            {
              if (this.IsErrorHandled(underlyingCollection, (JsonContract) contract, (object) wrappedList.Count, ex))
              {
                this.HandleError(reader, depth);
                continue;
              }
              throw;
            }
        }
      }
      throw new JsonSerializationException("Unexpected end when deserializing array.");
    }

    private object CreateAndPopulateObject(
      JsonReader reader,
      JsonObjectContract contract,
      string id)
    {
      object newObject = (object) null;
      if (contract.UnderlyingType.IsInterface || contract.UnderlyingType.IsAbstract)
        throw new JsonSerializationException("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantated.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
      if ((object) contract.OverrideConstructor != null)
      {
        if (contract.OverrideConstructor.GetParameters().Length > 0)
          return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.OverrideConstructor, id);
        newObject = contract.OverrideConstructor.Invoke((object[]) null);
      }
      else if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || this.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
        newObject = contract.DefaultCreator();
      else if ((object) contract.ParametrizedConstructor != null)
        return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.ParametrizedConstructor, id);
      if (newObject == null)
        throw new JsonSerializationException("Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
      this.PopulateObject(newObject, reader, contract, id);
      return newObject;
    }

    private object CreateObjectFromNonDefaultConstructor(
      JsonReader reader,
      JsonObjectContract contract,
      ConstructorInfo constructorInfo,
      string id)
    {
      ValidationUtils.ArgumentNotNull((object) constructorInfo, nameof (constructorInfo));
      Type underlyingType = contract.UnderlyingType;
      IDictionary<JsonProperty, object> dictionary1 = this.ResolvePropertyAndConstructorValues(contract, reader, underlyingType);
      IDictionary<ParameterInfo, object> dictionary2 = (IDictionary<ParameterInfo, object>) ((IEnumerable<ParameterInfo>) constructorInfo.GetParameters()).ToDictionary<ParameterInfo, ParameterInfo, object>((Func<ParameterInfo, ParameterInfo>) (p => p), (Func<ParameterInfo, object>) (p => (object) null));
      IDictionary<JsonProperty, object> dictionary3 = (IDictionary<JsonProperty, object>) new Dictionary<JsonProperty, object>();
      foreach (KeyValuePair<JsonProperty, object> keyValuePair in (IEnumerable<KeyValuePair<JsonProperty, object>>) dictionary1)
      {
        ParameterInfo key = dictionary2.ForgivingCaseSensitiveFind<KeyValuePair<ParameterInfo, object>>((Func<KeyValuePair<ParameterInfo, object>, string>) (kv => kv.Key.Name), keyValuePair.Key.UnderlyingName).Key;
        if (key != null)
          dictionary2[key] = keyValuePair.Value;
        else
          dictionary3.Add(keyValuePair);
      }
      object defaultConstructor = constructorInfo.Invoke(dictionary2.Values.ToArray<object>());
      if (id != null)
        this.Serializer.ReferenceResolver.AddReference((object) this, id, defaultConstructor);
      contract.InvokeOnDeserializing(defaultConstructor, this.Serializer.Context);
      foreach (KeyValuePair<JsonProperty, object> keyValuePair in (IEnumerable<KeyValuePair<JsonProperty, object>>) dictionary3)
      {
        JsonProperty key = keyValuePair.Key;
        object obj1 = keyValuePair.Value;
        if (this.ShouldSetPropertyValue(keyValuePair.Key, keyValuePair.Value))
          key.ValueProvider.SetValue(defaultConstructor, obj1);
        else if (!key.Writable && obj1 != null)
        {
          JsonContract jsonContract = this.Serializer.ContractResolver.ResolveContract(key.PropertyType);
          if (jsonContract is JsonArrayContract)
          {
            JsonArrayContract jsonArrayContract = jsonContract as JsonArrayContract;
            object list = key.ValueProvider.GetValue(defaultConstructor);
            if (list != null)
            {
              IWrappedCollection wrapper = jsonArrayContract.CreateWrapper(list);
              foreach (object obj2 in (IEnumerable) jsonArrayContract.CreateWrapper(obj1))
                wrapper.Add(obj2);
            }
          }
          else if (jsonContract is JsonDictionaryContract)
          {
            JsonDictionaryContract dictionaryContract = jsonContract as JsonDictionaryContract;
            object dictionary4 = key.ValueProvider.GetValue(defaultConstructor);
            if (dictionary4 != null)
            {
              IWrappedDictionary wrapper = dictionaryContract.CreateWrapper(dictionary4);
              foreach (DictionaryEntry dictionaryEntry in (IDictionary) dictionaryContract.CreateWrapper(obj1))
                wrapper.Add(dictionaryEntry.Key, dictionaryEntry.Value);
            }
          }
        }
      }
      contract.InvokeOnDeserialized(defaultConstructor, this.Serializer.Context);
      return defaultConstructor;
    }

    private IDictionary<JsonProperty, object> ResolvePropertyAndConstructorValues(
      JsonObjectContract contract,
      JsonReader reader,
      Type objectType)
    {
      IDictionary<JsonProperty, object> dictionary = (IDictionary<JsonProperty, object>) new Dictionary<JsonProperty, object>();
      bool flag = false;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string propertyName = reader.Value.ToString();
            JsonProperty jsonProperty = contract.ConstructorParameters.GetClosestMatchProperty(propertyName) ?? contract.Properties.GetClosestMatchProperty(propertyName);
            if (jsonProperty != null)
            {
              if (!this.ReadForType(reader, jsonProperty.PropertyType, jsonProperty.Converter))
                throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
              if (!jsonProperty.Ignored)
              {
                dictionary[jsonProperty] = this.CreateValueProperty(reader, jsonProperty, (object) null, true, (object) null);
                goto case JsonToken.Comment;
              }
              else
              {
                reader.Skip();
                goto case JsonToken.Comment;
              }
            }
            else
            {
              if (!reader.Read())
                throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
              if (this.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
                throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName, (object) objectType.Name));
              reader.Skip();
              goto case JsonToken.Comment;
            }
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            flag = true;
            goto case JsonToken.Comment;
          default:
            throw new JsonSerializationException("Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (!flag && reader.Read());
      return dictionary;
    }

    private bool ReadForType(JsonReader reader, Type t, JsonConverter propertyConverter)
    {
      if (this.GetConverter(this.GetContractSafe(t), propertyConverter) != null)
        return reader.Read();
      if ((object) t == (object) typeof (byte[]))
      {
        reader.ReadAsBytes();
        return true;
      }
      if ((object) t == (object) typeof (Decimal) || (object) t == (object) typeof (Decimal?))
      {
        reader.ReadAsDecimal();
        return true;
      }
      if ((object) t == (object) typeof (DateTimeOffset) || (object) t == (object) typeof (DateTimeOffset?))
      {
        reader.ReadAsDateTimeOffset();
        return true;
      }
      while (reader.Read())
      {
        if (reader.TokenType != JsonToken.Comment)
          return true;
      }
      return false;
    }

    private object PopulateObject(
      object newObject,
      JsonReader reader,
      JsonObjectContract contract,
      string id)
    {
      contract.InvokeOnDeserializing(newObject, this.Serializer.Context);
      Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary = contract.Properties.ToDictionary<JsonProperty, JsonProperty, JsonSerializerInternalReader.PropertyPresence>((Func<JsonProperty, JsonProperty>) (m => m), (Func<JsonProperty, JsonSerializerInternalReader.PropertyPresence>) (m => JsonSerializerInternalReader.PropertyPresence.None));
      if (id != null)
        this.Serializer.ReferenceResolver.AddReference((object) this, id, newObject);
      int depth = reader.Depth;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string str = reader.Value.ToString();
            try
            {
              JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(str);
              if (closestMatchProperty == null)
              {
                if (this.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
                  throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str, (object) contract.UnderlyingType.Name));
                reader.Skip();
                goto case JsonToken.Comment;
              }
              else
              {
                if (!this.ReadForType(reader, closestMatchProperty.PropertyType, closestMatchProperty.Converter))
                  throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str));
                this.SetPropertyPresence(reader, closestMatchProperty, dictionary);
                this.SetPropertyValue(closestMatchProperty, reader, newObject);
                goto case JsonToken.Comment;
              }
            }
            catch (Exception ex)
            {
              if (this.IsErrorHandled(newObject, (JsonContract) contract, (object) str, ex))
              {
                this.HandleError(reader, depth);
                goto case JsonToken.Comment;
              }
              else
                throw;
            }
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            foreach (KeyValuePair<JsonProperty, JsonSerializerInternalReader.PropertyPresence> keyValuePair in dictionary)
            {
              JsonProperty key = keyValuePair.Key;
              switch (keyValuePair.Value)
              {
                case JsonSerializerInternalReader.PropertyPresence.None:
                  if (key.Required == Required.AllowNull || key.Required == Required.Always)
                    throw new JsonSerializationException("Required property '{0}' not found in JSON.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) key.PropertyName));
                  if (this.HasFlag(key.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Populate) && key.Writable)
                  {
                    key.ValueProvider.SetValue(newObject, this.EnsureType(key.DefaultValue, CultureInfo.InvariantCulture, key.PropertyType));
                    continue;
                  }
                  continue;
                case JsonSerializerInternalReader.PropertyPresence.Null:
                  if (key.Required == Required.Always)
                    throw new JsonSerializationException("Required property '{0}' expects a value but got null.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) key.PropertyName));
                  continue;
                default:
                  continue;
              }
            }
            contract.InvokeOnDeserialized(newObject, this.Serializer.Context);
            return newObject;
          default:
            throw new JsonSerializationException("Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (reader.Read());
      throw new JsonSerializationException("Unexpected end when deserializing object.");
    }

    private void SetPropertyPresence(
      JsonReader reader,
      JsonProperty property,
      Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> requiredProperties)
    {
      if (property == null)
        return;
      requiredProperties[property] = reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined ? JsonSerializerInternalReader.PropertyPresence.Null : JsonSerializerInternalReader.PropertyPresence.Value;
    }

    private void HandleError(JsonReader reader, int initialDepth)
    {
      this.ClearErrorContext();
      reader.Skip();
      while (reader.Depth > initialDepth + 1)
        reader.Read();
    }

    internal enum PropertyPresence
    {
      None,
      Null,
      Value,
    }
  }
}
