// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamTilesFetcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;
using Yandex.Positioning;
using Yandex.Threading;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  [UsedImplicitly]
  internal class JamTilesFetcher : ITileFetcher<IJamTile>, IStateService
  {
    private readonly IJamManager _jamManager;
    private readonly IGeoTileConverter _geoTileConverter;
    private readonly IConfigMediator _configMediator;
    private readonly IPrinterManager _printerManager;
    private readonly INetworkInterfaceWrapper _networkInterfaceWrapper;
    private readonly ITrafficCommunicator _trafficCommunicator;
    private readonly IThreadPool _threadPool;

    public event EventHandler<TilesReadyEventArgs<IJamTile>> TilesReady;

    public event EventHandler<TileRequestFailedEventArgs> RequestFailed;

    public JamTilesFetcher(
      [NotNull] IJamManager jamManager,
      [NotNull] IGeoTileConverter geoTileConverter,
      [NotNull] IConfigMediator configMediator,
      [NotNull] IPrinterManager printerManager,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper,
      [NotNull] ITrafficCommunicator trafficCommunicator,
      [NotNull] IThreadPool threadPool)
    {
      if (jamManager == null)
        throw new ArgumentNullException(nameof (jamManager));
      if (geoTileConverter == null)
        throw new ArgumentNullException(nameof (geoTileConverter));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (printerManager == null)
        throw new ArgumentNullException(nameof (printerManager));
      if (networkInterfaceWrapper == null)
        throw new ArgumentNullException(nameof (networkInterfaceWrapper));
      if (trafficCommunicator == null)
        throw new ArgumentNullException(nameof (trafficCommunicator));
      if (threadPool == null)
        throw new ArgumentNullException(nameof (threadPool));
      this._jamManager = jamManager;
      this._trafficCommunicator = trafficCommunicator;
      this._threadPool = threadPool;
      this._trafficCommunicator.RequestCompleted += new RequestCompletedEventHandler<TrafficRequestParameters, JamTracks>(this.JamManagerRequestCompleted);
      this._trafficCommunicator.RequestFailed += new EventHandler<RequestFailedEventArgs<TrafficRequestParameters>>(this.JamManagerRequestFailed);
      this._geoTileConverter = geoTileConverter;
      this._configMediator = configMediator;
      this._printerManager = printerManager;
      this._networkInterfaceWrapper = networkInterfaceWrapper;
    }

    private void JamManagerRequestCompleted(
      object sender,
      RequestCompletedEventArgs<TrafficRequestParameters, JamTracks> e)
    {
      this.OnTilesReady(new TilesReadyEventArgs<IJamTile>(e.Parameters.TilesToLoad, (IList<IJamTile>) new IJamTile[0], (object) e.RequestResults));
    }

    private void JamManagerRequestFailed(
      object sender,
      RequestFailedEventArgs<TrafficRequestParameters> e)
    {
      this.OnRequestFailed(new TileRequestFailedEventArgs(e.Parameters.TilesToLoad));
    }

    public uint MaxTilesInSingleQuery => 0;

    public void QueryTilesAsync(IEnumerable<ITileInfo> tiles)
    {
      IList<ITileInfo> tilesList = tiles != null ? (IList<ITileInfo>) tiles.ToList<ITileInfo>() : throw new ArgumentNullException(nameof (tiles));
      string uuid;
      if (this._printerManager.State == ServiceState.Ready && !string.IsNullOrEmpty(uuid = this._configMediator.Uuid) && this._networkInterfaceWrapper.GetIsNetworkAvailable())
      {
        ITileInfo tileInfo = tilesList.FirstOrDefault<ITileInfo>();
        if (tileInfo == null)
        {
          this.OnRequestFailed(new TileRequestFailedEventArgs(tilesList));
        }
        else
        {
          GeoCoordinate topLeft;
          this._geoTileConverter.GetTileInfoGeoCoordinates(tileInfo, out topLeft, out GeoCoordinate _);
          GeoCoordinate bottomRight;
          this._geoTileConverter.GetTileInfoGeoCoordinates(tilesList.Last<ITileInfo>(), out GeoCoordinate _, out bottomRight);
          this._trafficCommunicator.Request(new TrafficRequestParameters(uuid, (uint) tileInfo.Zoom, topLeft, bottomRight, tilesList));
        }
      }
      else
        this._threadPool.QueueUserWorkItem((WaitCallback) (nothing => this.OnRequestFailed(new TileRequestFailedEventArgs(tilesList))), (object) null);
    }

    protected virtual void OnTilesReady(TilesReadyEventArgs<IJamTile> e)
    {
      EventHandler<TilesReadyEventArgs<IJamTile>> tilesReady = this.TilesReady;
      if (tilesReady == null)
        return;
      tilesReady((object) this, e);
    }

    protected virtual void OnRequestFailed(TileRequestFailedEventArgs e)
    {
      EventHandler<TileRequestFailedEventArgs> requestFailed = this.RequestFailed;
      if (requestFailed == null)
        return;
      requestFailed((object) this, e);
    }

    public event EventHandler<StateChangedEventArgs> StateChanged
    {
      add => this._jamManager.StateChanged += value;
      remove => this._jamManager.StateChanged -= value;
    }

    public ServiceState State => this._jamManager.State;
  }
}
