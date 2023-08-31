// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.HostLifecycleService
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;
using System.Threading;

namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HostLifecycleService
  {
    private static Lazy<IHostLifecycleNotifications> s_notifications = new Lazy<IHostLifecycleNotifications>(new Func<IHostLifecycleNotifications>(HostLifecycleService.InitializeNotifications));
    private static int _refCount;

    public static event EventHandler<HostSuspendingEventArgs> Suspending;

    public static event EventHandler<HostResumingEventArgs> Resuming;

    public static void AddRef()
    {
      if (Interlocked.Increment(ref HostLifecycleService._refCount) != 1)
        return;
      IHostLifecycleNotifications lifecycleNotifications = HostLifecycleService.s_notifications.Value;
      if (lifecycleNotifications == null)
        return;
      lifecycleNotifications.Suspending += new EventHandler<HostSuspendingEventArgs>(HostLifecycleService.OnSuspending);
      lifecycleNotifications.Resuming += new EventHandler<HostResumingEventArgs>(HostLifecycleService.OnResuming);
    }

    public static void Release()
    {
      if (Interlocked.Decrement(ref HostLifecycleService._refCount) != 0)
        return;
      IHostLifecycleNotifications lifecycleNotifications = HostLifecycleService.s_notifications.Value;
      if (lifecycleNotifications == null)
        return;
      lifecycleNotifications.Suspending -= new EventHandler<HostSuspendingEventArgs>(HostLifecycleService.OnSuspending);
      lifecycleNotifications.Resuming -= new EventHandler<HostResumingEventArgs>(HostLifecycleService.OnResuming);
    }

    private static void OnSuspending(object sender, HostSuspendingEventArgs e)
    {
      EventHandler<HostSuspendingEventArgs> suspending = HostLifecycleService.Suspending;
      if (suspending == null)
        return;
      suspending(sender, e);
    }

    private static void OnResuming(object sender, HostResumingEventArgs e)
    {
      EventHandler<HostResumingEventArgs> resuming = HostLifecycleService.Resuming;
      if (resuming == null)
        return;
      resuming(sender, e);
    }

    private static IHostLifecycleNotifications InitializeNotifications() => PlatformEnlightenmentProvider.Current.GetService<IHostLifecycleNotifications>();
  }
}
