// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ValidationUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
  internal static class ValidationUtils
  {
    public const string EmailAddressRegex = "^([a-zA-Z0-9_'+*$%\\^&!\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9:]{2,4})+$";
    public const string CurrencyRegex = "(^\\$?(?!0,?\\d)\\d{1,3}(,?\\d{3})*(\\.\\d\\d)?)$";
    public const string DateRegex = "^(((0?[1-9]|[12]\\d|3[01])[\\.\\-\\/](0?[13578]|1[02])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|[12]\\d|30)[\\.\\-\\/](0?[13456789]|1[012])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|1\\d|2[0-8])[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|(29[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00|[048])))$";
    public const string NumericRegex = "\\d*";

    public static void ArgumentNotNullOrEmpty(string value, string parameterName)
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(parameterName);
        case "":
          throw new ArgumentException("'{0}' cannot be empty.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName), parameterName);
      }
    }

    public static void ArgumentNotNullOrEmptyOrWhitespace(string value, string parameterName)
    {
      ValidationUtils.ArgumentNotNullOrEmpty(value, parameterName);
      if (StringUtils.IsWhiteSpace(value))
        throw new ArgumentException("'{0}' cannot only be whitespace.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName), parameterName);
    }

    public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
    {
      ValidationUtils.ArgumentNotNull((object) enumType, nameof (enumType));
      if (!enumType.IsEnum)
        throw new ArgumentException("Type {0} is not an Enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType), parameterName);
    }

    public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName) => ValidationUtils.ArgumentNotNullOrEmpty<T>(collection, parameterName, "Collection '{0}' cannot be empty.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName));

    public static void ArgumentNotNullOrEmpty<T>(
      ICollection<T> collection,
      string parameterName,
      string message)
    {
      if (collection == null)
        throw new ArgumentNullException(parameterName);
      if (collection.Count == 0)
        throw new ArgumentException(message, parameterName);
    }

    public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName) => ValidationUtils.ArgumentNotNullOrEmpty(collection, parameterName, "Collection '{0}' cannot be empty.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName));

    public static void ArgumentNotNullOrEmpty(
      ICollection collection,
      string parameterName,
      string message)
    {
      if (collection == null)
        throw new ArgumentNullException(parameterName);
      if (collection.Count == 0)
        throw new ArgumentException(message, parameterName);
    }

    public static void ArgumentNotNull(object value, string parameterName)
    {
      if (value == null)
        throw new ArgumentNullException(parameterName);
    }

    public static void ArgumentNotNegative(int value, string parameterName)
    {
      if (value <= 0)
        throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, (object) value, "Argument cannot be negative.");
    }

    public static void ArgumentNotNegative(int value, string parameterName, string message)
    {
      if (value <= 0)
        throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, (object) value, message);
    }

    public static void ArgumentNotZero(int value, string parameterName)
    {
      if (value == 0)
        throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, (object) value, "Argument cannot be zero.");
    }

    public static void ArgumentNotZero(int value, string parameterName, string message)
    {
      if (value == 0)
        throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, (object) value, message);
    }

    public static void ArgumentIsPositive<T>(T value, string parameterName) where T : struct, IComparable<T>
    {
      if (value.CompareTo(default (T)) != 1)
        throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, (object) value, "Positive number required.");
    }

    public static void ArgumentIsPositive(int value, string parameterName, string message)
    {
      if (value > 0)
        throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, (object) value, message);
    }

    public static void ObjectNotDisposed(bool disposed, Type objectType)
    {
      if (disposed)
        throw new ObjectDisposedException(objectType.Name);
    }

    public static void ArgumentConditionTrue(bool condition, string parameterName, string message)
    {
      if (!condition)
        throw new ArgumentException(message, parameterName);
    }
  }
}
