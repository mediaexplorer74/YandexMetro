// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonString
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonString : BsonValue
  {
    public int ByteCount { get; set; }

    public bool IncludeLength { get; set; }

    public BsonString(object value, bool includeLength)
      : base(value, BsonType.String)
    {
      this.IncludeLength = includeLength;
    }
  }
}
