// Decompiled with JetBrains decompiler
// Type: Yandex.Memory.MemoryUsageMonitor
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using Microsoft.Phone.Info;

namespace Yandex.Memory
{
  public class MemoryUsageMonitor : IMemoryUsageMonitor
  {
    public long ApplicationCurrentMemoryUsage => DeviceStatus.ApplicationCurrentMemoryUsage;

    public long ApplicationMemoryUsageLimit
    {
      get
      {
        long memoryUsageLimit = DeviceStatus.ApplicationMemoryUsageLimit;
        return memoryUsageLimit == 0L ? this.ApplicationGuaranteedMemoryUsage : memoryUsageLimit;
      }
    }

    public long ApplicationPeakMemoryUsage => DeviceStatus.ApplicationPeakMemoryUsage;

    public long DeviceTotalMemory => DeviceStatus.DeviceTotalMemory;

    public long ApplicationGuaranteedMemoryUsage => 94371840;
  }
}
