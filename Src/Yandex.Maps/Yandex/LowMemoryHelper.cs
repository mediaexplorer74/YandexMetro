// Decompiled with JetBrains decompiler
// Type: Yandex.LowMemoryHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Info;
using System;

namespace Yandex
{
  internal static class LowMemoryHelper
  {
    public static bool IsLowMemDevice { get; set; }

    static LowMemoryHelper()
    {
      try
      {
        LowMemoryHelper.IsLowMemDevice = (long) DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit") < 94371840L;
      }
      catch (ArgumentOutOfRangeException ex)
      {
        LowMemoryHelper.IsLowMemDevice = false;
      }
    }
  }
}
