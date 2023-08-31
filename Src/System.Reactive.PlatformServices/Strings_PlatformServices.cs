// Decompiled with JetBrains decompiler
// Type: System.Reactive.Strings_PlatformServices
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Reactive
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Strings_PlatformServices
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings_PlatformServices()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Strings_PlatformServices.resourceMan, (object) null))
          Strings_PlatformServices.resourceMan = new ResourceManager("System.Reactive.Strings_PlatformServices", typeof (Strings_PlatformServices).Assembly);
        return Strings_PlatformServices.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings_PlatformServices.resourceCulture;
      set => Strings_PlatformServices.resourceCulture = value;
    }

    internal static string WINRT_NO_SUB1MS_TIMERS => Strings_PlatformServices.ResourceManager.GetString(nameof (WINRT_NO_SUB1MS_TIMERS), Strings_PlatformServices.resourceCulture);
  }
}
