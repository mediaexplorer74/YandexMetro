// Decompiled with JetBrains decompiler
// Type: Yandex.LowMemoryHelper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using Microsoft.Phone.Info;
using System;

namespace Yandex
{
  public static class LowMemoryHelper
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
