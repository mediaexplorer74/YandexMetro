// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.HostLifecycleNotifications
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.Reactive.PlatformServices.Phone.Shell;

namespace System.Reactive.PlatformServices
{
  internal class HostLifecycleNotifications : IHostLifecycleNotifications
  {
    private EventHandler<ActivatedEventArgs> _activated;
    private EventHandler<DeactivatedEventArgs> _deactivated;

    public event EventHandler<HostSuspendingEventArgs> Suspending
    {
      add
      {
        this._deactivated = (EventHandler<DeactivatedEventArgs>) ((o, e) => value(o, new HostSuspendingEventArgs()));
        PhoneApplicationService current = PhoneApplicationService.Current;
        if (current == null)
          return;
        current.Deactivated += this._deactivated;
      }
      remove
      {
        PhoneApplicationService current = PhoneApplicationService.Current;
        if (current == null)
          return;
        current.Deactivated -= this._deactivated;
      }
    }

    public event EventHandler<HostResumingEventArgs> Resuming
    {
      add
      {
        this._activated = (EventHandler<ActivatedEventArgs>) ((o, e) =>
        {
          if (!e.IsApplicationInstancePreserved)
            return;
          value(o, new HostResumingEventArgs());
        });
        PhoneApplicationService current = PhoneApplicationService.Current;
        if (current == null)
          return;
        current.Activated += this._activated;
      }
      remove
      {
        PhoneApplicationService current = PhoneApplicationService.Current;
        if (current == null)
          return;
        current.Activated -= this._activated;
      }
    }
  }
}
