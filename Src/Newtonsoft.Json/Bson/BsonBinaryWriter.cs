// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonBinaryWriter
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Newtonsoft.Json.Bson
{
  internal class BsonBinaryWriter
  {
    private static readonly Encoding Encoding = Encoding.UTF8;
    private readonly BinaryWriter _writer;
    private byte[] _largeByteBuffer;
    private int _maxChars;

    public DateTimeKind DateTimeKindHandling { get; set; }

    public BsonBinaryWriter(Stream stream)
    {
      this.DateTimeKindHandling = DateTimeKind.Utc;
      this._writer = new BinaryWriter(stream);
    }

    public void Flush() => this._writer.Flush();

    public void Close() => this._writer.Close();

    public void WriteToken(BsonToken t)
    {
      this.CalculateSize(t);
      this.WriteTokenInternal(t);
    }

    private void WriteTokenInternal(BsonToken t)
    {
      switch (t.Type)
      {
        case BsonType.Number:
          this._writer.Write(Convert.ToDouble(((BsonValue) t).Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case BsonType.String:
          BsonString bsonString = (BsonString) t;
          this.WriteString((string) bsonString.Value, bsonString.ByteCount, new int?(bsonString.CalculatedSize - 4));
          break;
        case BsonType.Object:
          BsonObject bsonObject = (BsonObject) t;
          this._writer.Write(bsonObject.CalculatedSize);
          foreach (BsonProperty bsonProperty in bsonObject)
          {
            this._writer.Write((sbyte) bsonProperty.Value.Type);
            this.WriteString((string) bsonProperty.Name.Value, bsonProperty.Name.ByteCount, new int?());
            this.WriteTokenInternal(bsonProperty.Value);
          }
          this._writer.Write((byte) 0);
          break;
        case BsonType.Array:
          BsonArray bsonArray = (BsonArray) t;
          this._writer.Write(bsonArray.CalculatedSize);
          int i = 0;
          foreach (BsonToken t1 in bsonArray)
          {
            this._writer.Write((sbyte) t1.Type);
            this.WriteString(i.ToString((IFormatProvider) CultureInfo.InvariantCulture), MathUtils.IntLength(i), new int?());
            this.WriteTokenInternal(t1);
            ++i;
          }
          this._writer.Write((byte) 0);
          break;
        case BsonType.Binary:
          byte[] buffer = (byte[]) ((BsonValue) t).Value;
          this._writer.Write(buffer.Length);
          this._writer.Write((byte) 0);
          this._writer.Write(buffer);
          break;
        case BsonType.Undefined:
          break;
        case BsonType.Oid:
          this._writer.Write((byte[]) ((BsonValue) t).Value);
          break;
        case BsonType.Boolean:
          this._writer.Write((bool) ((BsonValue) t).Value);
          break;
        case BsonType.Date:
          BsonValue bsonValue = (BsonValue) t;
          long javaScriptTicks;
          if (bsonValue.Value is DateTime)
          {
            DateTime dateTime = (DateTime) bsonValue.Value;
            if (this.DateTimeKindHandling == DateTimeKind.Utc)
              dateTime = dateTime.ToUniversalTime();
            else if (this.DateTimeKindHandling == DateTimeKind.Local)
              dateTime = dateTime.ToLocalTime();
            javaScriptTicks = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, false);
          }
          else
          {
            DateTimeOffset dateTimeOffset = (DateTimeOffset) bsonValue.Value;
            javaScriptTicks = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.UtcDateTime, dateTimeOffset.Offset);
          }
          this._writer.Write(javaScriptTicks);
          break;
        case BsonType.Null:
          break;
        case BsonType.Regex:
          BsonRegex bsonRegex = (BsonRegex) t;
          this.WriteString((string) bsonRegex.Pattern.Value, bsonRegex.Pattern.ByteCount, new int?());
          this.WriteString((string) bsonRegex.Options.Value, bsonRegex.Options.ByteCount, new int?());
          break;
        case BsonType.Integer:
          this._writer.Write(Convert.ToInt32(((BsonValue) t).Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case BsonType.Long:
          this._writer.Write(Convert.ToInt64(((BsonValue) t).Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (t), "Unexpected token when writing BSON: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) t.Type));
      }
    }

    private void WriteString(string s, int byteCount, int? calculatedlengthPrefix)
    {
      if (calculatedlengthPrefix.HasValue)
        this._writer.Write(calculatedlengthPrefix.Value);
      if (s != null)
      {
        if (this._largeByteBuffer == null)
        {
          this._largeByteBuffer = new byte[256];
          this._maxChars = 256 / BsonBinaryWriter.Encoding.GetMaxByteCount(1);
        }
        if (byteCount <= 256)
        {
          BsonBinaryWriter.Encoding.GetBytes(s, 0, s.Length, this._largeByteBuffer, 0);
          this._writer.Write(this._largeByteBuffer, 0, byteCount);
        }
        else
        {
          int charIndex = 0;
          int charCount;
          for (int length = s.Length; length > 0; length -= charCount)
          {
            charCount = length > this._maxChars ? this._maxChars : length;
            this._writer.Write(this._largeByteBuffer, 0, BsonBinaryWriter.Encoding.GetBytes(s, charIndex, charCount, this._largeByteBuffer, 0));
            charIndex += charCount;
          }
        }
      }
      this._writer.Write((byte) 0);
    }

    private int CalculateSize(int stringByteCount) => stringByteCount + 1;

    private int CalculateSizeWithLength(int stringByteCount, bool includeSize) => (includeSize ? 5 : 1) + stringByteCount;

    private int CalculateSize(BsonToken t)
    {
      switch (t.Type)
      {
        case BsonType.Number:
          return 8;
        case BsonType.String:
          BsonString bsonString = (BsonString) t;
          string s = (string) bsonString.Value;
          bsonString.ByteCount = s != null ? BsonBinaryWriter.Encoding.GetByteCount(s) : 0;
          bsonString.CalculatedSize = this.CalculateSizeWithLength(bsonString.ByteCount, bsonString.IncludeLength);
          return bsonString.CalculatedSize;
        case BsonType.Object:
          BsonObject bsonObject = (BsonObject) t;
          int num1 = 4;
          foreach (BsonProperty bsonProperty in bsonObject)
          {
            int num2 = 1 + this.CalculateSize((BsonToken) bsonProperty.Name) + this.CalculateSize(bsonProperty.Value);
            num1 += num2;
          }
          int size = num1 + 1;
          bsonObject.CalculatedSize = size;
          return size;
        case BsonType.Array:
          BsonArray bsonArray = (BsonArray) t;
          int num3 = 4;
          int i = 0;
          foreach (BsonToken t1 in bsonArray)
          {
            ++num3;
            num3 += this.CalculateSize(MathUtils.IntLength(i));
            num3 += this.CalculateSize(t1);
            ++i;
          }
          int num4 = num3 + 1;
          bsonArray.CalculatedSize = num4;
          return bsonArray.CalculatedSize;
        case BsonType.Binary:
          BsonValue bsonValue = (BsonValue) t;
          byte[] numArray = (byte[]) bsonValue.Value;
          bsonValue.CalculatedSize = 5 + numArray.Length;
          return bsonValue.CalculatedSize;
        case BsonType.Undefined:
        case BsonType.Null:
          return 0;
        case BsonType.Oid:
          return 12;
        case BsonType.Boolean:
          return 1;
        case BsonType.Date:
          return 8;
        case BsonType.Regex:
          BsonRegex bsonRegex = (BsonRegex) t;
          int num5 = 0 + this.CalculateSize((BsonToken) bsonRegex.Pattern) + this.CalculateSize((BsonToken) bsonRegex.Options);
          bsonRegex.CalculatedSize = num5;
          return bsonRegex.CalculatedSize;
        case BsonType.Integer:
          return 4;
        case BsonType.Long:
          return 8;
        default:
          throw new ArgumentOutOfRangeException(nameof (t), "Unexpected token when writing BSON: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) t.Type));
      }
    }
  }
}
