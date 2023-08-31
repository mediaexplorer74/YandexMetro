// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapPresenterBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Yandex.DevUtils;
using Yandex.Ioc;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Media;
using Yandex.Patterns;
using Yandex.Positioning;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  internal class MapPresenterBase : IMapPresenterBase, IMapState, INotifyPropertyChanged, IDisposable
  {
    internal const string InstantCenterPropertyName = "InstantCenter";
    internal const string InstantZoomLevelPropertyName = "InstantZoomLevel";
    internal const string DisplayLayersPropertyName = "DisplayLayers";
    internal const string AnimationLevelPropertyName = "AnimationLevel";
    internal const string OperationModePropertyName = "OperationMode";
    internal const string ContentPaddingPropertyName = "ContentPadding";
    private const double EPSILON = 1E-07;
    protected readonly IPositionDispatcherBase _positionDispatcher;
    private readonly IManipulationWrapper _manipulationWrapper;
    protected readonly IUiDispatcher _uiDispatcher;
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly IPrinterManager _printerManager;
    private readonly ITileManager<ITile> _tileManager;
    private readonly IMapViewBase _view;
    private readonly IZoomInfo _zoomInfo;
    private readonly IZoomLevelConverter _zoomLevelConverter;
    private IMapContentLayer _mapContentLayer;
    private bool _initializePending = true;
    private bool _loadPending = true;
    private bool _isLoaded;
    private readonly IViewportPointConveter _viewportPointConverter;
    private bool _mapReloadDisabled;
    private OperationMode? _pendingOperationMode;
    private Action _pendingEnsureVisiblityAction;
    private bool _zoomChangePending;
    private bool _positionChangePending;

    protected MapControlModel Model { get; set; }

    public MapPresenterBase(IMapViewBase view)
      : this(view, IocSingleton<ControlsIocInitializer>.Resolve<IPositionDispatcherBase>(), IocSingleton<ControlsIocInitializer>.Resolve<IManipulationWrapper>())
    {
    }

    protected MapPresenterBase(
      IMapViewBase view,
      [NotNull] IPositionDispatcherBase positionDispatcher,
      [NotNull] IManipulationWrapper manipulationWrapper)
    {
      if (view == null)
        throw new ArgumentNullException(nameof (view));
      if (positionDispatcher == null)
        throw new ArgumentNullException(nameof (positionDispatcher));
      if (manipulationWrapper == null)
        throw new ArgumentNullException(nameof (manipulationWrapper));
      this.Model = new MapControlModel();
      this._view = view;
      this._view.ViewAreaChanged += new EventHandler(this.ViewAreaChanged);
      if (view is FrameworkElement node)
      {
        node.LayoutUpdated += new EventHandler(this.ViewLayoutUpdated);
        node.Loaded += new RoutedEventHandler(this.FrameworkElementOnLoaded);
        node.Unloaded += new RoutedEventHandler(this.FrameworkElementOnUnloaded);
        if (node.GetVisualParent() != null)
          this.OnViewLoaded();
      }
      this._tileManager = IocSingleton<ControlsIocInitializer>.Resolve<ITileManager<ITile>>();
      this._geoPixelConverter = IocSingleton<ControlsIocInitializer>.Resolve<IGeoPixelConverter>();
      this._printerManager = IocSingleton<ControlsIocInitializer>.Resolve<IPrinterManager>();
      this._zoomInfo = IocSingleton<ControlsIocInitializer>.Resolve<IZoomInfo>();
      this._uiDispatcher = IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>();
      this.Config = IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>();
      this.Config.PropertyChanged += new PropertyChangedEventHandler(this.ConfigOnPropertyChanged);
      this.ViewportPointConverter = IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>();
      this._zoomLevelConverter = IocSingleton<ControlsIocInitializer>.Resolve<IZoomLevelConverter>();
      this._viewportPointConverter = IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>();
      this._manipulationWrapper = manipulationWrapper;
      this._positionDispatcher = positionDispatcher;
      this.IsUserInteractionEnabled = false;
      if (this._positionDispatcher is IManipulationWrapperInitializable positionDispatcher1)
        positionDispatcher1.Initialize(this._manipulationWrapper);
      this._positionDispatcher.PositionChanged += new EventHandler<Yandex.Maps.Events.PositionChangedEventArgs>(this.PositionDispatcherPositionChanged);
      this._positionDispatcher.OperationStatusChanged += new EventHandler<OperationStatusChangedEventArgs>(this.PositionDispatcherOperationStatusChanged);
      this.AnimationLevel = AnimationLevel.Full;
    }

    private void ConfigOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "TilesStretchFactor":
          this.OnTilesStretchFactorChanged();
          break;
        case "TwilightModeEnabled":
          this.OnTwilightModeChanged();
          break;
      }
    }

    public void Dispose()
    {
      this._positionDispatcher.PositionChanged -= new EventHandler<Yandex.Maps.Events.PositionChangedEventArgs>(this.PositionDispatcherPositionChanged);
      this._positionDispatcher.OperationStatusChanged -= new EventHandler<OperationStatusChangedEventArgs>(this.PositionDispatcherOperationStatusChanged);
      this._positionDispatcher.Dispose();
    }

    public void DisableManipulations() => this._manipulationWrapper.DisableManipulations();

    public void EnableManipulations() => this._manipulationWrapper.EnableManipulations();

    private void FrameworkElementOnLoaded(object sender, RoutedEventArgs routedEventArgs) => this.OnViewLoaded();

    private void FrameworkElementOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
    {
      this._isLoaded = false;
      this._loadPending = true;
    }

    private void OnViewLoaded()
    {
      this._isLoaded = true;
      this.OperationMode = OperationMode.Full;
    }

    private void ViewLayoutUpdated(object sender, EventArgs e)
    {
      if (this._initializePending && this.TryInitializeView())
        this._initializePending = false;
      if (!this._isLoaded || this._initializePending || !this._loadPending)
        return;
      this.NotifyViewportChanged();
      if (!this.Reload())
        return;
      this._loadPending = false;
    }

    protected virtual bool TryInitializeView()
    {
      LayerManager layerManager = this._view.LayerManager;
      if (layerManager == null)
        return false;
      this.LayerManager = layerManager;
      layerManager.OperationMode = this.OperationMode;
      this._mapContentLayer = this._view.MapContentLayer;
      if (this._mapContentLayer != null)
        this._mapContentLayer.Layers = this.Model.DisplayLayers;
      return true;
    }

    protected LayerManager LayerManager { get; set; }

    protected IConfigMediator Config { get; set; }

    public Rect PlainViewPort
    {
      get
      {
        double instantZoomLevel = this.InstantZoomLevel;
        Point relativePoint1 = this.ViewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(new Point(-this.TranslateX, -this.TranslateY), instantZoomLevel));
        Point relativePoint2 = this.ViewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(new Point(this._view.ActualSize.Width, this._view.ActualSize.Height), instantZoomLevel));
        return new Rect(relativePoint1.X, relativePoint1.Y, relativePoint2.X, relativePoint2.Y);
      }
    }

    protected virtual void OnTwilightModeChanged()
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      if (this._mapContentLayer != null)
        this._mapContentLayer.DisposeContent();
      this._tileManager.ResetCachedTileBitmaps();
      this.Reload();
    }

    public event EventHandler<OperationStatusChangedEventArgs> OperationStatusChanged;

    public void Connect()
    {
      this._printerManager.ExecuteWhenReady(new Action(this.OnPrinterConnected));
      if (this._printerManager.State > ServiceState.Stopped)
        return;
      this._printerManager.Connect();
    }

    protected virtual void OnEnsureScroll()
    {
    }

    public void EnsureControlIsVisible(object control)
    {
      UIElement uiElement = control as UIElement;
      if (uiElement == null)
        throw new ArgumentException("UIElement is expected.");
      if (this.OperationMode != OperationMode.None)
      {
        this._pendingEnsureVisiblityAction = (Action) null;
        this.EnsureControlIsVisibleInternal(uiElement);
      }
      else
        this._pendingEnsureVisiblityAction = (Action) (() => this.EnsureControlIsVisibleInternal(uiElement));
    }

    private void EnsureControlIsVisibleInternal(UIElement mapChild)
    {
      this.OnEnsureScroll();
      DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) mapChild);
      Rect viewport = this._view.Viewport;
      Thickness contentPadding = this.ContentPadding;
      Alignment? alignment;
      Point positionOffset;
      if (!(((DependencyObject) mapChild).GetValue(MapLayer.LocationProperty) is GeoCoordinate position) && parent != null)
      {
        position = parent.GetValue(MapLayer.LocationProperty) as GeoCoordinate;
        alignment = parent.GetValue(MapLayer.AlignmentProperty) as Alignment?;
        positionOffset = (Point) parent.GetValue(MapLayer.PositionOffsetProperty);
      }
      else
      {
        alignment = ((DependencyObject) mapChild).GetValue(MapLayer.AlignmentProperty) as Alignment?;
        positionOffset = (Point) ((DependencyObject) mapChild).GetValue(MapLayer.PositionOffsetProperty);
      }
      if (position != null)
      {
        Rect viewportChildRect = MapLayer.GetViewportChildRect((MapBase) this._view, mapChild, position, alignment, positionOffset);
        this.EnsureRectangleIsVisible(viewport, new Rect(viewportChildRect.X, viewportChildRect.Y, viewportChildRect.Width, viewportChildRect.Height), contentPadding);
      }
      else
      {
        if (!(((DependencyObject) mapChild).GetValue(MapLayer.RelativeRectangleProperty) is RelativeRect relativeRect) && parent != null)
          relativeRect = parent.GetValue(MapLayer.RelativeRectangleProperty) as RelativeRect;
        if (relativeRect == null)
        {
          if (!(((DependencyObject) mapChild).GetValue(MapLayer.LocationRectangleProperty) is GeoCoordinatesRect geoCoordinatesRect) && parent != null)
            geoCoordinatesRect = parent.GetValue(MapLayer.LocationRectangleProperty) as GeoCoordinatesRect;
          if (geoCoordinatesRect != null)
          {
            Point relativePoint1 = this._geoPixelConverter.CoordinatesToRelativePoint(new GeoCoordinate(geoCoordinatesRect.North, geoCoordinatesRect.West));
            Point relativePoint2 = this._geoPixelConverter.CoordinatesToRelativePoint(new GeoCoordinate(geoCoordinatesRect.South, geoCoordinatesRect.East));
            relativeRect = new RelativeRect(relativePoint1.Y, relativePoint1.X, relativePoint2.Y, relativePoint2.X);
          }
        }
        if (relativeRect == null)
          return;
        this.EnsureRegionIsVisible(viewport, relativeRect, contentPadding);
      }
    }

    private void EnsureRegionIsVisible(Rect viewPort, RelativeRect relativeRect, Thickness padding)
    {
      double instantZoomLevel = this.InstantZoomLevel;
      double num1 = viewPort.Width - padding.Left - padding.Right;
      double num2 = viewPort.Height - padding.Top - padding.Bottom;
      Point viewportPoint1 = this._viewportPointConverter.RelativePointToViewportPoint(new Point(relativeRect.East - relativeRect.West, relativeRect.South - relativeRect.North), instantZoomLevel);
      double val1 = num1 / viewportPoint1.X;
      double val2 = num2 / viewportPoint1.Y;
      double zoomLevel = instantZoomLevel + this._zoomLevelConverter.StretchFactorToZoomLevel(Math.Min(val1, val2), (byte) 0);
      Point relativePoint = new Point((relativeRect.East + relativeRect.West) * 0.5, (relativeRect.North + relativeRect.South) * 0.5);
      double num3 = padding.Left + num1 * 0.5;
      double num4 = padding.Top + num2 * 0.5;
      Point areaCenterOffset = this.GetVisibleAreaCenterOffset();
      double num5 = areaCenterOffset.X - num3;
      double num6 = areaCenterOffset.Y - num4;
      Point viewportPoint2 = this._viewportPointConverter.CoordinatesToViewportPoint(this._geoPixelConverter.RelativePointToCoordinates(relativePoint), zoomLevel);
      viewportPoint2.X += num5;
      viewportPoint2.Y += num6;
      this.InstantCenter = relativePoint;
      this.InstantZoomLevel = zoomLevel;
    }

    private void EnsureRectangleIsVisible(Rect viewport, Rect targetRect, Thickness padding)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = viewport.X + padding.Left;
      double num4 = viewport.Y + padding.Top;
      double num5 = viewport.Height - padding.Top - padding.Bottom;
      double num6 = viewport.Width - padding.Left - padding.Right;
      if (num4 > targetRect.Y)
        num1 -= num4 - targetRect.Y;
      if (num4 + num5 < targetRect.Y + targetRect.Height)
        num1 -= num4 + num5 - targetRect.Y - targetRect.Height;
      if (num3 > targetRect.X)
        num2 -= num3 - targetRect.X;
      if (num3 + num6 < targetRect.X + targetRect.Width)
        num2 -= num3 + num6 - targetRect.X - targetRect.Width;
      if (Math.Abs(num2) <= 1.0 && Math.Abs(num1) <= 1.0)
        return;
      double instantZoomLevel = this.InstantZoomLevel;
      Point viewportPoint = this._viewportPointConverter.RelativePointToViewportPoint(this.InstantCenter, instantZoomLevel);
      viewportPoint.X += num2;
      viewportPoint.Y += num1;
      this.InstantCenter = this._viewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(viewportPoint, instantZoomLevel));
    }

    public void EnsureRectangleIsVisible([NotNull] GeoCoordinatesRect rect)
    {
      if (rect == null)
        throw new ArgumentNullException(nameof (rect));
      if (this.OperationMode != OperationMode.None)
      {
        this._pendingEnsureVisiblityAction = (Action) null;
        this.EnsureRectangleIsVisibleInternal(rect);
      }
      else
        this._pendingEnsureVisiblityAction = (Action) (() => this.EnsureRectangleIsVisibleInternal(rect));
    }

    public void EnsureRectangleIsVisible([NotNull] RelativeRect rect)
    {
      if (rect == null)
        throw new ArgumentNullException(nameof (rect));
      if (this.OperationMode != OperationMode.None)
      {
        this._pendingEnsureVisiblityAction = (Action) null;
        this.EnsureRectangleIsVisibleInternal(rect);
      }
      else
        this._pendingEnsureVisiblityAction = (Action) (() => this.EnsureRectangleIsVisibleInternal(rect));
    }

    private void EnsureRectangleIsVisibleInternal(GeoCoordinatesRect rect)
    {
      this.OnEnsureScroll();
      Point relativePoint1 = this._geoPixelConverter.CoordinatesToRelativePoint(new GeoCoordinate(rect.North, rect.West));
      Point relativePoint2 = this._geoPixelConverter.CoordinatesToRelativePoint(new GeoCoordinate(rect.South, rect.East));
      this.EnsureRegionIsVisible(this._view.Viewport, new RelativeRect(relativePoint1.Y, relativePoint1.X, relativePoint2.Y, relativePoint2.X), this.ContentPadding);
    }

    private void EnsureRectangleIsVisibleInternal(RelativeRect rect)
    {
      this.OnEnsureScroll();
      this.EnsureRegionIsVisible(this._view.Viewport, rect, this.ContentPadding);
    }

    public void EnsureVisibility([NotNull] IEnumerable<object> mapChildren)
    {
      if (mapChildren == null)
        throw new ArgumentNullException(nameof (mapChildren));
      GeoCoordinate[] array = mapChildren.OfType<DependencyObject>().Select<DependencyObject, GeoCoordinate>((Func<DependencyObject, GeoCoordinate>) (obj => MapPresenterBase.TryGetPropertyValue<GeoCoordinate>(obj, MapLayer.LocationProperty))).Where<GeoCoordinate>((Func<GeoCoordinate, bool>) (location => location != null)).ToArray<GeoCoordinate>();
      GeoCoordinatesRect rect = array.Length >= 2 ? new GeoCoordinatesRect((IEnumerable<GeoCoordinate>) array) : throw new ArgumentOutOfRangeException(nameof (mapChildren));
      if (this.OperationMode != OperationMode.None)
      {
        this._pendingEnsureVisiblityAction = (Action) null;
        this.EnsureRectangleIsVisibleInternal(rect);
      }
      else
        this._pendingEnsureVisiblityAction = (Action) (() => this.EnsureRectangleIsVisibleInternal(rect));
    }

    private static T TryGetPropertyValue<T>(DependencyObject element, DependencyProperty property)
    {
      T objA = (T) element.GetValue(property);
      if (!object.Equals((object) objA, (object) default (T)))
        return objA;
      DependencyObject parent = VisualTreeHelper.GetParent(element);
      return parent == null ? default (T) : (T) parent.GetValue(property);
    }

    public void CenterOnControl(object mapChild)
    {
      Rect viewportChildRect = MapLayer.GetViewportChildRect((MapBase) this._view, uiElement, (mapChild is UIElement uiElement ? MapPresenterBase.TryGetPropertyValue<GeoCoordinate>((DependencyObject) uiElement, MapLayer.LocationProperty) : throw new ArgumentException("UIElement is expected.")) ?? throw new ArgumentOutOfRangeException(nameof (mapChild)), MapPresenterBase.TryGetPropertyValue<Alignment?>((DependencyObject) uiElement, MapLayer.AlignmentProperty), MapPresenterBase.TryGetPropertyValue<Point>((DependencyObject) uiElement, MapLayer.PositionOffsetProperty));
      this.InstantCenter = this._viewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(new Point(viewportChildRect.X + viewportChildRect.Width * 0.5, viewportChildRect.Y + viewportChildRect.Height * 0.5), this.InstantZoomLevel));
    }

    public Point InstantCenter
    {
      get => this.Model.InstantCenter;
      set
      {
        if (DesignerProperties.IsInDesignMode)
          return;
        Point relativePoint = this._positionDispatcher.TargetPosition.RelativePoint;
        if (Math.Abs(value.X - relativePoint.X) < 1E-07 && Math.Abs(value.Y - relativePoint.Y) < 1E-07)
          return;
        if (this._mapReloadDisabled)
          this.DoTranslate(value);
        this.MoveToPosition(value);
      }
    }

    public BaseLayers DisplayLayers
    {
      get => this.Model.DisplayLayers;
      set
      {
        if (DesignerProperties.IsInDesignMode || this.Model.DisplayLayers == value)
          return;
        this.Model.DisplayLayers = value;
        if (this._mapContentLayer != null)
        {
          this._mapContentLayer.Layers = value;
          this._mapContentLayer.DisposeContent();
        }
        this.Reload();
        this.OnPropertyChanged(nameof (DisplayLayers));
      }
    }

    public double InstantZoomLevel
    {
      get => this.Model.InstantZoomLevel;
      set
      {
        if (DesignerProperties.IsInDesignMode || Math.Abs(this._positionDispatcher.TargetPosition.Zoom - value) < 1E-07)
          return;
        double num = Math.Max((double) this._zoomInfo.MinZoom, Math.Min((double) this._zoomInfo.MaxVisibleZoom, value));
        if (this._mapReloadDisabled)
          this.DoZoom(num);
        this._positionDispatcher.ZoomTo(num);
      }
    }

    protected Position TargetPosition => this._positionDispatcher.TargetPosition;

    public AnimationLevel AnimationLevel
    {
      get => this.Model.AnimationLevel;
      set
      {
        if (DesignerProperties.IsInDesignMode)
          return;
        this._positionDispatcher.AnimationLevel = value;
        if (this.Model.AnimationLevel == value)
          return;
        this.Model.AnimationLevel = value;
        this.OnPropertyChanged(nameof (AnimationLevel));
      }
    }

    public OperationMode OperationMode
    {
      get => this.Model.OperationMode;
      set
      {
        if (value != OperationMode.None)
        {
          Action ensureVisiblityAction = this._pendingEnsureVisiblityAction;
          this._pendingEnsureVisiblityAction = (Action) null;
          if (ensureVisiblityAction != null)
            this._uiDispatcher.BeginInvoke(ensureVisiblityAction);
        }
        if (this.Model.OperationMode == value)
          return;
        if (this._mapReloadDisabled && value != OperationMode.None)
        {
          this._pendingOperationMode = new OperationMode?(value);
        }
        else
        {
          this._pendingOperationMode = new OperationMode?();
          this.Model.OperationMode = value;
          this.OnOperationModeChanged(value);
          this.OnPropertyChanged(nameof (OperationMode));
        }
      }
    }

    public Thickness ContentPadding
    {
      get => this.Model.ContentPadding;
      set
      {
        if (this.Model.ContentPadding == value)
          return;
        this.Model.ContentPadding = value;
        this.OnPropertyChanged(nameof (ContentPadding));
      }
    }

    public void DisableMapReload()
    {
      this._pendingOperationMode = new OperationMode?(OperationMode.None);
      this._mapReloadDisabled = true;
      this.OperationMode = OperationMode.None;
    }

    public void EnableMapReload()
    {
      this._mapReloadDisabled = false;
      OperationMode? pendingOperationMode = this._pendingOperationMode;
      if (pendingOperationMode.HasValue)
        this.OperationMode = pendingOperationMode.Value;
      this.Reload();
    }

    private void ViewAreaChanged(object sender, EventArgs e) => this.OnViewAreaChanged();

    protected virtual void OnViewAreaChanged()
    {
      if (this._initializePending)
        return;
      this.NotifyViewportChanged();
      this.Reload();
    }

    private void NotifyViewportChanged()
    {
      if (this._view.ActualSize.Width == 0.0 && this._view.ActualSize.Height == 0.0)
        return;
      this._positionDispatcher.MapPositionChanged(this.InstantZoomLevel, this._view.Viewport);
      this._positionDispatcher.ResendPositionChangedEvent();
    }

    private Point GetVisibleAreaCenterOffset() => new Point(0.5 * this._view.ActualSize.Width, 0.5 * this._view.ActualSize.Height);

    protected bool IsUserInteractionEnabled
    {
      get => this._positionDispatcher.IsUserInteractionEnabled;
      set => this._positionDispatcher.IsUserInteractionEnabled = value;
    }

    protected virtual void OnOperationModeChanged(OperationMode value)
    {
      if (this.LayerManager == null)
        return;
      this.LayerManager.OperationMode = value;
    }

    private void OnTilesStretchFactorChanged()
    {
      if (this._mapContentLayer != null)
        this._mapContentLayer.DisposeContent();
      this.MoveToPosition(this.InstantCenter);
    }

    public void OnOperationStatusChangedChanged(OperationStatusChangedEventArgs e)
    {
      EventHandler<OperationStatusChangedEventArgs> operationStatusChanged = this.OperationStatusChanged;
      if (operationStatusChanged == null)
        return;
      operationStatusChanged((object) this, e);
    }

    private void PositionDispatcherOperationStatusChanged(
      object sender,
      OperationStatusChangedEventArgs e)
    {
      switch (e.OperationStatus)
      {
        case OperationStatus.Idle:
          this.OnPositionManagerIdle();
          break;
        case OperationStatus.Normal:
          this.OnPositionManagerNormal();
          break;
        case OperationStatus.Busy:
          this.OnPositionManagerBusy();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (e), "OperationStatus value is out of range");
      }
      OperationStatus operationStatus;
      switch (this.OperationMode)
      {
        case OperationMode.None:
          operationStatus = OperationStatus.Busy;
          break;
        case OperationMode.Restricted:
          operationStatus = OperationStatus.Normal;
          break;
        case OperationMode.Full:
          operationStatus = OperationStatus.Idle;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this._uiDispatcher.BeginInvoke((Action) (() => this.OnOperationStatusChangedChanged(new OperationStatusChangedEventArgs(operationStatus))));
    }

    private void PositionDispatcherPositionChanged(object sender, Yandex.Maps.Events.PositionChangedEventArgs e) => this._uiDispatcher.BeginInvoke((Action) (() =>
    {
      bool flag1 = this.DoZoom(e.ZoomLevel);
      bool flag2 = this.DoTranslate(e.Position);
      if (flag1 || flag2)
      {
        this._zoomChangePending |= flag1;
        this._positionChangePending |= flag2;
        this._view.UpdateProjection();
        this._positionDispatcher.MapPositionChanged(this.InstantZoomLevel, this._view.Viewport);
      }
      if (e.PositionChangeType != PositionChangeType.Final)
        return;
      if (this._zoomChangePending)
        this.OnPropertyChanged("InstantZoomLevel");
      if (this._positionChangePending)
        this.OnPropertyChanged("InstantCenter");
      this._zoomChangePending = false;
      this._positionChangePending = false;
    }));

    protected virtual void OnPrinterConnected() => this.Reload();

    protected Point ViewportPointToRelativePoint(Point point) => this.ViewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(point, this.InstantZoomLevel));

    public bool Reload()
    {
      if (this.OperationMode == OperationMode.None || this.OperationMode == OperationMode.Disabled || this.LayerManager == null || double.IsNaN(this.TargetPosition.Zoom))
        return false;
      this.LayerManager.Reload(this.GetTargetViewportRect());
      return true;
    }

    private void DisposeTilesOutsideArea()
    {
      if (this.OperationMode == OperationMode.None || this.OperationMode == OperationMode.Disabled || this.LayerManager == null)
        return;
      this.LayerManager.DisposeContentOutsideArea(this.GetTargetViewportRect());
    }

    public void ClearPersistentCache() => this._tileManager.ClearPersistentCache(new AsyncCallback(this.ClearPersistentCacheComplete), (object) null);

    public void ClearPersistentCacheComplete(IAsyncResult result)
    {
    }

    protected void ZoomIn(Point? origin)
    {
      if (origin.HasValue)
        this._positionDispatcher.ZoomIn(this.ViewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(new Point(origin.Value.X - this.TranslateX, origin.Value.Y - this.TranslateY), this.InstantZoomLevel)));
      else
        this._positionDispatcher.ZoomInWithCurrentPositionAsScaleCenter();
    }

    protected void ZoomOut(Point? origin)
    {
      if (origin.HasValue)
        this._positionDispatcher.ZoomOut(this.ViewportPointConverter.ViewportPointToRelativePoint(new ViewportPoint(new Point(origin.Value.X - this.TranslateX, origin.Value.Y - this.TranslateY), this.InstantZoomLevel)));
      else
        this._positionDispatcher.ZoomOutWithCurrentPositionAsScaleCenter();
    }

    private void MoveToPosition(Point relativePoint) => this._positionDispatcher.MoveTo(relativePoint);

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler == null)
        return;
      this._uiDispatcher.BeginInvoke((Action) (() => handler((object) this, new PropertyChangedEventArgs(propertyName))));
    }

    protected double ScaleX
    {
      get => this._view.ScaleX;
      private set => this._view.ScaleX = value;
    }

    protected double ScaleY
    {
      get => this._view.ScaleY;
      private set => this._view.ScaleY = value;
    }

    protected double TranslateX
    {
      get => this._view.TranslateX;
      private set => this._view.TranslateX = value;
    }

    protected double TranslateY
    {
      get => this._view.TranslateY;
      private set => this._view.TranslateY = value;
    }

    protected IViewportPointConveter ViewportPointConverter { get; set; }

    private bool DoZoom(double zoomLevel)
    {
      if (Math.Abs(this.Model.InstantZoomLevel - zoomLevel) < 1E-07)
        return false;
      this.Model.InstantZoomLevel = zoomLevel;
      if (zoomLevel < (double) this._zoomInfo.MinZoom || zoomLevel > (double) this._zoomInfo.MaxVisibleZoom)
        return false;
      this.ScaleX = this.ScaleY = this._zoomLevelConverter.ZoomLevelToStretchFactor(zoomLevel, this._zoomInfo.MaxVisibleZoom);
      return true;
    }

    private bool DoTranslate(Point relativePoint)
    {
      bool flag = false;
      this.Model.InstantCenter = relativePoint;
      Point viewportPoint = this.ViewportPointConverter.RelativePointToViewportPoint(relativePoint, this.InstantZoomLevel);
      Point areaCenterOffset = this.GetVisibleAreaCenterOffset();
      double num1 = areaCenterOffset.X - viewportPoint.X;
      if (Math.Abs(this.TranslateX - num1) > 1E-07)
      {
        this.TranslateX = num1;
        flag = true;
      }
      double num2 = areaCenterOffset.Y - viewportPoint.Y;
      if (Math.Abs(this.TranslateY - num2) > 1E-07)
      {
        this.TranslateY = num2;
        flag = true;
      }
      return flag;
    }

    protected virtual void OnPositionManagerBusy() => this.OperationMode = OperationMode.None;

    protected virtual void OnPositionManagerNormal() => this.OperationMode = OperationMode.Restricted;

    protected virtual void OnPositionManagerIdle()
    {
      this.OperationMode = OperationMode.Full;
      if (this._initializePending)
        return;
      this.Reload();
      this.DisposeTilesOutsideArea();
    }

    protected ViewportRect GetTargetViewportRect()
    {
      Position targetPosition = this.TargetPosition;
      Point viewportPoint = this._viewportPointConverter.RelativePointToViewportPoint(targetPosition.RelativePoint, targetPosition.Zoom);
      Point areaCenterOffset = this.GetVisibleAreaCenterOffset();
      Size actualSize = this._view.ActualSize;
      return new ViewportRect(new Rect(viewportPoint.X - areaCenterOffset.X, viewportPoint.Y - areaCenterOffset.Y, actualSize.Width, actualSize.Height), targetPosition.Zoom);
    }
  }
}
