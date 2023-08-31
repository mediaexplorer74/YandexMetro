// Decompiled with JetBrains decompiler
// Type: System.Reactive.Strings_Core
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

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
  internal class Strings_Core
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings_Core()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Strings_Core.resourceMan, (object) null))
          Strings_Core.resourceMan = new ResourceManager("System.Reactive.Strings_Core", typeof (Strings_Core).Assembly);
        return Strings_Core.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings_Core.resourceCulture;
      set => Strings_Core.resourceCulture = value;
    }

    internal static string CANT_OBTAIN_SCHEDULER => Strings_Core.ResourceManager.GetString(nameof (CANT_OBTAIN_SCHEDULER), Strings_Core.resourceCulture);

    internal static string COMPLETED_NO_VALUE => Strings_Core.ResourceManager.GetString(nameof (COMPLETED_NO_VALUE), Strings_Core.resourceCulture);

    internal static string DISPOSABLE_ALREADY_ASSIGNED => Strings_Core.ResourceManager.GetString(nameof (DISPOSABLE_ALREADY_ASSIGNED), Strings_Core.resourceCulture);

    internal static string FAILED_CLOCK_MONITORING => Strings_Core.ResourceManager.GetString(nameof (FAILED_CLOCK_MONITORING), Strings_Core.resourceCulture);

    internal static string HEAP_EMPTY => Strings_Core.ResourceManager.GetString(nameof (HEAP_EMPTY), Strings_Core.resourceCulture);

    internal static string REENTRANCY_DETECTED => Strings_Core.ResourceManager.GetString(nameof (REENTRANCY_DETECTED), Strings_Core.resourceCulture);

    internal static string OBSERVER_TERMINATED => Strings_Core.ResourceManager.GetString(nameof (OBSERVER_TERMINATED), Strings_Core.resourceCulture);

    internal static string SCHEDULER_OPERATION_ALREADY_AWAITED => Strings_Core.ResourceManager.GetString(nameof (SCHEDULER_OPERATION_ALREADY_AWAITED), Strings_Core.resourceCulture);
  }
}
