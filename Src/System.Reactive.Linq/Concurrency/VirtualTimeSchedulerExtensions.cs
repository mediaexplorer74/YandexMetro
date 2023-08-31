// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.VirtualTimeSchedulerExtensions
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
  public static class VirtualTimeSchedulerExtensions
  {
    public static IDisposable ScheduleRelative<TAbsolute, TRelative>(
      this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler,
      TRelative dueTime,
      Action action)
      where TAbsolute : IComparable<TAbsolute>
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.ScheduleRelative<Action>(action, dueTime, new Func<IScheduler, Action, IDisposable>(VirtualTimeSchedulerExtensions.Invoke));
    }

    public static IDisposable ScheduleAbsolute<TAbsolute, TRelative>(
      this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler,
      TAbsolute dueTime,
      Action action)
      where TAbsolute : IComparable<TAbsolute>
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.ScheduleAbsolute<Action>(action, dueTime, new Func<IScheduler, Action, IDisposable>(VirtualTimeSchedulerExtensions.Invoke));
    }

    private static IDisposable Invoke(IScheduler scheduler, Action action)
    {
      action();
      return Disposable.Empty;
    }
  }
}
