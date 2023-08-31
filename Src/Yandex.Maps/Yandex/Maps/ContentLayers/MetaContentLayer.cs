// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.MetaContentLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Collections.Interfaces;
using Yandex.DevUtils;
using Yandex.Ioc;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Events;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Controls.ContentLayers;
using Yandex.Maps.Controls.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Positioning;
using Yandex.Positioning.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [UsedImplicitly]
  [ComVisible(false)]
  public class MetaContentLayer : 
    ContentLayerBase,
    IMetaContentLayer,
    IContentLayer,
    IContentContainer
  {
    private readonly IVisibleTileLayer<ITile> _tileLayer;
    private readonly IPositionWatcher _positionWatcher;
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly ITileManager<ITile> _tileManager;
    private readonly TimeSpan _routeguidanceTimeout = new TimeSpan(0, 0, 30);
    private readonly List<TileLoadedNotifier> _notifiers = new List<TileLoadedNotifier>();
    private readonly object _notifiersSyncObj = new object();
    private bool _isTilesReadyAttached;
    [CanBeNull]
    private IList<ITileInfo> _visibleTilesInfos;
    private bool _trafficAvailable;
    public static readonly DependencyProperty RouteGuidanceEnabledProperty = DependencyProperty.Register(nameof (RouteGuidanceEnabled), typeof (bool), typeof (MetaContentLayer), new PropertyMetadata((object) true));
    public static readonly DependencyProperty RoutingEnabledProperty = DependencyProperty.Register(nameof (RoutingEnabled), typeof (bool), typeof (MetaContentLayer), new PropertyMetadata((object) true));
    private IDisposable _updateRouteGuidanceTimer;
    public static readonly DependencyProperty PanoramaAvailableProperty = DependencyProperty.Register(nameof (PanoramaAvailable), typeof (bool), typeof (MetaContentLayer), new PropertyMetadata((object) true));

    public MetaContentLayer()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IViewportTileConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IQueue<ILayerCommand>>(), IocSingleton<ControlsIocInitializer>.Resolve<IThread>(), IocSingleton<ControlsIocInitializer>.Resolve<ITileManager<ITile>>(), IocSingleton<ControlsIocInitializer>.Resolve<IVisibleTileLayer<ITile>>(), IocSingleton<ControlsIocInitializer>.Resolve<IPositionWatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IGeoPixelConverter>())
    {
    }

    public MetaContentLayer(
      IUiDispatcher uiDispatcher,
      IViewportTileConverter viewportTileConverter,
      IQueue<ILayerCommand> commandQueue,
      IThread thread,
      ITileManager<ITile> tileManager,
      IVisibleTileLayer<ITile> tileLayer,
      [NotNull] IPositionWatcher positionWatcher,
      [NotNull] IGeoPixelConverter geoPixelConverter)
      : base(uiDispatcher, viewportTileConverter, commandQueue, thread)
    {
      if (tileManager == null)
        throw new ArgumentNullException(nameof (tileManager));
      if (tileLayer == null)
        throw new ArgumentNullException(nameof (tileLayer));
      if (positionWatcher == null)
        throw new ArgumentNullException(nameof (positionWatcher));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      this._tileManager = tileManager;
      this._tileLayer = tileLayer;
      this._positionWatcher = positionWatcher;
      this._geoPixelConverter = geoPixelConverter;
      this.AttachTilesReady();
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.OnUnloaded);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this._updateRouteGuidanceTimer != null)
        return;
      this._updateRouteGuidanceTimer = Observable.Interval(this._routeguidanceTimeout).Subscribe<long>(new Action<long>(this.UpdateRouteGuidance));
    }

    private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this._updateRouteGuidanceTimer == null)
        return;
      this._updateRouteGuidanceTimer.Dispose();
      this._updateRouteGuidanceTimer = (IDisposable) null;
    }

    public bool TrafficAvailable
    {
      get => this._trafficAvailable;
      set
      {
        if (this._trafficAvailable == value)
          return;
        this._trafficAvailable = value;
        this.OnTrafficAvailbleChanged(new TrafficAvailableChangedArgs()
        {
          TrafficAvailable = value
        });
      }
    }

    public event EventHandler<TrafficAvailableChangedArgs> TrafficAvailbleChanged;

    public bool IsRoutingEnabledInCoordinates(GeoCoordinate coordinates)
    {
      ITile tileByCoordinates = this.GetVisibleTileByCoordinates(coordinates, this._activeZoom);
      return tileByCoordinates == null || tileByCoordinates.Metadata.Features.Routing;
    }

    public ITileLoadedNotifier GetTileByCoordinates(GeoCoordinate coordinates)
    {
      TileInfo tileInfo;
      ITile tileByCoordinates1 = this.GetVisibleTileByCoordinates(coordinates, this._activeZoom, out tileInfo);
      if (tileByCoordinates1 != null)
        return (ITileLoadedNotifier) new TileLoadedNotifier()
        {
          Tile = tileByCoordinates1
        };
      tileInfo.Layer = BaseLayers.meta;
      TileLoadedNotifier tileByCoordinates2 = new TileLoadedNotifier()
      {
        TileInfo = (ITileInfo) tileInfo
      };
      lock (this._notifiersSyncObj)
        this._notifiers.Add(tileByCoordinates2);
      this._tileManager.QueryTiles((IEnumerable<ITileInfo>) new TileInfo[1]
      {
        tileInfo
      });
      return (ITileLoadedNotifier) tileByCoordinates2;
    }

    protected virtual void OnTrafficAvailbleChanged(TrafficAvailableChangedArgs e)
    {
      EventHandler<TrafficAvailableChangedArgs> trafficAvailbleChanged = this.TrafficAvailbleChanged;
      if (trafficAvailbleChanged == null)
        return;
      trafficAvailbleChanged((object) this, e);
    }

    public bool RouteGuidanceEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(MetaContentLayer.RouteGuidanceEnabledProperty);
      private set => ((DependencyObject) this).SetValue(MetaContentLayer.RouteGuidanceEnabledProperty, (object) value);
    }

    public bool RoutingEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(MetaContentLayer.RoutingEnabledProperty);
      set => ((DependencyObject) this).SetValue(MetaContentLayer.RoutingEnabledProperty, (object) value);
    }

    private void DetachTilesReady()
    {
      this._tileManager.TilesReady -= new EventHandler<TilesReadyEventArgs<ITile>>(this.TileManagerTilesReady);
      this._isTilesReadyAttached = false;
    }

    private void AttachTilesReady()
    {
      if (this._isTilesReadyAttached)
        return;
      this._tileManager.TilesReady -= new EventHandler<TilesReadyEventArgs<ITile>>(this.TileManagerTilesReady);
      this._tileManager.TilesReady += new EventHandler<TilesReadyEventArgs<ITile>>(this.TileManagerTilesReady);
      this._isTilesReadyAttached = true;
    }

    protected override void OnCommand(ILayerCommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (command.Type == LayerCommandTypes.Custom && this.OperationMode == OperationMode.Full && command is TilesReadyLayerCommand<ITile> readyLayerCommand)
        this.OnTilesReady(readyLayerCommand.ReadyTiles);
      base.OnCommand(command);
    }

    protected override void OnDisposeContentOutsideArea(ViewportRect viewportRect) => this._tileLayer.DisposeTilesExcept((IList<ITileInfo>) this._viewportTileConverter.ViewportRectToTiles(viewportRect, this._activeZoom).ToList<ITileInfo>());

    protected override void OnDisposeContent() => this._tileLayer.DisposeTiles();

    protected override void OnOperationModeChanged(OperationMode newValue)
    {
      this._tileManager.OperationMode = newValue;
      if (newValue != OperationMode.None)
        this.AttachTilesReady();
      else
        this.DetachTilesReady();
    }

    protected override void OnReload(ViewportRect viewportRect)
    {
      base.OnReload(viewportRect);
      byte zoom = ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel);
      this.DoZoom(zoom);
      IList<ITileInfo> array1 = (IList<ITileInfo>) this._viewportTileConverter.ViewportRectToTiles(viewportRect, zoom).Select<ITileInfo, ITileInfo>((Func<ITileInfo, ITileInfo>) (tileInfo => (ITileInfo) new TileInfo(tileInfo.X, tileInfo.Y, tileInfo.Zoom, BaseLayers.meta))).ToArray<ITileInfo>();
      this._visibleTilesInfos = array1;
      ITileInfo[] array2 = array1.Where<ITileInfo>((Func<ITileInfo, bool>) (t => !this._tileLayer.VisibleTileInfos.Contains(t))).ToArray<ITileInfo>();
      if (!((IEnumerable<ITileInfo>) array2).Any<ITileInfo>())
        return;
      this._tileManager.QueryTiles((IEnumerable<ITileInfo>) array2);
    }

    private void OnTilesReady(IList<ITile> readyTiles)
    {
      IList<ITileInfo> visibleTilesInfos = this._visibleTilesInfos;
      if (visibleTilesInfos == null)
        return;
      this._tileLayer.DisplayTiles((IList<ITile>) readyTiles.Where<ITile>((Func<ITile, bool>) (t => visibleTilesInfos.Contains(t.TileInfo))).ToList<ITile>());
      if (!this._tileLayer.VisibleTiles.Any<ITile>())
        return;
      this.UpdateMeta((IEnumerable<ITile>) this._tileLayer.VisibleTiles);
      this.NotifyListeners(readyTiles);
    }

    private void NotifyListeners(IList<ITile> visibleTiles)
    {
      lock (this._notifiersSyncObj)
      {
        List<TileLoadedNotifier> tileLoadedNotifierList = new List<TileLoadedNotifier>();
        foreach (TileLoadedNotifier notifier1 in this._notifiers)
        {
          TileLoadedNotifier notifier = notifier1;
          ITile tile = visibleTiles.FirstOrDefault<ITile>((Func<ITile, bool>) (t => t.TileInfo.Equals((object) notifier.TileInfo)));
          if (tile != null)
          {
            tileLoadedNotifierList.Add(notifier);
            notifier.Tile = tile;
            notifier.OnLoaded(tile);
          }
        }
        foreach (TileLoadedNotifier tileLoadedNotifier in tileLoadedNotifierList)
          this._notifiers.Remove(tileLoadedNotifier);
      }
    }

    private void TileManagerTilesReady(object sender, TilesReadyEventArgs<ITile> e)
    {
      List<ITile> list = this.GetReadyTiles((IEnumerable<ITile>) e.Tiles).ToList<ITile>();
      if (!list.Any<ITile>())
        return;
      this.EnqueueCommand((ILayerCommand) new TilesReadyLayerCommand<ITile>((IList<ITile>) list));
    }

    private void UpdateMeta(IEnumerable<ITile> visibleTiles)
    {
      IList<TileMetaFeatures> list = (IList<TileMetaFeatures>) visibleTiles.Where<ITile>((Func<ITile, bool>) (tile => tile.Metadata != null && null != tile.Metadata.Features)).ToList<ITile>().Select<ITile, TileMetaFeatures>((Func<ITile, TileMetaFeatures>) (tile => tile.Metadata.Features)).ToList<TileMetaFeatures>();
      this.TrafficAvailable = list.Any<TileMetaFeatures>((Func<TileMetaFeatures, bool>) (t => t.Semaphore));
      bool routingEnabled = list.Any<TileMetaFeatures>((Func<TileMetaFeatures, bool>) (t => t.Routing));
      bool panoramaAvailable = list.Any<TileMetaFeatures>((Func<TileMetaFeatures, bool>) (t => t.Streetview));
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        this.RoutingEnabled = routingEnabled;
        this.PanoramaAvailable = panoramaAvailable;
      }));
    }

    private void UpdateRouteGuidance(long timeout)
    {
      if (!this._positionWatcher.Enabled || this._positionWatcher.Status != GeoPositionStatus.Ready || this._positionWatcher.LastPosition == null)
      {
        this._uiDispatcher.BeginInvoke((Action) (() => this.RouteGuidanceEnabled = false));
      }
      else
      {
        ITileLoadedNotifier tileByCoordinates = this.GetTileByCoordinates(this._positionWatcher.LastPosition.GeoCoordinate);
        ITile tile = tileByCoordinates.Tile;
        if (tile != null)
          this.UpdateRouteGuidanceEnabledByTile(tile);
        else
          tileByCoordinates.Loaded += (EventHandler<TileLoadedEventArgs>) ((sender, args) => this.UpdateRouteGuidanceEnabledByTile(args.Tile));
      }
    }

    private void UpdateRouteGuidanceEnabledByTile(ITile visibleTile)
    {
      bool routeGuidanceEnabled = false;
      if (visibleTile.Metadata != null && visibleTile.Metadata.Features != null)
        routeGuidanceEnabled = visibleTile.Metadata.Features.Routeguidance;
      this._uiDispatcher.BeginInvoke((Action) (() => this.RouteGuidanceEnabled = routeGuidanceEnabled));
    }

    private IEnumerable<ITile> GetReadyTiles(IEnumerable<ITile> tiles) => tiles.Where<ITile>((Func<ITile, bool>) (tile => (tile.Status & TileStatus.Error) != TileStatus.Error && tile.TileInfo.Layer == BaseLayers.meta));

    [CanBeNull]
    private ITile GetVisibleTileByCoordinates(GeoCoordinate coordinates, byte zoom) => this.GetVisibleTileByCoordinates(coordinates, zoom, out TileInfo _);

    [CanBeNull]
    private ITile GetVisibleTileByCoordinates(
      GeoCoordinate coordinates,
      byte zoom,
      out TileInfo tileInfo)
    {
      tileInfo = this._viewportTileConverter.GetTileByPoint(this._geoPixelConverter.CoordinatesToZoomPoint(coordinates, zoom), zoom) as TileInfo;
      if (tileInfo == (TileInfo) null)
        return (ITile) null;
      TileInfo tileInfo1 = tileInfo;
      return this._tileLayer.VisibleTiles.FirstOrDefault<ITile>((Func<ITile, bool>) (t => t.Metadata != null && t.Metadata.Features != null && t.TileInfo.EqualsCoordinates((ITileInfo) tileInfo1)));
    }

    protected override void OnDispose()
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      if (this._updateRouteGuidanceTimer != null)
        this._updateRouteGuidanceTimer.Dispose();
      base.OnDispose();
      this.OnDisposeContent();
    }

    public event EventHandler VisibleTilesChanged
    {
      add => this._tileLayer.VisibleTilesChanged += value;
      remove => this._tileLayer.VisibleTilesChanged -= value;
    }

    public IList<ITile> VisibleTiles => this._tileLayer.VisibleTiles;

    public bool AreAllVisibleTilesLoaded => this._visibleTilesInfos != null && !this._visibleTilesInfos.Except<ITileInfo>((IEnumerable<ITileInfo>) this._tileLayer.VisibleTileInfos).Any<ITileInfo>();

    public byte ActiveZoom => this._activeZoom;

    public bool PanoramaAvailable
    {
      get => (bool) ((DependencyObject) this).GetValue(MetaContentLayer.PanoramaAvailableProperty);
      set => ((DependencyObject) this).SetValue(MetaContentLayer.PanoramaAvailableProperty, (object) value);
    }
  }
}
