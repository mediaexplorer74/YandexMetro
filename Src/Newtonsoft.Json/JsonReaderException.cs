// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonReaderException
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  public class JsonReaderException : Exception
  {
    public int LineNumber { get; private set; }

    public int LinePosition { get; private set; }

    public JsonReaderException()
    {
    }

    public JsonReaderException(string message)
      : base(message)
    {
    }

    public JsonReaderException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal JsonReaderException(
      string message,
      Exception innerException,
      int lineNumber,
      int linePosition)
      : base(message, innerException)
    {
      this.LineNumber = lineNumber;
      this.LinePosition = linePosition;
    }
  }
}
