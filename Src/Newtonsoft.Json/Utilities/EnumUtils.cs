// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class EnumUtils
  {
    public static T Parse<T>(string enumMemberName) where T : struct => EnumUtils.Parse<T>(enumMemberName, false);

    public static T Parse<T>(string enumMemberName, bool ignoreCase) where T : struct
    {
      ValidationUtils.ArgumentTypeIsEnum(typeof (T), nameof (T));
      return (T) Enum.Parse(typeof (T), enumMemberName, ignoreCase);
    }

    public static bool TryParse<T>(string enumMemberName, bool ignoreCase, out T value) where T : struct
    {
      ValidationUtils.ArgumentTypeIsEnum(typeof (T), nameof (T));
      return MiscellaneousUtils.TryAction<T>((Creator<T>) (() => EnumUtils.Parse<T>(enumMemberName, ignoreCase)), out value);
    }

    public static IList<T> GetFlagsValues<T>(T value) where T : struct
    {
      Type type = typeof (T);
      if (!type.IsDefined(typeof (FlagsAttribute), false))
        throw new Exception("Enum type {0} is not a set of flags.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      ulong uint64 = Convert.ToUInt64((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
      EnumValues<ulong> namesAndValues = EnumUtils.GetNamesAndValues<T>();
      IList<T> flagsValues = (IList<T>) new List<T>();
      foreach (EnumValue<ulong> enumValue in (Collection<EnumValue<ulong>>) namesAndValues)
      {
        if (((long) uint64 & (long) enumValue.Value) == (long) enumValue.Value && enumValue.Value != 0UL)
          flagsValues.Add((T) Convert.ChangeType((object) enumValue.Value, underlyingType, (IFormatProvider) CultureInfo.CurrentCulture));
      }
      if (flagsValues.Count == 0 && namesAndValues.SingleOrDefault<EnumValue<ulong>>((Func<EnumValue<ulong>, bool>) (v => v.Value == 0UL)) != null)
        flagsValues.Add(default (T));
      return flagsValues;
    }

    public static EnumValues<ulong> GetNamesAndValues<T>() where T : struct => EnumUtils.GetNamesAndValues<ulong>(typeof (T));

    public static EnumValues<TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>()
      where TEnum : struct
      where TUnderlyingType : struct
    {
      return EnumUtils.GetNamesAndValues<TUnderlyingType>(typeof (TEnum));
    }

    public static EnumValues<TUnderlyingType> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
    {
      if ((object) enumType == null)
        throw new ArgumentNullException(nameof (enumType));
      ValidationUtils.ArgumentTypeIsEnum(enumType, nameof (enumType));
      IList<object> values = EnumUtils.GetValues(enumType);
      IList<string> names = EnumUtils.GetNames(enumType);
      EnumValues<TUnderlyingType> namesAndValues = new EnumValues<TUnderlyingType>();
      for (int index = 0; index < values.Count; ++index)
      {
        try
        {
          namesAndValues.Add(new EnumValue<TUnderlyingType>(names[index], (TUnderlyingType) Convert.ChangeType(values[index], typeof (TUnderlyingType), (IFormatProvider) CultureInfo.CurrentCulture)));
        }
        catch (OverflowException ex)
        {
          throw new Exception(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", new object[3]
          {
            (object) Enum.GetUnderlyingType(enumType),
            (object) typeof (TUnderlyingType),
            (object) Convert.ToUInt64(values[index], (IFormatProvider) CultureInfo.InvariantCulture)
          }), (Exception) ex);
        }
      }
      return namesAndValues;
    }

    public static IList<T> GetValues<T>() => (IList<T>) EnumUtils.GetValues(typeof (T)).Cast<T>().ToList<T>();

    public static IList<object> GetValues(Type enumType)
    {
      if (!enumType.IsEnum)
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<object> values = new List<object>();
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) enumType.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (field => field.IsLiteral)))
      {
        object obj = fieldInfo.GetValue((object) enumType);
        values.Add(obj);
      }
      return (IList<object>) values;
    }

    public static IList<string> GetNames<T>() => EnumUtils.GetNames(typeof (T));

    public static IList<string> GetNames(Type enumType)
    {
      if (!enumType.IsEnum)
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<string> names = new List<string>();
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) enumType.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (field => field.IsLiteral)))
        names.Add(fieldInfo.Name);
      return (IList<string>) names;
    }

    public static TEnumType GetMaximumValue<TEnumType>(Type enumType) where TEnumType : IConvertible, IComparable<TEnumType>
    {
      Type c = (object) enumType != null ? Enum.GetUnderlyingType(enumType) : throw new ArgumentNullException(nameof (enumType));
      if (!typeof (TEnumType).IsAssignableFrom(c))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "TEnumType is not assignable from the enum's underlying type of {0}.", new object[1]
        {
          (object) c.Name
        }));
      ulong num = 0;
      IList<object> values = EnumUtils.GetValues(enumType);
      if (enumType.IsDefined(typeof (FlagsAttribute), false))
      {
        foreach (TEnumType enumType1 in (IEnumerable<object>) values)
          num |= enumType1.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture);
      }
      else
      {
        foreach (TEnumType enumType2 in (IEnumerable<object>) values)
        {
          ulong uint64 = enumType2.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture);
          if (num.CompareTo(uint64) == -1)
            num = uint64;
        }
      }
      return (TEnumType) Convert.ChangeType((object) num, typeof (TEnumType), (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
