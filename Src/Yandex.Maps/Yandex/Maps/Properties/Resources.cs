// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Properties.Resources
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Yandex.Maps.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [ComVisible(false)]
  public class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Yandex.Maps.Properties.Resources.resourceMan, (object) null))
          Yandex.Maps.Properties.Resources.resourceMan = new ResourceManager("Yandex.Maps.Properties.Resources", typeof (Yandex.Maps.Properties.Resources).Assembly);
        return Yandex.Maps.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => Yandex.Maps.Properties.Resources.resourceCulture;
      set => Yandex.Maps.Properties.Resources.resourceCulture = value;
    }

    public static string NoMapImageResource => Yandex.Maps.Properties.Resources.ResourceManager.GetString(nameof (NoMapImageResource), Yandex.Maps.Properties.Resources.resourceCulture);

    public static string UserLocationGpsResource => Yandex.Maps.Properties.Resources.ResourceManager.GetString(nameof (UserLocationGpsResource), Yandex.Maps.Properties.Resources.resourceCulture);

    public static string UserLocationLbsResource => Yandex.Maps.Properties.Resources.ResourceManager.GetString(nameof (UserLocationLbsResource), Yandex.Maps.Properties.Resources.resourceCulture);

    public static string YandexLogoImageResource => Yandex.Maps.Properties.Resources.ResourceManager.GetString(nameof (YandexLogoImageResource), Yandex.Maps.Properties.Resources.resourceCulture);
  }
}
