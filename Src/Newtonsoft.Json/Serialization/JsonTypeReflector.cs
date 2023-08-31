// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonTypeReflector
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  internal static class JsonTypeReflector
  {
    public const string IdPropertyName = "$id";
    public const string RefPropertyName = "$ref";
    public const string TypePropertyName = "$type";
    public const string ValuePropertyName = "$value";
    public const string ArrayValuesPropertyName = "$values";
    public const string ShouldSerializePrefix = "ShouldSerialize";
    public const string SpecifiedPostfix = "Specified";
    private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> JsonConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetJsonConverterTypeFromAttribute));
    private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> TypeConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetTypeConverterTypeFromAttribute));
    private static bool? _dynamicCodeGeneration;

    private static Type GetTypeConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider)
    {
      TypeConverterAttribute attribute = JsonTypeReflector.GetAttribute<TypeConverterAttribute>(attributeProvider);
      return attribute == null ? (Type) null : Type.GetType(attribute.ConverterTypeName);
    }

    private static Type GetTypeConverterType(ICustomAttributeProvider attributeProvider) => JsonTypeReflector.TypeConverterTypeCache.Get(attributeProvider);

    public static JsonContainerAttribute GetJsonContainerAttribute(Type type) => CachedAttributeGetter<JsonContainerAttribute>.GetAttribute((ICustomAttributeProvider) type);

    public static JsonObjectAttribute GetJsonObjectAttribute(Type type) => JsonTypeReflector.GetJsonContainerAttribute(type) as JsonObjectAttribute;

    public static JsonArrayAttribute GetJsonArrayAttribute(Type type) => JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;

    public static DataContractAttribute GetDataContractAttribute(Type type)
    {
      DataContractAttribute contractAttribute = (DataContractAttribute) null;
      for (Type type1 = type; contractAttribute == null && (object) type1 != null; type1 = type1.BaseType)
        contractAttribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute((ICustomAttributeProvider) type1);
      return contractAttribute;
    }

    public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
    {
      if (memberInfo.MemberType == MemberTypes.Field)
        return CachedAttributeGetter<DataMemberAttribute>.GetAttribute((ICustomAttributeProvider) memberInfo);
      PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
      DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute((ICustomAttributeProvider) propertyInfo);
      if (attribute == null && propertyInfo.IsVirtual())
      {
        for (Type targetType = propertyInfo.DeclaringType; attribute == null && (object) targetType != null; targetType = targetType.BaseType)
        {
          PropertyInfo memberInfoFromType = (PropertyInfo) ReflectionUtils.GetMemberInfoFromType(targetType, (MemberInfo) propertyInfo);
          if ((object) memberInfoFromType != null && memberInfoFromType.IsVirtual())
            attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute((ICustomAttributeProvider) memberInfoFromType);
        }
      }
      return attribute;
    }

    public static MemberSerialization GetObjectMemberSerialization(Type objectType)
    {
      JsonObjectAttribute jsonObjectAttribute = JsonTypeReflector.GetJsonObjectAttribute(objectType);
      if (jsonObjectAttribute != null)
        return jsonObjectAttribute.MemberSerialization;
      return JsonTypeReflector.GetDataContractAttribute(objectType) != null ? MemberSerialization.OptIn : MemberSerialization.OptOut;
    }

    private static Type GetJsonConverterType(ICustomAttributeProvider attributeProvider) => JsonTypeReflector.JsonConverterTypeCache.Get(attributeProvider);

    private static Type GetJsonConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider) => JsonTypeReflector.GetAttribute<JsonConverterAttribute>(attributeProvider)?.ConverterType;

    public static JsonConverter GetJsonConverter(
      ICustomAttributeProvider attributeProvider,
      Type targetConvertedType)
    {
      Type jsonConverterType = JsonTypeReflector.GetJsonConverterType(attributeProvider);
      if ((object) jsonConverterType == null)
        return (JsonConverter) null;
      JsonConverter converterInstance = JsonConverterAttribute.CreateJsonConverterInstance(jsonConverterType);
      return converterInstance.CanConvert(targetConvertedType) ? converterInstance : throw new JsonSerializationException("JsonConverter {0} on {1} is not compatible with member type {2}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) converterInstance.GetType().Name, (object) attributeProvider, (object) targetConvertedType.Name));
    }

    public static TypeConverter GetTypeConverter(Type type)
    {
      Type typeConverterType = JsonTypeReflector.GetTypeConverterType((ICustomAttributeProvider) type);
      return (object) typeConverterType != null ? (TypeConverter) ReflectionUtils.CreateInstance(typeConverterType) : (TypeConverter) null;
    }

    private static T GetAttribute<T>(Type type) where T : Attribute
    {
      T attribute1 = ReflectionUtils.GetAttribute<T>((ICustomAttributeProvider) type, true);
      if ((object) attribute1 != null)
        return attribute1;
      foreach (ICustomAttributeProvider attributeProvider in type.GetInterfaces())
      {
        T attribute2 = ReflectionUtils.GetAttribute<T>(attributeProvider, true);
        if ((object) attribute2 != null)
          return attribute2;
      }
      return default (T);
    }

    private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
    {
      T attribute1 = ReflectionUtils.GetAttribute<T>((ICustomAttributeProvider) memberInfo, true);
      if ((object) attribute1 != null)
        return attribute1;
      foreach (Type targetType in memberInfo.DeclaringType.GetInterfaces())
      {
        MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
        if ((object) memberInfoFromType != null)
        {
          T attribute2 = ReflectionUtils.GetAttribute<T>((ICustomAttributeProvider) memberInfoFromType, true);
          if ((object) attribute2 != null)
            return attribute2;
        }
      }
      return default (T);
    }

    public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
    {
      Type type = attributeProvider as Type;
      if ((object) type != null)
        return JsonTypeReflector.GetAttribute<T>(type);
      MemberInfo memberInfo = attributeProvider as MemberInfo;
      return (object) memberInfo != null ? JsonTypeReflector.GetAttribute<T>(memberInfo) : ReflectionUtils.GetAttribute<T>(attributeProvider, true);
    }

    public static bool DynamicCodeGeneration
    {
      get
      {
        if (!JsonTypeReflector._dynamicCodeGeneration.HasValue)
          JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
        return JsonTypeReflector._dynamicCodeGeneration.Value;
      }
    }

    public static ReflectionDelegateFactory ReflectionDelegateFactory => LateBoundReflectionDelegateFactory.Instance;
  }
}
