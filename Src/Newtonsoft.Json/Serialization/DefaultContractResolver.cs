// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultContractResolver
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public class DefaultContractResolver : IContractResolver
  {
    private static readonly IContractResolver _instance = (IContractResolver) new DefaultContractResolver(true);
    private static readonly IList<JsonConverter> BuiltInConverters = (IList<JsonConverter>) new List<JsonConverter>()
    {
      (JsonConverter) new BinaryConverter(),
      (JsonConverter) new KeyValuePairConverter(),
      (JsonConverter) new XmlNodeConverter(),
      (JsonConverter) new BsonObjectIdConverter()
    };
    private static Dictionary<ResolverContractKey, JsonContract> _sharedContractCache;
    private static readonly object _typeContractCacheLock = new object();
    private Dictionary<ResolverContractKey, JsonContract> _instanceContractCache;
    private readonly bool _sharedCache;

    internal static IContractResolver Instance => DefaultContractResolver._instance;

    public bool DynamicCodeGeneration => JsonTypeReflector.DynamicCodeGeneration;

    public BindingFlags DefaultMembersSearchFlags { get; set; }

    public bool SerializeCompilerGeneratedMembers { get; set; }

    public DefaultContractResolver()
      : this(false)
    {
    }

    public DefaultContractResolver(bool shareCache)
    {
      this.DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public;
      this._sharedCache = shareCache;
    }

    private Dictionary<ResolverContractKey, JsonContract> GetCache() => this._sharedCache ? DefaultContractResolver._sharedContractCache : this._instanceContractCache;

    private void UpdateCache(
      Dictionary<ResolverContractKey, JsonContract> cache)
    {
      if (this._sharedCache)
        DefaultContractResolver._sharedContractCache = cache;
      else
        this._instanceContractCache = cache;
    }

    public virtual JsonContract ResolveContract(Type type)
    {
      ResolverContractKey key = (object) type != null ? new ResolverContractKey(this.GetType(), type) : throw new ArgumentNullException(nameof (type));
      Dictionary<ResolverContractKey, JsonContract> cache1 = this.GetCache();
      JsonContract contract;
      if (cache1 == null || !cache1.TryGetValue(key, out contract))
      {
        contract = this.CreateContract(type);
        lock (DefaultContractResolver._typeContractCacheLock)
        {
          Dictionary<ResolverContractKey, JsonContract> cache2 = this.GetCache();
          Dictionary<ResolverContractKey, JsonContract> cache3 = cache2 != null ? new Dictionary<ResolverContractKey, JsonContract>((IDictionary<ResolverContractKey, JsonContract>) cache2) : new Dictionary<ResolverContractKey, JsonContract>();
          cache3[key] = contract;
          this.UpdateCache(cache3);
        }
      }
      return contract;
    }

    protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
    {
      DataContractAttribute contractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
      List<MemberInfo> list1 = ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags).Where<MemberInfo>((Func<MemberInfo, bool>) (m => !ReflectionUtils.IsIndexedProperty(m))).ToList<MemberInfo>();
      List<MemberInfo> list2 = ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where<MemberInfo>((Func<MemberInfo, bool>) (m => !ReflectionUtils.IsIndexedProperty(m))).ToList<MemberInfo>();
      List<MemberInfo> serializableMembers = new List<MemberInfo>();
      foreach (MemberInfo memberInfo in list2)
      {
        if (this.SerializeCompilerGeneratedMembers || !memberInfo.IsDefined(typeof (CompilerGeneratedAttribute), true))
        {
          if (list1.Contains(memberInfo))
            serializableMembers.Add(memberInfo);
          else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>((ICustomAttributeProvider) memberInfo) != null)
            serializableMembers.Add(memberInfo);
          else if (contractAttribute != null && JsonTypeReflector.GetAttribute<DataMemberAttribute>((ICustomAttributeProvider) memberInfo) != null)
            serializableMembers.Add(memberInfo);
        }
      }
      return serializableMembers;
    }

    protected virtual JsonObjectContract CreateObjectContract(Type objectType)
    {
      JsonObjectContract contract = new JsonObjectContract(objectType);
      this.InitializeContract((JsonContract) contract);
      contract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType);
      contract.Properties.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateProperties(contract.UnderlyingType, contract.MemberSerialization));
      if (((IEnumerable<ConstructorInfo>) objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Any<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c.IsDefined(typeof (JsonConstructorAttribute), true))))
      {
        ConstructorInfo attributeConstructor = this.GetAttributeConstructor(objectType);
        if ((object) attributeConstructor != null)
        {
          contract.OverrideConstructor = attributeConstructor;
          contract.ConstructorParameters.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateConstructorParameters(attributeConstructor, contract.Properties));
        }
      }
      else if (contract.DefaultCreator == null || contract.DefaultCreatorNonPublic)
      {
        ConstructorInfo parametrizedConstructor = this.GetParametrizedConstructor(objectType);
        if ((object) parametrizedConstructor != null)
        {
          contract.ParametrizedConstructor = parametrizedConstructor;
          contract.ConstructorParameters.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateConstructorParameters(parametrizedConstructor, contract.Properties));
        }
      }
      return contract;
    }

    private ConstructorInfo GetAttributeConstructor(Type objectType)
    {
      IList<ConstructorInfo> list = (IList<ConstructorInfo>) ((IEnumerable<ConstructorInfo>) objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c.IsDefined(typeof (JsonConstructorAttribute), true))).ToList<ConstructorInfo>();
      if (list.Count > 1)
        throw new Exception("Multiple constructors with the JsonConstructorAttribute.");
      return list.Count == 1 ? list[0] : (ConstructorInfo) null;
    }

    private ConstructorInfo GetParametrizedConstructor(Type objectType)
    {
      IList<ConstructorInfo> constructors = (IList<ConstructorInfo>) objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
      return constructors.Count == 1 ? constructors[0] : (ConstructorInfo) null;
    }

    protected virtual IList<JsonProperty> CreateConstructorParameters(
      ConstructorInfo constructor,
      JsonPropertyCollection memberProperties)
    {
      ParameterInfo[] parameters = constructor.GetParameters();
      JsonPropertyCollection constructorParameters = new JsonPropertyCollection(constructor.DeclaringType);
      foreach (ParameterInfo parameterInfo in parameters)
      {
        JsonProperty matchingMemberProperty = memberProperties.GetClosestMatchProperty(parameterInfo.Name);
        if (matchingMemberProperty != null && (object) matchingMemberProperty.PropertyType != (object) parameterInfo.ParameterType)
          matchingMemberProperty = (JsonProperty) null;
        JsonProperty constructorParameter = this.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
        if (constructorParameter != null)
          constructorParameters.AddProperty(constructorParameter);
      }
      return (IList<JsonProperty>) constructorParameters;
    }

    protected virtual JsonProperty CreatePropertyFromConstructorParameter(
      JsonProperty matchingMemberProperty,
      ParameterInfo parameterInfo)
    {
      JsonProperty property = new JsonProperty();
      property.PropertyType = parameterInfo.ParameterType;
      this.SetPropertySettingsFromAttributes(property, (ICustomAttributeProvider) parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out bool _, out bool _);
      property.Readable = false;
      property.Writable = true;
      if (matchingMemberProperty != null)
      {
        property.PropertyName = property.PropertyName != parameterInfo.Name ? property.PropertyName : matchingMemberProperty.PropertyName;
        property.Converter = property.Converter ?? matchingMemberProperty.Converter;
        property.MemberConverter = property.MemberConverter ?? matchingMemberProperty.MemberConverter;
        property.DefaultValue = property.DefaultValue ?? matchingMemberProperty.DefaultValue;
        property.Required = property.Required != Required.Default ? property.Required : matchingMemberProperty.Required;
        JsonProperty jsonProperty1 = property;
        bool? isReference = property.IsReference;
        bool? nullable1 = isReference.HasValue ? new bool?(isReference.GetValueOrDefault()) : matchingMemberProperty.IsReference;
        jsonProperty1.IsReference = nullable1;
        JsonProperty jsonProperty2 = property;
        NullValueHandling? nullValueHandling = property.NullValueHandling;
        NullValueHandling? nullable2 = nullValueHandling.HasValue ? new NullValueHandling?(nullValueHandling.GetValueOrDefault()) : matchingMemberProperty.NullValueHandling;
        jsonProperty2.NullValueHandling = nullable2;
        JsonProperty jsonProperty3 = property;
        DefaultValueHandling? defaultValueHandling = property.DefaultValueHandling;
        DefaultValueHandling? nullable3 = defaultValueHandling.HasValue ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault()) : matchingMemberProperty.DefaultValueHandling;
        jsonProperty3.DefaultValueHandling = nullable3;
        JsonProperty jsonProperty4 = property;
        ReferenceLoopHandling? referenceLoopHandling = property.ReferenceLoopHandling;
        ReferenceLoopHandling? nullable4 = referenceLoopHandling.HasValue ? new ReferenceLoopHandling?(referenceLoopHandling.GetValueOrDefault()) : matchingMemberProperty.ReferenceLoopHandling;
        jsonProperty4.ReferenceLoopHandling = nullable4;
        JsonProperty jsonProperty5 = property;
        ObjectCreationHandling? creationHandling = property.ObjectCreationHandling;
        ObjectCreationHandling? nullable5 = creationHandling.HasValue ? new ObjectCreationHandling?(creationHandling.GetValueOrDefault()) : matchingMemberProperty.ObjectCreationHandling;
        jsonProperty5.ObjectCreationHandling = nullable5;
        JsonProperty jsonProperty6 = property;
        TypeNameHandling? typeNameHandling = property.TypeNameHandling;
        TypeNameHandling? nullable6 = typeNameHandling.HasValue ? new TypeNameHandling?(typeNameHandling.GetValueOrDefault()) : matchingMemberProperty.TypeNameHandling;
        jsonProperty6.TypeNameHandling = nullable6;
      }
      return property;
    }

    protected virtual JsonConverter ResolveContractConverter(Type objectType) => JsonTypeReflector.GetJsonConverter((ICustomAttributeProvider) objectType, objectType);

    private Func<object> GetDefaultCreator(Type createdType) => JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);

    private void InitializeContract(JsonContract contract)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(contract.UnderlyingType);
      if (containerAttribute != null)
      {
        contract.IsReference = containerAttribute._isReference;
      }
      else
      {
        DataContractAttribute contractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.UnderlyingType);
        if (contractAttribute != null && contractAttribute.IsReference)
          contract.IsReference = new bool?(true);
      }
      contract.Converter = this.ResolveContractConverter(contract.UnderlyingType);
      contract.InternalConverter = JsonSerializer.GetMatchingConverter(DefaultContractResolver.BuiltInConverters, contract.UnderlyingType);
      if (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType)
      {
        contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
        contract.DefaultCreatorNonPublic = !contract.CreatedType.IsValueType && (object) ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null;
      }
      this.ResolveCallbackMethods(contract, contract.UnderlyingType);
    }

    private void ResolveCallbackMethods(JsonContract contract, Type t)
    {
      if ((object) t.BaseType != null)
        this.ResolveCallbackMethods(contract, t.BaseType);
      MethodInfo onSerializing;
      MethodInfo onSerialized;
      MethodInfo onDeserializing;
      MethodInfo onDeserialized;
      MethodInfo onError;
      this.GetCallbackMethodsForType(t, out onSerializing, out onSerialized, out onDeserializing, out onDeserialized, out onError);
      if ((object) onSerializing != null)
        contract.OnSerializing = onSerializing;
      if ((object) onSerialized != null)
        contract.OnSerialized = onSerialized;
      if ((object) onDeserializing != null)
        contract.OnDeserializing = onDeserializing;
      if ((object) onDeserialized != null)
        contract.OnDeserialized = onDeserialized;
      if ((object) onError == null)
        return;
      contract.OnError = onError;
    }

    private void GetCallbackMethodsForType(
      Type type,
      out MethodInfo onSerializing,
      out MethodInfo onSerialized,
      out MethodInfo onDeserializing,
      out MethodInfo onDeserialized,
      out MethodInfo onError)
    {
      onSerializing = (MethodInfo) null;
      onSerialized = (MethodInfo) null;
      onDeserializing = (MethodInfo) null;
      onDeserialized = (MethodInfo) null;
      onError = (MethodInfo) null;
      foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!method.ContainsGenericParameters)
        {
          Type prevAttributeType = (Type) null;
          ParameterInfo[] parameters = method.GetParameters();
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnSerializingAttribute), onSerializing, ref prevAttributeType))
            onSerializing = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnSerializedAttribute), onSerialized, ref prevAttributeType))
            onSerialized = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnDeserializingAttribute), onDeserializing, ref prevAttributeType))
            onDeserializing = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnDeserializedAttribute), onDeserialized, ref prevAttributeType))
            onDeserialized = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnErrorAttribute), onError, ref prevAttributeType))
            onError = method;
        }
      }
    }

    protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
    {
      JsonDictionaryContract contract = new JsonDictionaryContract(objectType);
      this.InitializeContract((JsonContract) contract);
      contract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
      return contract;
    }

    protected virtual JsonArrayContract CreateArrayContract(Type objectType)
    {
      JsonArrayContract contract = new JsonArrayContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
    {
      JsonPrimitiveContract contract = new JsonPrimitiveContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonLinqContract CreateLinqContract(Type objectType)
    {
      JsonLinqContract contract = new JsonLinqContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonStringContract CreateStringContract(Type objectType)
    {
      JsonStringContract contract = new JsonStringContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonContract CreateContract(Type objectType)
    {
      Type type = ReflectionUtils.EnsureNotNullableType(objectType);
      if (JsonConvert.IsJsonPrimitiveType(type))
        return (JsonContract) this.CreatePrimitiveContract(type);
      if (JsonTypeReflector.GetJsonObjectAttribute(type) != null)
        return (JsonContract) this.CreateObjectContract(type);
      if (JsonTypeReflector.GetJsonArrayAttribute(type) != null)
        return (JsonContract) this.CreateArrayContract(type);
      if ((object) type == (object) typeof (JToken) || type.IsSubclassOf(typeof (JToken)))
        return (JsonContract) this.CreateLinqContract(type);
      if (CollectionUtils.IsDictionaryType(type))
        return (JsonContract) this.CreateDictionaryContract(type);
      if (typeof (IEnumerable).IsAssignableFrom(type))
        return (JsonContract) this.CreateArrayContract(type);
      return DefaultContractResolver.CanConvertToString(type) ? (JsonContract) this.CreateStringContract(type) : (JsonContract) this.CreateObjectContract(type);
    }

    internal static bool CanConvertToString(Type type)
    {
      TypeConverter converter = ConvertUtils.GetConverter(type);
      return converter != null && (object) converter.GetType() != (object) typeof (TypeConverter) && converter.CanConvertTo(typeof (string)) || (object) type == (object) typeof (Type) || type.IsSubclassOf(typeof (Type)) || (object) type == (object) typeof (Guid) || (object) type == (object) typeof (Uri) || (object) type == (object) typeof (TimeSpan);
    }

    private static bool IsValidCallback(
      MethodInfo method,
      ParameterInfo[] parameters,
      Type attributeType,
      MethodInfo currentCallback,
      ref Type prevAttributeType)
    {
      if (!method.IsDefined(attributeType, false))
        return false;
      if ((object) currentCallback != null)
        throw new Exception("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) method, (object) currentCallback, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) attributeType));
      if ((object) prevAttributeType != null)
        throw new Exception("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) prevAttributeType, (object) attributeType, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method));
      if (method.IsVirtual)
        throw new Exception("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) method, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) attributeType));
      if ((object) method.ReturnType != (object) typeof (void))
        throw new Exception("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method));
      if ((object) attributeType == (object) typeof (OnErrorAttribute))
      {
        if (parameters == null || parameters.Length != 2 || (object) parameters[0].ParameterType != (object) typeof (StreamingContext) || (object) parameters[1].ParameterType != (object) typeof (ErrorContext))
          throw new Exception("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method, (object) typeof (StreamingContext), (object) typeof (ErrorContext)));
      }
      else if (parameters == null || parameters.Length != 1 || (object) parameters[0].ParameterType != (object) typeof (StreamingContext))
        throw new Exception("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method, (object) typeof (StreamingContext)));
      prevAttributeType = attributeType;
      return true;
    }

    internal static string GetClrTypeFullName(Type type)
    {
      if (type.IsGenericTypeDefinition || !type.ContainsGenericParameters)
        return type.FullName;
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}.{1}", new object[2]
      {
        (object) type.Namespace,
        (object) type.Name
      });
    }

    protected virtual IList<JsonProperty> CreateProperties(
      Type type,
      MemberSerialization memberSerialization)
    {
      List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
      if (serializableMembers == null)
        throw new JsonSerializationException("Null collection of seralizable members returned.");
      JsonPropertyCollection source = new JsonPropertyCollection(type);
      foreach (MemberInfo member in serializableMembers)
      {
        JsonProperty property = this.CreateProperty(member, memberSerialization);
        if (property != null)
          source.AddProperty(property);
      }
      return (IList<JsonProperty>) source.OrderBy<JsonProperty, int>((Func<JsonProperty, int>) (p => p.Order ?? -1)).ToList<JsonProperty>();
    }

    protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member) => (IValueProvider) new ReflectionValueProvider(member);

    protected virtual JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty property = new JsonProperty();
      property.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
      property.ValueProvider = this.CreateMemberValueProvider(member);
      bool allowNonPublicAccess;
      bool hasExplicitAttribute;
      this.SetPropertySettingsFromAttributes(property, (ICustomAttributeProvider) member, member.Name, member.DeclaringType, memberSerialization, out allowNonPublicAccess, out hasExplicitAttribute);
      property.Readable = ReflectionUtils.CanReadMemberValue(member, allowNonPublicAccess);
      property.Writable = ReflectionUtils.CanSetMemberValue(member, allowNonPublicAccess, hasExplicitAttribute);
      property.ShouldSerialize = this.CreateShouldSerializeTest(member);
      this.SetIsSpecifiedActions(property, member, allowNonPublicAccess);
      return property;
    }

    private void SetPropertySettingsFromAttributes(
      JsonProperty property,
      ICustomAttributeProvider attributeProvider,
      string name,
      Type declaringType,
      MemberSerialization memberSerialization,
      out bool allowNonPublicAccess,
      out bool hasExplicitAttribute)
    {
      hasExplicitAttribute = false;
      DataMemberAttribute dataMemberAttribute = JsonTypeReflector.GetDataContractAttribute(declaringType) == null || (object) (attributeProvider as MemberInfo) == null ? (DataMemberAttribute) null : JsonTypeReflector.GetDataMemberAttribute((MemberInfo) attributeProvider);
      JsonPropertyAttribute attribute1 = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
      if (attribute1 != null)
        hasExplicitAttribute = true;
      bool flag = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null;
      string propertyName = attribute1 == null || attribute1.PropertyName == null ? (dataMemberAttribute == null || dataMemberAttribute.Name == null ? name : dataMemberAttribute.Name) : attribute1.PropertyName;
      property.PropertyName = this.ResolvePropertyName(propertyName);
      property.UnderlyingName = name;
      if (attribute1 != null)
      {
        property.Required = attribute1.Required;
        property.Order = attribute1._order;
      }
      else if (dataMemberAttribute != null)
      {
        property.Required = dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default;
        property.Order = dataMemberAttribute.Order != -1 ? new int?(dataMemberAttribute.Order) : new int?();
      }
      else
        property.Required = Required.Default;
      property.Ignored = flag || memberSerialization == MemberSerialization.OptIn && attribute1 == null && dataMemberAttribute == null;
      property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
      property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
      DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
      property.DefaultValue = attribute2?.Value;
      property.NullValueHandling = (NullValueHandling?) attribute1?._nullValueHandling;
      property.DefaultValueHandling = (DefaultValueHandling?) attribute1?._defaultValueHandling;
      property.ReferenceLoopHandling = (ReferenceLoopHandling?) attribute1?._referenceLoopHandling;
      property.ObjectCreationHandling = (ObjectCreationHandling?) attribute1?._objectCreationHandling;
      property.TypeNameHandling = (TypeNameHandling?) attribute1?._typeNameHandling;
      property.IsReference = (bool?) attribute1?._isReference;
      allowNonPublicAccess = false;
      if ((this.DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
        allowNonPublicAccess = true;
      if (attribute1 != null)
        allowNonPublicAccess = true;
      if (dataMemberAttribute == null)
        return;
      allowNonPublicAccess = true;
      hasExplicitAttribute = true;
    }

    private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
    {
      MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, new Type[0]);
      if ((object) method == null || (object) method.ReturnType != (object) typeof (bool))
        return (Predicate<object>) null;
      MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Predicate<object>) (o => (bool) shouldSerializeCall(o, new object[0]));
    }

    private void SetIsSpecifiedActions(
      JsonProperty property,
      MemberInfo member,
      bool allowNonPublicAccess)
    {
      MemberInfo memberInfo = (MemberInfo) member.DeclaringType.GetProperty(member.Name + "Specified");
      if ((object) memberInfo == null)
        memberInfo = (MemberInfo) member.DeclaringType.GetField(member.Name + "Specified");
      if ((object) memberInfo == null || (object) ReflectionUtils.GetMemberUnderlyingType(memberInfo) != (object) typeof (bool))
        return;
      Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
      property.GetIsSpecified = (Predicate<object>) (o => (bool) specifiedPropertyGet(o));
      if (!ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
        return;
      property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
    }

    protected internal virtual string ResolvePropertyName(string propertyName) => propertyName;
  }
}
