// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.SystemClock
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;
using System.Threading;

namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class SystemClock
  {
    private static Lazy<ISystemClock> s_serviceSystemClock = new Lazy<ISystemClock>(new Func<ISystemClock>(SystemClock.InitializeSystemClock));
    private static Lazy<INotifySystemClockChanged> s_serviceSystemClockChanged = new Lazy<INotifySystemClockChanged>(new Func<INotifySystemClockChanged>(SystemClock.InitializeSystemClockChanged));
    private static int _refCount;

    public static DateTimeOffset UtcNow => SystemClock.s_serviceSystemClock.Value.UtcNow;

    public static event EventHandler<SystemClockChangedEventArgs> SystemClockChanged;

    public static void AddRef()
    {
      if (Interlocked.Increment(ref SystemClock._refCount) != 1)
        return;
      SystemClock.s_serviceSystemClockChanged.Value.SystemClockChanged += new EventHandler<SystemClockChangedEventArgs>(SystemClock.OnSystemClockChanged);
    }

    public static void Release()
    {
      if (Interlocked.Decrement(ref SystemClock._refCount) != 0)
        return;
      SystemClock.s_serviceSystemClockChanged.Value.SystemClockChanged -= new EventHandler<SystemClockChangedEventArgs>(SystemClock.OnSystemClockChanged);
    }

    private static void OnSystemClockChanged(object sender, SystemClockChangedEventArgs e)
    {
      EventHandler<SystemClockChangedEventArgs> systemClockChanged = SystemClock.SystemClockChanged;
      if (systemClockChanged == null)
        return;
      systemClockChanged(sender, e);
    }

    private static ISystemClock InitializeSystemClock() => PlatformEnlightenmentProvider.Current.GetService<ISystemClock>() ?? (ISystemClock) new DefaultSystemClock();

    private static INotifySystemClockChanged InitializeSystemClockChanged() => PlatformEnlightenmentProvider.Current.GetService<INotifySystemClockChanged>() ?? (INotifySystemClockChanged) new DefaultSystemClockMonitor();
  }
}
