// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Interfaces.IStartupQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Maps.PrinterClient.Interfaces
{
  internal interface IStartupQueryBuilder
  {
    string GetStartupQuery(
      Version app_version,
      string app_platform,
      double screen_w,
      double screen_h,
      string uuid,
      string manufacturer,
      string model,
      string platformtype,
      string os_version,
      bool utf);
  }
}
