// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.App.Interfaces;
using Yandex.DevUtils;
using Yandex.Maps.API;
using Yandex.Maps.API.Adapters;
using Yandex.Maps.Config;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.Config;
using Yandex.Maps.PrinterClient.DataAdapters;
using Yandex.Maps.PrinterClient.EventArgs;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Patterns;
using Yandex.Positioning;

namespace Yandex.Maps.PrinterClient
{
  internal class PrinterManager : IPrinterManager, IStateService
  {
    private readonly IPrinterStartupManager _startupManager;
    private readonly IApplicationIdentity _appIdentity;
    private readonly IConfigMediator _configMediator;
    private readonly IOperatorInfoDataAdapter _operatorInfoDataAdapter;
    private ServiceState _state;

    public PrinterManager(
      IPrinterStartupManager startupManager,
      IApplicationIdentity appIdentity,
      IConfigMediator configMediator,
      [NotNull] IOperatorInfoDataAdapter operatorInfoDataAdapter)
    {
      if (startupManager == null)
        throw new ArgumentNullException(nameof (startupManager));
      if (appIdentity == null)
        throw new ArgumentNullException(nameof (appIdentity));
      if (operatorInfoDataAdapter == null)
        throw new ArgumentNullException(nameof (operatorInfoDataAdapter));
      this._startupManager = startupManager;
      this._appIdentity = appIdentity;
      this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));
      this._operatorInfoDataAdapter = operatorInfoDataAdapter;
      this._startupManager.StartupCompleted += new EventHandler<PrinterStartupEventArgs>(this.StartupManagerStartupCompleted);
      this._startupManager.StartupFailed += new EventHandler(this.StartupManagerStartupFailed);
    }

    private void StartupManagerStartupFailed(object sender, System.EventArgs e) => this.State = ServiceState.Fail;

    public void Connect()
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      string uuid = this._configMediator.Uuid;
      this.State = ServiceState.Starting;
      this._startupManager.Startup(this._appIdentity.ApplicationVersion, this._appIdentity.Platform, uuid);
    }

    private void StartupManagerStartupCompleted(object sender, PrinterStartupEventArgs e)
    {
      string uuid = this._configMediator.Uuid;
      this._configMediator.Uuid = e.Config.Uuid;
      this._configMediator.QueryHosts.SetHosts(((IEnumerable<QueryHost>) e.Config.QueryHosts).Where<QueryHost>((Func<QueryHost, bool>) (item => item.HostType != HostTypes.undefined)));
      if (string.IsNullOrEmpty(uuid))
      {
        this.Connect();
      }
      else
      {
        if (e.Config.MapLayers != null)
          this._configMediator.MapLayers = (IDictionary<BaseLayers, Yandex.Maps.Config.MapLayer>) ((IEnumerable<Yandex.Maps.PrinterClient.Config.MapLayer>) e.Config.MapLayers).ToDictionary<Yandex.Maps.PrinterClient.Config.MapLayer, BaseLayers, Yandex.Maps.Config.MapLayer>((Func<Yandex.Maps.PrinterClient.Config.MapLayer, BaseLayers>) (key => LayerAdapter.StringToLayer(key.Request)), (Func<Yandex.Maps.PrinterClient.Config.MapLayer, Yandex.Maps.Config.MapLayer>) (value => new Yandex.Maps.Config.MapLayer()
          {
            Id = value.Id,
            Layer = LayerAdapter.StringToLayer(value.Request),
            MapVersion = (ushort) value.Version,
            Name = value.Name,
            IsService = value.Service == 1,
            SizeInPixels = value.SizeInPixels
          }));
        this._configMediator.JamCollectConfig = new JamCollectConfig()
        {
          Enabled = e.Config.ProviderFeatures.TrafficCollect.Enabled == (byte) 1,
          ScanTimeout = TimeSpan.FromSeconds((double) e.Config.ProviderFeatures.TrafficCollect.ScanTimeout),
          SendTimeout = TimeSpan.FromSeconds((double) e.Config.ProviderFeatures.TrafficCollect.SendTimeout),
          ErrorTimeout = TimeSpan.FromSeconds((double) e.Config.ProviderFeatures.TrafficCollect.ErrorTimeout)
        };
        MapPosition openPos = e.Config.OpenPos;
        if (openPos != null && openPos.Success == 0)
          this._configMediator.DefaultPosition = new GeoCoordinate(openPos.Latitude, openPos.Longitude);
        this._configMediator.OperatorConfig = this._operatorInfoDataAdapter.ReadOperatorConfig(e.Config);
        if (e.Config.ObjectIntervals != null)
        {
          ObjectInterval objectInterval = ((IEnumerable<ObjectInterval>) e.Config.ObjectIntervals).FirstOrDefault<ObjectInterval>((Func<ObjectInterval, bool>) (i => i.ObjectType == ObjectTypes.scaleline));
          if (objectInterval != null)
            this._configMediator.ScaleLineShowInterval = new ObjectShowInterval()
            {
              MaxZoom = objectInterval.MaxZoom,
              MinZoom = objectInterval.MinZoom
            };
        }
        this._configMediator.IsUserPoiEnabled = e.Config.ProviderFeatures.UserPoi != null && e.Config.ProviderFeatures.UserPoi.Enabled == 1;
        this.State = ServiceState.Ready;
      }
    }

    public event EventHandler<StateChangedEventArgs> StateChanged;

    protected virtual void OnStateChanged(StateChangedEventArgs e)
    {
      EventHandler<StateChangedEventArgs> stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, e);
    }

    public ServiceState State
    {
      get => this._state;
      private set
      {
        if (this._state == value)
          return;
        this._state = value;
        this.OnStateChanged(new StateChangedEventArgs(value));
      }
    }
  }
}
