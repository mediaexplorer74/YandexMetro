// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JValue
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>
  {
    private JTokenType _valueType;
    private object _value;

    internal JValue(object value, JTokenType type)
    {
      this._value = value;
      this._valueType = type;
    }

    public JValue(JValue other)
      : this(other.Value, other.Type)
    {
    }

    public JValue(long value)
      : this((object) value, JTokenType.Integer)
    {
    }

    [CLSCompliant(false)]
    public JValue(ulong value)
      : this((object) value, JTokenType.Integer)
    {
    }

    public JValue(double value)
      : this((object) value, JTokenType.Float)
    {
    }

    public JValue(DateTime value)
      : this((object) value, JTokenType.Date)
    {
    }

    public JValue(bool value)
      : this((object) value, JTokenType.Boolean)
    {
    }

    public JValue(string value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(Guid value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(Uri value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(TimeSpan value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(object value)
      : this(value, JValue.GetValueType(new JTokenType?(), value))
    {
    }

    internal override bool DeepEquals(JToken node) => node is JValue v2 && JValue.ValuesEquals(this, v2);

    public override bool HasValues => false;

    private static int Compare(JTokenType valueType, object objA, object objB)
    {
      if (objA == null && objB == null)
        return 0;
      if (objA != null && objB == null)
        return 1;
      if (objA == null && objB != null)
        return -1;
      switch (valueType)
      {
        case JTokenType.Comment:
        case JTokenType.String:
        case JTokenType.Raw:
          return Convert.ToString(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToString(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Integer:
          if (objA is ulong || objB is ulong || objA is Decimal || objB is Decimal)
            return Convert.ToDecimal(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, (IFormatProvider) CultureInfo.InvariantCulture));
          return objA is float || objB is float || objA is double || objB is double ? JValue.CompareFloat(objA, objB) : Convert.ToInt64(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Float:
          return JValue.CompareFloat(objA, objB);
        case JTokenType.Boolean:
          return Convert.ToBoolean(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToBoolean(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Date:
          if (objA is DateTime)
            return Convert.ToDateTime(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToDateTime(objB, (IFormatProvider) CultureInfo.InvariantCulture));
          return objB is DateTimeOffset other ? ((DateTimeOffset) objA).CompareTo(other) : throw new ArgumentException("Object must be of type DateTimeOffset.");
        case JTokenType.Bytes:
          if (!(objB is byte[]))
            throw new ArgumentException("Object must be of type byte[].");
          byte[] a1 = objA as byte[];
          byte[] a2 = objB as byte[];
          if (a1 == null)
            return -1;
          return a2 == null ? 1 : MiscellaneousUtils.ByteArrayCompare(a1, a2);
        case JTokenType.Guid:
          return objB is Guid guid ? ((Guid) objA).CompareTo(guid) : throw new ArgumentException("Object must be of type Guid.");
        case JTokenType.Uri:
          if (!(objB is Uri))
            throw new ArgumentException("Object must be of type Uri.");
          Uri uri1 = (Uri) objA;
          Uri uri2 = (Uri) objB;
          return Comparer<string>.Default.Compare(uri1.ToString(), uri2.ToString());
        case JTokenType.TimeSpan:
          return objB is TimeSpan timeSpan ? ((TimeSpan) objA).CompareTo(timeSpan) : throw new ArgumentException("Object must be of type TimeSpan.");
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof (valueType), (object) valueType, "Unexpected value type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) valueType));
      }
    }

    private static int CompareFloat(object objA, object objB)
    {
      double d1 = Convert.ToDouble(objA, (IFormatProvider) CultureInfo.InvariantCulture);
      double d2 = Convert.ToDouble(objB, (IFormatProvider) CultureInfo.InvariantCulture);
      return MathUtils.ApproxEquals(d1, d2) ? 0 : d1.CompareTo(d2);
    }

    internal override JToken CloneToken() => (JToken) new JValue(this);

    public static JValue CreateComment(string value) => new JValue((object) value, JTokenType.Comment);

    public static JValue CreateString(string value) => new JValue((object) value, JTokenType.String);

    private static JTokenType GetValueType(JTokenType? current, object value)
    {
      if (value == null || value == DBNull.Value)
        return JTokenType.Null;
      switch (value)
      {
        case string _:
          return JValue.GetStringValueType(current);
        case long _:
        case int _:
        case short _:
        case sbyte _:
        case ulong _:
        case uint _:
        case ushort _:
        case byte _:
          return JTokenType.Integer;
        case Enum _:
          return JTokenType.Integer;
        case double _:
        case float _:
        case Decimal _:
          return JTokenType.Float;
        case DateTime _:
          return JTokenType.Date;
        case DateTimeOffset _:
          return JTokenType.Date;
        case byte[] _:
          return JTokenType.Bytes;
        case bool _:
          return JTokenType.Boolean;
        case Guid _:
          return JTokenType.Guid;
        case Uri _:
          return JTokenType.Uri;
        case TimeSpan _:
          return JTokenType.TimeSpan;
        default:
          throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
      }
    }

    private static JTokenType GetStringValueType(JTokenType? current)
    {
      if (!current.HasValue)
        return JTokenType.String;
      switch (current.Value)
      {
        case JTokenType.Comment:
        case JTokenType.String:
        case JTokenType.Raw:
          return current.Value;
        default:
          return JTokenType.String;
      }
    }

    public override JTokenType Type => this._valueType;

    public object Value
    {
      get => this._value;
      set
      {
        if ((this._value != null ? (object) this._value.GetType() : (object) (System.Type) null) != (object) value?.GetType())
          this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
        this._value = value;
      }
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      switch (this._valueType)
      {
        case JTokenType.Comment:
          writer.WriteComment(this._value.ToString());
          break;
        case JTokenType.Null:
          writer.WriteNull();
          break;
        case JTokenType.Undefined:
          writer.WriteUndefined();
          break;
        case JTokenType.Raw:
          writer.WriteRawValue(this._value != null ? this._value.ToString() : (string) null);
          break;
        default:
          JsonConverter matchingConverter;
          if (this._value != null && (matchingConverter = JsonSerializer.GetMatchingConverter((IList<JsonConverter>) converters, this._value.GetType())) != null)
          {
            matchingConverter.WriteJson(writer, this._value, new JsonSerializer());
            break;
          }
          switch (this._valueType)
          {
            case JTokenType.Integer:
              writer.WriteValue(Convert.ToInt64(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case JTokenType.Float:
              writer.WriteValue(Convert.ToDouble(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case JTokenType.String:
              writer.WriteValue(this._value != null ? this._value.ToString() : (string) null);
              return;
            case JTokenType.Boolean:
              writer.WriteValue(Convert.ToBoolean(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case JTokenType.Date:
              if (this._value is DateTimeOffset)
              {
                writer.WriteValue((DateTimeOffset) this._value);
                return;
              }
              writer.WriteValue(Convert.ToDateTime(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case JTokenType.Bytes:
              writer.WriteValue((byte[]) this._value);
              return;
            case JTokenType.Guid:
            case JTokenType.Uri:
            case JTokenType.TimeSpan:
              writer.WriteValue(this._value != null ? this._value.ToString() : (string) null);
              return;
            default:
              throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", (object) this._valueType, "Unexpected token type.");
          }
      }
    }

    internal override int GetDeepHashCode()
    {
      int hashCode = this._value != null ? this._value.GetHashCode() : 0;
      return this._valueType.GetHashCode() ^ hashCode;
    }

    private static bool ValuesEquals(JValue v1, JValue v2)
    {
      if (v1 == v2)
        return true;
      return v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0;
    }

    public bool Equals(JValue other) => other != null && JValue.ValuesEquals(this, other);

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      return obj is JValue other ? this.Equals(other) : base.Equals(obj);
    }

    public override int GetHashCode() => this._value == null ? 0 : this._value.GetHashCode();

    public override string ToString() => this._value == null ? string.Empty : this._value.ToString();

    public string ToString(string format) => this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(IFormatProvider formatProvider) => this.ToString((string) null, formatProvider);

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (this._value == null)
        return string.Empty;
      return this._value is IFormattable formattable ? formattable.ToString(format, formatProvider) : this._value.ToString();
    }

    int IComparable.CompareTo(object obj) => obj == null ? 1 : JValue.Compare(this._valueType, this._value, obj is JValue ? ((JValue) obj).Value : obj);

    public int CompareTo(JValue obj) => obj == null ? 1 : JValue.Compare(this._valueType, this._value, obj._value);
  }
}
