// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.GeocodeManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Yandex.Maps.Geocoding.Dto;
using Yandex.Maps.Geocoding.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Patterns;
using Yandex.Positioning;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Geocoding
{
  internal class GeocodeManager : IGeocodeManager
  {
    private readonly ICommunicator<GeocodeRequestParameters, GeocodeResult> _geocodeCommunicator;
    private readonly IDictionary<GeoCoordinate, GeocodeResult> _repository;
    private readonly IPrinterManager _printerManager;

    public GeocodeManager(
      ICommunicator<GeocodeRequestParameters, GeocodeResult> geocodeCommunicator,
      IDictionary<GeoCoordinate, GeocodeResult> repository,
      [NotNull] IPrinterManager printerManager)
    {
      if (geocodeCommunicator == null)
        throw new ArgumentNullException(nameof (geocodeCommunicator));
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      if (printerManager == null)
        throw new ArgumentNullException(nameof (printerManager));
      this._geocodeCommunicator = geocodeCommunicator;
      this._repository = repository;
      this._printerManager = printerManager;
      this._geocodeCommunicator.RequestCompleted += new RequestCompletedEventHandler<GeocodeRequestParameters, GeocodeResult>(this.GeocodeCommunicatorRequestCompleted);
    }

    public event RequestCompletedEventHandler<GeocodeRequestParameters, GeocodeResult> GeocodeCompleted;

    public void Query(GeocodeRequestParameters request)
    {
      if (request == null)
        return;
      GeocodeResult requestResults;
      if (this._repository.TryGetValue(request.GeoCoordinates, out requestResults))
        this.OnGeocodeCompleted(new RequestCompletedEventArgs<GeocodeRequestParameters, GeocodeResult>(request, requestResults));
      else if (this._printerManager.State == ServiceState.Ready)
      {
        this._geocodeCommunicator.Request(request);
      }
      else
      {
        this._printerManager.ExecuteWhenReady((Action) (() => this._geocodeCommunicator.Request(request)));
        if (this._printerManager.State != ServiceState.Stopped)
          return;
        this._printerManager.Connect();
      }
    }

    private void GeocodeCommunicatorRequestCompleted(
      object sender,
      RequestCompletedEventArgs<GeocodeRequestParameters, GeocodeResult> e)
    {
      this._repository[e.RequestResults.Coordinates] = e.RequestResults;
      this.OnGeocodeCompleted(new RequestCompletedEventArgs<GeocodeRequestParameters, GeocodeResult>(e.Parameters, e.RequestResults));
    }

    private void OnGeocodeCompleted(
      RequestCompletedEventArgs<GeocodeRequestParameters, GeocodeResult> e)
    {
      RequestCompletedEventHandler<GeocodeRequestParameters, GeocodeResult> geocodeCompleted = this.GeocodeCompleted;
      if (geocodeCompleted == null)
        return;
      geocodeCompleted((object) this, e);
    }

    public event EventHandler<RequestFailedEventArgs<GeocodeRequestParameters>> GeocodeFailed
    {
      add => this._geocodeCommunicator.RequestFailed += value;
      remove => this._geocodeCommunicator.RequestFailed -= value;
    }
  }
}
