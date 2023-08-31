// Decompiled with JetBrains decompiler
// Type: System.Reactive.Strings_Linq
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

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
  internal class Strings_Linq
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings_Linq()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Strings_Linq.resourceMan, (object) null))
          Strings_Linq.resourceMan = new ResourceManager("System.Reactive.Strings_Linq", typeof (Strings_Linq).Assembly);
        return Strings_Linq.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings_Linq.resourceCulture;
      set => Strings_Linq.resourceCulture = value;
    }

    internal static string COULD_NOT_FIND_INSTANCE_EVENT => Strings_Linq.ResourceManager.GetString(nameof (COULD_NOT_FIND_INSTANCE_EVENT), Strings_Linq.resourceCulture);

    internal static string COULD_NOT_FIND_STATIC_EVENT => Strings_Linq.ResourceManager.GetString(nameof (COULD_NOT_FIND_STATIC_EVENT), Strings_Linq.resourceCulture);

    internal static string EVENT_ADD_METHOD_SHOULD_TAKE_ONE_PARAMETER => Strings_Linq.ResourceManager.GetString(nameof (EVENT_ADD_METHOD_SHOULD_TAKE_ONE_PARAMETER), Strings_Linq.resourceCulture);

    internal static string EVENT_ARGS_NOT_ASSIGNABLE => Strings_Linq.ResourceManager.GetString(nameof (EVENT_ARGS_NOT_ASSIGNABLE), Strings_Linq.resourceCulture);

    internal static string EVENT_MISSING_ADD_METHOD => Strings_Linq.ResourceManager.GetString(nameof (EVENT_MISSING_ADD_METHOD), Strings_Linq.resourceCulture);

    internal static string EVENT_MISSING_REMOVE_METHOD => Strings_Linq.ResourceManager.GetString(nameof (EVENT_MISSING_REMOVE_METHOD), Strings_Linq.resourceCulture);

    internal static string EVENT_MUST_RETURN_VOID => Strings_Linq.ResourceManager.GetString(nameof (EVENT_MUST_RETURN_VOID), Strings_Linq.resourceCulture);

    internal static string EVENT_PATTERN_REQUIRES_TWO_PARAMETERS => Strings_Linq.ResourceManager.GetString(nameof (EVENT_PATTERN_REQUIRES_TWO_PARAMETERS), Strings_Linq.resourceCulture);

    internal static string EVENT_REMOVE_METHOD_SHOULD_TAKE_ONE_PARAMETER => Strings_Linq.ResourceManager.GetString(nameof (EVENT_REMOVE_METHOD_SHOULD_TAKE_ONE_PARAMETER), Strings_Linq.resourceCulture);

    internal static string EVENT_SENDER_NOT_ASSIGNABLE => Strings_Linq.ResourceManager.GetString(nameof (EVENT_SENDER_NOT_ASSIGNABLE), Strings_Linq.resourceCulture);

    internal static string EVENT_WINRT_REMOVE_METHOD_SHOULD_TAKE_ERT => Strings_Linq.ResourceManager.GetString(nameof (EVENT_WINRT_REMOVE_METHOD_SHOULD_TAKE_ERT), Strings_Linq.resourceCulture);

    internal static string MORE_THAN_ONE_ELEMENT => Strings_Linq.ResourceManager.GetString(nameof (MORE_THAN_ONE_ELEMENT), Strings_Linq.resourceCulture);

    internal static string MORE_THAN_ONE_MATCHING_ELEMENT => Strings_Linq.ResourceManager.GetString(nameof (MORE_THAN_ONE_MATCHING_ELEMENT), Strings_Linq.resourceCulture);

    internal static string NO_ELEMENTS => Strings_Linq.ResourceManager.GetString(nameof (NO_ELEMENTS), Strings_Linq.resourceCulture);

    internal static string NO_MATCHING_ELEMENTS => Strings_Linq.ResourceManager.GetString(nameof (NO_MATCHING_ELEMENTS), Strings_Linq.resourceCulture);

    internal static string CANT_ADVANCE_WHILE_RUNNING => Strings_Linq.ResourceManager.GetString(nameof (CANT_ADVANCE_WHILE_RUNNING), Strings_Linq.resourceCulture);
  }
}
