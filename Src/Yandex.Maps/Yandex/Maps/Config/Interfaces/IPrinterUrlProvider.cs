// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.Interfaces.IPrinterUrlProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Patterns;

namespace Yandex.Maps.Config.Interfaces
{
  internal interface IPrinterUrlProvider : IStateService
  {
    string GetRootStartupUrl();

    string GetRootPrinterUrl();

    string GetTrafficGetUrl();

    string GetTrafficCollectUrl();

    string GetJamStylesUrl();

    string GetSearchUrl();

    string GetSuggestsUrl();

    string GetRouteBuilderUrl();

    string GetGeocodeUrl();

    string GetAgreementUrl();

    string GetPoiBaseUrl();

    string GetCapptainLoggerUrl();

    string GetCapptainIpToCountryUrl();

    string GetStylesBaseUrl();

    string GetStationCardBaseUrl();

    string GetStreetViewBaseUrl();
  }
}
