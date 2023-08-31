// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JToken
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
  public abstract class JToken : 
    IJEnumerable<JToken>,
    IEnumerable<JToken>,
    IEnumerable,
    IJsonLineInfo
  {
    private JContainer _parent;
    private JToken _previous;
    private JToken _next;
    private static JTokenEqualityComparer _equalityComparer;
    private int? _lineNumber;
    private int? _linePosition;

    public static JTokenEqualityComparer EqualityComparer
    {
      get
      {
        if (JToken._equalityComparer == null)
          JToken._equalityComparer = new JTokenEqualityComparer();
        return JToken._equalityComparer;
      }
    }

    public JContainer Parent
    {
      [DebuggerStepThrough] get => this._parent;
      internal set => this._parent = value;
    }

    public JToken Root
    {
      get
      {
        JContainer parent = this.Parent;
        if (parent == null)
          return this;
        while (parent.Parent != null)
          parent = parent.Parent;
        return (JToken) parent;
      }
    }

    internal abstract JToken CloneToken();

    internal abstract bool DeepEquals(JToken node);

    public abstract JTokenType Type { get; }

    public abstract bool HasValues { get; }

    public static bool DeepEquals(JToken t1, JToken t2)
    {
      if (t1 == t2)
        return true;
      return t1 != null && t2 != null && t1.DeepEquals(t2);
    }

    public JToken Next
    {
      get => this._next;
      internal set => this._next = value;
    }

    public JToken Previous
    {
      get => this._previous;
      internal set => this._previous = value;
    }

    internal JToken()
    {
    }

    public void AddAfterSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this) + 1, content);
    }

    public void AddBeforeSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this), content);
    }

    public IEnumerable<JToken> Ancestors()
    {
      for (JToken parent = (JToken) this.Parent; parent != null; parent = (JToken) parent.Parent)
        yield return parent;
    }

    public IEnumerable<JToken> AfterSelf()
    {
      if (this.Parent != null)
      {
        for (JToken o = this.Next; o != null; o = o.Next)
          yield return o;
      }
    }

    public IEnumerable<JToken> BeforeSelf()
    {
      for (JToken o = this.Parent.First; o != this; o = o.Next)
        yield return o;
    }

    public virtual JToken this[object key]
    {
      get => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      set => throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    public virtual T Value<T>(object key) => this[key].Convert<JToken, T>();

    public virtual JToken First => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));

    public virtual JToken Last => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));

    public virtual JEnumerable<JToken> Children() => JEnumerable<JToken>.Empty;

    public JEnumerable<T> Children<T>() where T : JToken => new JEnumerable<T>(this.Children().OfType<T>());

    public virtual IEnumerable<T> Values<T>() => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));

    public void Remove()
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.RemoveItem(this);
    }

    public void Replace(JToken value)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.ReplaceItem(this, value);
    }

    public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

    public override string ToString() => this.ToString(Formatting.Indented);

    public string ToString(Formatting formatting, params JsonConverter[] converters)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter);
        writer.Formatting = formatting;
        this.WriteTo((JsonWriter) writer, converters);
        return stringWriter.ToString();
      }
    }

    private static JValue EnsureValue(JToken value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value is JProperty)
        value = ((JProperty) value).Value;
      return value as JValue;
    }

    private static string GetType(JToken token)
    {
      ValidationUtils.ArgumentNotNull((object) token, nameof (token));
      if (token is JProperty)
        token = ((JProperty) token).Value;
      return token.Type.ToString();
    }

    private static bool IsNullable(JToken o) => o.Type == JTokenType.Undefined || o.Type == JTokenType.Null;

    private static bool ValidateFloat(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Float || o.Type == JTokenType.Integer)
        return true;
      return nullable && JToken.IsNullable(o);
    }

    private static bool ValidateInteger(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Integer)
        return true;
      return nullable && JToken.IsNullable(o);
    }

    private static bool ValidateDate(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Date)
        return true;
      return nullable && JToken.IsNullable(o);
    }

    private static bool ValidateBoolean(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Boolean)
        return true;
      return nullable && JToken.IsNullable(o);
    }

    private static bool ValidateString(JToken o) => o.Type == JTokenType.String || o.Type == JTokenType.Comment || o.Type == JTokenType.Raw || JToken.IsNullable(o);

    private static bool ValidateBytes(JToken o) => o.Type == JTokenType.Bytes || JToken.IsNullable(o);

    public static explicit operator bool(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateBoolean((JToken) o, false) ? (bool) o.Value : throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator DateTimeOffset(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateDate((JToken) o, false) ? (DateTimeOffset) o.Value : throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator bool?(JToken value)
    {
      if (value == null)
        return new bool?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateBoolean((JToken) o, true) ? (bool?) o.Value : throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator long(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateInteger((JToken) o, false) ? (long) o.Value : throw new ArgumentException("Can not convert {0} to Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator DateTime?(JToken value)
    {
      if (value == null)
        return new DateTime?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateDate((JToken) o, true) ? (DateTime?) o.Value : throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator DateTimeOffset?(JToken value)
    {
      if (value == null)
        return new DateTimeOffset?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateDate((JToken) o, true) ? (DateTimeOffset?) o.Value : throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator Decimal?(JToken value)
    {
      if (value == null)
        return new Decimal?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateFloat((JToken) o, true))
        throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new Decimal?() : new Decimal?(Convert.ToDecimal(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator double?(JToken value)
    {
      if (value == null)
        return new double?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateFloat((JToken) o, true) ? (double?) o.Value : throw new ArgumentException("Can not convert {0} to Double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator int(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator short(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to Int16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator ushort(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToUInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator int?(JToken value)
    {
      if (value == null)
        return new int?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, true))
        throw new ArgumentException("Can not convert {0} to Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new int?() : new int?(Convert.ToInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator short?(JToken value)
    {
      if (value == null)
        return new short?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, true))
        throw new ArgumentException("Can not convert {0} to Int16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new short?() : new short?(Convert.ToInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    public static explicit operator ushort?(JToken value)
    {
      if (value == null)
        return new ushort?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, true))
        throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new ushort?() : new ushort?((ushort) Convert.ToInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator DateTime(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateDate((JToken) o, false) ? (DateTime) o.Value : throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator long?(JToken value)
    {
      if (value == null)
        return new long?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateInteger((JToken) o, true) ? (long?) o.Value : throw new ArgumentException("Can not convert {0} to Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator float?(JToken value)
    {
      if (value == null)
        return new float?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateFloat((JToken) o, true))
        throw new ArgumentException("Can not convert {0} to Single.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new float?() : new float?(Convert.ToSingle(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator Decimal(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateFloat((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToDecimal(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator uint?(JToken value)
    {
      if (value == null)
        return new uint?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateInteger((JToken) o, true) ? (uint?) o.Value : throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    [CLSCompliant(false)]
    public static explicit operator ulong?(JToken value)
    {
      if (value == null)
        return new ulong?();
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateInteger((JToken) o, true) ? (ulong?) o.Value : throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator double(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateFloat((JToken) o, false) ? (double) o.Value : throw new ArgumentException("Can not convert {0} to Double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator float(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateFloat((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to Single.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToSingle(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator string(JToken value)
    {
      if (value == null)
        return (string) null;
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateString((JToken) o) ? (string) o.Value : throw new ArgumentException("Can not convert {0} to String.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    [CLSCompliant(false)]
    public static explicit operator uint(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToUInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator ulong(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateInteger((JToken) o, false))
        throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToUInt64(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator byte[](JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateBytes((JToken) o) ? (byte[]) o.Value : throw new ArgumentException("Can not convert {0} to byte array.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static implicit operator JToken(bool value) => (JToken) new JValue(value);

    public static implicit operator JToken(DateTimeOffset value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(bool? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(long value) => (JToken) new JValue(value);

    public static implicit operator JToken(DateTime? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(DateTimeOffset? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(Decimal? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(double? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(short value) => (JToken) new JValue((long) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ushort value) => (JToken) new JValue((long) value);

    public static implicit operator JToken(int value) => (JToken) new JValue((long) value);

    public static implicit operator JToken(int? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(DateTime value) => (JToken) new JValue(value);

    public static implicit operator JToken(long? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(float? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(Decimal value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(short? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ushort? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(uint? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ulong? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(double value) => (JToken) new JValue(value);

    public static implicit operator JToken(float value) => (JToken) new JValue((double) value);

    public static implicit operator JToken(string value) => (JToken) new JValue(value);

    [CLSCompliant(false)]
    public static implicit operator JToken(uint value) => (JToken) new JValue((long) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ulong value) => (JToken) new JValue(value);

    public static implicit operator JToken(byte[] value) => (JToken) new JValue((object) value);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) ((IEnumerable<JToken>) this).GetEnumerator();

    IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator() => this.Children().GetEnumerator();

    internal abstract int GetDeepHashCode();

    IJEnumerable<JToken> IJEnumerable<JToken>.this[object key] => (IJEnumerable<JToken>) this[key];

    public JsonReader CreateReader() => (JsonReader) new JTokenReader(this);

    internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull(o, nameof (o));
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, nameof (jsonSerializer));
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jsonSerializer.Serialize((JsonWriter) jtokenWriter, o);
        return jtokenWriter.Token;
      }
    }

    public static JToken FromObject(object o) => JToken.FromObjectInternal(o, new JsonSerializer());

    public static JToken FromObject(object o, JsonSerializer jsonSerializer) => JToken.FromObjectInternal(o, jsonSerializer);

    public T ToObject<T>() => this.ToObject<T>(new JsonSerializer());

    public T ToObject<T>(JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, nameof (jsonSerializer));
      using (JTokenReader reader = new JTokenReader(this))
        return jsonSerializer.Deserialize<T>((JsonReader) reader);
    }

    public static JToken ReadFrom(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw new Exception("Error reading JToken from JsonReader.");
      if (reader.TokenType == JsonToken.StartObject)
        return (JToken) JObject.Load(reader);
      if (reader.TokenType == JsonToken.StartArray)
        return (JToken) JArray.Load(reader);
      if (reader.TokenType == JsonToken.PropertyName)
        return (JToken) JProperty.Load(reader);
      if (reader.TokenType == JsonToken.StartConstructor)
        return (JToken) JConstructor.Load(reader);
      return !JsonReader.IsStartToken(reader.TokenType) ? (JToken) new JValue(reader.Value) : throw new Exception("Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    public static JToken Parse(string json) => JToken.Load((JsonReader) new JsonTextReader((TextReader) new StringReader(json)));

    public static JToken Load(JsonReader reader) => JToken.ReadFrom(reader);

    internal void SetLineInfo(IJsonLineInfo lineInfo)
    {
      if (lineInfo == null || !lineInfo.HasLineInfo())
        return;
      this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
    }

    internal void SetLineInfo(int lineNumber, int linePosition)
    {
      this._lineNumber = new int?(lineNumber);
      this._linePosition = new int?(linePosition);
    }

    bool IJsonLineInfo.HasLineInfo() => this._lineNumber.HasValue && this._linePosition.HasValue;

    int IJsonLineInfo.LineNumber => this._lineNumber ?? 0;

    int IJsonLineInfo.LinePosition => this._linePosition ?? 0;

    public JToken SelectToken(string path) => this.SelectToken(path, false);

    public JToken SelectToken(string path, bool errorWhenNoMatch) => new JPath(path).Evaluate(this, errorWhenNoMatch);

    public JToken DeepClone() => this.CloneToken();
  }
}
