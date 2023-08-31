// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonWriter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json
{
  public abstract class JsonWriter : IDisposable
  {
    private static readonly JsonWriter.State[][] stateArray = new JsonWriter.State[8][]
    {
      new JsonWriter.State[10]
      {
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Property,
        JsonWriter.State.Error,
        JsonWriter.State.Property,
        JsonWriter.State.Property,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Property,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Object,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Property,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Object,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Object,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Array,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      }
    };
    private int _top;
    private readonly List<JTokenType> _stack;
    private JsonWriter.State _currentState;
    private Formatting _formatting;

    public bool CloseOutput { get; set; }

    protected internal int Top => this._top;

    public WriteState WriteState
    {
      get
      {
        switch (this._currentState)
        {
          case JsonWriter.State.Start:
            return WriteState.Start;
          case JsonWriter.State.Property:
            return WriteState.Property;
          case JsonWriter.State.ObjectStart:
          case JsonWriter.State.Object:
            return WriteState.Object;
          case JsonWriter.State.ArrayStart:
          case JsonWriter.State.Array:
            return WriteState.Array;
          case JsonWriter.State.ConstructorStart:
          case JsonWriter.State.Constructor:
            return WriteState.Constructor;
          case JsonWriter.State.Closed:
            return WriteState.Closed;
          case JsonWriter.State.Error:
            return WriteState.Error;
          default:
            throw new JsonWriterException("Invalid state: " + (object) this._currentState);
        }
      }
    }

    public Formatting Formatting
    {
      get => this._formatting;
      set => this._formatting = value;
    }

    protected JsonWriter()
    {
      this._stack = new List<JTokenType>(8);
      this._stack.Add(JTokenType.None);
      this._currentState = JsonWriter.State.Start;
      this._formatting = Formatting.None;
      this.CloseOutput = true;
    }

    private void Push(JTokenType value)
    {
      ++this._top;
      if (this._stack.Count <= this._top)
        this._stack.Add(value);
      else
        this._stack[this._top] = value;
    }

    private JTokenType Pop()
    {
      JTokenType jtokenType = this.Peek();
      --this._top;
      return jtokenType;
    }

    private JTokenType Peek() => this._stack[this._top];

    public abstract void Flush();

    public virtual void Close() => this.AutoCompleteAll();

    public virtual void WriteStartObject()
    {
      this.AutoComplete(JsonToken.StartObject);
      this.Push(JTokenType.Object);
    }

    public virtual void WriteEndObject() => this.AutoCompleteClose(JsonToken.EndObject);

    public virtual void WriteStartArray()
    {
      this.AutoComplete(JsonToken.StartArray);
      this.Push(JTokenType.Array);
    }

    public virtual void WriteEndArray() => this.AutoCompleteClose(JsonToken.EndArray);

    public virtual void WriteStartConstructor(string name)
    {
      this.AutoComplete(JsonToken.StartConstructor);
      this.Push(JTokenType.Constructor);
    }

    public virtual void WriteEndConstructor() => this.AutoCompleteClose(JsonToken.EndConstructor);

    public virtual void WritePropertyName(string name) => this.AutoComplete(JsonToken.PropertyName);

    public virtual void WriteEnd() => this.WriteEnd(this.Peek());

    public void WriteToken(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      int initialDepth = reader.TokenType != JsonToken.None ? (this.IsStartToken(reader.TokenType) ? reader.Depth : reader.Depth + 1) : -1;
      this.WriteToken(reader, initialDepth);
    }

    internal void WriteToken(JsonReader reader, int initialDepth)
    {
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.None:
            continue;
          case JsonToken.StartObject:
            this.WriteStartObject();
            goto case JsonToken.None;
          case JsonToken.StartArray:
            this.WriteStartArray();
            goto case JsonToken.None;
          case JsonToken.StartConstructor:
            if (string.Compare(reader.Value.ToString(), "Date", StringComparison.Ordinal) == 0)
            {
              this.WriteConstructorDate(reader);
              goto case JsonToken.None;
            }
            else
            {
              this.WriteStartConstructor(reader.Value.ToString());
              goto case JsonToken.None;
            }
          case JsonToken.PropertyName:
            this.WritePropertyName(reader.Value.ToString());
            goto case JsonToken.None;
          case JsonToken.Comment:
            this.WriteComment(reader.Value.ToString());
            goto case JsonToken.None;
          case JsonToken.Raw:
            this.WriteRawValue((string) reader.Value);
            goto case JsonToken.None;
          case JsonToken.Integer:
            this.WriteValue(Convert.ToInt64(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            goto case JsonToken.None;
          case JsonToken.Float:
            this.WriteValue(Convert.ToDouble(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            goto case JsonToken.None;
          case JsonToken.String:
            this.WriteValue(reader.Value.ToString());
            goto case JsonToken.None;
          case JsonToken.Boolean:
            this.WriteValue(Convert.ToBoolean(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            goto case JsonToken.None;
          case JsonToken.Null:
            this.WriteNull();
            goto case JsonToken.None;
          case JsonToken.Undefined:
            this.WriteUndefined();
            goto case JsonToken.None;
          case JsonToken.EndObject:
            this.WriteEndObject();
            goto case JsonToken.None;
          case JsonToken.EndArray:
            this.WriteEndArray();
            goto case JsonToken.None;
          case JsonToken.EndConstructor:
            this.WriteEndConstructor();
            goto case JsonToken.None;
          case JsonToken.Date:
            this.WriteValue((DateTime) reader.Value);
            goto case JsonToken.None;
          case JsonToken.Bytes:
            this.WriteValue((byte[]) reader.Value);
            goto case JsonToken.None;
          default:
            throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", (object) reader.TokenType, "Unexpected token type.");
        }
      }
      while (initialDepth - 1 < reader.Depth - (this.IsEndToken(reader.TokenType) ? 1 : 0) && reader.Read());
    }

    private void WriteConstructorDate(JsonReader reader)
    {
      if (!reader.Read())
        throw new Exception("Unexpected end while reading date constructor.");
      DateTime dateTime = reader.TokenType == JsonToken.Integer ? JsonConvert.ConvertJavaScriptTicksToDateTime((long) reader.Value) : throw new Exception("Unexpected token while reading date constructor. Expected Integer, got " + (object) reader.TokenType);
      if (!reader.Read())
        throw new Exception("Unexpected end while reading date constructor.");
      if (reader.TokenType != JsonToken.EndConstructor)
        throw new Exception("Unexpected token while reading date constructor. Expected EndConstructor, got " + (object) reader.TokenType);
      this.WriteValue(dateTime);
    }

    private bool IsEndToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
        case JsonToken.EndArray:
        case JsonToken.EndConstructor:
          return true;
        default:
          return false;
      }
    }

    private bool IsStartToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.StartObject:
        case JsonToken.StartArray:
        case JsonToken.StartConstructor:
          return true;
        default:
          return false;
      }
    }

    private void WriteEnd(JTokenType type)
    {
      switch (type)
      {
        case JTokenType.Object:
          this.WriteEndObject();
          break;
        case JTokenType.Array:
          this.WriteEndArray();
          break;
        case JTokenType.Constructor:
          this.WriteEndConstructor();
          break;
        default:
          throw new JsonWriterException("Unexpected type when writing end: " + (object) type);
      }
    }

    private void AutoCompleteAll()
    {
      while (this._top > 0)
        this.WriteEnd();
    }

    private JTokenType GetTypeForCloseToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          return JTokenType.Object;
        case JsonToken.EndArray:
          return JTokenType.Array;
        case JsonToken.EndConstructor:
          return JTokenType.Constructor;
        default:
          throw new JsonWriterException("No type for token: " + (object) token);
      }
    }

    private JsonToken GetCloseTokenForType(JTokenType type)
    {
      switch (type)
      {
        case JTokenType.Object:
          return JsonToken.EndObject;
        case JTokenType.Array:
          return JsonToken.EndArray;
        case JTokenType.Constructor:
          return JsonToken.EndConstructor;
        default:
          throw new JsonWriterException("No close token for type: " + (object) type);
      }
    }

    private void AutoCompleteClose(JsonToken tokenBeingClosed)
    {
      int num = 0;
      for (int index = 0; index < this._top; ++index)
      {
        if (this._stack[this._top - index] == this.GetTypeForCloseToken(tokenBeingClosed))
        {
          num = index + 1;
          break;
        }
      }
      if (num == 0)
        throw new JsonWriterException("No token to close.");
      for (int index = 0; index < num; ++index)
      {
        JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
        if (this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
          this.WriteIndent();
        this.WriteEnd(closeTokenForType);
      }
      JTokenType jtokenType = this.Peek();
      switch (jtokenType)
      {
        case JTokenType.None:
          this._currentState = JsonWriter.State.Start;
          break;
        case JTokenType.Object:
          this._currentState = JsonWriter.State.Object;
          break;
        case JTokenType.Array:
          this._currentState = JsonWriter.State.Array;
          break;
        case JTokenType.Constructor:
          this._currentState = JsonWriter.State.Array;
          break;
        default:
          throw new JsonWriterException("Unknown JsonType: " + (object) jtokenType);
      }
    }

    protected virtual void WriteEnd(JsonToken token)
    {
    }

    protected virtual void WriteIndent()
    {
    }

    protected virtual void WriteValueDelimiter()
    {
    }

    protected virtual void WriteIndentSpace()
    {
    }

    internal void AutoComplete(JsonToken tokenBeingWritten)
    {
      int index;
      switch (tokenBeingWritten)
      {
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          index = 7;
          break;
        default:
          index = (int) tokenBeingWritten;
          break;
      }
      JsonWriter.State state = JsonWriter.stateArray[index][(int) this._currentState];
      if (state == JsonWriter.State.Error)
        throw new JsonWriterException("Token {0} in state {1} would result in an invalid JavaScript object.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) tokenBeingWritten.ToString(), (object) this._currentState.ToString()));
      if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
        this.WriteValueDelimiter();
      else if (this._currentState == JsonWriter.State.Property && this._formatting == Formatting.Indented)
        this.WriteIndentSpace();
      WriteState writeState = this.WriteState;
      if (tokenBeingWritten == JsonToken.PropertyName && writeState != WriteState.Start || writeState == WriteState.Array || writeState == WriteState.Constructor)
        this.WriteIndent();
      this._currentState = state;
    }

    public virtual void WriteNull() => this.AutoComplete(JsonToken.Null);

    public virtual void WriteUndefined() => this.AutoComplete(JsonToken.Undefined);

    public virtual void WriteRaw(string json)
    {
    }

    public virtual void WriteRawValue(string json)
    {
      this.AutoComplete(JsonToken.Undefined);
      this.WriteRaw(json);
    }

    public virtual void WriteValue(string value) => this.AutoComplete(JsonToken.String);

    public virtual void WriteValue(int value) => this.AutoComplete(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(uint value) => this.AutoComplete(JsonToken.Integer);

    public virtual void WriteValue(long value) => this.AutoComplete(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(ulong value) => this.AutoComplete(JsonToken.Integer);

    public virtual void WriteValue(float value) => this.AutoComplete(JsonToken.Float);

    public virtual void WriteValue(double value) => this.AutoComplete(JsonToken.Float);

    public virtual void WriteValue(bool value) => this.AutoComplete(JsonToken.Boolean);

    public virtual void WriteValue(short value) => this.AutoComplete(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(ushort value) => this.AutoComplete(JsonToken.Integer);

    public virtual void WriteValue(char value) => this.AutoComplete(JsonToken.String);

    public virtual void WriteValue(byte value) => this.AutoComplete(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(sbyte value) => this.AutoComplete(JsonToken.Integer);

    public virtual void WriteValue(Decimal value) => this.AutoComplete(JsonToken.Float);

    public virtual void WriteValue(DateTime value) => this.AutoComplete(JsonToken.Date);

    public virtual void WriteValue(DateTimeOffset value) => this.AutoComplete(JsonToken.Date);

    public virtual void WriteValue(Guid value) => this.AutoComplete(JsonToken.String);

    public virtual void WriteValue(TimeSpan value) => this.AutoComplete(JsonToken.String);

    public virtual void WriteValue(int? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(uint? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(long? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ulong? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(float? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(double? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(bool? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(short? value)
    {
      short? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ushort? value)
    {
      ushort? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(char? value)
    {
      char? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(byte? value)
    {
      byte? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(sbyte? value)
    {
      sbyte? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(Decimal? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(DateTime? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(DateTimeOffset? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(Guid? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(TimeSpan? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(byte[] value)
    {
      if (value == null)
        this.WriteNull();
      else
        this.AutoComplete(JsonToken.Bytes);
    }

    public virtual void WriteValue(Uri value)
    {
      if (value == (Uri) null)
        this.WriteNull();
      else
        this.AutoComplete(JsonToken.String);
    }

    public virtual void WriteValue(object value)
    {
      switch (value)
      {
        case null:
          this.WriteNull();
          return;
        case IConvertible _:
          IConvertible convertible = value as IConvertible;
          switch (convertible.GetTypeCode())
          {
            case TypeCode.DBNull:
              this.WriteNull();
              return;
            case TypeCode.Boolean:
              this.WriteValue(convertible.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Char:
              this.WriteValue(convertible.ToChar((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.SByte:
              this.WriteValue(convertible.ToSByte((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Byte:
              this.WriteValue(convertible.ToByte((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Int16:
              this.WriteValue(convertible.ToInt16((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.UInt16:
              this.WriteValue(convertible.ToUInt16((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Int32:
              this.WriteValue(convertible.ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.UInt32:
              this.WriteValue(convertible.ToUInt32((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Int64:
              this.WriteValue(convertible.ToInt64((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.UInt64:
              this.WriteValue(convertible.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Single:
              this.WriteValue(convertible.ToSingle((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Double:
              this.WriteValue(convertible.ToDouble((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Decimal:
              this.WriteValue(convertible.ToDecimal((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.DateTime:
              this.WriteValue(convertible.ToDateTime((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.String:
              this.WriteValue(convertible.ToString((IFormatProvider) CultureInfo.InvariantCulture));
              return;
          }
          break;
        case DateTimeOffset dateTimeOffset:
          this.WriteValue(dateTimeOffset);
          return;
        case byte[] _:
          this.WriteValue((byte[]) value);
          return;
        case Guid guid:
          this.WriteValue(guid);
          return;
        case Uri _:
          this.WriteValue((Uri) value);
          return;
        case TimeSpan timeSpan:
          this.WriteValue(timeSpan);
          return;
      }
      throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    public virtual void WriteComment(string text) => this.AutoComplete(JsonToken.Comment);

    public virtual void WriteWhitespace(string ws)
    {
      if (ws != null && !StringUtils.IsWhiteSpace(ws))
        throw new JsonWriterException("Only white space characters should be used.");
    }

    void IDisposable.Dispose() => this.Dispose(true);

    private void Dispose(bool disposing)
    {
      if (this.WriteState == WriteState.Closed)
        return;
      this.Close();
    }

    private enum State
    {
      Start,
      Property,
      ObjectStart,
      Object,
      ArrayStart,
      Array,
      ConstructorStart,
      Constructor,
      Bytes,
      Closed,
      Error,
    }
  }
}
