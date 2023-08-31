// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TrafficContentLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Collections.Interfaces;
using Yandex.Common;
using Yandex.Ioc;
using Yandex.Maps.API;
using Yandex.Maps.API.Extensions;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Controls.ContentLayers;
using Yandex.Maps.Controls.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Media;
using Yandex.Patterns;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [ComVisible(false)]
  public class TrafficContentLayer : 
    TileContentLayerBase<IJamTile>,
    ITrafficContentLayer,
    ITileContentLayer,
    IContentLayer,
    IContentContainer
  {
    private const BaseLayers TrafficLayer = BaseLayers.trf;
    private static readonly TimeSpan ScheduledJobTimeout = TimeSpan.FromSeconds(60.0);
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly IJamManager _jamManager;
    private readonly IPrinterManager _printerManager;
    private readonly IJamRender _jamRender;
    private readonly ITrafficTileZoomLayerBuilder _layerBuilder;
    private readonly ITileManager<IJamTile> _tileManager;
    private readonly IViewportPointConveter _viewportPointConverter;
    private Timer _scheduleJobTimer;
    private bool _enabled;
    private ViewportRect? _lastViewportRect;
    private byte? _trafficValue;

    public TrafficContentLayer()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<ITrafficTileZoomLayerBuilder>(), IocSingleton<ControlsIocInitializer>.Resolve<IDictionary<byte, ITileZoomLayer<IJamTile>>>(), IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IZoomLevelConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IViewportTileConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IQueue<ILayerCommand>>(), IocSingleton<ControlsIocInitializer>.Resolve<IThread>(), IocSingleton<ControlsIocInitializer>.Resolve<ITileManager<IJamTile>>(), IocSingleton<ControlsIocInitializer>.Resolve<IGeoPixelConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>(), IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>(), IocSingleton<ControlsIocInitializer>.Resolve<IJamManager>(), IocSingleton<ControlsIocInitializer>.Resolve<IPrinterManager>(), IocSingleton<ControlsIocInitializer>.Resolve<INetworkInterfaceWrapper>(), IocSingleton<ControlsIocInitializer>.Resolve<IJamRender>())
    {
    }

    public TrafficContentLayer(
      [NotNull] ITrafficTileZoomLayerBuilder layerBuilder,
      IDictionary<byte, ITileZoomLayer<IJamTile>> layerRepository,
      IUiDispatcher uiDispatcher,
      IZoomLevelConverter zoomLevelConverter,
      IViewportTileConverter viewportTileConverter,
      IQueue<ILayerCommand> commandQueue,
      IThread thread,
      ITileManager<IJamTile> tileManager,
      IGeoPixelConverter geoPixelConverter,
      IViewportPointConveter viewportPointConverter,
      IConfigMediator configMediator,
      IJamManager jamManager,
      [NotNull] IPrinterManager printerManager,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper,
      [NotNull] IJamRender jamRender)
      : base((ITileZoomLayerBuilder<IJamTile>) layerBuilder, layerRepository, zoomLevelConverter, uiDispatcher, viewportTileConverter, commandQueue, thread, configMediator, networkInterfaceWrapper)
    {
      if (layerBuilder == null)
        throw new ArgumentNullException(nameof (layerBuilder));
      if (tileManager == null)
        throw new ArgumentNullException(nameof (tileManager));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      if (viewportPointConverter == null)
        throw new ArgumentNullException(nameof (viewportPointConverter));
      if (printerManager == null)
        throw new ArgumentNullException(nameof (printerManager));
      if (jamRender == null)
        throw new ArgumentNullException(nameof (jamRender));
      this._layerBuilder = layerBuilder;
      this._tileManager = tileManager;
      this._tileManager.TilesReady += new EventHandler<TilesReadyEventArgs<IJamTile>>(((TileContentLayerBase<IJamTile>) this).TileManagerTilesReady);
      this._geoPixelConverter = geoPixelConverter;
      this._viewportPointConverter = viewportPointConverter;
      this._jamManager = jamManager;
      this._printerManager = printerManager;
      this._jamRender = jamRender;
      this._jamManager.ExecuteWhenReady(new Action(this.TryReload));
      this._printerManager.ExecuteWhenReady(new Action(this.TryInitializeJamManager));
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.TrafficContentLayerLoaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.TrafficContentLayerUnloaded);
      this._scheduleJobTimer = new Timer((TimerCallback) (state => this.OnScheduledJob()), (object) null, TrafficContentLayer.ScheduledJobTimeout, TrafficContentLayer.ScheduledJobTimeout);
    }

    private void TrafficContentLayerUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._scheduleJobTimer == null)
        return;
      this._scheduleJobTimer.Change(-1, -1);
    }

    private void TrafficContentLayerLoaded(object sender, RoutedEventArgs e)
    {
      if (this._scheduleJobTimer == null)
        return;
      this._scheduleJobTimer.Change(TrafficContentLayer.ScheduledJobTimeout, TrafficContentLayer.ScheduledJobTimeout);
    }

    private void TryInitializeJamManager()
    {
      if (this.OperationMode <= OperationMode.None || this._jamManager.State == ServiceState.Ready || this._jamManager.State > ServiceState.Stopped)
        return;
      this._jamManager.Connect();
    }

    public void ResetCachedTileBitmaps() => this._tileManager.ResetCachedTileBitmaps();

    public event EventHandler<TrafficValueChangedArgs> TrafficValueChanged;

    public bool Enabled
    {
      get => this._enabled;
      set
      {
        this._enabled = value;
        this.RemoveExtraZoomLevels(ContentLayerBase.ZoomLevelToZoom(this.ActiveViewportRect.ZoomLevel));
        this.UpdateJamTileManagerOperationMode();
      }
    }

    private void TryReload()
    {
      if (!this.Enabled || !this._lastViewportRect.HasValue)
        return;
      this.EnqueueCommand((ILayerCommand) new LayerCommand(LayerCommandTypes.Reload, this._lastViewportRect.Value));
    }

    protected override void OnOperationModeChanged(OperationMode newValue)
    {
      base.OnOperationModeChanged(newValue);
      if (this._printerManager.State == ServiceState.Ready)
        this.TryInitializeJamManager();
      this.UpdateJamTileManagerOperationMode();
    }

    protected override void OnReset()
    {
      base.OnReset();
      this._tileManager.Flush();
    }

    private void Reload(ViewportRect viewportRect, bool invalidateViewport)
    {
      this._lastViewportRect = new ViewportRect?(viewportRect);
      byte zoom = ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel);
      this.UpdateTrafficValue();
      ITileZoomLayer<IJamTile> layer = this.GetLayer(zoom);
      IEnumerable<ITileInfo> first = this._viewportTileConverter.ViewportRectToTiles(viewportRect.Expand(1.2), zoom).Select<ITileInfo, ITileInfo>((Func<ITileInfo, ITileInfo>) (tileInfo => (ITileInfo) new TileInfo(tileInfo.X, tileInfo.Y, tileInfo.Zoom, BaseLayers.trf)));
      List<ITileInfo> requestMapTiles = (invalidateViewport ? first : first.Except<ITileInfo>((IEnumerable<ITileInfo>) layer.VisibleTileInfos)).ToList<ITileInfo>();
      if (invalidateViewport)
        this._tileManager.RemoveTiles((Func<IJamTile, bool>) (tile => requestMapTiles.IndexOf(tile.TileInfo) != 0));
      this._tileManager.CancelQueryingTilesExcept((IList<ITileInfo>) requestMapTiles);
      if (!requestMapTiles.Any<ITileInfo>())
        return;
      this._tileManager.QueryTiles((IEnumerable<ITileInfo>) requestMapTiles);
    }

    private void RemoveExtraZoomLevels(byte zoomToKeep)
    {
      foreach (byte zoom in this.LayerRepository.Select<KeyValuePair<byte, ITileZoomLayer<IJamTile>>, byte>((Func<KeyValuePair<byte, ITileZoomLayer<IJamTile>>, byte>) (item => item.Key)).Where<byte>((Func<byte, bool>) (key => (int) key != (int) zoomToKeep)).ToArray<byte>())
        this.RemoveLayer(zoom);
    }

    private void UpdateTrafficValue()
    {
      IEnumerable<IJamTile> list = (IEnumerable<IJamTile>) this.GetVisibleTiles().ToList<IJamTile>();
      if (!list.Any<IJamTile>())
        return;
      IEnumerable<TrafficInformer> activeInformers = TrafficContentLayer.GetActiveInformers(list);
      Point relativePoint1 = this._viewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(new Point(this.ActiveViewportRect.Rect.Left + this.ActiveViewportRect.Rect.Width * 0.5, this.ActiveViewportRect.Rect.Top + this.ActiveViewportRect.Rect.Height * 0.5), this.ActiveViewportRect.ZoomLevel));
      byte? nullable1 = new byte?();
      if (activeInformers != null)
      {
        double num1 = double.MaxValue;
        foreach (TrafficInformer trafficInformer in activeInformers)
        {
          Point relativePoint2 = this._geoPixelConverter.CoordinatesToRelativePoint(trafficInformer.Position);
          double num2 = (relativePoint2.X - relativePoint1.X) * (relativePoint2.X - relativePoint1.X) + (relativePoint2.Y - relativePoint1.Y) * (relativePoint2.Y - relativePoint1.Y);
          if (num1 > num2)
          {
            num1 = num2;
            nullable1 = trafficInformer.HasColor ? new byte?((byte) trafficInformer.Value) : new byte?();
          }
        }
      }
      byte? trafficValue = this._trafficValue;
      byte? nullable2 = nullable1;
      if (((int) trafficValue.GetValueOrDefault() != (int) nullable2.GetValueOrDefault() ? 0 : (trafficValue.HasValue == nullable2.HasValue ? 1 : 0)) != 0)
        return;
      this._trafficValue = nullable1;
      this._uiDispatcher.BeginInvoke((Action) (() => this.OnTrafficValueChanged(new TrafficValueChangedArgs(this._trafficValue))));
    }

    private IEnumerable<IJamTile> GetVisibleTiles()
    {
      ITileZoomLayer<IJamTile> layer1 = this.GetLayer(ContentLayerBase.ZoomLevelToZoom(this.ActiveViewportRect.ZoomLevel));
      return !layer1.VisibleTiles.Any<IJamTile>() ? this.LayerRepository.Values.SelectMany<ITileZoomLayer<IJamTile>, IJamTile>((Func<ITileZoomLayer<IJamTile>, IEnumerable<IJamTile>>) (layer => (IEnumerable<IJamTile>) layer.VisibleTiles)) : (IEnumerable<IJamTile>) layer1.VisibleTiles;
    }

    private static IEnumerable<TrafficInformer> GetActiveInformers(
      IEnumerable<IJamTile> visibleTiles)
    {
      DateTime utcNow = DateTime.UtcNow;
      return visibleTiles.SelectMany<IJamTile, TrafficInformer>((Func<IJamTile, IEnumerable<TrafficInformer>>) (tile => tile.Informers.Where<TrafficInformer>((Func<TrafficInformer, bool>) (informer => informer.ExpiresAt > utcNow)))).Distinct<TrafficInformer>();
    }

    private void UpdateJamTileManagerOperationMode()
    {
      if (this.Enabled)
      {
        this._tileManager.OperationMode = this.OperationMode;
        this._jamRender.OperationMode = this.OperationMode;
      }
      else
      {
        this._tileManager.Flush();
        this._tileManager.OperationMode = OperationMode.Disabled;
        this._jamRender.OperationMode = OperationMode.Disabled;
      }
    }

    public void OnTrafficValueChanged(TrafficValueChangedArgs e)
    {
      EventHandler<TrafficValueChangedArgs> trafficValueChanged = this.TrafficValueChanged;
      if (trafficValueChanged == null)
        return;
      trafficValueChanged((object) this, e);
    }

    public override void Reload(ViewportRect viewportRect)
    {
      this._lastViewportRect = new ViewportRect?(viewportRect);
      if (!this.Enabled || this._jamManager.State != ServiceState.Ready)
        return;
      base.Reload(viewportRect);
    }

    protected override void OnDisposeContentOutsideArea(ViewportRect viewportRect)
    {
      this.RemoveExtraZoomLevels(ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel));
      foreach (ITileZoomLayer<IJamTile> tileZoomLayer in (IEnumerable<ITileZoomLayer<IJamTile>>) this.LayerRepository.Values)
      {
        List<ITileInfo> list = this._viewportTileConverter.ViewportRectToTiles(viewportRect.Expand(2.0), tileZoomLayer.Zoom).ToList<ITileInfo>();
        tileZoomLayer.DisposeTilesExcept((IList<ITileInfo>) list);
      }
    }

    protected override void OnCommand(ILayerCommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (command.Type == LayerCommandTypes.Custom && this.OperationMode == OperationMode.Full)
      {
        if (command is TilesReadyLayerCommand<IJamTile> readyLayerCommand)
          this.OnTilesReady(readyLayerCommand.ReadyTiles);
        if (command is RefreshTilesLayerCommand tilesLayerCommand)
          this.Reload(tilesLayerCommand.ViewportRect, true);
      }
      base.OnCommand(command);
    }

    protected virtual void OnTilesReady(IList<IJamTile> readyTiles)
    {
      byte zoom = ContentLayerBase.ZoomLevelToZoom(this.ActiveViewportRect.ZoomLevel);
      foreach (ITileZoomLayer<IJamTile> tileZoomLayer in (IEnumerable<ITileZoomLayer<IJamTile>>) this.LayerRepository.Values)
      {
        if ((int) tileZoomLayer.Zoom == (int) zoom)
          tileZoomLayer.DisplayTiles(readyTiles);
      }
      this.UpdateTrafficValue();
    }

    protected override void OnReload(ViewportRect viewportRect)
    {
      base.OnReload(viewportRect);
      this.RemoveExtraZoomLevels(ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel));
      this.Reload(viewportRect, false);
    }

    private void OnScheduledJob() => this.EnqueueCommand((ILayerCommand) new RefreshTilesLayerCommand(this.ActiveViewportRect));

    protected override void OnDispose()
    {
      this._tileManager.TilesReady -= new EventHandler<TilesReadyEventArgs<IJamTile>>(((TileContentLayerBase<IJamTile>) this).TileManagerTilesReady);
      if (this._scheduleJobTimer != null)
        this._scheduleJobTimer.Dispose();
      this._scheduleJobTimer = (Timer) null;
      base.OnDispose();
      this.OnDisposeContent();
    }

    protected override ITileZoomLayer<IJamTile> CreateLayer(byte zoom) => this._layerBuilder.CreateLayer(zoom, this._jamRender);
  }
}
