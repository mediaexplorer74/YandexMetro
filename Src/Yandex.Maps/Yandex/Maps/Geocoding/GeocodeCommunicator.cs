// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.GeocodeCommunicator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.ItemsCounter;
using Yandex.Maps.Geocoding.Dto;
using Yandex.Maps.Geocoding.Interfaces;
using Yandex.Patterns;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Geocoding
{
  [UsedImplicitly]
  internal class GeocodeCommunicator : 
    DefaultCommunicatorBase<IGeocodeQueryBuilder, GeocodeRequestParameters, GeocodeResult>
  {
    public GeocodeCommunicator(
      [NotNull] IGeocodeQueryBuilder queryBuilder,
      [NotNull] IGenericXmlSerializer<GeocodeResult> serializer,
      [NotNull] IMapWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(queryBuilder, serializer, (IWebClientFactory) webClientFactory, itemCounter)
    {
    }

    public override void Request(GeocodeRequestParameters parameters)
    {
      if (this._queryBuilder.State != ServiceState.Ready)
        throw new InvalidOperationException("Unable to execute geocode request. Hosts are not initalized yet. Have you set MapGlobalSettings.ApiKey?");
      string query = this._queryBuilder.GetQuery(parameters.GeoCoordinates);
      this.Execute(parameters, query);
    }

    protected override void AfterRequestExecuted(
      GeocodeRequestParameters requestParameters,
      GeocodeResult result)
    {
      result.Coordinates = requestParameters.GeoCoordinates;
    }
  }
}
