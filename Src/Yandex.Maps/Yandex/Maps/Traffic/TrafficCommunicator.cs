// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.TrafficCommunicator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.ItemsCounter;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  [UsedImplicitly]
  internal class TrafficCommunicator : 
    DefaultCommunicatorBase<IJamQueryBuilder, TrafficRequestParameters, JamTracks>,
    ITrafficCommunicator,
    ICommunicator<TrafficRequestParameters, JamTracks>
  {
    private const bool UseBetaJams = true;

    public TrafficCommunicator(
      [NotNull] IJamQueryBuilder jamQueryBuilder,
      [NotNull] IGenericXmlSerializer<JamTracks> jamTracksSerializer,
      [NotNull] IMapWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(jamQueryBuilder, jamTracksSerializer, (IWebClientFactory) webClientFactory, itemCounter)
    {
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
    }

    public override void Request(TrafficRequestParameters parameters)
    {
      string tracksQuery = this._queryBuilder.GetTracksQuery(parameters.Uuid, parameters.Zoom, parameters.TopLeft.Latitude, parameters.TopLeft.Longitude, parameters.BottomRight.Latitude, parameters.BottomRight.Longitude, true, true);
      this.Execute(parameters, tracksQuery);
    }

    protected override void AfterRequestExecuted(
      TrafficRequestParameters requestParameters,
      JamTracks result)
    {
    }
  }
}
