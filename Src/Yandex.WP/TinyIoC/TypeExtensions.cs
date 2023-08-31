// Decompiled with JetBrains decompiler
// Type: TinyIoC.TypeExtensions
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyIoC
{
  public static class TypeExtensions
  {
    private static SafeDictionary<TypeExtensions.GenericMethodCacheKey, MethodInfo> _genericMethodCache = new SafeDictionary<TypeExtensions.GenericMethodCacheKey, MethodInfo>();

    public static MethodInfo GetGenericMethod(
      this Type sourceType,
      BindingFlags bindingFlags,
      string methodName,
      Type[] genericTypes,
      Type[] parameterTypes)
    {
      TypeExtensions.GenericMethodCacheKey key = new TypeExtensions.GenericMethodCacheKey(sourceType, methodName, genericTypes, parameterTypes);
      MethodInfo method;
      if (!TypeExtensions._genericMethodCache.TryGetValue(key, out method))
      {
        method = TypeExtensions.GetMethod(sourceType, bindingFlags, methodName, genericTypes, parameterTypes);
        TypeExtensions._genericMethodCache[key] = method;
      }
      return method;
    }

    private static MethodInfo GetMethod(
      Type sourceType,
      BindingFlags bindingFlags,
      string methodName,
      Type[] genericTypes,
      Type[] parameterTypes)
    {
      List<MethodInfo> list = ((IEnumerable<MethodInfo>) sourceType.GetMethods(bindingFlags)).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => string.Equals(methodName, mi.Name, StringComparison.Ordinal))).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.ContainsGenericParameters)).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.GetGenericArguments().Length == genericTypes.Length)).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.GetParameters().Length == parameterTypes.Length)).Select<MethodInfo, MethodInfo>((Func<MethodInfo, MethodInfo>) (mi => mi.MakeGenericMethod(genericTypes))).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => ((IEnumerable<ParameterInfo>) mi.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (pi => pi.ParameterType)).SequenceEqual<Type>((IEnumerable<Type>) parameterTypes))).ToList<MethodInfo>();
      return list.Count <= 1 ? list.FirstOrDefault<MethodInfo>() : throw new AmbiguousMatchException();
    }

    public static bool IsInterface(this Type type) => type.IsInterface;

    public static bool IsAbstract(this Type type) => type.IsAbstract;

    public static bool IsClass(this Type type) => type.IsClass;

    public static bool IsGenericTypeDefinition(this Type type) => type.IsGenericTypeDefinition;

    public static bool IsGenericType(this Type type) => type.IsGenericType;

    public static bool IsPrimitive(this Type type) => type.IsPrimitive;

    public static bool IsValueType(this Type type) => type.IsValueType;

    public static System.Reflection.Assembly Assembly(this Type type) => type.Assembly;

    public static Type BaseType(this Type type) => type.BaseType;

    private sealed class GenericMethodCacheKey
    {
      private readonly Type _sourceType;
      private readonly string _methodName;
      private readonly Type[] _genericTypes;
      private readonly Type[] _parameterTypes;
      private readonly int _hashCode;

      public GenericMethodCacheKey(
        Type sourceType,
        string methodName,
        Type[] genericTypes,
        Type[] parameterTypes)
      {
        this._sourceType = sourceType;
        this._methodName = methodName;
        this._genericTypes = genericTypes;
        this._parameterTypes = parameterTypes;
        this._hashCode = this.GenerateHashCode();
      }

      public override bool Equals(object obj)
      {
        if (!(obj is TypeExtensions.GenericMethodCacheKey genericMethodCacheKey) || this._sourceType != genericMethodCacheKey._sourceType || !string.Equals(this._methodName, genericMethodCacheKey._methodName, StringComparison.Ordinal) || this._genericTypes.Length != genericMethodCacheKey._genericTypes.Length || this._parameterTypes.Length != genericMethodCacheKey._parameterTypes.Length)
          return false;
        for (int index = 0; index < this._genericTypes.Length; ++index)
        {
          if (this._genericTypes[index] != genericMethodCacheKey._genericTypes[index])
            return false;
        }
        for (int index = 0; index < this._parameterTypes.Length; ++index)
        {
          if (this._parameterTypes[index] != genericMethodCacheKey._parameterTypes[index])
            return false;
        }
        return true;
      }

      public override int GetHashCode() => this._hashCode;

      private int GenerateHashCode()
      {
        int hashCode = this._sourceType.GetHashCode() * 397 ^ this._methodName.GetHashCode();
        for (int index = 0; index < this._genericTypes.Length; ++index)
          hashCode = hashCode * 397 ^ this._genericTypes[index].GetHashCode();
        for (int index = 0; index < this._parameterTypes.Length; ++index)
          hashCode = hashCode * 397 ^ this._parameterTypes[index].GetHashCode();
        return hashCode;
      }
    }
  }
}
