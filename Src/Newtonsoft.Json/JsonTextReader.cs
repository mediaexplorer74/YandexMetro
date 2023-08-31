// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonTextReader
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json
{
  public class JsonTextReader : JsonReader, IJsonLineInfo
  {
    private const int LineFeedValue = 10;
    private const int CarriageReturnValue = 13;
    private readonly TextReader _reader;
    private readonly StringBuffer _buffer;
    private char? _lastChar;
    private int _currentLinePosition;
    private int _currentLineNumber;
    private bool _end;
    private JsonTextReader.ReadType _readType;
    private CultureInfo _culture;

    public CultureInfo Culture
    {
      get => this._culture ?? CultureInfo.CurrentCulture;
      set => this._culture = value;
    }

    public JsonTextReader(TextReader reader)
    {
      this._reader = reader != null ? reader : throw new ArgumentNullException(nameof (reader));
      this._buffer = new StringBuffer(4096);
      this._currentLineNumber = 1;
    }

    private void ParseString(char quote)
    {
      this.ReadStringIntoBuffer(quote);
      if (this._readType == JsonTextReader.ReadType.ReadAsBytes)
      {
        byte[] numArray;
        if (this._buffer.Position == 0)
        {
          numArray = new byte[0];
        }
        else
        {
          numArray = Convert.FromBase64CharArray(this._buffer.GetInternalBuffer(), 0, this._buffer.Position);
          this._buffer.Position = 0;
        }
        this.SetToken(JsonToken.Bytes, (object) numArray);
      }
      else
      {
        string text = this._buffer.ToString();
        this._buffer.Position = 0;
        if (text.StartsWith("/Date(", StringComparison.Ordinal) && text.EndsWith(")/", StringComparison.Ordinal))
        {
          this.ParseDate(text);
        }
        else
        {
          this.SetToken(JsonToken.String, (object) text);
          this.QuoteChar = quote;
        }
      }
    }

    private void ReadStringIntoBuffer(char quote)
    {
      char ch1;
      while (true)
      {
        char ch2 = this.MoveNext();
        switch (ch2)
        {
          case char.MinValue:
            if (!this._end)
            {
              this._buffer.Append(char.MinValue);
              continue;
            }
            goto label_2;
          case '"':
          case '\'':
            if ((int) ch2 != (int) quote)
            {
              this._buffer.Append(ch2);
              continue;
            }
            goto label_6;
          case '\\':
            if ((ch1 = this.MoveNext()) != char.MinValue || !this._end)
            {
              switch (ch1)
              {
                case '"':
                case '\'':
                case '/':
                  this._buffer.Append(ch1);
                  continue;
                case '\\':
                  this._buffer.Append('\\');
                  continue;
                case 'b':
                  this._buffer.Append('\b');
                  continue;
                case 'f':
                  this._buffer.Append('\f');
                  continue;
                case 'n':
                  this._buffer.Append('\n');
                  continue;
                case 'r':
                  this._buffer.Append('\r');
                  continue;
                case 't':
                  this._buffer.Append('\t');
                  continue;
                case 'u':
                  char[] chArray = new char[4];
                  for (int index = 0; index < chArray.Length; ++index)
                  {
                    char ch3;
                    if ((ch3 = this.MoveNext()) != char.MinValue || !this._end)
                      chArray[index] = ch3;
                    else
                      throw this.CreateJsonReaderException("Unexpected end while parsing unicode character. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
                  }
                  this._buffer.Append(Convert.ToChar(int.Parse(new string(chArray), NumberStyles.HexNumber, (IFormatProvider) NumberFormatInfo.InvariantInfo)));
                  continue;
                default:
                  goto label_20;
              }
            }
            else
              goto label_21;
          default:
            this._buffer.Append(ch2);
            continue;
        }
      }
label_2:
      throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", (object) quote, (object) this._currentLineNumber, (object) this._currentLinePosition);
label_20:
      throw this.CreateJsonReaderException("Bad JSON escape sequence: {0}. Line {1}, position {2}.", (object) ("\\" + (object) ch1), (object) this._currentLineNumber, (object) this._currentLinePosition);
label_21:
      throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", (object) quote, (object) this._currentLineNumber, (object) this._currentLinePosition);
label_6:;
    }

    private JsonReaderException CreateJsonReaderException(string format, params object[] args) => new JsonReaderException(format.FormatWith((IFormatProvider) CultureInfo.InvariantCulture, args), (Exception) null, this._currentLineNumber, this._currentLinePosition);

    private TimeSpan ReadOffset(string offsetText)
    {
      bool flag = offsetText[0] == '-';
      int num1 = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
      int num2 = 0;
      if (offsetText.Length >= 5)
        num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
      TimeSpan timeSpan = TimeSpan.FromHours((double) num1) + TimeSpan.FromMinutes((double) num2);
      if (flag)
        timeSpan = timeSpan.Negate();
      return timeSpan;
    }

    private void ParseDate(string text)
    {
      string s = text.Substring(6, text.Length - 8);
      DateTimeKind dateTimeKind = DateTimeKind.Utc;
      int num = s.IndexOf('+', 1);
      if (num == -1)
        num = s.IndexOf('-', 1);
      TimeSpan offset = TimeSpan.Zero;
      if (num != -1)
      {
        dateTimeKind = DateTimeKind.Local;
        offset = this.ReadOffset(s.Substring(num));
        s = s.Substring(0, num);
      }
      DateTime dateTime1 = JsonConvert.ConvertJavaScriptTicksToDateTime(long.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture));
      if (this._readType == JsonTextReader.ReadType.ReadAsDateTimeOffset)
      {
        this.SetToken(JsonToken.Date, (object) new DateTimeOffset(dateTime1.Add(offset).Ticks, offset));
      }
      else
      {
        DateTime dateTime2;
        switch (dateTimeKind)
        {
          case DateTimeKind.Unspecified:
            dateTime2 = DateTime.SpecifyKind(dateTime1.ToLocalTime(), DateTimeKind.Unspecified);
            break;
          case DateTimeKind.Local:
            dateTime2 = dateTime1.ToLocalTime();
            break;
          default:
            dateTime2 = dateTime1;
            break;
        }
        this.SetToken(JsonToken.Date, (object) dateTime2);
      }
    }

    private char MoveNext()
    {
      int num = this._reader.Read();
      switch (num)
      {
        case -1:
          this._end = true;
          return char.MinValue;
        case 10:
          ++this._currentLineNumber;
          this._currentLinePosition = 0;
          break;
        case 13:
          if (this._reader.Peek() == 10)
            this._reader.Read();
          ++this._currentLineNumber;
          this._currentLinePosition = 0;
          break;
        default:
          ++this._currentLinePosition;
          break;
      }
      return (char) num;
    }

    private bool HasNext() => this._reader.Peek() != -1;

    private int PeekNext() => this._reader.Peek();

    public override bool Read()
    {
      this._readType = JsonTextReader.ReadType.Read;
      return this.ReadInternal();
    }

    public override byte[] ReadAsBytes()
    {
      this._readType = JsonTextReader.ReadType.ReadAsBytes;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.Null)
            return (byte[]) null;
          if (this.TokenType == JsonToken.Bytes)
            return (byte[]) this.Value;
          if (this.TokenType == JsonToken.StartArray)
          {
            List<byte> byteList = new List<byte>();
            while (this.ReadInternal())
            {
              switch (this.TokenType)
              {
                case JsonToken.Comment:
                  continue;
                case JsonToken.Integer:
                  byteList.Add(Convert.ToByte(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
                  continue;
                case JsonToken.EndArray:
                  byte[] array = byteList.ToArray();
                  this.SetToken(JsonToken.Bytes, (object) array);
                  return array;
                default:
                  throw this.CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", (object) this.TokenType, (object) this._currentLineNumber, (object) this._currentLinePosition);
              }
            }
            throw this.CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
          }
          throw this.CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", (object) this.TokenType, (object) this._currentLineNumber, (object) this._currentLinePosition);
        }
      }
      throw this.CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    public override Decimal? ReadAsDecimal()
    {
      this._readType = JsonTextReader.ReadType.ReadAsDecimal;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.Null)
            return new Decimal?();
          if (this.TokenType == JsonToken.Float)
            return (Decimal?) this.Value;
          Decimal result;
          if (this.TokenType == JsonToken.String && Decimal.TryParse((string) this.Value, NumberStyles.Number, (IFormatProvider) this.Culture, out result))
          {
            this.SetToken(JsonToken.Float, (object) result);
            return new Decimal?(result);
          }
          throw this.CreateJsonReaderException("Unexpected token when reading decimal: {0}. Line {1}, position {2}.", (object) this.TokenType, (object) this._currentLineNumber, (object) this._currentLinePosition);
        }
      }
      throw this.CreateJsonReaderException("Unexpected end when reading decimal: Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    public override DateTimeOffset? ReadAsDateTimeOffset()
    {
      this._readType = JsonTextReader.ReadType.ReadAsDateTimeOffset;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.Null)
            return new DateTimeOffset?();
          if (this.TokenType == JsonToken.Date)
            return new DateTimeOffset?((DateTimeOffset) this.Value);
          DateTimeOffset result;
          if (this.TokenType == JsonToken.String && DateTimeOffset.TryParse((string) this.Value, (IFormatProvider) this.Culture, DateTimeStyles.None, out result))
          {
            this.SetToken(JsonToken.Date, (object) result);
            return new DateTimeOffset?(result);
          }
          throw this.CreateJsonReaderException("Unexpected token when reading date: {0}. Line {1}, position {2}.", (object) this.TokenType, (object) this._currentLineNumber, (object) this._currentLinePosition);
        }
      }
      throw this.CreateJsonReaderException("Unexpected end when reading date: Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private bool ReadInternal()
    {
      char currentChar;
      do
      {
        char? lastChar = this._lastChar;
        if ((lastChar.HasValue ? new int?((int) lastChar.GetValueOrDefault()) : new int?()).HasValue)
        {
          currentChar = this._lastChar.Value;
          this._lastChar = new char?();
        }
        else
          currentChar = this.MoveNext();
        if (currentChar == char.MinValue && this._end)
          return false;
        switch (this.CurrentState)
        {
          case JsonReader.State.Start:
          case JsonReader.State.Property:
          case JsonReader.State.ArrayStart:
          case JsonReader.State.Array:
          case JsonReader.State.ConstructorStart:
          case JsonReader.State.Constructor:
            return this.ParseValue(currentChar);
          case JsonReader.State.Complete:
          case JsonReader.State.Closed:
          case JsonReader.State.Error:
            continue;
          case JsonReader.State.ObjectStart:
          case JsonReader.State.Object:
            return this.ParseObject(currentChar);
          case JsonReader.State.PostValue:
            continue;
          default:
            goto label_10;
        }
      }
      while (!this.ParsePostValue(currentChar));
      return true;
label_10:
      throw this.CreateJsonReaderException("Unexpected state: {0}. Line {1}, position {2}.", (object) this.CurrentState, (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private bool ParsePostValue(char currentChar)
    {
      do
      {
        switch (currentChar)
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            continue;
          case ')':
            this.SetToken(JsonToken.EndConstructor);
            return true;
          case ',':
            this.SetStateBasedOnCurrent();
            return false;
          case '/':
            this.ParseComment();
            return true;
          case ']':
            this.SetToken(JsonToken.EndArray);
            return true;
          case '}':
            this.SetToken(JsonToken.EndObject);
            return true;
          default:
            if (!char.IsWhiteSpace(currentChar))
              throw this.CreateJsonReaderException("After parsing a value an unexpected character was encountered: {0}. Line {1}, position {2}.", (object) currentChar, (object) this._currentLineNumber, (object) this._currentLinePosition);
            goto case '\t';
        }
      }
      while ((currentChar = this.MoveNext()) != char.MinValue || !this._end);
      return false;
    }

    private bool ParseObject(char currentChar)
    {
      do
      {
        switch (currentChar)
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            continue;
          case '/':
            this.ParseComment();
            return true;
          case '}':
            this.SetToken(JsonToken.EndObject);
            return true;
          default:
            if (!char.IsWhiteSpace(currentChar))
              return this.ParseProperty(currentChar);
            goto case '\t';
        }
      }
      while ((currentChar = this.MoveNext()) != char.MinValue || !this._end);
      return false;
    }

    private bool ParseProperty(char firstChar)
    {
      char firstChar1 = firstChar;
      char quote;
      char ch;
      if (this.ValidIdentifierChar(firstChar1))
      {
        quote = char.MinValue;
        ch = this.ParseUnquotedProperty(firstChar1);
      }
      else
      {
        quote = firstChar1 == '"' || firstChar1 == '\'' ? firstChar1 : throw this.CreateJsonReaderException("Invalid property identifier character: {0}. Line {1}, position {2}.", (object) firstChar1, (object) this._currentLineNumber, (object) this._currentLinePosition);
        this.ReadStringIntoBuffer(quote);
        ch = this.MoveNext();
      }
      if (ch != ':')
      {
        char finalChar = this.MoveNext();
        this.EatWhitespace(finalChar, false, out finalChar);
        if (finalChar != ':')
          throw this.CreateJsonReaderException("Invalid character after parsing property name. Expected ':' but got: {0}. Line {1}, position {2}.", (object) finalChar, (object) this._currentLineNumber, (object) this._currentLinePosition);
      }
      this.SetToken(JsonToken.PropertyName, (object) this._buffer.ToString());
      this.QuoteChar = quote;
      this._buffer.Position = 0;
      return true;
    }

    private bool ValidIdentifierChar(char value) => char.IsLetterOrDigit(value) || value == '_' || value == '$';

    private char ParseUnquotedProperty(char firstChar)
    {
      this._buffer.Append(firstChar);
      char c;
      while ((c = this.MoveNext()) != char.MinValue || !this._end)
      {
        if (char.IsWhiteSpace(c) || c == ':')
          return c;
        if (this.ValidIdentifierChar(c))
          this._buffer.Append(c);
        else
          throw this.CreateJsonReaderException("Invalid JavaScript property identifier character: {0}. Line {1}, position {2}.", (object) c, (object) this._currentLineNumber, (object) this._currentLinePosition);
      }
      throw this.CreateJsonReaderException("Unexpected end when parsing unquoted property name. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private bool ParseValue(char currentChar)
    {
      do
      {
        switch (currentChar)
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            continue;
          case '"':
          case '\'':
            this.ParseString(currentChar);
            return true;
          case ')':
            this.SetToken(JsonToken.EndConstructor);
            return true;
          case ',':
            this.SetToken(JsonToken.Undefined);
            return true;
          case '-':
            if (this.PeekNext() == 73)
              this.ParseNumberNegativeInfinity();
            else
              this.ParseNumber(currentChar);
            return true;
          case '/':
            this.ParseComment();
            return true;
          case 'I':
            this.ParseNumberPositiveInfinity();
            return true;
          case 'N':
            this.ParseNumberNaN();
            return true;
          case '[':
            this.SetToken(JsonToken.StartArray);
            return true;
          case ']':
            this.SetToken(JsonToken.EndArray);
            return true;
          case 'f':
            this.ParseFalse();
            return true;
          case 'n':
            if (this.HasNext())
            {
              switch ((char) this.PeekNext())
              {
                case 'e':
                  this.ParseConstructor();
                  break;
                case 'u':
                  this.ParseNull();
                  break;
                default:
                  throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", (object) currentChar, (object) this._currentLineNumber, (object) this._currentLinePosition);
              }
              return true;
            }
            throw this.CreateJsonReaderException("Unexpected end. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
          case 't':
            this.ParseTrue();
            return true;
          case 'u':
            this.ParseUndefined();
            return true;
          case '{':
            this.SetToken(JsonToken.StartObject);
            return true;
          case '}':
            this.SetToken(JsonToken.EndObject);
            return true;
          default:
            if (!char.IsWhiteSpace(currentChar))
            {
              if (char.IsNumber(currentChar) || currentChar == '-' || currentChar == '.')
              {
                this.ParseNumber(currentChar);
                return true;
              }
              throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", (object) currentChar, (object) this._currentLineNumber, (object) this._currentLinePosition);
            }
            goto case '\t';
        }
      }
      while ((currentChar = this.MoveNext()) != char.MinValue || !this._end);
      return false;
    }

    private bool EatWhitespace(char initialChar, bool oneOrMore, out char finalChar)
    {
      bool flag = false;
      char c;
      for (c = initialChar; c == ' ' || char.IsWhiteSpace(c); c = this.MoveNext())
        flag = true;
      finalChar = c;
      return !oneOrMore || flag;
    }

    private void ParseConstructor()
    {
      if (!this.MatchValue('n', "new", true))
        return;
      char finalChar = this.MoveNext();
      if (!this.EatWhitespace(finalChar, true, out finalChar))
        return;
      for (; char.IsLetter(finalChar); finalChar = this.MoveNext())
        this._buffer.Append(finalChar);
      this.EatWhitespace(finalChar, false, out finalChar);
      if (finalChar != '(')
        throw this.CreateJsonReaderException("Unexpected character while parsing constructor: {0}. Line {1}, position {2}.", (object) finalChar, (object) this._currentLineNumber, (object) this._currentLinePosition);
      string str = this._buffer.ToString();
      this._buffer.Position = 0;
      this.SetToken(JsonToken.StartConstructor, (object) str);
    }

    private void ParseNumber(char firstChar)
    {
      char c = firstChar;
      bool flag1 = false;
      do
      {
        if (this.IsSeperator(c))
        {
          flag1 = true;
          this._lastChar = new char?(c);
        }
        else
          this._buffer.Append(c);
      }
      while (!flag1 && ((c = this.MoveNext()) != char.MinValue || !this._end));
      string s = this._buffer.ToString();
      bool flag2 = firstChar == '0' && !s.StartsWith("0.", StringComparison.OrdinalIgnoreCase);
      object obj;
      JsonToken newToken;
      if (this._readType == JsonTextReader.ReadType.ReadAsDecimal)
      {
        obj = !flag2 ? (object) Decimal.Parse(s, NumberStyles.Number | NumberStyles.AllowExponent, (IFormatProvider) CultureInfo.InvariantCulture) : (object) Convert.ToDecimal(s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(s, 16) : Convert.ToInt64(s, 8));
        newToken = JsonToken.Float;
      }
      else if (flag2)
      {
        obj = (object) (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(s, 16) : Convert.ToInt64(s, 8));
        newToken = JsonToken.Integer;
      }
      else
      {
        if (s.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
        {
          if (s.IndexOf("e", StringComparison.OrdinalIgnoreCase) == -1)
          {
            try
            {
              obj = (object) Convert.ToInt64(s, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch (OverflowException ex)
            {
              throw new JsonReaderException("JSON integer {0} is too large or small for an Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s), (Exception) ex);
            }
            newToken = JsonToken.Integer;
            goto label_15;
          }
        }
        obj = (object) Convert.ToDouble(s, (IFormatProvider) CultureInfo.InvariantCulture);
        newToken = JsonToken.Float;
      }
label_15:
      this._buffer.Position = 0;
      this.SetToken(newToken, obj);
    }

    private void ParseComment()
    {
      if (this.MoveNext() == '*')
      {
        char ch1;
        while ((ch1 = this.MoveNext()) != char.MinValue || !this._end)
        {
          if (ch1 == '*')
          {
            char ch2;
            if ((ch2 = this.MoveNext()) != char.MinValue || !this._end)
            {
              if (ch2 != '/')
              {
                this._buffer.Append('*');
                this._buffer.Append(ch2);
              }
              else
                break;
            }
          }
          else
            this._buffer.Append(ch1);
        }
        this.SetToken(JsonToken.Comment, (object) this._buffer.ToString());
        this._buffer.Position = 0;
      }
      else
        throw this.CreateJsonReaderException("Error parsing comment. Expected: *. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private bool MatchValue(char firstChar, string value)
    {
      char ch = firstChar;
      int index = 0;
      while ((int) ch == (int) value[index])
      {
        ++index;
        if (index >= value.Length || (ch = this.MoveNext()) == char.MinValue && this._end)
          break;
      }
      return index == value.Length;
    }

    private bool MatchValue(char firstChar, string value, bool noTrailingNonSeperatorCharacters)
    {
      bool flag = this.MatchValue(firstChar, value);
      if (!noTrailingNonSeperatorCharacters)
        return flag;
      int num = this.PeekNext();
      char c = num != -1 ? (char) num : char.MinValue;
      return flag && (c == char.MinValue || this.IsSeperator(c));
    }

    private bool IsSeperator(char c)
    {
      switch (c)
      {
        case '\t':
        case '\n':
        case '\r':
        case ' ':
          return true;
        case ')':
          if (this.CurrentState == JsonReader.State.Constructor || this.CurrentState == JsonReader.State.ConstructorStart)
            return true;
          break;
        case ',':
        case ']':
        case '}':
          return true;
        case '/':
          return this.HasNext() && this.PeekNext() == 42;
        default:
          if (char.IsWhiteSpace(c))
            return true;
          break;
      }
      return false;
    }

    private void ParseTrue()
    {
      if (this.MatchValue('t', JsonConvert.True, true))
        this.SetToken(JsonToken.Boolean, (object) true);
      else
        throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private void ParseNull()
    {
      if (this.MatchValue('n', JsonConvert.Null, true))
        this.SetToken(JsonToken.Null);
      else
        throw this.CreateJsonReaderException("Error parsing null value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private void ParseUndefined()
    {
      if (this.MatchValue('u', JsonConvert.Undefined, true))
        this.SetToken(JsonToken.Undefined);
      else
        throw this.CreateJsonReaderException("Error parsing undefined value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private void ParseFalse()
    {
      if (this.MatchValue('f', JsonConvert.False, true))
        this.SetToken(JsonToken.Boolean, (object) false);
      else
        throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private void ParseNumberNegativeInfinity()
    {
      if (this.MatchValue('-', JsonConvert.NegativeInfinity, true))
        this.SetToken(JsonToken.Float, (object) double.NegativeInfinity);
      else
        throw this.CreateJsonReaderException("Error parsing negative infinity value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private void ParseNumberPositiveInfinity()
    {
      if (this.MatchValue('I', JsonConvert.PositiveInfinity, true))
        this.SetToken(JsonToken.Float, (object) double.PositiveInfinity);
      else
        throw this.CreateJsonReaderException("Error parsing positive infinity value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    private void ParseNumberNaN()
    {
      if (this.MatchValue('N', JsonConvert.NaN, true))
        this.SetToken(JsonToken.Float, (object) double.NaN);
      else
        throw this.CreateJsonReaderException("Error parsing NaN value. Line {0}, position {1}.", (object) this._currentLineNumber, (object) this._currentLinePosition);
    }

    public override void Close()
    {
      base.Close();
      if (this.CloseInput && this._reader != null)
        this._reader.Close();
      if (this._buffer == null)
        return;
      this._buffer.Clear();
    }

    public bool HasLineInfo() => true;

    public int LineNumber => this.CurrentState == JsonReader.State.Start ? 0 : this._currentLineNumber;

    public int LinePosition => this._currentLinePosition;

    private enum ReadType
    {
      Read,
      ReadAsBytes,
      ReadAsDecimal,
      ReadAsDateTimeOffset,
    }
  }
}
