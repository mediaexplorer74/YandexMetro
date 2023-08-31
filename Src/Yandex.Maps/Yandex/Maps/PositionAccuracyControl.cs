// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionAccuracyControl
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Yandex.Ioc;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Media;
using Yandex.Positioning;
using Yandex.Positioning.Events;
using Yandex.Positioning.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  [TemplatePart(Name = "PositionEllipse", Type = typeof (Ellipse))]
  [TemplatePart(Name = "AccuracyStoryboard", Type = typeof (Storyboard))]
  [TemplateVisualState(Name = "EllipseVisible", GroupName = "AccuracyEllipseStates")]
  [TemplatePart(Name = "LayoutRoot", Type = typeof (Grid))]
  [TemplateVisualState(Name = "EllipseHidden", GroupName = "AccuracyEllipseStates")]
  [ComVisible(false)]
  public class PositionAccuracyControl : Control, IProjectable
  {
    private const string LayoutRoot = "LayoutRoot";
    private const string PositionEllipse = "PositionEllipse";
    private const string AccuracyStoryboard = "AccuracyStoryboard";
    private const string EllipseHiddenState = "EllipseHidden";
    private const string EllipseVisibleState = "EllipseVisible";
    private const string AccuracyEllipseStates = "AccuracyEllipseStates";
    private const int MaxDiameterToShowAccuracyEllipse = 1500;
    private Ellipse _positionEllipse;
    private DoubleAnimation _scaleXAnimation;
    private DoubleAnimation _scaleYAnimation;
    private Storyboard _accuracyStoryboard;
    private readonly IPositionWatcher _positionWatcher;
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly IUiDispatcher _uiDispatcher;
    private GeoPosition _currentGeoPosition;
    private double _currentDiameter;
    private MapBase _parentMap;
    private bool _shouldUpdateEllipseSize = true;
    private double _antiAccuracyEllipseWidth;
    private readonly object _syncAccuracyUpdate = new object();
    private Grid _layoutRoot;

    public PositionAccuracyControl()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IPositionWatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IGeoPixelConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>(), IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>())
    {
    }

    public PositionAccuracyControl(
      [NotNull] IPositionWatcher positionWatcher,
      [NotNull] IGeoPixelConverter geoPixelConverter,
      [NotNull] IViewportPointConveter viewportPointConveter,
      IUiDispatcher uiDispatcher)
    {
      this.DefaultStyleKey = (object) typeof (PositionAccuracyControl);
      MapLayer.SetAlignment((DependencyObject) this, Alignment.Center);
      if (positionWatcher == null)
        throw new ArgumentNullException(nameof (positionWatcher));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      if (viewportPointConveter == null)
        throw new ArgumentNullException(nameof (viewportPointConveter));
      this._positionWatcher = positionWatcher;
      this._geoPixelConverter = geoPixelConverter;
      this._viewportPointConveter = viewportPointConveter;
      this._uiDispatcher = uiDispatcher;
      this._positionWatcher.StatusUpdated += new EventHandler<PositionWatcherStatusChangedEventArgs>(this.PositionWatcherStatusUpdated);
      this._positionWatcher.PositionChanged += new EventHandler<Yandex.Positioning.PositionChangedEventArgs>(this.PositionWatcherPositionChanged);
      this.HidePosition();
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._layoutRoot = (Grid) this.GetTemplateChild("LayoutRoot");
      if (this._layoutRoot != null)
      {
        this._accuracyStoryboard = (Storyboard) ((FrameworkElement) this._layoutRoot).Resources[(object) "AccuracyStoryboard"];
        ((Timeline) this._accuracyStoryboard).Completed += new EventHandler(this.AccuracyStoryboardCompleted);
        this._scaleXAnimation = (DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this._accuracyStoryboard.Children)[0];
        this._scaleYAnimation = (DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this._accuracyStoryboard.Children)[1];
      }
      this._positionEllipse = (Ellipse) this.GetTemplateChild("PositionEllipse");
      this._antiAccuracyEllipseWidth = 1.0 / ((FrameworkElement) this._positionEllipse).Width;
    }

    private void PositionWatcherStatusUpdated(
      object sender,
      PositionWatcherStatusChangedEventArgs e)
    {
      if (this._positionWatcher.Status == GeoPositionStatus.Ready)
        return;
      this._currentGeoPosition = (GeoPosition) null;
      this.HidePosition();
    }

    private void AccuracyStoryboardCompleted(object sender, EventArgs e)
    {
      lock (this._syncAccuracyUpdate)
      {
        if (!this._shouldUpdateEllipseSize || this._accuracyStoryboard.GetCurrentState() == null)
          return;
        this._shouldUpdateEllipseSize = false;
        this._antiAccuracyEllipseWidth = 1.0 / this._currentDiameter;
        ((CompositeTransform) ((UIElement) this._positionEllipse).RenderTransform).ScaleX = 1.0;
        ((CompositeTransform) ((UIElement) this._positionEllipse).RenderTransform).ScaleY = 1.0;
      }
      ((FrameworkElement) this._positionEllipse).Width = this._currentDiameter;
      ((FrameworkElement) this._positionEllipse).Height = this._currentDiameter;
    }

    private void PositionWatcherPositionChanged(object sender, Yandex.Positioning.PositionChangedEventArgs e)
    {
      if (this._positionWatcher.Status != GeoPositionStatus.Ready)
        this.HidePosition();
      else
        this.DisplayPosition(this._positionWatcher.LastPosition);
    }

    private void DisplayPosition([NotNull] GeoPosition geoPosition)
    {
      this._currentGeoPosition = geoPosition != null ? geoPosition : throw new ArgumentNullException(nameof (geoPosition));
      this._uiDispatcher.BeginInvoke((Action) (() => this.UpdateAccuracy(this._currentGeoPosition)));
    }

    private void UpdateAccuracy([NotNull] GeoPosition geoPosition)
    {
      if (geoPosition == null)
        throw new ArgumentNullException(nameof (geoPosition));
      if (this.ParentMap == null || double.IsNaN(geoPosition.HorizontalAccuracy))
        return;
      double viewportLength = this.AccuracyToViewportLength(geoPosition);
      if (Math.Abs(viewportLength - this._currentDiameter) < double.Epsilon)
        return;
      this._currentDiameter = viewportLength;
      if (0.0 < viewportLength && viewportLength <= 1500.0)
      {
        VisualStateManager.GoToState((Control) this, "EllipseVisible", true);
        lock (this._syncAccuracyUpdate)
        {
          if (this._scaleXAnimation == null || this._scaleYAnimation == null || this._accuracyStoryboard == null)
            return;
          this._scaleXAnimation.To = this._scaleYAnimation.To = new double?(viewportLength * this._antiAccuracyEllipseWidth);
          this._accuracyStoryboard.Begin();
        }
      }
      else
        VisualStateManager.GoToState((Control) this, "EllipseHidden", true);
    }

    private double AccuracyToViewportLength(GeoPosition geoPosition) => this._viewportPointConveter.RelativePointToViewportPoint(new Point(this._geoPixelConverter.MetersToRelativeDistance(geoPosition.GeoCoordinate.Latitude, geoPosition.HorizontalAccuracy), 0.0), this.ParentMap.InstantZoomLevel).X;

    public void HidePosition() => this._uiDispatcher.BeginInvoke((Action) (() => VisualStateManager.GoToState((Control) this, "EllipseHidden", true)));

    public MapBase ParentMap
    {
      get => this._parentMap ?? this.InitializeParentMap();
      set
      {
        if (this._parentMap == value)
          return;
        if (this._parentMap != null)
          this._parentMap.OperationStatusChanged -= new EventHandler<OperationStatusChangedEventArgs>(this.ParentMapOperationStatusChanged);
        this._parentMap = value;
        if (this._parentMap == null)
          return;
        this._parentMap.OperationStatusChanged += new EventHandler<OperationStatusChangedEventArgs>(this.ParentMapOperationStatusChanged);
      }
    }

    [CanBeNull]
    private MapBase InitializeParentMap()
    {
      mapBase = (MapBase) null;
      DependencyObject dependencyObject = (DependencyObject) this;
      while (true)
      {
        switch (dependencyObject)
        {
          case null:
          case MapBase mapBase:
            goto label_3;
          default:
            dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            continue;
        }
      }
label_3:
      this.ParentMap = mapBase;
      return this._parentMap;
    }

    private void ParentMapOperationStatusChanged(object sender, OperationStatusChangedEventArgs e)
    {
      if (e.OperationStatus == OperationStatus.Busy)
        return;
      this._shouldUpdateEllipseSize = true;
    }

    public void ProjectionUpdated(ProjectionUpdateLevel updateLevel)
    {
      if (updateLevel == ProjectionUpdateLevel.None || updateLevel == ProjectionUpdateLevel.Linear)
        return;
      if (this._currentGeoPosition != null)
        this.UpdateAccuracy(this._currentGeoPosition);
      ((UIElement) this).InvalidateMeasure();
    }
  }
}
