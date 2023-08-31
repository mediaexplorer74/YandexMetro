// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DateTimeConverterBase
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Converters
{
  public abstract class DateTimeConverterBase : JsonConverter
  {
    public override bool CanConvert(Type objectType) => (object) objectType == (object) typeof (DateTime) || (object) objectType == (object) typeof (DateTime?) || (object) objectType == (object) typeof (DateTimeOffset) || (object) objectType == (object) typeof (DateTimeOffset?);
  }
}
