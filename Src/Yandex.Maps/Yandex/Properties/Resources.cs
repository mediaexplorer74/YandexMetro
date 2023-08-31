// Decompiled with JetBrains decompiler
// Type: Yandex.Properties.Resources
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Yandex.Properties
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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

    internal static string DaysFormatString => Yandex.Properties.Resources.ResourceManager.GetString(nameof (DaysFormatString), Yandex.Properties.Resources.resourceCulture);

    internal static string HoursFormatString => Yandex.Properties.Resources.ResourceManager.GetString(nameof (HoursFormatString), Yandex.Properties.Resources.resourceCulture);

    internal static string MesureUnitKiloMetreShort => Yandex.Properties.Resources.ResourceManager.GetString(nameof (MesureUnitKiloMetreShort), Yandex.Properties.Resources.resourceCulture);

    internal static string MesureUnitMeterShort => Yandex.Properties.Resources.ResourceManager.GetString(nameof (MesureUnitMeterShort), Yandex.Properties.Resources.resourceCulture);

    internal static string MinutesFromatString => Yandex.Properties.Resources.ResourceManager.GetString(nameof (MinutesFromatString), Yandex.Properties.Resources.resourceCulture);

    internal static string PairFormat => Yandex.Properties.Resources.ResourceManager.GetString(nameof (PairFormat), Yandex.Properties.Resources.resourceCulture);
  }
}
