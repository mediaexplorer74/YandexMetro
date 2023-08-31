// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Map
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Yandex.Controls;
using Yandex.DevUtils;
using Yandex.Input;
using Yandex.Input.Interfaces;
using Yandex.Ioc;
using Yandex.ItemsCounter;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Media;
using Yandex.Positioning;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  [TemplatePart(Name = "PositionControlTransform", Type = typeof (CompositeTransform))]
  [TemplatePart(Name = "PositionAccuracyControlTransform", Type = typeof (CompositeTransform))]
  [TemplatePart(Name = "PositionControl", Type = typeof (PositionControl))]
  [TemplatePart(Name = "LayerManager", Type = typeof (LayerManager))]
  [TemplatePart(Name = "MapContentLayer", Type = typeof (Yandex.Maps.ContentLayers.MapContentLayer))]
  [TemplatePart(Name = "TrafficContentLayer", Type = typeof (Yandex.Maps.ContentLayers.TrafficContentLayer))]
  [TemplatePart(Name = "MetaContentLayer", Type = typeof (Yandex.Maps.ContentLayers.MetaContentLayer))]
  [TemplatePart(Name = "TouchWrapper", Type = typeof (TouchWrapper))]
  [TemplatePart(Name = "BackgroundLayer", Type = typeof (TiledImagePanel))]
  [TemplatePart(Name = "LayoutRoot", Type = typeof (Grid))]
  [TemplatePart(Name = "PositionAccuracyControl", Type = typeof (PositionAccuracyControl))]
  [ComVisible(false)]
  public class Map : MapBase, IMapView, IMapViewBase, IMapState, ICompositeTransformation
  {
    private const string LayerManagerName = "LayerManager";
    private const string BackgroundLayerName = "BackgroundLayer";
    private const string MapContentLayerName = "MapContentLayer";
    private const string TrafficContentLayerName = "TrafficContentLayer";
    private const string PositionControlName = "PositionControl";
    private const string PositionControlTransformName = "PositionControlTransform";
    private const string PositionAccuracyControlName = "PositionAccuracyControl";
    private const string PositionAccuracyControlTransformName = "PositionAccuracyControlTransform";
    private const string TouchWrapperName = "TouchWrapper";
    private const string LayoutRootName = "LayoutRoot";
    private const string MetaContentLayerName = "MetaContentLayer";
    public static readonly DependencyProperty TrafficEnabledProperty = DependencyProperty.Register(nameof (TrafficEnabled), typeof (bool), typeof (Map), new PropertyMetadata((object) false, new PropertyChangedCallback(Map.TrafficEnabledChanged)));
    public static readonly DependencyProperty UseLocationProperty = DependencyProperty.Register(nameof (UseLocation), typeof (bool), typeof (MapBase), new PropertyMetadata((object) false, new PropertyChangedCallback(Map.UseLocationChanged)));
    public static readonly DependencyProperty FixedContentTemplateProperty = DependencyProperty.Register(nameof (FixedContentTemplate), typeof (DataTemplate), typeof (Map), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PlainViewPortProperty = DependencyProperty.Register(nameof (PlainViewPort), typeof (Rect), typeof (Map), new PropertyMetadata((object) new Rect()));
    internal static readonly DependencyProperty FixedContentProperty = DependencyProperty.Register(nameof (FixedContent), typeof (object), typeof (Map), new PropertyMetadata((object) null));
    internal static readonly DependencyProperty HorizontalFixedContentAlignmentProperty = DependencyProperty.Register(nameof (HorizontalFixedContentAlignment), typeof (HorizontalAlignment), typeof (Map), new PropertyMetadata((object) (HorizontalAlignment) 0));
    internal static readonly DependencyProperty VerticalFixedContentAlignmentProperty = DependencyProperty.Register(nameof (VerticalFixedContentAlignment), typeof (VerticalAlignment), typeof (Map), new PropertyMetadata((object) (VerticalAlignment) 0));
    internal static readonly DependencyProperty FixedPaddingProperty = DependencyProperty.Register(nameof (FixedPadding), typeof (Thickness), typeof (Map), new PropertyMetadata((object) new Thickness()));
    internal static readonly DependencyProperty RouteGuidanceEnabledProperty = DependencyProperty.Register(nameof (RouteGuidanceEnabled), typeof (bool), typeof (Map), new PropertyMetadata((object) false));
    internal static readonly DependencyProperty RoutingEnabledProperty = DependencyProperty.Register(nameof (RoutingEnabled), typeof (bool), typeof (Map), new PropertyMetadata((object) true));
    internal static readonly DependencyProperty PanoramaAvailableProperty = DependencyProperty.Register(nameof (PanoramaAvailable), typeof (bool), typeof (Map), (PropertyMetadata) null);
    private static readonly IItemCounter _itemCounter;
    private TiledImagePanel _backgroundLayer;
    private bool _backgroundTranslationEnabled;
    private LayerManager _layerManager;
    private IMapPresenter _mapPresenter;
    private CompositeTransform _positionControlTransform;
    private CompositeTransform _positionAccuracyControlTransform;
    private readonly IPositionWatcherWrapper _positionWatcherWrapper;
    private readonly IUiDispatcher _uiDispatcher;
    private readonly IGeoPixelConverter _geoPixelConverter;
    private Panel _layoutRoot;
    private PositionControl _positionControl;
    private PositionAccuracyControl _positionAccuracyControl;
    private ITouchWrapper _touchWrapper;

    public DataTemplate FixedContentTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(Map.FixedContentTemplateProperty);
      set => ((DependencyObject) this).SetValue(Map.FixedContentTemplateProperty, (object) value);
    }

    internal object FixedContent
    {
      get => ((DependencyObject) this).GetValue(Map.FixedContentProperty);
      set => ((DependencyObject) this).SetValue(Map.FixedContentProperty, value);
    }

    internal HorizontalAlignment HorizontalFixedContentAlignment
    {
      get => (HorizontalAlignment) ((DependencyObject) this).GetValue(Map.HorizontalFixedContentAlignmentProperty);
      set => ((DependencyObject) this).SetValue(Map.HorizontalFixedContentAlignmentProperty, (object) value);
    }

    internal VerticalAlignment VerticalFixedContentAlignment
    {
      get => (VerticalAlignment) ((DependencyObject) this).GetValue(Map.VerticalFixedContentAlignmentProperty);
      set => ((DependencyObject) this).SetValue(Map.VerticalFixedContentAlignmentProperty, (object) value);
    }

    internal Thickness FixedPadding
    {
      get => (Thickness) ((DependencyObject) this).GetValue(Map.FixedPaddingProperty);
      set => ((DependencyObject) this).SetValue(Map.FixedPaddingProperty, (object) value);
    }

    internal bool RouteGuidanceEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(Map.RouteGuidanceEnabledProperty);
      set => ((DependencyObject) this).SetValue(Map.RouteGuidanceEnabledProperty, (object) value);
    }

    internal bool RoutingEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(Map.RoutingEnabledProperty);
      set => ((DependencyObject) this).SetValue(Map.RoutingEnabledProperty, (object) value);
    }

    internal bool PanoramaAvailable
    {
      get => (bool) ((DependencyObject) this).GetValue(Map.PanoramaAvailableProperty);
      set => ((DependencyObject) this).SetValue(Map.PanoramaAvailableProperty, (object) value);
    }

    [Obsolete("Use MapGlobalSettings.Instance.EnableLocationService instead.")]
    public static bool EnableLocationService
    {
      get => MapGlobalSettings.Instance.EnableLocationService;
      set => MapGlobalSettings.Instance.EnableLocationService = value;
    }

    static Map()
    {
      try
      {
        Map._itemCounter = IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<IItemCounter>();
      }
      catch (Exception ex)
      {
      }
    }

    public Map()
      : this(IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<IPositionWatcherWrapper>(), IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<IUiDispatcher>(), IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<IGeoPixelConverter>(), IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<ITouchHandler>())
    {
    }

    private Map(
      [NotNull] IPositionWatcherWrapper positionWatcherWrapper,
      [NotNull] IUiDispatcher uiDispatcher,
      [NotNull] IGeoPixelConverter geoPixelConverter,
      [NotNull] ITouchHandler touchHandler)
    {
      ((Control) this).DefaultStyleKey = (object) typeof (Map);
      if (DesignerProperties.IsInDesignMode)
        return;
      if (positionWatcherWrapper == null)
        throw new ArgumentNullException(nameof (positionWatcherWrapper));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      this.TouchHandler = touchHandler != null ? touchHandler : throw new ArgumentNullException(nameof (touchHandler));
      touchHandler.Control = (object) this;
      this._positionWatcherWrapper = positionWatcherWrapper;
      this._uiDispatcher = uiDispatcher;
      this._geoPixelConverter = geoPixelConverter;
      ((FrameworkElement) this).LayoutUpdated += new EventHandler(this.MapLayoutUpdated);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.MapLoaded);
    }

    private void MapLoaded(object sender, RoutedEventArgs e) => this.OnLoaded();

    private void MapLayoutUpdated(object sender, EventArgs e) => this.OnLayoutUpdated();

    public event EventHandler LayoutUpdated;

    public event EventHandler Loaded;

    private void OnLoaded()
    {
      EventHandler loaded = this.Loaded;
      if (loaded == null)
        return;
      loaded((object) this, EventArgs.Empty);
    }

    private void OnLayoutUpdated()
    {
      EventHandler layoutUpdated = this.LayoutUpdated;
      if (layoutUpdated == null)
        return;
      layoutUpdated((object) this, EventArgs.Empty);
    }

    public static bool IsDownloading => Map._itemCounter != null && Map._itemCounter.Count != 0L;

    internal bool IsRoutingEnabledInCoordinates(GeoCoordinate coordinates) => this.MetaContentLayer.IsRoutingEnabledInCoordinates(coordinates);

    private static void UseLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      ((Map) d).OnUseLocationChanged(e);
    }

    protected virtual void OnUseLocationChanged(DependencyPropertyChangedEventArgs e) => this.OnUseLocationChanged((bool) e.NewValue);

    private void OnUseLocationChanged(bool value)
    {
      if (this._mapPresenter != null)
        this._mapPresenter.UseLocation = value;
      if (value)
      {
        int num = Map.EnableLocationService ? 1 : 0;
      }
      if (this._positionControl != null)
      {
        if (value)
        {
          ((UIElement) this._positionControl).Visibility = (Visibility) 0;
        }
        else
        {
          ((UIElement) this._positionControl).Visibility = (Visibility) 1;
          this._positionControl.HidePosition();
        }
      }
      if (this._positionAccuracyControl != null)
      {
        if (value)
        {
          ((UIElement) this._positionAccuracyControl).Visibility = (Visibility) 0;
        }
        else
        {
          ((UIElement) this._positionAccuracyControl).Visibility = (Visibility) 1;
          this._positionAccuracyControl.HidePosition();
        }
      }
      ((UIElement) this).InvalidateMeasure();
    }

    public bool TrafficEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(Map.TrafficEnabledProperty);
      set => ((DependencyObject) this).SetValue(Map.TrafficEnabledProperty, (object) value);
    }

    public override LayerManager LayerManager => this._layerManager;

    public ITrafficContentLayer TrafficContentLayer { get; private set; }

    public IMetaContentLayer MetaContentLayer { get; private set; }

    [CanBeNull]
    public ITouchHandler TouchHandler { get; private set; }

    public IMapContentLayer MapContentLayer { get; private set; }

    bool IMapView.BackgroundTranslationEnabled
    {
      get => this._backgroundTranslationEnabled;
      set
      {
        if (this._backgroundLayer == null || value == this._backgroundTranslationEnabled)
          return;
        if (value)
          this.UpdateBackground();
        ((UIElement) this._backgroundLayer).Visibility = value ? (Visibility) 0 : (Visibility) 1;
        this._backgroundTranslationEnabled = value;
      }
    }

    public bool UseLocation
    {
      get => (bool) ((DependencyObject) this).GetValue(Map.UseLocationProperty);
      set => ((DependencyObject) this).SetValue(Map.UseLocationProperty, (object) value);
    }

    public Rect PlainViewPort
    {
      get => (Rect) ((DependencyObject) this).GetValue(Map.PlainViewPortProperty);
      set => ((DependencyObject) this).SetValue(Map.PlainViewPortProperty, (object) value);
    }

    public override void UpdateProjection()
    {
      base.UpdateProjection();
      if (this._backgroundTranslationEnabled)
        this.UpdateBackground();
      this.UpdatePositionControlTranslation();
    }

    public event EventHandler<TrafficAvailableChangedArgs> TrafficAvailbleChanged;

    public event EventHandler<TrafficValueChangedArgs> TrafficValueChanged;

    protected void OnTrafficAvailbleChanged(TrafficAvailableChangedArgs e)
    {
      EventHandler<TrafficAvailableChangedArgs> trafficAvailbleChanged = this.TrafficAvailbleChanged;
      if (trafficAvailbleChanged == null)
        return;
      trafficAvailbleChanged((object) this, e);
    }

    protected void OnTrafficValueChanged(TrafficValueChangedArgs e)
    {
      EventHandler<TrafficValueChangedArgs> trafficValueChanged = this.TrafficValueChanged;
      if (trafficValueChanged == null)
        return;
      trafficValueChanged((object) this, e);
    }

    public override void OnApplyTemplate()
    {
      this.MapPresenter = (IMapPresenterBase) (this._mapPresenter = (IMapPresenter) IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<IMapPresenterFactory>().Get(MapPresenterType.Common, (IMapViewBase) this));
      this._layoutRoot = (Panel) (((Control) this).GetTemplateChild("LayoutRoot") as Grid);
      this._layerManager = ((Control) this).GetTemplateChild("LayerManager") as LayerManager;
      this._backgroundLayer = ((Control) this).GetTemplateChild("BackgroundLayer") as TiledImagePanel;
      this.MapContentLayer = (IMapContentLayer) (((Control) this).GetTemplateChild("MapContentLayer") as Yandex.Maps.ContentLayers.MapContentLayer);
      this.TrafficContentLayer = (ITrafficContentLayer) (((Control) this).GetTemplateChild("TrafficContentLayer") as Yandex.Maps.ContentLayers.TrafficContentLayer);
      Yandex.Maps.ContentLayers.MetaContentLayer templateChild = ((Control) this).GetTemplateChild("MetaContentLayer") as Yandex.Maps.ContentLayers.MetaContentLayer;
      this.MetaContentLayer = (IMetaContentLayer) templateChild;
      this._positionControl = ((Control) this).GetTemplateChild("PositionControl") as PositionControl;
      this._positionControlTransform = ((Control) this).GetTemplateChild("PositionControlTransform") as CompositeTransform;
      this._positionAccuracyControl = ((Control) this).GetTemplateChild("PositionAccuracyControl") as PositionAccuracyControl;
      this._positionAccuracyControlTransform = ((Control) this).GetTemplateChild("PositionAccuracyControlTransform") as CompositeTransform;
      this._touchWrapper = (ITouchWrapper) (((Control) this).GetTemplateChild("TouchWrapper") as TouchWrapper);
      if (this.TouchHandler is TouchManipulationSource touchHandler)
        touchHandler.TouchWrapper = this._touchWrapper;
      if (templateChild != null)
      {
        ((FrameworkElement) this).SetBinding(Map.RouteGuidanceEnabledProperty, new Binding()
        {
          Path = new PropertyPath("RouteGuidanceEnabled", new object[0]),
          Source = (object) templateChild,
          Mode = (BindingMode) 1
        });
        ((FrameworkElement) this).SetBinding(Map.RoutingEnabledProperty, new Binding()
        {
          Path = new PropertyPath("RoutingEnabled", new object[0]),
          Source = (object) templateChild,
          Mode = (BindingMode) 1
        });
        ((FrameworkElement) this).SetBinding(Map.PanoramaAvailableProperty, new Binding()
        {
          Path = new PropertyPath("PanoramaAvailable", new object[0]),
          Source = (object) templateChild,
          Mode = (BindingMode) 1
        });
      }
      this.OnUseLocationChanged(this.UseLocation);
      IocSingleton<Yandex.Maps.IoC.ControlsIocInitializer>.Resolve<IPositionWatcherManager>();
      this._positionWatcherWrapper.PositionChangedWithoutMapMoving += new EventHandler(this.PositionWatcherWrapperPositionChangedWithoutMapMoving);
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.MapSizeChanged);
      base.OnApplyTemplate();
    }

    internal override void UpdateMapPresenter(
      IMapPresenterBase oldValue,
      IMapPresenterBase newValue)
    {
      base.UpdateMapPresenter(oldValue, newValue);
      if (!(newValue is IMapPresenter mapPresenter))
        return;
      mapPresenter.UseLocation = this.UseLocation;
      mapPresenter.IsTrafficEnabled = this.TrafficEnabled;
      if (oldValue != null)
      {
        this._mapPresenter.TrafficAvailbleChanged -= new EventHandler<TrafficAvailableChangedArgs>(this.MapPresenterTrafficAvailbleChanged);
        this._mapPresenter.TrafficValueChanged -= new EventHandler<TrafficValueChangedArgs>(this.MapPresenterTrafficValueChanged);
      }
      this._mapPresenter.TrafficAvailbleChanged += new EventHandler<TrafficAvailableChangedArgs>(this.MapPresenterTrafficAvailbleChanged);
      this._mapPresenter.TrafficValueChanged += new EventHandler<TrafficValueChangedArgs>(this.MapPresenterTrafficValueChanged);
    }

    protected override void OnMapPresenterOperationStatusChanged(OperationStatus operationStatus)
    {
      base.OnMapPresenterOperationStatusChanged(operationStatus);
      if (operationStatus != OperationStatus.Idle)
        return;
      this.PlainViewPort = this._mapPresenter.PlainViewPort;
    }

    protected override void MapsPresenterPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.MapsPresenterPropertyChanged(sender, e);
      switch (e.PropertyName)
      {
        case "UseLocation":
          this.UseLocation = this._mapPresenter.UseLocation;
          break;
      }
    }

    private void MapSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this._mapPresenter != null)
        this.PlainViewPort = this._mapPresenter.PlainViewPort;
      if (this._backgroundLayer == null)
        return;
      ((FrameworkElement) this._backgroundLayer).Width = ((FrameworkElement) this).ActualWidth + (double) (2 * this._backgroundLayer.HorizontalPeriod);
      ((FrameworkElement) this._backgroundLayer).Height = ((FrameworkElement) this).ActualHeight + (double) (2 * this._backgroundLayer.VerticalPeriod);
    }

    private void UpdateBackground()
    {
      if (this._backgroundLayer == null)
        return;
      TranslateTransform renderTransform = (TranslateTransform) ((UIElement) this._backgroundLayer).RenderTransform;
      ICompositeTransformation compositeTransformation = (ICompositeTransformation) this;
      double num1 = compositeTransformation.TranslateX % (double) this._backgroundLayer.HorizontalPeriod;
      if (num1 > 0.0)
        num1 -= (double) this._backgroundLayer.HorizontalPeriod;
      renderTransform.X = num1;
      double num2 = compositeTransformation.TranslateY % (double) this._backgroundLayer.VerticalPeriod;
      if (num2 > 0.0)
        num2 -= (double) this._backgroundLayer.VerticalPeriod;
      renderTransform.Y = num2;
    }

    private void PositionWatcherWrapperPositionChangedWithoutMapMoving(object sender, EventArgs e) => this._uiDispatcher.BeginInvoke(new Action(this.UpdatePositionControlTranslation));

    private void UpdatePositionControlTranslation()
    {
      Point? centerScreenOffset = this.GetPinnedMarkerPointCenterScreenOffset();
      Point? nullable = centerScreenOffset.HasValue ? new Point?(centerScreenOffset.GetValueOrDefault()) : this.GetMarkerPointCenterScreenOffset();
      if (this._positionControlTransform != null && nullable.HasValue)
      {
        this._positionControlTransform.TranslateX = nullable.Value.X;
        this._positionControlTransform.TranslateY = nullable.Value.Y;
      }
      if (this._positionAccuracyControlTransform == null || !nullable.HasValue)
        return;
      this._positionAccuracyControlTransform.TranslateX = nullable.Value.X;
      this._positionAccuracyControlTransform.TranslateY = nullable.Value.Y;
    }

    private Point? GetPinnedMarkerPointCenterScreenOffset()
    {
      Point? centerScreenOffset = this._positionWatcherWrapper.PinnedMarkerPointCenterScreenOffset;
      if (!centerScreenOffset.HasValue)
        return new Point?();
      ICompositeTransformation compositeTransformation = (ICompositeTransformation) this;
      return new Point?(new Point(-compositeTransformation.TranslateX - centerScreenOffset.Value.X + this.ActualSize.Width * 0.5, -compositeTransformation.TranslateY - centerScreenOffset.Value.Y + this.ActualSize.Height * 0.5));
    }

    private Point? GetMarkerPointCenterScreenOffset()
    {
      GeoCoordinate lastCoordinates = this._positionWatcherWrapper.LastCoordinates;
      return lastCoordinates == null ? new Point?() : new Point?(this._viewportPointConveter.RelativePointToViewportPoint(this._geoPixelConverter.CoordinatesToRelativePoint(lastCoordinates), this.InstantZoomLevel));
    }

    private void MapPresenterTrafficAvailbleChanged(object sender, TrafficAvailableChangedArgs e) => this.OnTrafficAvailbleChanged(e);

    private void MapPresenterTrafficValueChanged(object sender, TrafficValueChangedArgs e) => this.OnTrafficValueChanged(e);

    private static void TrafficEnabledChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((Map) d).OnTrafficEnabledChanged(e);
    }

    private void OnTrafficEnabledChanged(DependencyPropertyChangedEventArgs e)
    {
      if (this._mapPresenter == null)
        return;
      this._mapPresenter.IsTrafficEnabled = (bool) e.NewValue;
    }

    public void ZoomIn() => this._mapPresenter.ZoomIn();

    public void ZoomOut() => this._mapPresenter.ZoomOut();

    internal void ClearPersistentCache()
    {
      if (this._mapPresenter == null)
        return;
      this._mapPresenter.ClearPersistentCache();
    }

    public GeoPositionStatus JumpToCurrentLocation()
    {
      if (this._mapPresenter != null)
        return this._mapPresenter.JumpToCurrentLocation();
      if (Debugger.IsAttached)
        throw new InvalidOperationException("Map control is not properly initialized yet.");
      return GeoPositionStatus.NoData;
    }
  }
}
