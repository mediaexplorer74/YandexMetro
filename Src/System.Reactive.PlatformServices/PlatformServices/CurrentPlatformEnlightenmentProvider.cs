// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.CurrentPlatformEnlightenmentProvider
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;

namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class CurrentPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
  {
    public virtual T GetService<T>(object[] args) where T : class
    {
      Type type1 = typeof (T);
      if ((object) type1 == (object) typeof (IConcurrencyAbstractionLayer))
        return (T) new ConcurrencyAbstractionLayerImpl();
      if ((object) type1 == (object) typeof (IScheduler) && args != null)
      {
        switch ((string) args[0])
        {
          case "ThreadPool":
            return (T) ThreadPoolScheduler.Instance;
          case "NewThread":
            return (T) NewThreadScheduler.Default;
        }
      }
      if ((object) type1 == (object) typeof (IHostLifecycleNotifications))
        return (T) new HostLifecycleNotifications();
      if ((object) type1 == (object) typeof (IQueryServices) && Debugger.IsAttached)
      {
        Type type2 = Type.GetType("System.Reactive.Linq.QueryDebugger, " + new AssemblyName(type1.Assembly.FullName)
        {
          Name = "System.Reactive.Debugger"
        }.FullName, false);
        if ((object) type2 != null)
          return (T) Activator.CreateInstance(type2);
      }
      return default (T);
    }
  }
}
