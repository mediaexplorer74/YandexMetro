// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.PlatformEnlightenmentProvider
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reflection;

namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class PlatformEnlightenmentProvider
  {
    private static readonly object s_gate = new object();
    private static IPlatformEnlightenmentProvider s_current;

    public static IPlatformEnlightenmentProvider Current
    {
      get
      {
        if (PlatformEnlightenmentProvider.s_current == null)
        {
          lock (PlatformEnlightenmentProvider.s_gate)
          {
            if (PlatformEnlightenmentProvider.s_current == null)
            {
              Type type = Type.GetType("System.Reactive.PlatformServices.CurrentPlatformEnlightenmentProvider, " + new AssemblyName(typeof (IPlatformEnlightenmentProvider).Assembly.FullName)
              {
                Name = "System.Reactive.PlatformServices"
              }.FullName, false);
              PlatformEnlightenmentProvider.s_current = (object) type == null ? (IPlatformEnlightenmentProvider) new DefaultPlatformEnlightenmentProvider() : (IPlatformEnlightenmentProvider) Activator.CreateInstance(type);
            }
          }
        }
        return PlatformEnlightenmentProvider.s_current;
      }
      set
      {
        lock (PlatformEnlightenmentProvider.s_gate)
          PlatformEnlightenmentProvider.s_current = value;
      }
    }
  }
}
