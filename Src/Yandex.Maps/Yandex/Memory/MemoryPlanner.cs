// Decompiled with JetBrains decompiler
// Type: Yandex.Memory.MemoryPlanner
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;

namespace Yandex.Memory
{
  internal class MemoryPlanner : IMemoryPlanner
  {
    private const double HeavyMemoryPressureRatio = 0.9;
    private const double MediumMemoryPressureRatio = 0.8;
    private static readonly TimeSpan _UpdateInterval = TimeSpan.FromSeconds(3.0);
    private readonly IMemoryUsageMonitor _memoryUsageMonitor;
    private MemoryPressure _currentMemoryPressure;
    private DateTime _nextUpdateTime = DateTime.MinValue;

    public MemoryPlanner([NotNull] IMemoryUsageMonitor memoryUsageMonitor) => this._memoryUsageMonitor = memoryUsageMonitor != null ? memoryUsageMonitor : throw new ArgumentNullException(nameof (memoryUsageMonitor));

    public MemoryPressure CurrentMemoryPressure
    {
      get
      {
        this.UpdateValue();
        return this._currentMemoryPressure;
      }
    }

    private void UpdateValue()
    {
      DateTime utcNow = DateTime.UtcNow;
      if (utcNow < this._nextUpdateTime)
        return;
      this._nextUpdateTime = utcNow + MemoryPlanner._UpdateInterval;
      long currentMemoryUsage = this._memoryUsageMonitor.ApplicationCurrentMemoryUsage;
      long memoryUsageLimit = this._memoryUsageMonitor.ApplicationMemoryUsageLimit;
      long guaranteedMemoryUsage = this._memoryUsageMonitor.ApplicationGuaranteedMemoryUsage;
      if ((double) currentMemoryUsage > 0.9 * (double) memoryUsageLimit)
        this._currentMemoryPressure = MemoryPressure.Heavy;
      else if (currentMemoryUsage > guaranteedMemoryUsage || (double) currentMemoryUsage > 0.8 * (double) memoryUsageLimit)
        this._currentMemoryPressure = MemoryPressure.Medium;
      else
        this._currentMemoryPressure = MemoryPressure.None;
    }
  }
}
