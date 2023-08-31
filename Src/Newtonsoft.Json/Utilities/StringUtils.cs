// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
  internal static class StringUtils
  {
    public const string CarriageReturnLineFeed = "\r\n";
    public const string Empty = "";
    public const char CarriageReturn = '\r';
    public const char LineFeed = '\n';
    public const char Tab = '\t';

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) format, nameof (format));
      return string.Format(provider, format, args);
    }

    public static bool ContainsWhiteSpace(string s)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      for (int index = 0; index < s.Length; ++index)
      {
        if (char.IsWhiteSpace(s[index]))
          return true;
      }
      return false;
    }

    public static bool IsWhiteSpace(string s)
    {
      switch (s)
      {
        case null:
          throw new ArgumentNullException(nameof (s));
        case "":
          return false;
        default:
          for (int index = 0; index < s.Length; ++index)
          {
            if (!char.IsWhiteSpace(s[index]))
              return false;
          }
          return true;
      }
    }

    public static string EnsureEndsWith(string target, string value)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (target.Length >= value.Length)
      {
        if (string.Compare(target, target.Length - value.Length, value, 0, value.Length, StringComparison.OrdinalIgnoreCase) == 0)
          return target;
        string strA = target.TrimEnd((char[]) null);
        if (string.Compare(strA, strA.Length - value.Length, value, 0, value.Length, StringComparison.OrdinalIgnoreCase) == 0)
          return target;
      }
      return target + value;
    }

    public static bool IsNullOrEmptyOrWhiteSpace(string s) => string.IsNullOrEmpty(s) || StringUtils.IsWhiteSpace(s);

    public static void IfNotNullOrEmpty(string value, Action<string> action) => StringUtils.IfNotNullOrEmpty(value, action, (Action<string>) null);

    private static void IfNotNullOrEmpty(
      string value,
      Action<string> trueAction,
      Action<string> falseAction)
    {
      if (!string.IsNullOrEmpty(value))
      {
        if (trueAction == null)
          return;
        trueAction(value);
      }
      else
      {
        if (falseAction == null)
          return;
        falseAction(value);
      }
    }

    public static string Indent(string s, int indentation) => StringUtils.Indent(s, indentation, ' ');

    public static string Indent(string s, int indentation, char indentChar)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      if (indentation <= 0)
        throw new ArgumentException("Must be greater than zero.", nameof (indentation));
      StringReader stringReader = new StringReader(s);
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      StringUtils.ActionTextReaderLine((TextReader) stringReader, (TextWriter) stringWriter, (StringUtils.ActionLine) ((tw, line) =>
      {
        tw.Write(new string(indentChar, indentation));
        tw.Write(line);
      }));
      return stringWriter.ToString();
    }

    private static void ActionTextReaderLine(
      TextReader textReader,
      TextWriter textWriter,
      StringUtils.ActionLine lineAction)
    {
      bool flag = true;
      string line;
      while ((line = textReader.ReadLine()) != null)
      {
        if (!flag)
          textWriter.WriteLine();
        else
          flag = false;
        lineAction(textWriter, line);
      }
    }

    public static string NumberLines(string s)
    {
      StringReader stringReader = s != null ? new StringReader(s) : throw new ArgumentNullException(nameof (s));
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      int lineNumber = 1;
      StringUtils.ActionTextReaderLine((TextReader) stringReader, (TextWriter) stringWriter, (StringUtils.ActionLine) ((tw, line) =>
      {
        tw.Write(lineNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture).PadLeft(4));
        tw.Write(". ");
        tw.Write(line);
        ++lineNumber;
      }));
      return stringWriter.ToString();
    }

    public static string NullEmptyString(string s) => !string.IsNullOrEmpty(s) ? s : (string) null;

    public static string ReplaceNewLines(string s, string replacement)
    {
      StringReader stringReader = new StringReader(s);
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      string str;
      while ((str = stringReader.ReadLine()) != null)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(replacement);
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }

    public static string Truncate(string s, int maximumLength) => StringUtils.Truncate(s, maximumLength, "...");

    public static string Truncate(string s, int maximumLength, string suffix)
    {
      if (suffix == null)
        throw new ArgumentNullException(nameof (suffix));
      if (maximumLength <= 0)
        throw new ArgumentException("Maximum length must be greater than zero.", nameof (maximumLength));
      int length = maximumLength - suffix.Length;
      if (length <= 0)
        throw new ArgumentException("Length of suffix string is greater or equal to maximumLength");
      return s != null && s.Length > maximumLength ? s.Substring(0, length).Trim() + suffix : s;
    }

    public static StringWriter CreateStringWriter(int capacity) => new StringWriter(new StringBuilder(capacity), (IFormatProvider) CultureInfo.InvariantCulture);

    public static int? GetLength(string value) => value?.Length;

    public static string ToCharAsUnicode(char c) => new string(new char[6]
    {
      '\\',
      'u',
      MathUtils.IntToHex((int) c >> 12 & 15),
      MathUtils.IntToHex((int) c >> 8 & 15),
      MathUtils.IntToHex((int) c >> 4 & 15),
      MathUtils.IntToHex((int) c & 15)
    });

    public static void WriteCharAsUnicode(TextWriter writer, char c)
    {
      ValidationUtils.ArgumentNotNull((object) writer, nameof (writer));
      char hex1 = MathUtils.IntToHex((int) c >> 12 & 15);
      char hex2 = MathUtils.IntToHex((int) c >> 8 & 15);
      char hex3 = MathUtils.IntToHex((int) c >> 4 & 15);
      char hex4 = MathUtils.IntToHex((int) c & 15);
      writer.Write('\\');
      writer.Write('u');
      writer.Write(hex1);
      writer.Write(hex2);
      writer.Write(hex3);
      writer.Write(hex4);
    }

    public static TSource ForgivingCaseSensitiveFind<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, string> valueSelector,
      string testValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (valueSelector == null)
        throw new ArgumentNullException(nameof (valueSelector));
      IEnumerable<TSource> source1 = source.Where<TSource>((Func<TSource, bool>) (s => string.Compare(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase) == 0));
      return source1.Count<TSource>() <= 1 ? source1.SingleOrDefault<TSource>() : source.Where<TSource>((Func<TSource, bool>) (s => string.Compare(valueSelector(s), testValue, StringComparison.Ordinal) == 0)).SingleOrDefault<TSource>();
    }

    public static string ToCamelCase(string s)
    {
      if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
        return s;
      string camelCase = char.ToLower(s[0], CultureInfo.InvariantCulture).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      if (s.Length > 1)
        camelCase += s.Substring(1);
      return camelCase;
    }

    private delegate void ActionLine(TextWriter textWriter, string line);
  }
}
