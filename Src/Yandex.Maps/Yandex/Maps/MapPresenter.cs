// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapPresenter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using JetBrains.Annotations;
using System;
using System.ComponentModel;
using Yandex.Input.Events;
using Yandex.Input.Interfaces;
using Yandex.Ioc;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps
{
  internal class MapPresenter : 
    MapPresenterBase,
    IMapPresenter,
    IMapPresenterBase,
    INotifyPropertyChanged,
    IDisposable,
    IMap,
    IMapState
  {
    internal const string UseLocationPropertyName = "UseLocation";
    private IMetaContentLayer _metaContentLayer;
    private readonly IPositionWatcherWrapper _positionWatcher;
    private ITrafficContentLayer _trafficContentLayer;
    private readonly IMapView _view;
    private readonly IManipulationWrapper _manipulationWrapper;
    private ITouchHandler _touchHandler;
    private bool _isTrafficEnabled;

    public MapPresenter(IMapView view)
      : this(view, IocSingleton<ControlsIocInitializer>.Resolve<IPositionDispatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IManipulationWrapper>())
    {
    }

    protected MapPresenter(
      IMapView view,
      IPositionDispatcher positionDispatcher,
      [NotNull] IManipulationWrapper manipulationWrapper)
      : base((IMapViewBase) view, (IPositionDispatcherBase) positionDispatcher, manipulationWrapper)
    {
      if (view == null)
        throw new ArgumentNullException(nameof (view));
      if (manipulationWrapper == null)
        throw new ArgumentNullException(nameof (manipulationWrapper));
      this._view = view;
      this._manipulationWrapper = manipulationWrapper;
      this.IsUserInteractionEnabled = true;
      this._positionWatcher = IocSingleton<ControlsIocInitializer>.Resolve<IPositionWatcherWrapper>();
      if (this._positionWatcher is IManipulationWrapperInitializable positionWatcher)
        positionWatcher.Initialize(this._manipulationWrapper);
      this.Config.PropertyChanged += new PropertyChangedEventHandler(this.ConfigPropertyChanged);
    }

    private IPositionDispatcher PositionDispatcher => (IPositionDispatcher) this._positionDispatcher;

    private void ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "UseLocation":
          this.OnPropertyChanged("UseLocation");
          break;
      }
    }

    protected override bool TryInitializeView()
    {
      if (!base.TryInitializeView())
        return false;
      if (this._trafficContentLayer != null)
        this._trafficContentLayer.TrafficValueChanged -= new EventHandler<TrafficValueChangedArgs>(this.TrafficContentLayerTrafficValueChanged);
      this._trafficContentLayer = this._view.TrafficContentLayer;
      if (this._trafficContentLayer != null)
      {
        this._trafficContentLayer.TrafficValueChanged += new EventHandler<TrafficValueChangedArgs>(this.TrafficContentLayerTrafficValueChanged);
        this.UpdateTrafficContentLayerVisibility(this.IsTrafficEnabled);
      }
      if (this._metaContentLayer != null)
        this._metaContentLayer.TrafficAvailbleChanged -= new EventHandler<TrafficAvailableChangedArgs>(this.MetaContentLayerTrafficAvailbleChanged);
      this._metaContentLayer = this._view.MetaContentLayer;
      if (this._metaContentLayer != null)
        this._metaContentLayer.TrafficAvailbleChanged += new EventHandler<TrafficAvailableChangedArgs>(this.MetaContentLayerTrafficAvailbleChanged);
      this._touchHandler = this._view.TouchHandler;
      if (this._touchHandler != null)
      {
        if (this._manipulationWrapper is ITouchHandlerInitializable manipulationWrapper)
          manipulationWrapper.Initialize(this._touchHandler);
        this._touchHandler.MultiTap += new EventHandler<MultiTapEventArgs>(this.TouchHandlerMultiTap);
        this._touchHandler.DoubleTap += new EventHandler<DoubleTapEventArgs>(this.TouchHandlerDoubleTap);
      }
      return true;
    }

    protected override void OnEnsureScroll() => this._positionWatcher.DisablePositionFollowing();

    public event EventHandler<TrafficAvailableChangedArgs> TrafficAvailbleChanged;

    public event EventHandler<TrafficValueChangedArgs> TrafficValueChanged;

    public GeoPositionStatus JumpToCurrentLocation()
    {
      if (!this.UseLocation)
        return GeoPositionStatus.NoData;
      if (this._positionWatcher.Status != GeoPositionStatus.Ready)
        return this._positionWatcher.Status;
      this.PositionDispatcher.JumpToCurrentLocation();
      return GeoPositionStatus.Ready;
    }

    public bool IsTrafficEnabled
    {
      get => this._isTrafficEnabled;
      set
      {
        this._isTrafficEnabled = value;
        this.UpdateTrafficContentLayerVisibility(value);
      }
    }

    private void UpdateTrafficContentLayerVisibility(bool enabled)
    {
      if (this._trafficContentLayer == null)
        return;
      this._trafficContentLayer.Enabled = enabled;
      if (enabled)
      {
        if (this._trafficContentLayer.Control.GetVisualParent() == null)
          this.LayerManager.AddTileContentLayer((ITileContentLayer) this._trafficContentLayer);
        this.ReloadTrafficContentLayer();
      }
      else
      {
        this.LayerManager.RemoveTileContentLayer((ITileContentLayer) this._trafficContentLayer);
        this._trafficContentLayer.DisposeContent();
      }
    }

    protected override void OnPrinterConnected()
    {
      base.OnPrinterConnected();
      if (!this.IsTrafficEnabled || this._trafficContentLayer == null)
        return;
      this.ReloadTrafficContentLayer();
    }

    public void ZoomIn()
    {
      this.DisposeCurrentTrafficContentLayer();
      this.ZoomIn(new Point?());
    }

    public void ZoomOut()
    {
      this.DisposeCurrentTrafficContentLayer();
      this.ZoomOut(new Point?());
    }

    protected override void OnTwilightModeChanged()
    {
      base.OnTwilightModeChanged();
      this.DisposeCurrentTrafficContentLayer();
      if (!this.IsTrafficEnabled || this._trafficContentLayer == null)
        return;
      this._trafficContentLayer.ResetCachedTileBitmaps();
      this.ReloadTrafficContentLayer();
    }

    private void ReloadTrafficContentLayer() => this._trafficContentLayer.Reload(this.GetTargetViewportRect());

    public bool UseLocation
    {
      get => this.PositionDispatcher.IsPositionWatchingEnabled;
      set => this.PositionDispatcher.IsPositionWatchingEnabled = value;
    }

    private void OnTrafficAvailbleChanged(TrafficAvailableChangedArgs e)
    {
      EventHandler<TrafficAvailableChangedArgs> trafficAvailbleChanged = this.TrafficAvailbleChanged;
      if (trafficAvailbleChanged == null)
        return;
      trafficAvailbleChanged((object) this, e);
    }

    private void OnTrafficValueChanged(TrafficValueChangedArgs e)
    {
      EventHandler<TrafficValueChangedArgs> trafficValueChanged = this.TrafficValueChanged;
      if (trafficValueChanged == null)
        return;
      trafficValueChanged((object) this, e);
    }

    private void TrafficContentLayerTrafficValueChanged(object sender, TrafficValueChangedArgs e) => this.OnTrafficValueChanged(e);

    private void MetaContentLayerTrafficAvailbleChanged(
      object sender,
      TrafficAvailableChangedArgs e)
    {
      this.OnTrafficAvailbleChanged(e);
    }

    private void DisposeCurrentTrafficContentLayer()
    {
      if (!this.IsTrafficEnabled || this._trafficContentLayer == null)
        return;
      this._trafficContentLayer.DisposeContent();
    }

    protected override void OnPositionManagerIdle()
    {
      base.OnPositionManagerIdle();
      this._uiDispatcher.BeginInvoke((Action) (() => this._view.BackgroundTranslationEnabled = true));
    }

    protected override void OnPropertyChanged(string propertyName)
    {
      if (propertyName == "InstantZoomLevel")
        this._uiDispatcher.BeginInvoke((Action) (() => this._view.BackgroundTranslationEnabled = false));
      base.OnPropertyChanged(propertyName);
    }

    private void TouchHandlerDoubleTap(object sender, DoubleTapEventArgs e)
    {
      this.DisposeCurrentTrafficContentLayer();
      this.ZoomIn(new Point?(e.Origin));
    }

    private void TouchHandlerMultiTap(object sender, MultiTapEventArgs e)
    {
      this.DisposeCurrentTrafficContentLayer();
      this.ZoomOut(new Point?(e.Origin));
    }
  }
}
