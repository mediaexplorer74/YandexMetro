// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Yandex.DevUtils;
using Yandex.Ioc;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.TypeConverters;
using Yandex.Media;
using Yandex.Positioning;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  [TemplatePart(Name = "ContentLayer", Type = typeof (MapLayer))]
  [TemplatePart(Name = "RootLayer", Type = typeof (MapLayer))]
  [TemplateVisualState(Name = "TwilightModeEnabled")]
  [TemplateVisualState(Name = "TwilightModeDisabled")]
  [ComVisible(false)]
  public abstract class MapBase : ContentControl, IMapState, IDisposable, ICompositeTransformation
  {
    private const double DefaultZoom = 5.0;
    private const BaseLayers DefaultLayer = BaseLayers.map;
    private const string RootLayerName = "RootLayer";
    private const string ContentLayerName = "ContentLayer";
    private const string TwilightModeEnabledName = "TwilightModeEnabled";
    private const string TwilightModeDisabledName = "TwilightModeDisabled";
    private static readonly GeoCoordinate DefaultCoordinates = new GeoCoordinate(55.733837, 37.588088);
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly IConfigMediator _config;
    public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register(nameof (ZoomLevel), typeof (double), typeof (MapBase), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(MapBase.ZoomLevelChanged)));
    public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof (Center), typeof (GeoCoordinate), typeof (MapBase), new PropertyMetadata((object) MapBase.DefaultCoordinates, new PropertyChangedCallback(MapBase.CenterChanged)));
    public static readonly DependencyProperty DisplayLayersProperty = DependencyProperty.Register(nameof (DisplayLayers), typeof (BaseLayers), typeof (MapBase), new PropertyMetadata((object) BaseLayers.map, new PropertyChangedCallback(MapBase.DisplayLayersChanged)));
    public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(nameof (ContentPadding), typeof (Thickness), typeof (MapBase), new PropertyMetadata((object) new Thickness(12.0), new PropertyChangedCallback(MapBase.ContentPaddingChanged)));
    public static readonly DependencyProperty OperationStatusProperty = DependencyProperty.Register(nameof (OperationStatus), typeof (OperationStatus), typeof (MapBase), new PropertyMetadata((object) OperationStatus.Idle));
    public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register(nameof (ApiKey), typeof (string), typeof (MapBase), new PropertyMetadata((object) null, new PropertyChangedCallback(MapBase.ApiKeyChanged)));
    private readonly ICompositeTransformation _compositeTransformation;
    private MatrixTransform _matrixTransform;
    protected readonly IViewportPointConveter _viewportPointConveter;
    private IMapPresenterBase _mapPresenter;
    private ProjectionUpdateLevel _projectionUpdateLevel;
    private TranslateTransform _rootTransform;
    private AnimationLevel _animationLevel = AnimationLevel.Full;
    private MapLayer _rootLayer;
    private MapLayer _contentLayer;
    private bool _mapPresenterCenterChanging;
    private bool _mapPresenterZoomLevelChanging;
    private readonly IUiDispatcher _uiDispatcher;

    public void EnsureControlIsVisible(UIElement mapChild) => this._mapPresenter.EnsureControlIsVisible((object) mapChild);

    public void EnsureRectangleIsVisible(GeoCoordinatesRect rect) => this._mapPresenter.EnsureRectangleIsVisible(rect);

    public void EnsureRectangleIsVisible(RelativeRect rect) => this._mapPresenter.EnsureRectangleIsVisible(rect);

    public void EnsureVisibility(IEnumerable<object> mapChildren) => this._mapPresenter.EnsureVisibility(mapChildren);

    public void CenterOnControl(UIElement mapChild) => this._mapPresenter.CenterOnControl((object) mapChild);

    public Thickness ContentPadding
    {
      get => (Thickness) ((DependencyObject) this).GetValue(MapBase.ContentPaddingProperty);
      set => ((DependencyObject) this).SetValue(MapBase.ContentPaddingProperty, (object) value);
    }

    private static void ApiKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode || e.OldValue == e.NewValue)
        return;
      MapGlobalSettings.Instance.ApiKey = e.NewValue as string;
    }

    [Obsolete("Better use MapGlobalSettings.ApiKey", false)]
    public string ApiKey
    {
      get => (string) ((DependencyObject) this).GetValue(MapBase.ApiKeyProperty);
      set => ((DependencyObject) this).SetValue(MapBase.ApiKeyProperty, (object) value);
    }

    public event EventHandler ViewAreaChanged;

    [NotNull]
    public IPushPinManager PushPinManager { get; private set; }

    protected MapBase()
    {
      IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>().CheckAccess();
      this._viewportPointConveter = IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>();
      this._geoPixelConverter = IocSingleton<ControlsIocInitializer>.Resolve<IGeoPixelConverter>();
      this._uiDispatcher = IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>();
      this.PushPinManager = IocSingleton<ControlsIocInitializer>.Resolve<IPushPinManager>();
      this._compositeTransformation = (ICompositeTransformation) new CompositeTransformation()
      {
        ScaleX = 1.0,
        ScaleY = 1.0
      };
      this._config = IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>();
      this._config.PropertyChanged += new PropertyChangedEventHandler(this.ConfigPropertyChanged);
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.LayoutRootSizeChanged);
    }

    private void ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "TwilightModeEnabled":
          this.ApplyTwilightModeChange();
          break;
      }
    }

    public MapLayer RootLayer => this._rootLayer;

    public MapLayer ContentLayer => this._contentLayer;

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._rootLayer = ((Control) this).GetTemplateChild("RootLayer") as MapLayer;
      this._contentLayer = ((Control) this).GetTemplateChild("ContentLayer") as MapLayer;
      this._rootTransform = ((UIElement) this.RootLayer).RenderTransform as TranslateTransform;
      if (this.LayerManager != null)
        this._matrixTransform = ((UIElement) this.LayerManager).RenderTransform as MatrixTransform;
      this.ApplyTwilightModeChange();
    }

    private void LayoutRootSizeChanged(object sender, SizeChangedEventArgs e)
    {
      Point point = new Point(((FrameworkElement) this).ActualWidth, ((FrameworkElement) this).ActualHeight);
      this.ActualSize = new Size(point.X, point.Y);
      this.OnViewAreaChange();
      ((UIElement) this).Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(Canvas.GetLeft((UIElement) this), Canvas.GetTop((UIElement) this), point.X, point.Y)
      };
    }

    public Rect Viewport => new Rect(-this._compositeTransformation.TranslateX, -this._compositeTransformation.TranslateY, this.ActualSize.Width, this.ActualSize.Height);

    [NotNull]
    internal IMapPresenterBase MapPresenter
    {
      get => this._mapPresenter;
      set
      {
        if (this._mapPresenter == value)
          return;
        IMapPresenterBase mapPresenter = this._mapPresenter;
        this._mapPresenter = value;
        this.UpdateMapPresenter(mapPresenter, value);
      }
    }

    internal virtual void UpdateMapPresenter(IMapPresenterBase oldValue, IMapPresenterBase newValue)
    {
      if (DesignerProperties.IsInDesignMode || oldValue == newValue)
        return;
      if (oldValue != null)
      {
        oldValue.PropertyChanged -= new PropertyChangedEventHandler(this.MapsPresenterPropertyChanged);
        oldValue.OperationStatusChanged -= new EventHandler<OperationStatusChangedEventArgs>(this.MapPresenterOperationStatusChanged);
        oldValue.Dispose();
      }
      newValue.DisableMapReload();
      newValue.DisplayLayers = this.DisplayLayers;
      newValue.InstantZoomLevel = this.ZoomLevel;
      GeoCoordinate center = this.Center;
      if (center != null)
        newValue.InstantCenter = this._geoPixelConverter.CoordinatesToRelativePoint(center);
      newValue.AnimationLevel = this.AnimationLevel;
      newValue.ContentPadding = new Thickness(this.ContentPadding.Left, this.ContentPadding.Top, this.ContentPadding.Right, this.ContentPadding.Bottom);
      newValue.EnableMapReload();
      newValue.PropertyChanged += new PropertyChangedEventHandler(this.MapsPresenterPropertyChanged);
      newValue.OperationStatusChanged += new EventHandler<OperationStatusChangedEventArgs>(this.MapPresenterOperationStatusChanged);
    }

    private void MapPresenterOperationStatusChanged(
      object sender,
      OperationStatusChangedEventArgs e)
    {
      this.OnMapPresenterOperationStatusChanged(e.OperationStatus);
      this.OnOperationStatusChanged(e);
    }

    protected virtual void OnMapPresenterOperationStatusChanged(OperationStatus operationStatus) => this.OperationStatus = operationStatus;

    protected virtual void MapsPresenterPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "InstantCenter":
          this._mapPresenterCenterChanging = true;
          this.Center = this._geoPixelConverter.RelativePointToCoordinates(this.MapPresenter.InstantCenter);
          this._mapPresenterCenterChanging = false;
          break;
        case "InstantZoomLevel":
          this._mapPresenterZoomLevelChanging = true;
          this.ZoomLevel = this.MapPresenter.InstantZoomLevel;
          this._mapPresenterZoomLevelChanging = false;
          break;
      }
    }

    public Size ViewportSize => new Size(this.ActualSize.Width, this.ActualSize.Height);

    public event EventHandler<OperationStatusChangedEventArgs> OperationStatusChanged;

    [TypeConverter(typeof (GeoCoordinatesConverter))]
    public GeoCoordinate Center
    {
      get => (GeoCoordinate) ((DependencyObject) this).GetValue(MapBase.CenterProperty);
      set => ((DependencyObject) this).SetValue(MapBase.CenterProperty, (object) value);
    }

    public double ZoomLevel
    {
      get => (double) ((DependencyObject) this).GetValue(MapBase.ZoomLevelProperty);
      set => ((DependencyObject) this).SetValue(MapBase.ZoomLevelProperty, (object) value);
    }

    public OperationStatus OperationStatus
    {
      get => (OperationStatus) ((DependencyObject) this).GetValue(MapBase.OperationStatusProperty);
      internal set => ((DependencyObject) this).SetValue(MapBase.OperationStatusProperty, (object) value);
    }

    public double InstantZoomLevel
    {
      get => this._mapPresenter.InstantZoomLevel;
      set => throw new NotImplementedException();
    }

    public Point InstantCenter
    {
      get => this._mapPresenter.InstantCenter;
      set => throw new NotImplementedException();
    }

    public AnimationLevel AnimationLevel
    {
      get => this._animationLevel;
      set
      {
        this._animationLevel = value;
        if (this._mapPresenter == null)
          return;
        this._mapPresenter.AnimationLevel = value;
      }
    }

    internal OperationMode OperationMode => DesignerProperties.IsInDesignMode ? OperationMode.Full : this._mapPresenter.OperationMode;

    public BaseLayers DisplayLayers
    {
      get => (BaseLayers) ((DependencyObject) this).GetValue(MapBase.DisplayLayersProperty);
      set => ((DependencyObject) this).SetValue(MapBase.DisplayLayersProperty, (object) value);
    }

    private void UpdateProjectionUpdateLevel(ProjectionUpdateLevel newLevel) => this._projectionUpdateLevel |= newLevel;

    double ICompositeTransformation.TranslateX
    {
      get => this._compositeTransformation.TranslateX;
      set
      {
        this.UpdateProjectionUpdateLevel(ProjectionUpdateLevel.Linear);
        this._compositeTransformation.TranslateX = value;
      }
    }

    private void UpdateMatrix()
    {
      if (this._matrixTransform == null)
        return;
      this._matrixTransform.Matrix = new Matrix(this._compositeTransformation.ScaleX, 0.0, 0.0, this._compositeTransformation.ScaleY, 0.0, 0.0);
    }

    double ICompositeTransformation.TranslateY
    {
      get => this._compositeTransformation.TranslateY;
      set
      {
        this.UpdateProjectionUpdateLevel(ProjectionUpdateLevel.Linear);
        this._compositeTransformation.TranslateY = value;
      }
    }

    double ICompositeTransformation.Rotation
    {
      get => this._compositeTransformation.Rotation;
      set => this._compositeTransformation.Rotation = value;
    }

    double ICompositeTransformation.ScaleX
    {
      get => this._compositeTransformation.ScaleX;
      set
      {
        this.UpdateProjectionUpdateLevel(ProjectionUpdateLevel.Full);
        this._compositeTransformation.ScaleX = value;
      }
    }

    double ICompositeTransformation.ScaleY
    {
      get => this._compositeTransformation.ScaleY;
      set
      {
        this.UpdateProjectionUpdateLevel(ProjectionUpdateLevel.Full);
        this._compositeTransformation.ScaleY = value;
      }
    }

    public abstract LayerManager LayerManager { get; }

    public virtual void UpdateProjection()
    {
      if (this.RootLayer == null)
        return;
      ProjectionUpdateLevel projectionUpdateLevel = this._projectionUpdateLevel;
      this._projectionUpdateLevel = ProjectionUpdateLevel.None;
      this.RootLayer.ProjectionUpdated(projectionUpdateLevel);
      if ((projectionUpdateLevel & ProjectionUpdateLevel.Linear) == ProjectionUpdateLevel.Linear && this._rootTransform != null)
      {
        Rect viewport = this.Viewport;
        this._rootTransform.X = Math.Round(-viewport.X);
        this._rootTransform.Y = Math.Round(-viewport.Y);
      }
      if (projectionUpdateLevel != ProjectionUpdateLevel.Full)
        return;
      this.UpdateMatrix();
    }

    public Size ActualSize { get; private set; }

    protected virtual void OnViewAreaChange()
    {
      if (this.ViewAreaChanged == null)
        return;
      this.ViewAreaChanged((object) this, EventArgs.Empty);
    }

    public Point CoordinatesToViewportPoint(GeoCoordinate value) => this._viewportPointConveter.CoordinatesToViewportPoint(value, this._mapPresenter.InstantZoomLevel);

    public GeoCoordinate ViewportPointToCoordinates(Point point) => this._viewportPointConveter.ViewportPointToCoordinates(new Point(point.X + this.Viewport.X, point.Y + this.Viewport.Y), this._mapPresenter.InstantZoomLevel);

    internal Point RelativePointToViewportPoint(Point value) => this._viewportPointConveter.RelativePointToViewportPoint(value, this._mapPresenter.InstantZoomLevel);

    private static void ContentPaddingChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((MapBase) d).OnContentPaddingChanged(e);
    }

    protected virtual void OnContentPaddingChanged(DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode || this.MapPresenter == null)
        return;
      Thickness newValue = (Thickness) e.NewValue;
      this.MapPresenter.ContentPadding = new Thickness(newValue.Left, newValue.Top, newValue.Right, newValue.Bottom);
    }

    private static void ZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      ((MapBase) d).OnZoomLevelChanged(e);
    }

    private static void CenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      ((MapBase) d).OnCenterChanged(e);
    }

    private static void DisplayLayersChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      ((MapBase) d).OnDisplayLayerChanged(e);
    }

    protected virtual void OnZoomLevelChanged(DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode || this.MapPresenter == null || this._mapPresenterZoomLevelChanging)
        return;
      this.MapPresenter.InstantZoomLevel = (double) e.NewValue;
    }

    protected virtual void OnCenterChanged(DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode || this.MapPresenter == null || !(e.NewValue is GeoCoordinate newValue) || e.NewValue.Equals(e.OldValue) || this._mapPresenterCenterChanging)
        return;
      this.MapPresenter.InstantCenter = this._geoPixelConverter.CoordinatesToRelativePoint(newValue);
    }

    protected virtual void OnDisplayLayerChanged(DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode || this.MapPresenter == null)
        return;
      this.MapPresenter.DisplayLayers = (BaseLayers) e.NewValue;
    }

    public void Dispose()
    {
      IMapPresenterBase mapPresenter = this.MapPresenter;
      if (mapPresenter != null)
      {
        mapPresenter.PropertyChanged -= new PropertyChangedEventHandler(this.MapsPresenterPropertyChanged);
        mapPresenter.Dispose();
      }
      this.LayerManager?.Dispose();
      BehaviorCollection behaviorCollection = (BehaviorCollection) ((DependencyObject) this).GetValue(Interaction.BehaviorsProperty);
      if (behaviorCollection == null)
        return;
      foreach (Behavior behavior in (DependencyObjectCollection<Behavior>) behaviorCollection)
        behavior.Detach();
    }

    private void OnOperationStatusChanged(OperationStatusChangedEventArgs e)
    {
      EventHandler<OperationStatusChangedEventArgs> operationStatusChanged = this.OperationStatusChanged;
      if (operationStatusChanged == null)
        return;
      operationStatusChanged((object) this, e);
    }

    private void ApplyTwilightModeChange()
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      this._uiDispatcher.BeginInvoke((Action) (() => VisualStateManager.GoToState((Control) this, this._config.TwilightModeEnabled ? "TwilightModeEnabled" : "TwilightModeDisabled", false)));
    }
  }
}
