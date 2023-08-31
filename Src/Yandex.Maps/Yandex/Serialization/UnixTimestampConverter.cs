// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.UnixTimestampConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Serialization
{
  internal class UnixTimestampConverter
  {
    public static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, 0);

    public static DateTime GetDateTime(uint unixTimestamp) => UnixTimestampConverter.Epoch.AddSeconds((double) unixTimestamp);

    public static uint GetUnixTime(DateTime dateTime) => (uint) (dateTime.ToUniversalTime() - UnixTimestampConverter.Epoch).TotalSeconds;
  }
}
