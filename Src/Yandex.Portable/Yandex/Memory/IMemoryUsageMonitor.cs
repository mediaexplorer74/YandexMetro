﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Memory.IMemoryUsageMonitor
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.Memory
{
  public interface IMemoryUsageMonitor
  {
    long ApplicationCurrentMemoryUsage { get; }

    long ApplicationMemoryUsageLimit { get; }

    long ApplicationPeakMemoryUsage { get; }

    long ApplicationGuaranteedMemoryUsage { get; }

    long DeviceTotalMemory { get; }
  }
}
