// Decompiled with JetBrains decompiler
// Type: Yandex.Properties.Resources
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Yandex.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Yandex.Properties.Resources.resourceMan, (object) null))
          Yandex.Properties.Resources.resourceMan = new ResourceManager("Yandex.Properties.Resources", typeof (Yandex.Properties.Resources).Assembly);
        return Yandex.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Yandex.Properties.Resources.resourceCulture;
      set => Yandex.Properties.Resources.resourceCulture = value;
    }

    internal static string Error => Yandex.Properties.Resources.ResourceManager.GetString(nameof (Error), Yandex.Properties.Resources.resourceCulture);
  }
}
