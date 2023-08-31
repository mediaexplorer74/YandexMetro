// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionControl
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Yandex.Ioc;
using Yandex.Maps.IoC;
using Yandex.Positioning;
using Yandex.Positioning.Events;
using Yandex.Positioning.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  [TemplatePart(Name = "LayoutRoot", Type = typeof (Grid))]
  [TemplateVisualState(Name = "PositionUnknown", GroupName = "GpsLbs")]
  [TemplateVisualState(Name = "PositionStop", GroupName = "GpsLbs")]
  [TemplatePart(Name = "AngleStoryboard", Type = typeof (Storyboard))]
  [TemplateVisualState(Name = "PositionMove", GroupName = "GpsLbs")]
  [ComVisible(false)]
  public class PositionControl : Control
  {
    private const string GpsLbs = "GpsLbs";
    private const string PositionMove = "PositionMove";
    private const string PositionStop = "PositionStop";
    private const string PositionUnknown = "PositionUnknown";
    private const string LayoutRoot = "LayoutRoot";
    private const string AngleStoryboard = "AngleStoryboard";
    private readonly IPositionWatcher _positionWatcher;
    private readonly IUiDispatcher _uiDispatcher;
    private double? _currentGeoPositionCourse;
    private bool _hidden;
    private Storyboard _angleStoryboard;
    private DoubleAnimation _angleAnimation;
    private Grid _layoutRoot;

    public PositionControl()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IPositionWatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>())
    {
    }

    private PositionControl([NotNull] IPositionWatcher positionWatcher, IUiDispatcher uiDispatcher)
    {
      this.DefaultStyleKey = (object) typeof (PositionControl);
      MapLayer.SetAlignment((DependencyObject) this, Alignment.Center);
      this._positionWatcher = positionWatcher != null ? positionWatcher : throw new ArgumentNullException(nameof (positionWatcher));
      this._uiDispatcher = uiDispatcher;
      this._positionWatcher.StatusUpdated += new EventHandler<PositionWatcherStatusChangedEventArgs>(this.PositionWatcherStatusUpdated);
      this._positionWatcher.PositionChanged += new EventHandler<PositionChangedEventArgs>(this.PositionWatcherPositionChanged);
      this.HidePosition();
      this._uiDispatcher.BeginInvokeExplicit((Action) (() =>
      {
        if (this._positionWatcher.Status != GeoPositionStatus.Ready)
          return;
        this.DisplayPosition(this._positionWatcher.LastPosition);
      }));
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._layoutRoot = (Grid) this.GetTemplateChild("LayoutRoot");
      if (this._layoutRoot == null)
        return;
      this._angleStoryboard = (Storyboard) ((FrameworkElement) this._layoutRoot).Resources[(object) "AngleStoryboard"];
      this._angleAnimation = (DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this._angleStoryboard.Children)[0];
    }

    private void PositionWatcherStatusUpdated(
      object sender,
      PositionWatcherStatusChangedEventArgs e)
    {
      if (this._positionWatcher.Status == GeoPositionStatus.Ready)
        return;
      this._uiDispatcher.BeginInvoke(new Action(this.HidePosition));
    }

    private void PositionWatcherPositionChanged(object sender, PositionChangedEventArgs e)
    {
      if (this._positionWatcher.Status != GeoPositionStatus.Ready)
        this._uiDispatcher.BeginInvoke(new Action(this.HidePosition));
      else
        this._uiDispatcher.BeginInvoke((Action) (() => this.DisplayPosition(this._positionWatcher.LastPosition)));
    }

    private void DisplayPosition([NotNull] GeoPosition geoPosition)
    {
      if (geoPosition == null)
        throw new ArgumentNullException(nameof (geoPosition));
      if (!this._currentGeoPositionCourse.HasValue || this._currentGeoPositionCourse.Value.CompareTo(geoPosition.Course) != 0 || this._hidden)
      {
        this.UpdateGeoPositionImageAndRotation(geoPosition.Course);
        this._currentGeoPositionCourse = new double?(geoPosition.Course);
      }
      this._hidden = false;
    }

    public void HidePosition()
    {
      VisualStateManager.GoToState((Control) this, "PositionUnknown", true);
      this._hidden = true;
    }

    private void UpdateGeoPositionImageAndRotation(double course)
    {
      if (this._angleAnimation == null || this._angleStoryboard == null)
        return;
      if (double.IsNaN(course))
      {
        VisualStateManager.GoToState((Control) this, "PositionStop", true);
        this._angleAnimation.From = this._angleAnimation.To = new double?(0.0);
        this._angleStoryboard.Begin();
      }
      else
      {
        VisualStateManager.GoToState((Control) this, "PositionMove", true);
        this._angleAnimation.From = new double?(PositionControl.CalcAngleFrom(this._angleAnimation.To.HasValue ? this._angleAnimation.To.Value : 0.0, course));
        this._angleAnimation.To = new double?(course);
        this._angleStoryboard.Begin();
      }
    }

    private static double CalcAngleFrom(double currentAngle, double course)
    {
      if (Math.Abs(currentAngle - course) < 180.0)
        return currentAngle;
      return currentAngle < 180.0 ? currentAngle + 360.0 : currentAngle - 360.0;
    }
  }
}
