// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TileContentLayerBase`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Yandex.Collections.Interfaces;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls.ContentLayers;
using Yandex.Maps.Interfaces;
using Yandex.Maps.Repository.Events;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [ComVisible(false)]
  public abstract class TileContentLayerBase<T> : 
    ContentLayerBase,
    ITileContentLayer,
    IContentLayer,
    IContentContainer
    where T : ITile
  {
    private const int TopZIndex = 100;
    protected const double DisposeViewportExtendRatio = 2.0;
    protected const double ReloadViewportExtendRatio = 1.2;
    protected readonly IConfigMediator _configMediator;
    private readonly ITileZoomLayerBuilder<T> _layerBuilder;
    private readonly IDictionary<byte, ITileZoomLayer<T>> _layerRepository;
    private readonly INetworkInterfaceWrapper _networkInterfaceWrapper;
    protected readonly IZoomLevelConverter _zoomLevelConverter;

    protected TileContentLayerBase(
      ITileZoomLayerBuilder<T> layerBuilder,
      IDictionary<byte, ITileZoomLayer<T>> layerRepository,
      IZoomLevelConverter zoomLevelConverter,
      IUiDispatcher uiDispatcher,
      IViewportTileConverter viewportTileConverter,
      IQueue<ILayerCommand> commandQueue,
      IThread thread,
      IConfigMediator configMediator,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper)
      : base(uiDispatcher, viewportTileConverter, commandQueue, thread)
    {
      if (layerBuilder == null)
        throw new ArgumentNullException(nameof (layerBuilder));
      if (layerRepository == null)
        throw new ArgumentNullException(nameof (layerRepository));
      if (zoomLevelConverter == null)
        throw new ArgumentNullException(nameof (zoomLevelConverter));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (networkInterfaceWrapper == null)
        throw new ArgumentNullException(nameof (networkInterfaceWrapper));
      this._layerBuilder = layerBuilder;
      this._layerRepository = layerRepository;
      this._zoomLevelConverter = zoomLevelConverter;
      this._configMediator = configMediator;
      this._networkInterfaceWrapper = networkInterfaceWrapper;
      this._networkInterfaceWrapper.IsNetworkAvailableChanged += new EventHandler(this.NetworkInterfaceWrapperIsNetworkAvailableChanged);
      configMediator.PropertyChanged += new PropertyChangedEventHandler(this.ConfigMediatorPropertyChanged);
      this.ZoomLevelsToDisplaySimultaneously = (int) configMediator.ZoomLevelsToDisplaySimultaneouslyDefault;
    }

    public int ZoomLevelsToDisplaySimultaneously { get; set; }

    protected IDictionary<byte, ITileZoomLayer<T>> LayerRepository => this._layerRepository;

    public FrameworkElement Control => (FrameworkElement) this;

    private void NetworkInterfaceWrapperIsNetworkAvailableChanged(object sender, EventArgs e)
    {
      if (!this._networkInterfaceWrapper.GetIsNetworkAvailable())
        return;
      IList<ITileInfo> visibleTiles = this.GetActiveLayer().VisibleTileInfos;
      if (!this._viewportTileConverter.GetTilesByArea(this.ActiveViewportRect.Rect, (byte) this.ActiveViewportRect.ZoomLevel).Any<ITileInfo>((Func<ITileInfo, bool>) (t => !visibleTiles.Any<ITileInfo>((Func<ITileInfo, bool>) (vt => vt.EqualsCoordinates(t))))))
        return;
      this.Reload(this.ActiveViewportRect);
    }

    protected override void OnDispose()
    {
      this._configMediator.PropertyChanged -= new PropertyChangedEventHandler(this.ConfigMediatorPropertyChanged);
      base.OnDispose();
    }

    private void ConfigMediatorPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "TilesStretchFactor":
          this.Reset();
          break;
      }
    }

    protected override void OnOperationModeChanged(OperationMode newValue)
    {
    }

    protected override void OnReset()
    {
      lock (this._layerRepository)
      {
        foreach (byte zoom in this._layerRepository.Keys.ToArray<byte>())
          this.RemoveLayer(zoom);
      }
    }

    protected void TileManagerTilesReady(object sender, TilesReadyEventArgs<T> e)
    {
      T[] array = e.Tiles.Where<T>((Func<T, bool>) (tile => (tile.Status & TileStatus.Error) != TileStatus.Error)).ToArray<T>();
      if (!((IEnumerable<T>) array).Any<T>())
        return;
      this.EnqueueCommand((ILayerCommand) new TilesReadyLayerCommand<T>((IList<T>) array));
    }

    private ITileZoomLayer<T> AddLayer(byte zoom)
    {
      ITileZoomLayer<T> layer = this.CreateLayer(zoom);
      this._layerRepository.Add(zoom, layer);
      layer.Scale = 1.0 / this._zoomLevelConverter.SafeZoomLevelToStretchFactor((double) zoom);
      Canvas.SetZIndex(layer.Control, (int) zoom);
      ((PresentationFrameworkCollection<UIElement>) this.Children).Add(layer.Control);
      return layer;
    }

    protected virtual ITileZoomLayer<T> CreateLayer(byte zoom) => this._layerBuilder.CreateLayer(zoom);

    protected void RemoveLayer(byte zoom)
    {
      ITileZoomLayer<T> layer;
      if (!this._layerRepository.TryGetValue(zoom, out layer))
        return;
      this._layerRepository.Remove(zoom);
      this._uiDispatcher.BeginInvoke((Action) (() => ((PresentationFrameworkCollection<UIElement>) this.Children).Remove(layer.Control)));
      layer.DisposeTiles();
    }

    protected override void OnDisposeContent()
    {
      foreach (IVisibleTileLayer<T> visibleTileLayer in (IEnumerable<ITileZoomLayer<T>>) this._layerRepository.Values)
        visibleTileLayer.DisposeTiles();
    }

    protected override void OnDisposeContentOutsideArea(ViewportRect viewportRect)
    {
      byte zoom = ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel);
      lock (this._layerRepository)
      {
        foreach (byte zoom1 in this._layerRepository.Values.Where<ITileZoomLayer<T>>((Func<ITileZoomLayer<T>, bool>) (l => (int) l.Zoom > (int) zoom || (int) l.Zoom < (int) zoom - this.ZoomLevelsToDisplaySimultaneously)).Select<ITileZoomLayer<T>, byte>((Func<ITileZoomLayer<T>, byte>) (l => l.Zoom)).ToArray<byte>())
          this.RemoveLayer(zoom1);
      }
    }

    protected void DoZoom(ITileZoomLayer<T> activeLayer)
    {
      byte zoom1 = activeLayer != null ? activeLayer.Zoom : throw new ArgumentNullException(nameof (activeLayer));
      this.DoZoom(zoom1);
      IEnumerable<ITileZoomLayer<T>> tileZoomLayers = (IEnumerable<ITileZoomLayer<T>>) this.GetActiveLayers(zoom1).ToList<ITileZoomLayer<T>>();
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        Canvas.SetZIndex(activeLayer.Control, 100);
        foreach (ITileZoomLayer<T> tileZoomLayer in tileZoomLayers)
        {
          byte zoom2 = tileZoomLayer.Zoom;
          if (tileZoomLayer != activeLayer)
            Canvas.SetZIndex(tileZoomLayer.Control, (int) zoom2);
        }
      }));
    }

    [NotNull]
    protected ITileZoomLayer<T> GetLayer(byte zoom)
    {
      ITileZoomLayer<T> layer;
      if (this._layerRepository.TryGetValue(zoom, out layer))
        return layer;
      lock (this._layerRepository)
      {
        if (this._layerRepository.TryGetValue(zoom, out layer))
          return layer;
        ITileZoomLayer<T> newLayer = (ITileZoomLayer<T>) null;
        this._uiDispatcher.Invoke((SendOrPostCallback) (argument => newLayer = this.AddLayer(zoom)), (object) null);
        return newLayer;
      }
    }

    protected ITileZoomLayer<T> GetActiveLayer() => this.GetLayer(this._activeZoom);

    private IEnumerable<ITileZoomLayer<T>> GetActiveLayers(byte zoom) => this._layerRepository.Values.Where<ITileZoomLayer<T>>((Func<ITileZoomLayer<T>, bool>) (layer => (int) layer.Zoom <= (int) zoom + 1 || (int) layer.Zoom > (int) zoom - 2));
  }
}
