// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DisableOptimizationsScheduler
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Concurrency
{
  internal class DisableOptimizationsScheduler : SchedulerWrapper
  {
    private readonly Type[] _optimizationInterfaces;

    public DisableOptimizationsScheduler(IScheduler scheduler)
      : base(scheduler)
    {
      this._optimizationInterfaces = Scheduler.OPTIMIZATIONS;
    }

    public DisableOptimizationsScheduler(IScheduler scheduler, Type[] optimizationInterfaces)
      : base(scheduler)
    {
      this._optimizationInterfaces = optimizationInterfaces;
    }

    protected override SchedulerWrapper Clone(IScheduler scheduler) => (SchedulerWrapper) new DisableOptimizationsScheduler(scheduler, this._optimizationInterfaces);

    protected override bool TryGetService(
      IServiceProvider provider,
      Type serviceType,
      out object service)
    {
      service = (object) null;
      return ((IEnumerable<Type>) this._optimizationInterfaces).Contains<Type>(serviceType);
    }
  }
}
