// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Resources
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Yandex.App.Information
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
        if (object.ReferenceEquals((object) Yandex.App.Information.Resources.resourceMan, (object) null))
          Yandex.App.Information.Resources.resourceMan = new ResourceManager("Yandex.App.Information.Resources", typeof (Yandex.App.Information.Resources).Assembly);
        return Yandex.App.Information.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => Yandex.App.Information.Resources.resourceCulture;
      set => Yandex.App.Information.Resources.resourceCulture = value;
    }

    public static string About => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (About), Yandex.App.Information.Resources.resourceCulture);

    public static string Applications => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (Applications), Yandex.App.Information.Resources.resourceCulture);

    public static string BuildFormat => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (BuildFormat), Yandex.App.Information.Resources.resourceCulture);

    public static string CopyrightFormat => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (CopyrightFormat), Yandex.App.Information.Resources.resourceCulture);

    public static string Feedback => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (Feedback), Yandex.App.Information.Resources.resourceCulture);

    public static string FeedbackSubject => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (FeedbackSubject), Yandex.App.Information.Resources.resourceCulture);

    public static string LicenseAgreement => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (LicenseAgreement), Yandex.App.Information.Resources.resourceCulture);

    public static string Loading => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (Loading), Yandex.App.Information.Resources.resourceCulture);

    public static string LoadingOfAppItemsHasFailed => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (LoadingOfAppItemsHasFailed), Yandex.App.Information.Resources.resourceCulture);

    public static string Marketplace => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (Marketplace), Yandex.App.Information.Resources.resourceCulture);

    public static string OtherApplications => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (OtherApplications), Yandex.App.Information.Resources.resourceCulture);

    public static string UuidMessageFormat => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (UuidMessageFormat), Yandex.App.Information.Resources.resourceCulture);

    public static string VersionFormat => Yandex.App.Information.Resources.ResourceManager.GetString(nameof (VersionFormat), Yandex.App.Information.Resources.resourceCulture);
  }
}
