// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.AppListRequestParameters
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.Globalization;

namespace Yandex.App.Information.AppList
{
  public class AppListRequestParameters
  {
    public AppListRequestParameters(
      int? geoCode,
      string applicationId,
      string platform,
      CultureInfo cultureInfo)
    {
      this.GeoCode = geoCode;
      this.ApplicationId = applicationId;
      this.Platform = platform;
      this.CultureInfo = cultureInfo;
    }

    public int? GeoCode { get; private set; }

    public string ApplicationId { get; private set; }

    public string Platform { get; private set; }

    public CultureInfo CultureInfo { get; private set; }
  }
}
