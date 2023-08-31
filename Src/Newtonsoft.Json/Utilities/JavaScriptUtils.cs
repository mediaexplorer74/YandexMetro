// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.JavaScriptUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System.IO;

namespace Newtonsoft.Json.Utilities
{
  internal static class JavaScriptUtils
  {
    public static void WriteEscapedJavaScriptString(
      TextWriter writer,
      string value,
      char delimiter,
      bool appendDelimiters)
    {
      if (appendDelimiters)
        writer.Write(delimiter);
      if (value != null)
      {
        int index1 = 0;
        int count = 0;
        char[] buffer = (char[]) null;
        for (int index2 = 0; index2 < value.Length; ++index2)
        {
          char c = value[index2];
          string str;
          switch (c)
          {
            case '\b':
              str = "\\b";
              break;
            case '\t':
              str = "\\t";
              break;
            case '\n':
              str = "\\n";
              break;
            case '\f':
              str = "\\f";
              break;
            case '\r':
              str = "\\r";
              break;
            case '"':
              str = delimiter == '"' ? "\\\"" : (string) null;
              break;
            case '\'':
              str = delimiter == '\'' ? "\\'" : (string) null;
              break;
            case '\\':
              str = "\\\\";
              break;
            case '\u0085':
              str = "\\u0085";
              break;
            case '\u2028':
              str = "\\u2028";
              break;
            case '\u2029':
              str = "\\u2029";
              break;
            default:
              str = c <= '\u001F' ? StringUtils.ToCharAsUnicode(c) : (string) null;
              break;
          }
          if (str != null)
          {
            if (buffer == null)
              buffer = value.ToCharArray();
            if (count > 0)
            {
              writer.Write(buffer, index1, count);
              count = 0;
            }
            writer.Write(str);
            index1 = index2 + 1;
          }
          else
            ++count;
        }
        if (count > 0)
        {
          if (index1 == 0)
            writer.Write(value);
          else
            writer.Write(buffer, index1, count);
        }
      }
      if (!appendDelimiters)
        return;
      writer.Write(delimiter);
    }

    public static string ToEscapedJavaScriptString(string value) => JavaScriptUtils.ToEscapedJavaScriptString(value, '"', true);

    public static string ToEscapedJavaScriptString(
      string value,
      char delimiter,
      bool appendDelimiters)
    {
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(StringUtils.GetLength(value) ?? 16))
      {
        JavaScriptUtils.WriteEscapedJavaScriptString((TextWriter) stringWriter, value, delimiter, appendDelimiters);
        return stringWriter.ToString();
      }
    }
  }
}
