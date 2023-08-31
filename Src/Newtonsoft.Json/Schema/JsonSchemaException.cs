// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaException
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Schema
{
  public class JsonSchemaException : Exception
  {
    public int LineNumber { get; private set; }

    public int LinePosition { get; private set; }

    public JsonSchemaException()
    {
    }

    public JsonSchemaException(string message)
      : base(message)
    {
    }

    public JsonSchemaException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal JsonSchemaException(
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
