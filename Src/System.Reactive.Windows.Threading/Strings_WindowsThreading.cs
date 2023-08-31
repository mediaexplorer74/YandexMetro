// Decompiled with JetBrains decompiler
// Type: System.Reactive.Strings_WindowsThreading
// Assembly: System.Reactive.Windows.Threading, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: D0078FA9-FF17-4C3C-823C-96AD815797E4
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Windows.Threading.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Reactive
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  internal class Strings_WindowsThreading
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings_WindowsThreading()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Strings_WindowsThreading.resourceMan, (object) null))
          Strings_WindowsThreading.resourceMan = new ResourceManager("System.Reactive.Strings_WindowsThreading", typeof (Strings_WindowsThreading).Assembly);
        return Strings_WindowsThreading.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings_WindowsThreading.resourceCulture;
      set => Strings_WindowsThreading.resourceCulture = value;
    }

    internal static string NO_DISPATCHER_CURRENT_THREAD => Strings_WindowsThreading.ResourceManager.GetString(nameof (NO_DISPATCHER_CURRENT_THREAD), Strings_WindowsThreading.resourceCulture);

    internal static string NO_WINDOW_CURRENT => Strings_WindowsThreading.ResourceManager.GetString(nameof (NO_WINDOW_CURRENT), Strings_WindowsThreading.resourceCulture);
  }
}
