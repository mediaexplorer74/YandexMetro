// Decompiled with JetBrains decompiler
// Type: Yandex.Memory.MemoryUsageMonitor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Info;

namespace Yandex.Memory
{
  internal class MemoryUsageMonitor : IMemoryUsageMonitor
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
