// Decompiled with JetBrains decompiler
// Type: System.Reactive.Constants_Core
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  internal class Constants_Core
  {
    private const string OBSOLETE_REFACTORING = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies.";
    public const string OBSOLETE_SCHEDULER_NEWTHREAD = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use NewThreadScheduler.Default to obtain an instance of this scheduler type. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
    public const string OBSOLETE_SCHEDULER_TASKPOOL = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use TaskPoolScheduler.Default to obtain an instance of this scheduler type. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
    public const string OBSOLETE_SCHEDULER_THREADPOOL = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Consider using Scheduler.Default to obtain the platform's most appropriate pool-based scheduler. In order to access a specific pool-based scheduler, please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use the appropriate scheduler in the System.Reactive.Concurrency namespace. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
    public const string OBSOLETE_SCHEDULEREQUIRED = "This instance property is no longer supported. Use CurrentThreadScheduler.IsScheduleRequired instead. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
  }
}
