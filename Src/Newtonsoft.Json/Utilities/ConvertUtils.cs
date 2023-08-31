// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ConvertUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class ConvertUtils
  {
    private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));

    private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
    {
      MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[1]
      {
        t.InitialType
      });
      if ((object) method == null)
        method = t.TargetType.GetMethod("op_Explicit", new Type[1]
        {
          t.InitialType
        });
      if ((object) method == null)
        return (Func<object, object>) null;
      MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Func<object, object>) (o => call((object) null, new object[1]
      {
        o
      }));
    }

    public static bool CanConvertType(
      Type initialType,
      Type targetType,
      bool allowTypeNameToString)
    {
      ValidationUtils.ArgumentNotNull((object) initialType, nameof (initialType));
      ValidationUtils.ArgumentNotNull((object) targetType, nameof (targetType));
      if (ReflectionUtils.IsNullableType(targetType))
        targetType = Nullable.GetUnderlyingType(targetType);
      if ((object) targetType == (object) initialType || typeof (IConvertible).IsAssignableFrom(initialType) && typeof (IConvertible).IsAssignableFrom(targetType) || (object) initialType == (object) typeof (DateTime) && (object) targetType == (object) typeof (DateTimeOffset) || (object) initialType == (object) typeof (Guid) && ((object) targetType == (object) typeof (Guid) || (object) targetType == (object) typeof (string)) || (object) initialType == (object) typeof (Type) && (object) targetType == (object) typeof (string))
        return true;
      TypeConverter converter1 = ConvertUtils.GetConverter(initialType);
      if (converter1 != null && !ConvertUtils.IsComponentConverter(converter1) && converter1.CanConvertTo(targetType) && (allowTypeNameToString || (object) converter1.GetType() != (object) typeof (TypeConverter)))
        return true;
      TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
      return converter2 != null && !ConvertUtils.IsComponentConverter(converter2) && converter2.CanConvertFrom(initialType) || (object) initialType == (object) typeof (DBNull) && ReflectionUtils.IsNullable(targetType);
    }

    private static bool IsComponentConverter(TypeConverter converter) => false;

    public static T Convert<T>(object initialValue) => ConvertUtils.Convert<T>(initialValue, CultureInfo.CurrentCulture);

    public static T Convert<T>(object initialValue, CultureInfo culture) => (T) ConvertUtils.Convert(initialValue, culture, typeof (T));

    public static object Convert(object initialValue, CultureInfo culture, Type targetType)
    {
      if (initialValue == null)
        throw new ArgumentNullException(nameof (initialValue));
      if (ReflectionUtils.IsNullableType(targetType))
        targetType = Nullable.GetUnderlyingType(targetType);
      Type type = initialValue.GetType();
      if ((object) targetType == (object) type)
        return initialValue;
      if (initialValue is string && typeof (Type).IsAssignableFrom(targetType))
        return (object) Type.GetType((string) initialValue, true);
      if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
        throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) targetType), nameof (targetType));
      switch (initialValue)
      {
        case IConvertible _ when typeof (IConvertible).IsAssignableFrom(targetType):
          if (targetType.IsEnum)
          {
            if (initialValue is string)
              return Enum.Parse(targetType, initialValue.ToString(), true);
            if (ConvertUtils.IsInteger(initialValue))
              return Enum.ToObject(targetType, initialValue);
          }
          return System.Convert.ChangeType(initialValue, targetType, (IFormatProvider) culture);
        case DateTime _ when (object) targetType == (object) typeof (DateTimeOffset):
          return (object) new DateTimeOffset((DateTime) initialValue);
        case string _:
          if ((object) targetType == (object) typeof (Guid))
            return (object) new Guid((string) initialValue);
          if ((object) targetType == (object) typeof (Uri))
            return (object) new Uri((string) initialValue);
          if ((object) targetType == (object) typeof (TimeSpan))
            return (object) TimeSpan.Parse((string) initialValue);
          break;
      }
      TypeConverter converter1 = ConvertUtils.GetConverter(type);
      if (converter1 != null && converter1.CanConvertTo(targetType))
        return converter1.ConvertTo(initialValue, targetType);
      TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
      if (converter2 != null && converter2.CanConvertFrom(type))
        return converter2.ConvertFrom(initialValue);
      if (initialValue == DBNull.Value)
        return ReflectionUtils.IsNullable(targetType) ? ConvertUtils.EnsureTypeAssignable((object) null, type, targetType) : throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type, (object) targetType));
      throw new Exception("Can not convert from {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type, (object) targetType));
    }

    public static bool TryConvert<T>(object initialValue, out T convertedValue) => ConvertUtils.TryConvert<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);

    public static bool TryConvert<T>(
      object initialValue,
      CultureInfo culture,
      out T convertedValue)
    {
      return MiscellaneousUtils.TryAction<T>((Creator<T>) (() =>
      {
        object convertedValue1;
        ConvertUtils.TryConvert(initialValue, CultureInfo.CurrentCulture, typeof (T), out convertedValue1);
        return (T) convertedValue1;
      }), out convertedValue);
    }

    public static bool TryConvert(
      object initialValue,
      CultureInfo culture,
      Type targetType,
      out object convertedValue)
    {
      return MiscellaneousUtils.TryAction<object>((Creator<object>) (() => ConvertUtils.Convert(initialValue, culture, targetType)), out convertedValue);
    }

    public static T ConvertOrCast<T>(object initialValue) => ConvertUtils.ConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture);

    public static T ConvertOrCast<T>(object initialValue, CultureInfo culture) => (T) ConvertUtils.ConvertOrCast(initialValue, culture, typeof (T));

    public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
    {
      if ((object) targetType == (object) typeof (object))
        return initialValue;
      if (initialValue == null && ReflectionUtils.IsNullable(targetType))
        return (object) null;
      object convertedValue;
      return ConvertUtils.TryConvert(initialValue, culture, targetType, out convertedValue) ? convertedValue : ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
    }

    public static bool TryConvertOrCast<T>(object initialValue, out T convertedValue) => ConvertUtils.TryConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);

    public static bool TryConvertOrCast<T>(
      object initialValue,
      CultureInfo culture,
      out T convertedValue)
    {
      return MiscellaneousUtils.TryAction<T>((Creator<T>) (() =>
      {
        object convertedValue1;
        ConvertUtils.TryConvertOrCast(initialValue, CultureInfo.CurrentCulture, typeof (T), out convertedValue1);
        return (T) convertedValue1;
      }), out convertedValue);
    }

    public static bool TryConvertOrCast(
      object initialValue,
      CultureInfo culture,
      Type targetType,
      out object convertedValue)
    {
      return MiscellaneousUtils.TryAction<object>((Creator<object>) (() => ConvertUtils.ConvertOrCast(initialValue, culture, targetType)), out convertedValue);
    }

    private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
    {
      Type type = value?.GetType();
      if (value != null)
      {
        if (targetType.IsAssignableFrom(type))
          return value;
        Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
        if (func != null)
          return func(value);
      }
      else if (ReflectionUtils.IsNullable(targetType))
        return (object) null;
      throw new Exception("Could not cast or convert from {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) initialType != null ? (object) initialType.ToString() : (object) "{null}", (object) targetType));
    }

    internal static TypeConverter GetConverter(Type t) => JsonTypeReflector.GetTypeConverter(t);

    public static bool IsInteger(object value)
    {
      switch (System.Convert.GetTypeCode(value))
      {
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
    {
      private readonly Type _initialType;
      private readonly Type _targetType;

      public Type InitialType => this._initialType;

      public Type TargetType => this._targetType;

      public TypeConvertKey(Type initialType, Type targetType)
      {
        this._initialType = initialType;
        this._targetType = targetType;
      }

      public override int GetHashCode() => this._initialType.GetHashCode() ^ this._targetType.GetHashCode();

      public override bool Equals(object obj) => obj is ConvertUtils.TypeConvertKey other && this.Equals(other);

      public bool Equals(ConvertUtils.TypeConvertKey other) => (object) this._initialType == (object) other._initialType && (object) this._targetType == (object) other._targetType;
    }
  }
}
