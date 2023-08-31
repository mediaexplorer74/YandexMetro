// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ReflectionUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
  internal static class ReflectionUtils
  {
    public static bool IsVirtual(this PropertyInfo propertyInfo)
    {
      ValidationUtils.ArgumentNotNull((object) propertyInfo, nameof (propertyInfo));
      MethodInfo getMethod = propertyInfo.GetGetMethod();
      if ((object) getMethod != null && getMethod.IsVirtual)
        return true;
      MethodInfo setMethod = propertyInfo.GetSetMethod();
      return (object) setMethod != null && setMethod.IsVirtual;
    }

    public static Type GetObjectType(object v) => v?.GetType();

    public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat) => ReflectionUtils.GetTypeName(t, assemblyFormat, (SerializationBinder) null);

    public static string GetTypeName(
      Type t,
      FormatterAssemblyStyle assemblyFormat,
      SerializationBinder binder)
    {
      string fullyQualifiedTypeName;
      if (binder != null)
      {
        string assemblyName;
        string typeName;
        binder.BindToName(t, out assemblyName, out typeName);
        fullyQualifiedTypeName = typeName + (assemblyName == null ? "" : ", " + assemblyName);
      }
      else
        fullyQualifiedTypeName = t.AssemblyQualifiedName;
      switch (assemblyFormat)
      {
        case FormatterAssemblyStyle.Simple:
          return ReflectionUtils.RemoveAssemblyDetails(fullyQualifiedTypeName);
        case FormatterAssemblyStyle.Full:
          return t.AssemblyQualifiedName;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < fullyQualifiedTypeName.Length; ++index)
      {
        char ch = fullyQualifiedTypeName[index];
        switch (ch)
        {
          case ',':
            if (!flag1)
            {
              flag1 = true;
              stringBuilder.Append(ch);
              break;
            }
            flag2 = true;
            break;
          case '[':
            flag1 = false;
            flag2 = false;
            stringBuilder.Append(ch);
            break;
          case ']':
            flag1 = false;
            flag2 = false;
            stringBuilder.Append(ch);
            break;
          default:
            if (!flag2)
            {
              stringBuilder.Append(ch);
              break;
            }
            break;
        }
      }
      return stringBuilder.ToString();
    }

    public static bool IsInstantiatableType(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, nameof (t));
      return !t.IsAbstract && !t.IsInterface && !t.IsArray && !t.IsGenericTypeDefinition && (object) t != (object) typeof (void) && ReflectionUtils.HasDefaultConstructor(t);
    }

    public static bool HasDefaultConstructor(Type t) => ReflectionUtils.HasDefaultConstructor(t, false);

    public static bool HasDefaultConstructor(Type t, bool nonPublic)
    {
      ValidationUtils.ArgumentNotNull((object) t, nameof (t));
      return t.IsValueType || (object) ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
    }

    public static ConstructorInfo GetDefaultConstructor(Type t) => ReflectionUtils.GetDefaultConstructor(t, false);

    public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
    {
      BindingFlags bindingFlags = BindingFlags.Public;
      if (nonPublic)
        bindingFlags |= BindingFlags.NonPublic;
      return t.GetConstructor(bindingFlags | BindingFlags.Instance, (Binder) null, new Type[0], (ParameterModifier[]) null);
    }

    public static bool IsNullable(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, nameof (t));
      return !t.IsValueType || ReflectionUtils.IsNullableType(t);
    }

    public static bool IsNullableType(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, nameof (t));
      return t.IsGenericType && (object) t.GetGenericTypeDefinition() == (object) typeof (Nullable<>);
    }

    public static Type EnsureNotNullableType(Type t) => !ReflectionUtils.IsNullableType(t) ? t : Nullable.GetUnderlyingType(t);

    public static bool IsUnitializedValue(object value)
    {
      if (value == null)
        return true;
      object unitializedValue = ReflectionUtils.CreateUnitializedValue(value.GetType());
      return value.Equals(unitializedValue);
    }

    public static object CreateUnitializedValue(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      if (type.IsGenericTypeDefinition)
        throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type), nameof (type));
      if (type.IsClass || type.IsInterface || (object) type == (object) typeof (void))
        return (object) null;
      return type.IsValueType ? Activator.CreateInstance(type) : throw new ArgumentException("Type {0} cannot be instantiated.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type), nameof (type));
    }

    public static bool IsPropertyIndexed(PropertyInfo property)
    {
      ValidationUtils.ArgumentNotNull((object) property, nameof (property));
      return !CollectionUtils.IsNullOrEmpty<ParameterInfo>((ICollection<ParameterInfo>) property.GetIndexParameters());
    }

    public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition) => ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out Type _);

    public static bool ImplementsGenericDefinition(
      Type type,
      Type genericInterfaceDefinition,
      out Type implementingType)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      ValidationUtils.ArgumentNotNull((object) genericInterfaceDefinition, nameof (genericInterfaceDefinition));
      if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
        throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) genericInterfaceDefinition));
      if (type.IsInterface && type.IsGenericType)
      {
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if ((object) genericInterfaceDefinition == (object) genericTypeDefinition)
        {
          implementingType = type;
          return true;
        }
      }
      foreach (Type type1 in type.GetInterfaces())
      {
        if (type1.IsGenericType)
        {
          Type genericTypeDefinition = type1.GetGenericTypeDefinition();
          if ((object) genericInterfaceDefinition == (object) genericTypeDefinition)
          {
            implementingType = type1;
            return true;
          }
        }
      }
      implementingType = (Type) null;
      return false;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
    {
      for (Type type1 = type; (object) type1 != null; type1 = type1.BaseType)
      {
        if (string.Equals(type1.FullName, fullTypeName, StringComparison.Ordinal))
        {
          match = type1;
          return true;
        }
      }
      foreach (MemberInfo memberInfo in type.GetInterfaces())
      {
        if (string.Equals(memberInfo.Name, fullTypeName, StringComparison.Ordinal))
        {
          match = type;
          return true;
        }
      }
      match = (Type) null;
      return false;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName) => type.AssignableToTypeName(fullTypeName, out Type _);

    public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition) => ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out Type _);

    public static bool InheritsGenericDefinition(
      Type type,
      Type genericClassDefinition,
      out Type implementingType)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      ValidationUtils.ArgumentNotNull((object) genericClassDefinition, nameof (genericClassDefinition));
      if (!genericClassDefinition.IsClass || !genericClassDefinition.IsGenericTypeDefinition)
        throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) genericClassDefinition));
      return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
    }

    private static bool InheritsGenericDefinitionInternal(
      Type currentType,
      Type genericClassDefinition,
      out Type implementingType)
    {
      if (currentType.IsGenericType)
      {
        Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
        if ((object) genericClassDefinition == (object) genericTypeDefinition)
        {
          implementingType = currentType;
          return true;
        }
      }
      if ((object) currentType.BaseType != null)
        return ReflectionUtils.InheritsGenericDefinitionInternal(currentType.BaseType, genericClassDefinition, out implementingType);
      implementingType = (Type) null;
      return false;
    }

    public static Type GetCollectionItemType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      if (type.IsArray)
        return type.GetElementType();
      Type implementingType;
      if (ReflectionUtils.ImplementsGenericDefinition(type, typeof (IEnumerable<>), out implementingType))
        return !implementingType.IsGenericTypeDefinition ? implementingType.GetGenericArguments()[0] : throw new Exception("Type {0} is not a collection.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      if (typeof (IEnumerable).IsAssignableFrom(type))
        return (Type) null;
      throw new Exception("Type {0} is not a collection.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
    }

    public static void GetDictionaryKeyValueTypes(
      Type dictionaryType,
      out Type keyType,
      out Type valueType)
    {
      ValidationUtils.ArgumentNotNull((object) dictionaryType, "type");
      Type implementingType;
      if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof (IDictionary<,>), out implementingType))
      {
        Type[] typeArray = !implementingType.IsGenericTypeDefinition ? implementingType.GetGenericArguments() : throw new Exception("Type {0} is not a dictionary.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) dictionaryType));
        keyType = typeArray[0];
        valueType = typeArray[1];
      }
      else if (typeof (IDictionary).IsAssignableFrom(dictionaryType))
      {
        keyType = (Type) null;
        valueType = (Type) null;
      }
      else
        throw new Exception("Type {0} is not a dictionary.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) dictionaryType));
    }

    public static Type GetDictionaryValueType(Type dictionaryType)
    {
      Type valueType;
      ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out Type _, out valueType);
      return valueType;
    }

    public static Type GetDictionaryKeyType(Type dictionaryType)
    {
      Type keyType;
      ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out keyType, out Type _);
      return keyType;
    }

    public static bool ItemsUnitializedValue<T>(IList<T> list)
    {
      ValidationUtils.ArgumentNotNull((object) list, nameof (list));
      Type collectionItemType = ReflectionUtils.GetCollectionItemType(list.GetType());
      if (collectionItemType.IsValueType)
      {
        object unitializedValue = ReflectionUtils.CreateUnitializedValue(collectionItemType);
        for (int index = 0; index < list.Count; ++index)
        {
          if (!list[index].Equals(unitializedValue))
            return false;
        }
      }
      else if (collectionItemType.IsClass)
      {
        for (int index = 0; index < list.Count; ++index)
        {
          if ((object) list[index] != null)
            return false;
        }
      }
      else
        throw new Exception("Type {0} is neither a ValueType or a Class.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) collectionItemType));
      return true;
    }

    public static Type GetMemberUnderlyingType(MemberInfo member)
    {
      ValidationUtils.ArgumentNotNull((object) member, nameof (member));
      switch (member.MemberType)
      {
        case MemberTypes.Event:
          return ((EventInfo) member).EventHandlerType;
        case MemberTypes.Field:
          return ((FieldInfo) member).FieldType;
        case MemberTypes.Property:
          return ((PropertyInfo) member).PropertyType;
        default:
          throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", nameof (member));
      }
    }

    public static bool IsIndexedProperty(MemberInfo member)
    {
      ValidationUtils.ArgumentNotNull((object) member, nameof (member));
      PropertyInfo property = member as PropertyInfo;
      return (object) property != null && ReflectionUtils.IsIndexedProperty(property);
    }

    public static bool IsIndexedProperty(PropertyInfo property)
    {
      ValidationUtils.ArgumentNotNull((object) property, nameof (property));
      return property.GetIndexParameters().Length > 0;
    }

    public static object GetMemberValue(MemberInfo member, object target)
    {
      ValidationUtils.ArgumentNotNull((object) member, nameof (member));
      ValidationUtils.ArgumentNotNull(target, nameof (target));
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          return ((FieldInfo) member).GetValue(target);
        case MemberTypes.Property:
          try
          {
            return ((PropertyInfo) member).GetValue(target, (object[]) null);
          }
          catch (TargetParameterCountException ex)
          {
            throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) member.Name), (Exception) ex);
          }
        default:
          throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) CultureInfo.InvariantCulture, (object) member.Name), nameof (member));
      }
    }

    public static void SetMemberValue(MemberInfo member, object target, object value)
    {
      ValidationUtils.ArgumentNotNull((object) member, nameof (member));
      ValidationUtils.ArgumentNotNull(target, nameof (target));
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          ((FieldInfo) member).SetValue(target, value);
          break;
        case MemberTypes.Property:
          ((PropertyInfo) member).SetValue(target, value, (object[]) null);
          break;
        default:
          throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) member.Name), nameof (member));
      }
    }

    public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo fieldInfo = (FieldInfo) member;
          return nonPublic || fieldInfo.IsPublic;
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) member;
          if (!propertyInfo.CanRead)
            return false;
          return nonPublic || (object) propertyInfo.GetGetMethod(nonPublic) != null;
        default:
          return false;
      }
    }

    public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo fieldInfo = (FieldInfo) member;
          return (!fieldInfo.IsInitOnly || canSetReadOnly) && (nonPublic || fieldInfo.IsPublic);
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) member;
          if (!propertyInfo.CanWrite)
            return false;
          return nonPublic || (object) propertyInfo.GetSetMethod(nonPublic) != null;
        default:
          return false;
      }
    }

    public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr) => ReflectionUtils.GetFieldsAndProperties(typeof (T), bindingAttr);

    public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
    {
      List<MemberInfo> memberInfoList = new List<MemberInfo>();
      memberInfoList.AddRange((IEnumerable) ReflectionUtils.GetFields(type, bindingAttr));
      memberInfoList.AddRange((IEnumerable) ReflectionUtils.GetProperties(type, bindingAttr));
      List<MemberInfo> fieldsAndProperties = new List<MemberInfo>(memberInfoList.Count);
      foreach (var data in memberInfoList.GroupBy<MemberInfo, string>((Func<MemberInfo, string>) (m => m.Name)).Select(g => new
      {
        Count = g.Count<MemberInfo>(),
        Members = g.Cast<MemberInfo>()
      }))
      {
        if (data.Count == 1)
        {
          fieldsAndProperties.Add(data.Members.First<MemberInfo>());
        }
        else
        {
          IEnumerable<MemberInfo> collection = data.Members.Where<MemberInfo>((Func<MemberInfo, bool>) (m => !ReflectionUtils.IsOverridenGenericMember(m, bindingAttr) || m.Name == "Item"));
          fieldsAndProperties.AddRange(collection);
        }
      }
      return fieldsAndProperties;
    }

    private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
    {
      Type type = memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property ? memberInfo.DeclaringType : throw new ArgumentException("Member must be a field or property.");
      if (!type.IsGenericType)
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      if ((object) genericTypeDefinition == null)
        return false;
      MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.Name, bindingAttr);
      return member.Length != 0 && ReflectionUtils.GetMemberUnderlyingType(member[0]).IsGenericParameter;
    }

    public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute => ReflectionUtils.GetAttribute<T>(attributeProvider, true);

    public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute => CollectionUtils.GetSingleItem<T>((IList<T>) ReflectionUtils.GetAttributes<T>(attributeProvider, inherit), true);

    public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
    {
      ValidationUtils.ArgumentNotNull((object) attributeProvider, nameof (attributeProvider));
      if ((object) (attributeProvider as Type) != null)
        return (T[]) ((Type) attributeProvider).GetCustomAttributes(typeof (T), inherit);
      if ((object) (attributeProvider as Assembly) != null)
        return (T[]) Attribute.GetCustomAttributes((Assembly) attributeProvider, typeof (T), inherit);
      if ((object) (attributeProvider as MemberInfo) != null)
        return (T[]) Attribute.GetCustomAttributes((MemberInfo) attributeProvider, typeof (T), inherit);
      if ((object) (attributeProvider as Module) != null)
        return (T[]) Attribute.GetCustomAttributes((Module) attributeProvider, typeof (T), inherit);
      return attributeProvider is ParameterInfo ? (T[]) Attribute.GetCustomAttributes((ParameterInfo) attributeProvider, typeof (T), inherit) : (T[]) attributeProvider.GetCustomAttributes(typeof (T), inherit);
    }

    public static string GetNameAndAssessmblyName(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, nameof (t));
      return t.FullName + ", " + t.Assembly.GetName().Name;
    }

    public static Type MakeGenericType(Type genericTypeDefinition, params Type[] innerTypes)
    {
      ValidationUtils.ArgumentNotNull((object) genericTypeDefinition, nameof (genericTypeDefinition));
      ValidationUtils.ArgumentNotNullOrEmpty<Type>((ICollection<Type>) innerTypes, nameof (innerTypes));
      ValidationUtils.ArgumentConditionTrue((genericTypeDefinition.IsGenericTypeDefinition ? 1 : 0) != 0, nameof (genericTypeDefinition), "Type {0} is not a generic type definition.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) genericTypeDefinition));
      return genericTypeDefinition.MakeGenericType(innerTypes);
    }

    public static object CreateGeneric(
      Type genericTypeDefinition,
      Type innerType,
      params object[] args)
    {
      return ReflectionUtils.CreateGeneric(genericTypeDefinition, (IList<Type>) new Type[1]
      {
        innerType
      }, args);
    }

    public static object CreateGeneric(
      Type genericTypeDefinition,
      IList<Type> innerTypes,
      params object[] args)
    {
      return ReflectionUtils.CreateGeneric(genericTypeDefinition, innerTypes, (Func<Type, IList<object>, object>) ((t, a) => ReflectionUtils.CreateInstance(t, a.ToArray<object>())), args);
    }

    public static object CreateGeneric(
      Type genericTypeDefinition,
      IList<Type> innerTypes,
      Func<Type, IList<object>, object> instanceCreator,
      params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) genericTypeDefinition, nameof (genericTypeDefinition));
      ValidationUtils.ArgumentNotNullOrEmpty<Type>((ICollection<Type>) innerTypes, nameof (innerTypes));
      ValidationUtils.ArgumentNotNull((object) instanceCreator, "createInstance");
      Type type = ReflectionUtils.MakeGenericType(genericTypeDefinition, innerTypes.ToArray<Type>());
      return instanceCreator(type, (IList<object>) args);
    }

    public static bool IsCompatibleValue(object value, Type type)
    {
      if (value == null)
        return ReflectionUtils.IsNullable(type);
      return type.IsAssignableFrom(value.GetType());
    }

    public static object CreateInstance(Type type, params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      return Activator.CreateInstance(type, args);
    }

    public static void SplitFullyQualifiedTypeName(
      string fullyQualifiedTypeName,
      out string typeName,
      out string assemblyName)
    {
      int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
      if (assemblyDelimiterIndex.HasValue)
      {
        typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
        assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
      }
      else
      {
        typeName = fullyQualifiedTypeName;
        assemblyName = (string) null;
      }
    }

    private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
    {
      int num = 0;
      for (int index = 0; index < fullyQualifiedTypeName.Length; ++index)
      {
        switch (fullyQualifiedTypeName[index])
        {
          case ',':
            if (num == 0)
              return new int?(index);
            break;
          case '[':
            ++num;
            break;
          case ']':
            --num;
            break;
        }
      }
      return new int?();
    }

    public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
    {
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
      if (memberInfo.MemberType != MemberTypes.Property)
        return ((IEnumerable<MemberInfo>) targetType.GetMember(memberInfo.Name, memberInfo.MemberType, bindingAttr)).SingleOrDefault<MemberInfo>();
      PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
      Type[] array = ((IEnumerable<ParameterInfo>) propertyInfo.GetIndexParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).ToArray<Type>();
      return (MemberInfo) targetType.GetProperty(propertyInfo.Name, bindingAttr, (Binder) null, propertyInfo.PropertyType, array, (ParameterModifier[]) null);
    }

    public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
    {
      ValidationUtils.ArgumentNotNull((object) targetType, nameof (targetType));
      List<MemberInfo> memberInfoList = new List<MemberInfo>((IEnumerable<MemberInfo>) targetType.GetFields(bindingAttr));
      ReflectionUtils.GetChildPrivateFields((IList<MemberInfo>) memberInfoList, targetType, bindingAttr);
      return memberInfoList.Cast<FieldInfo>();
    }

    private static void GetChildPrivateFields(
      IList<MemberInfo> initialFields,
      Type targetType,
      BindingFlags bindingAttr)
    {
      if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
        return;
      BindingFlags bindingAttr1 = bindingAttr.RemoveFlag(BindingFlags.Public);
      while ((object) (targetType = targetType.BaseType) != null)
      {
        IEnumerable<MemberInfo> collection = ((IEnumerable<FieldInfo>) targetType.GetFields(bindingAttr1)).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsPrivate)).Cast<MemberInfo>();
        initialFields.AddRange<MemberInfo>(collection);
      }
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
    {
      ValidationUtils.ArgumentNotNull((object) targetType, nameof (targetType));
      List<PropertyInfo> initialProperties = new List<PropertyInfo>((IEnumerable<PropertyInfo>) targetType.GetProperties(bindingAttr));
      ReflectionUtils.GetChildPrivateProperties((IList<PropertyInfo>) initialProperties, targetType, bindingAttr);
      for (int index = 0; index < initialProperties.Count; ++index)
      {
        PropertyInfo propertyInfo = initialProperties[index];
        if ((object) propertyInfo.DeclaringType != (object) targetType)
        {
          PropertyInfo memberInfoFromType = (PropertyInfo) ReflectionUtils.GetMemberInfoFromType(propertyInfo.DeclaringType, (MemberInfo) propertyInfo);
          initialProperties[index] = memberInfoFromType;
        }
      }
      return (IEnumerable<PropertyInfo>) initialProperties;
    }

    public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag) => (bindingAttr & flag) != flag ? bindingAttr : bindingAttr ^ flag;

    private static void GetChildPrivateProperties(
      IList<PropertyInfo> initialProperties,
      Type targetType,
      BindingFlags bindingAttr)
    {
      if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
        return;
      BindingFlags bindingAttr1 = bindingAttr.RemoveFlag(BindingFlags.Public);
      while ((object) (targetType = targetType.BaseType) != null)
      {
        foreach (PropertyInfo property in targetType.GetProperties(bindingAttr1))
        {
          PropertyInfo nonPublicProperty = property;
          int index = initialProperties.IndexOf<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == nonPublicProperty.Name));
          if (index == -1)
            initialProperties.Add(nonPublicProperty);
          else
            initialProperties[index] = nonPublicProperty;
        }
      }
    }
  }
}
