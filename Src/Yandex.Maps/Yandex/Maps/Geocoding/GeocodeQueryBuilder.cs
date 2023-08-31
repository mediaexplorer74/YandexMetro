// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.GeocodeQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Geocoding.Interfaces;
using Yandex.Patterns;
using Yandex.Positioning;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Geocoding
{
  internal class GeocodeQueryBuilder : IGeocodeQueryBuilder, IQueryBuilder, IStateService
  {
    private readonly IConfigMediator _configMediator;
    private readonly IGeocodeUrlProvider _urlProvider;

    public GeocodeQueryBuilder(IGeocodeUrlProvider urlProvider, [NotNull] IConfigMediator configMediator)
    {
      if (urlProvider == null)
        throw new ArgumentNullException(nameof (urlProvider));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      this._urlProvider = urlProvider;
      this._configMediator = configMediator;
    }

    [NotNull]
    public string GetQuery(GeoCoordinate requestParameters)
    {
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters["uuid"] = (object) this._configMediator.Uuid;
      parameters["ll"] = (object) requestParameters.ToString();
      parameters["ver"] = (object) 2;
      parameters["gzip"] = (object) 0;
      StringBuilder stringBuilder = new StringBuilder(this._urlProvider.GetBaseUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters));
      return stringBuilder.ToString();
    }

    public event EventHandler<StateChangedEventArgs> StateChanged
    {
      add => this._urlProvider.StateChanged += value;
      remove => this._urlProvider.StateChanged -= value;
    }

    public ServiceState State => this._urlProvider.State;
  }
}
