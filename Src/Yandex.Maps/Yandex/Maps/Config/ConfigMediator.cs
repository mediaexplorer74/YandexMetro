// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.ConfigMediator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Yandex.App;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Positioning;

namespace Yandex.Maps.Config
{
  internal class ConfigMediator : IConfigMediator, INotifyPropertyChanged
  {
    public const string MapLayersPropertyName = "MapLayers";
    public const string UseLocationPropertyName = "UseLocation";
    public const string CollectJamInformationPropertyName = "CollectJamInformation";
    public const string TilesStretchFactorPropertyName = "TilesStretchFactor";
    public const string TwilightModeEnabledPropertyName = "TwilightModeEnabled";
    private const string UuidKey = "uuid";
    private const double TilesStretchFactorEpsilon = 1E-10;
    private readonly IApplicationSettings _applicationSettings;
    private bool _collectJamInformation = true;
    private IDictionary<BaseLayers, MapLayer> _mapLayers;
    private double _tilesStretchFactor;
    private bool _useLocation;
    private string _uuid;
    private bool _twilightModeEnabled;

    public ConfigMediator(
      [NotNull] ITileSize tilesize,
      [NotNull] IApplicationSettings applicationSettings,
      [NotNull] IPrinterTileRequestSizeConverter printerTileRequestSizeConverter,
      [NotNull] QueryHosts queryHosts)
    {
      if (tilesize == null)
        throw new ArgumentNullException(nameof (tilesize));
      if (applicationSettings == null)
        throw new ArgumentNullException(nameof (applicationSettings));
      if (printerTileRequestSizeConverter == null)
        throw new ArgumentNullException(nameof (printerTileRequestSizeConverter));
      if (queryHosts == null)
        throw new ArgumentNullException(nameof (queryHosts));
      this._applicationSettings = applicationSettings;
      this.DisplayTileSize = (IDisplayTileSize) new Yandex.Maps.API.DisplayTileSize(tilesize);
      this.QueryHosts = queryHosts;
      this.MapLayers = (IDictionary<BaseLayers, MapLayer>) new Dictionary<BaseLayers, MapLayer>();
      this.TileReloadTimeout = TimeSpan.FromMilliseconds(60000.0);
      string str;
      if (this._applicationSettings.TryGetValue<string>("uuid", out str))
        this.Uuid = str;
      this.TileType = printerTileRequestSizeConverter.ConvertSize((int) tilesize.Height);
      this.TilesStretchFactor = 2.0;
    }

    public string Uuid
    {
      get => this._uuid;
      set
      {
        this._uuid = value;
        this._applicationSettings["uuid"] = (object) value;
      }
    }

    public QueryHosts QueryHosts { get; set; }

    public IDictionary<BaseLayers, MapLayer> MapLayers
    {
      get => this._mapLayers;
      set
      {
        this._mapLayers = value;
        this.OnPropertyChanged(nameof (MapLayers));
      }
    }

    public bool EnableLocationService
    {
      get => this._useLocation;
      set
      {
        if (this._useLocation == value)
          return;
        this._useLocation = value;
        this.OnPropertyChanged("UseLocation");
      }
    }

    public GeoCoordinate DefaultPosition { get; set; }

    public TimeSpan TileReloadTimeout { get; set; }

    public JamCollectConfig JamCollectConfig { get; set; }

    public OperatorConfig OperatorConfig { get; set; }

    public ObjectShowInterval ScaleLineShowInterval { get; set; }

    public bool CollectJamInformation
    {
      get => this._collectJamInformation;
      set
      {
        if (this._collectJamInformation == value)
          return;
        this._collectJamInformation = value;
        this.OnPropertyChanged(nameof (CollectJamInformation));
      }
    }

    public double TilesStretchFactor
    {
      get => this._tilesStretchFactor;
      set
      {
        if (Math.Abs(this._tilesStretchFactor - value) < 1E-10)
          return;
        this._tilesStretchFactor = value;
        this.DisplayTileSize.UpdateTileSize(this._tilesStretchFactor);
        this.OnPropertyChanged(nameof (TilesStretchFactor));
      }
    }

    public IDisplayTileSize DisplayTileSize { get; private set; }

    public string ApiKey { get; set; }

    public TileType TileType { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public bool TwilightModeEnabled
    {
      get => this._twilightModeEnabled;
      set
      {
        if (this._twilightModeEnabled == value)
          return;
        this._twilightModeEnabled = value;
        this.OnPropertyChanged(nameof (TwilightModeEnabled));
      }
    }

    public byte ZoomLevelsToDisplaySimultaneouslyDefault { get; set; }

    public bool IsUserPoiEnabled { get; set; }
  }
}
