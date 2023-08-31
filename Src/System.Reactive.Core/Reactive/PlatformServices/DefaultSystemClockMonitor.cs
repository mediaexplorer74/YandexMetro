// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.DefaultSystemClockMonitor
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.PlatformServices
{
  internal class DefaultSystemClockMonitor : PeriodicTimerSystemClockMonitor
  {
    private static readonly TimeSpan DEFAULT_PERIOD = TimeSpan.FromSeconds(1.0);

    public DefaultSystemClockMonitor()
      : base(DefaultSystemClockMonitor.DEFAULT_PERIOD)
    {
    }
  }
}
