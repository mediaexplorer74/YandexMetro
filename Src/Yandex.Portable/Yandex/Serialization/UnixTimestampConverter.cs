// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.UnixTimestampConverter
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.Serialization
{
  public class UnixTimestampConverter
  {
    public static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, 0);

    public static DateTime GetDateTime(uint unixTimestamp) => UnixTimestampConverter.Epoch.AddSeconds((double) unixTimestamp);

    public static uint GetUnixTime(DateTime dateTime) => (uint) (dateTime.ToUniversalTime() - UnixTimestampConverter.Epoch).TotalSeconds;
  }
}
