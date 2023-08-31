// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerDefaults
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Concurrency
{
  internal static class SchedulerDefaults
  {
    internal static IScheduler ConstantTimeOperations => (IScheduler) ImmediateScheduler.Instance;

    internal static IScheduler TailRecursion => (IScheduler) ImmediateScheduler.Instance;

    internal static IScheduler Iteration => (IScheduler) CurrentThreadScheduler.Instance;

    internal static IScheduler TimeBasedOperations => (IScheduler) DefaultScheduler.Instance;

    internal static IScheduler AsyncConversions => (IScheduler) DefaultScheduler.Instance;
  }
}
