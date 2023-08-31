// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.PrinterUrlProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Config
{
  internal class PrinterUrlProvider : IPrinterUrlProvider, IStateService
  {
    private readonly IConfigMediator _configMediator;

    public PrinterUrlProvider(IConfigMediator printerConfigMediator) => this._configMediator = printerConfigMediator != null ? printerConfigMediator : throw new ArgumentNullException(nameof (printerConfigMediator));

    protected virtual string GetUrlByHostType(HostTypes hostType)
    {
      string urlByHostType;
      if (!this._configMediator.QueryHosts.TryGetValue(hostType, out urlByHostType))
        this._configMediator.QueryHosts.TryGetValue(HostTypes.@default, out urlByHostType);
      return urlByHostType;
    }

    public virtual string GetRootStartupUrl() => "http://mobile-partners.maps.yandex.net/startup";

    public virtual string GetRootPrinterUrl() => this.GetUrlByHostType(HostTypes.tiles) + "/printer";

    public string GetTrafficGetUrl() => this.GetUrlByHostType(HostTypes.trafficget) + "/jamsvec";

    public string GetTrafficCollectUrl() => this.GetUrlByHostType(HostTypes.trafficcollect) + "/ymm_collect/2.x/";

    public string GetJamStylesUrl() => this.GetUrlByHostType(HostTypes.@default);

    public string GetSearchUrl() => this.GetUrlByHostType(HostTypes.search) + "/searchnearby";

    public string GetSuggestsUrl() => this.GetUrlByHostType(HostTypes.suggest) + "/suggest";

    public string GetRouteBuilderUrl() => this.GetUrlByHostType(HostTypes.@default) + "/route_builder";

    public string GetGeocodeUrl() => this.GetUrlByHostType(HostTypes.@default) + "/geocode";

    [CanBeNull]
    public string GetAgreementUrl()
    {
      string urlByHostType = this.GetUrlByHostType(HostTypes.agreement);
      return !string.IsNullOrEmpty(urlByHostType) ? urlByHostType + "/maps_mobile_agreement" : (string) null;
    }

    public string GetPoiBaseUrl() => this.GetUrlByHostType(HostTypes.@default) + "/userpoi";

    public string GetCapptainLoggerUrl() => this.GetUrlByHostType(HostTypes.caplogger) + "/caplogger";

    public string GetCapptainIpToCountryUrl() => this.GetUrlByHostType(HostTypes.capiptocountry) + "/ip-to-country";

    public string GetStylesBaseUrl() => this.GetUrlByHostType(HostTypes.@default) + "/styles";

    public string GetStationCardBaseUrl() => this.GetUrlByHostType(HostTypes.@default) + "/getstationcard";

    public string GetStreetViewBaseUrl() => this.GetUrlByHostType(HostTypes.@default) + "/stv";

    public event EventHandler<StateChangedEventArgs> StateChanged
    {
      add => this._configMediator.QueryHosts.StateChanged += value;
      remove => this._configMediator.QueryHosts.StateChanged -= value;
    }

    public ServiceState State => this._configMediator.QueryHosts.State;
  }
}
