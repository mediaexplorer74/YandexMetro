// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.MetroMapView
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Y.Common;
using Y.Metro.ServiceLayer.Entities;
using Y.Metro.ServiceLayer.Enums;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common.Control;
using Y.UI.Common.Utility;
using Yandex.Metro.Logic;
using Yandex.Metro.Logic.Behaviors;
using Yandex.Metro.Logic.FastScheme;
using Yandex.Metro.Resources;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views
{
  public class MetroMapView : MetroPage, IMetroMap
  {
    private BindableApplicationBarIconButton _btnClearRoute;
    private BindableApplicationBarIconButton _btnPlaneMode;
    private BindableApplicationBarMenuItem _mPodorosnik;
    private bool _allowRestorationAnimation = true;
    private bool _allowFocus = true;
    private bool _isDragging;
    private bool _isPinching;
    private Point _ptPinchPositionStart;
    private Matrix _initialMatrix;
    private bool _isLoaded;
    private ShakeDetector _shakeDetector;
    internal Storyboard SearchStationActivateAnimationFrom;
    internal Storyboard SearchStationActivateAnimationTo;
    internal Storyboard SearchStationDeactivateAnimationFrom;
    internal Storyboard SearchStationDeactivateAnimationFromFast;
    internal Storyboard SearchStationDeactivateAnimationToFast;
    internal Storyboard SearchStationDeactivateAnimationTo;
    internal Storyboard MapTypeSwitchAnimation;
    internal Storyboard MapTypeSwitchBackAnimation;
    internal Storyboard SelectStationAnimation;
    internal Storyboard LogoAnimation;
    internal Grid LayoutRoot;
    internal Grid SchemeMap;
    internal TranslateTransform SchemeMapTransform;
    internal Canvas MapRoot;
    internal BitmapCache cacheMode;
    internal MatrixTransform previousTransform;
    internal TransformGroup currentTransform;
    internal ScaleTransform scaleTransform;
    internal RotateTransform rotateTransform;
    internal TranslateTransform translateTransform;
    internal Canvas RouteRoot;
    internal ScrollViewer PlainMap;
    internal TranslateTransform PlainMapScrollTransform;
    internal PlaneProjection PlainMapTransform;
    internal Canvas PlainMapCanvas;
    internal TapButton revertButton;
    internal Grid GridRoute;
    internal TranslateTransform RouteTransform;
    internal Grid StartStationGrid;
    internal Line StartLine;
    internal Grid EndStationGrid;
    internal Line EndLine;
    internal Pivot pivot;
    internal Ellipse animateStationCircle;
    internal TranslateTransform EllipseTransform;
    internal Image logoImage;
    internal TranslateTransform LogoTransform;
    internal BindableApplicationBar bar;
    private bool _contentLoaded;

    public MetroMapView()
    {
      this.InitializeComponent();
      Locator.MainStatic.MetroMap = (IMetroMap) this;
      Locator.SettingsStatic.PropertyChanged += (PropertyChangedEventHandler) ((s, e) => this.HandlePropertyChanged(e.PropertyName));
      Locator.MainStatic.PropertyChanged += (PropertyChangedEventHandler) ((s, e) => this.HandlePropertyChanged(e.PropertyName));
      this.UpdateApplicationBarButtons();
    }

    private void UpdateApplicationBarButtons()
    {
      this._btnPlaneMode = new BindableApplicationBarIconButton()
      {
        Text = Localization.Menu_PlainMap,
        IconUri = new Uri("/Images/list.png", UriKind.Relative),
        Command = (ICommand) Locator.MainStatic.SwithModeCommand
      };
      this._mPodorosnik = new BindableApplicationBarMenuItem()
      {
        Text = Localization.Podorosnik,
        Command = (ICommand) Locator.MainStatic.PodorosnikCommand
      };
      this._btnClearRoute = new BindableApplicationBarIconButton()
      {
        Text = Localization.Menu_ClearRoute,
        IconUri = new Uri("/Toolkit.Content/ApplicationBar.Delete.png", UriKind.Relative),
        Command = (ICommand) Locator.MainStatic.ClearRouteCommand
      };
    }

    private void HandlePropertyChanged(string property)
    {
      switch (property)
      {
        case "SelectRoute":
          this.GenerateRoute();
          break;
      }
    }

    private void ChooseStationExecuted(object sender, EventArgs e)
    {
      FrameworkElement element = (FrameworkElement) sender;
      Locator.MainStatic.ChooseStation(element.DataContext as MetroStation, element);
    }

    private void OnHideSelectStationControl(object sender, MouseButtonEventArgs e)
    {
      if (!Locator.MainStatic.IsStationPopupVisible)
        return;
      Locator.MainStatic.HideSelectedStationControl();
      e.Handled = true;
    }

    private void PivotLoadedPivotItem(object sender, PivotItemEventArgs e) => ThreadPool.QueueUserWorkItem((WaitCallback) (unused =>
    {
      Thread.Sleep(10);
      ((DependencyObject) this).Dispatcher.BeginInvoke(new Action(this.UpdateSelectedRoute));
    }));

    private void UpdateSelectedRoute()
    {
      Route route = ((Collection<Route>) Locator.MainStatic.Routes)[this.pivot.SelectedIndex];
      RouteTimeHelper.UpdateRouteTime(route, MetroService.Instance.AppSettings.TimeType);
      Locator.MainStatic.SelectRoute = route;
    }

    private void GestureListenerPreventDragDeltaGesture(object sender, DragDeltaGestureEventArgs e) => e.Handled = true;

    private void GestureListenerPreventDragCompletedGesture(
      object sender,
      DragCompletedGestureEventArgs e)
    {
      e.Handled = true;
    }

    private void GenerateRoute()
    {
      if (Locator.MainStatic.SelectRoute != null)
      {
        Route route = Locator.MainStatic.SelectRoute;
        ThreadPool.QueueUserWorkItem((WaitCallback) (unused =>
        {
          Thread.Sleep(500);
          Dictionary<int, MetroStation>.ValueCollection values = route.RouteScheme.Stations.Values;
          double num1 = values.Min<MetroStation>((Func<MetroStation, double>) (r => Math.Min(r.SchemePosition.X, r.TextPosition.X)));
          double num2 = values.Max<MetroStation>((Func<MetroStation, double>) (r => Math.Max(r.SchemePosition.X, r.TextPosition.X)));
          double num3 = values.Min<MetroStation>((Func<MetroStation, double>) (r => Math.Min(r.SchemePosition.Y, r.TextPosition.Y)));
          double num4 = values.Max<MetroStation>((Func<MetroStation, double>) (r => Math.Max(r.SchemePosition.Y, r.TextPosition.Y)));
          double num5 = Math.Abs(num2 - num1) + 30.0;
          double num6 = Math.Abs(num3 - num4) + 30.0;
          double widthCentr = Math.Abs(num1 + num2) / 2.0;
          double heightCentr = Math.Abs(num3 + num4) / 2.0;
          double val2 = 480.0 / num5;
          this.SetCentrPosition(Math.Min(Math.Min(600.0 / num6, val2), 3.0), heightCentr, widthCentr, false);
        }));
        List<UIElement> _routeObjects = new List<UIElement>();
        MapGenerator.GenerateMapFromScheme(this.RouteRoot, route.RouteScheme, new EventHandler<GestureEventArgs>(this.ChooseStationExecuted), true, _routeObjects);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) this.RouteRoot).Children).Clear();
        foreach (UIElement uiElement in _routeObjects)
          ((PresentationFrameworkCollection<UIElement>) ((Panel) this.RouteRoot).Children).Add(uiElement);
      }
      else
        ((PresentationFrameworkCollection<UIElement>) ((Panel) this.RouteRoot).Children).Clear();
    }

    private void SetCentrPosition(
      double scale,
      double heightCentr,
      double widthCentr,
      bool isStation = true)
    {
      DispatcherHelper.UI((Action) (() =>
      {
        if (isStation)
        {
          Point point1 = ((GeneralTransform) this.previousTransform).Transform(new Point(0.0, 0.0));
          Point point2 = ((GeneralTransform) this.previousTransform).Transform(new Point(FastKeeper.Scheme.Width, FastKeeper.Scheme.Height));
          double num1 = point2.X - point1.X;
          double num2 = point2.Y - point1.Y;
          bool flag1 = num1 < 470.0 && Math.Abs(num1 - 470.0) > 0.01;
          bool flag2 = num2 < 620.0 && Math.Abs(num2 - 620.0) > 0.01;
          if (flag1 || flag2)
            return;
        }
        Point point = ((GeneralTransform) this.previousTransform).Transform(new Point(widthCentr, heightCentr));
        double num = scale / this.previousTransform.Matrix.M11;
        this.translateTransform.X = 240.0 - point.X;
        this.translateTransform.Y = 320.0 - point.Y;
        this.scaleTransform.CenterX = widthCentr;
        this.scaleTransform.CenterY = heightCentr;
        this.scaleTransform.CenterX = point.X;
        this.scaleTransform.CenterY = point.Y;
        this.scaleTransform.ScaleX = this.scaleTransform.ScaleY = num;
        ((DependencyObject) this).Dispatcher.BeginInvoke(new Action(this.TransferTransforms));
      }));
    }

    private void ReadSchemeAndGenerateMap()
    {
      Locator.ProgressStatic.StartJob(Localization.Progress_Map);
      Canvas routeRoot = this.RouteRoot;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.MapRoot).Children).Clear();
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.MapRoot).Children).Add((UIElement) routeRoot);
      ApplicationSettings appSettings = MetroService.Instance.AppSettings;
      SettingsViewModel settingsStatic = Locator.SettingsStatic;
      settingsStatic.ActualMeta = new SchemeMeta()
      {
        Language = settingsStatic.MapLanguage.Code,
        Type = appSettings.Scheme.Type
      };
      FastKeeper.Scheme = IsolatedStorageHelper.ReadScheme(settingsStatic.ActualMeta.Type, settingsStatic.ActualMeta.Language);
      this.GenerateSchemeNew(FastKeeper.Scheme);
      MetroMapView.ShowSavedRoute(appSettings, settingsStatic);
      this.UpdateAppBar(Locator.MainStatic.IsRouteAvailable, false);
      Locator.ProgressStatic.StopJob(Localization.Progress_Map);
      this.ShowWorkTimeMessage();
      Locator.MainStatic.UpdateNearestStation();
    }

    private void ShowWorkTimeMessage()
    {
      MetroScheme scheme = FastKeeper.Scheme;
      if (scheme == null || scheme.WorkTime == null)
        return;
      DateTime dateTime = DateTime.Parse(scheme.WorkTime.OpenTime, (IFormatProvider) CultureHelper.EnUs);
      if (!(DateTime.Now >= DateTime.Parse(scheme.WorkTime.CloseTime, (IFormatProvider) CultureHelper.EnUs)) || !(DateTime.Now <= dateTime))
        return;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => MessageBox.Show(string.Format(Localization.Metro_WorkTimeMessage, (object) scheme.WorkTime.CloseTime, (object) scheme.WorkTime.OpenTime))));
    }

    private static void ShowSavedRoute(ApplicationSettings appSettings, SettingsViewModel settings)
    {
      if (appSettings.Route == null || appSettings.Route.SchemaId != appSettings.Scheme.Id)
        return;
      Dictionary<int, MetroStation> stations = FastKeeper.Scheme.Stations;
      MetroStation parameter1 = stations[appSettings.Route.FromStationId];
      MetroStation parameter2 = stations[appSettings.Route.ToStationId];
      MainViewModel mainStatic = Locator.MainStatic;
      mainStatic.StartStationCommand.Execute(parameter1);
      mainStatic.EndStationCommand.Execute(parameter2);
    }

    private void GenerateSchemeNew(MetroScheme metroScheme)
    {
      if (metroScheme == null)
        return;
      this.ResetMapWidthHeightNew(metroScheme);
      MapGenerator.GenerateMapFromScheme(this.MapRoot, metroScheme, new EventHandler<GestureEventArgs>(this.ChooseStationExecuted));
      MapGenerator.GenerateSelectUserControl(this.MapRoot, metroScheme, new MouseButtonEventHandler(this.OnHideSelectStationControl));
    }

    private void ResetMapWidthHeightNew(MetroScheme metroScheme)
    {
      Scheme scheme = MetroService.Instance.AppSettings.Scheme;
      SchemeMatrix matrix = scheme.Matrix;
      this._initialMatrix = new Matrix()
      {
        OffsetX = matrix.OffsetX,
        OffsetY = matrix.OffsetY,
        M11 = matrix.M11,
        M22 = matrix.M22
      };
      this.previousTransform.Matrix = scheme.Type == SchemeType.Minsk || scheme.Type == SchemeType.Kharkiv ? this._initialMatrix : new Matrix();
      ((FrameworkElement) this.MapRoot).Height = ((FrameworkElement) this.RouteRoot).Height = metroScheme.Height;
      ((FrameworkElement) this.MapRoot).Width = ((FrameworkElement) this.RouteRoot).Width = metroScheme.Width;
      Constants.MinMapScale = Math.Min(460.0 / metroScheme.Width, 767.0 / metroScheme.Height);
      if (((FrameworkElement) this.MapRoot).Width < 480.0)
        this.translateTransform.X = (480.0 - ((FrameworkElement) this.MapRoot).Width) / 2.0;
      if (((FrameworkElement) this.MapRoot).Height < 622.0)
        this.translateTransform.Y = (622.0 - ((FrameworkElement) this.MapRoot).Height) / 2.0;
      this.TransferTransforms();
      this._isLoaded = true;
    }

    private void InitializeShakeDetector()
    {
      this._shakeDetector = new ShakeDetector();
      this._shakeDetector.ShakeEvent += new EventHandler<EventArgs>(this.ShakeDetectorShakeEvent);
      this._shakeDetector.Start();
    }

    private void StopShakeDetectorMonitor() => this._shakeDetector.Stop();

    private void ShakeDetectorShakeEvent(object sender, EventArgs e) => Locator.MainStatic.ShowClearRouteMessage();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      try
      {
        this.InitializeShakeDetector();
      }
      catch (Exception ex)
      {
        MetroService.Instance.HandleException(ex);
      }
      base.OnNavigatedTo(e);
      if (e.NavigationMode == null)
        this.ReadSchemeAndGenerateMap();
      if (e.NavigationMode != 1 || Locator.SettingsStatic.TouchRotationEnabled || !MetroService.Instance.AppState.RotationWasEnabled)
        return;
      this.ResetMapWidthHeightNew(FastKeeper.Scheme);
    }

    protected virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
      try
      {
        this.StopShakeDetectorMonitor();
      }
      catch (Exception ex)
      {
        MetroService.Instance.HandleException(ex);
      }
      MetroService.Instance.AppState.RotationWasEnabled = Locator.SettingsStatic.TouchRotationEnabled;
      ((Page) this).OnNavigatingFrom(e);
    }

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      if (Locator.MainStatic.IsStationPopupVisible)
      {
        e.Cancel = true;
        Locator.MainStatic.HideSelectedStationControl();
      }
      if (Locator.MainStatic.SearchIsActive)
      {
        e.Cancel = true;
        this.SetSearch(false, false);
      }
      if (Locator.MainStatic.IsPlainMode)
      {
        e.Cancel = true;
        Locator.MainStatic.IsPlainMode = false;
        this.AnimateNearestStation(false);
      }
      base.OnBackKeyPress(e);
    }

    private void StartStationTap(object sender, RoutedEventArgs e)
    {
      Locator.MainStatic.IsFromDirection = true;
      this.SetSearch(true, false);
    }

    private void EndStationTap(object sender, RoutedEventArgs e)
    {
      Locator.MainStatic.IsFromDirection = false;
      this.SetSearch(true, false);
    }

    public void GenerateMap() => this.ReadSchemeAndGenerateMap();

    public void FocusStation(MetroStation station, bool chooseStation = false)
    {
      MainViewModel main = Locator.MainStatic;
      ThreadPool.QueueUserWorkItem((WaitCallback) (s =>
      {
        this._allowFocus = true;
        Thread.Sleep(1000);
        if (!chooseStation && (this._isDragging || this._isPinching || !this._allowFocus || main.IsHoldStation || main.IsStartStationAvailable && main.IsEndStationAvailable))
          return;
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          Ellipse station2 = MapGenerator.GetStation(station.Id);
          if (!(((FrameworkElement) station2).DataContext is MetroStation dataContext2))
            return;
          this.SetCentrPosition(Constants.MinMapScale < 1.0 ? 1.0 : Constants.MinMapScale, dataContext2.SchemePosition.Y, dataContext2.SchemePosition.X);
          if (!chooseStation)
            return;
          Locator.MainStatic.ChooseStation(dataContext2, (FrameworkElement) station2);
        }));
      }));
    }

    public void SetDefaultZoom(Route route)
    {
      if (route == null)
        return;
      Dictionary<int, MetroStation>.ValueCollection values = route.RouteScheme.Stations.Values;
      double num1 = values.Min<MetroStation>((Func<MetroStation, double>) (r => Math.Min(r.SchemePosition.X, r.TextPosition.X)));
      double num2 = values.Max<MetroStation>((Func<MetroStation, double>) (r => Math.Max(r.SchemePosition.X, r.TextPosition.X)));
      double num3 = values.Min<MetroStation>((Func<MetroStation, double>) (r => Math.Min(r.SchemePosition.Y, r.TextPosition.Y)));
      double num4 = values.Max<MetroStation>((Func<MetroStation, double>) (r => Math.Max(r.SchemePosition.Y, r.TextPosition.Y)));
      double widthCentr = Math.Abs(num1 + num2) / 2.0;
      this.SetCentrPosition(1.0, Math.Abs(num3 + num4) / 2.0, widthCentr, false);
    }

    public void SetSearch(bool isActive, bool skipAnimation = false)
    {
      if (isActive)
      {
        Locator.MainStatic.SearchIsActive = true;
        if (Locator.MainStatic.IsFromDirection)
          this.SearchStationActivateAnimationFrom.Begin();
        else
          this.SearchStationActivateAnimationTo.Begin();
      }
      else
      {
        Locator.MainStatic.SearchIsActive = false;
        if (Locator.MainStatic.IsFromDirection)
        {
          if (skipAnimation)
            this.SearchStationDeactivateAnimationFromFast.Begin();
          else
            this.SearchStationDeactivateAnimationFrom.Begin();
        }
        else if (skipAnimation)
          this.SearchStationDeactivateAnimationToFast.Begin();
        else
          this.SearchStationDeactivateAnimationTo.Begin();
      }
    }

    private void DrugAnimationCompleted(object sender, EventArgs e)
    {
      if (Locator.MainStatic.NearestStation == null)
        return;
      this.AnimationBigEllipse(0);
    }

    private void AnimationBigEllipse(int time = 200)
    {
      MainViewModel mainStatic = Locator.MainStatic;
      PlainModeBehavior.UpdatePosition((Ellipse) ((FrameworkElement) this.PlainMapCanvas).FindName(PlainModeBehavior.GpsEllipse));
      Storyboard resource = (Storyboard) ((FrameworkElement) this.PlainMapCanvas).Resources[(object) "gpsPlainAnimation"];
      ((Timeline) resource).BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds((double) time));
      ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) resource.Children)[3]).To = new double?(6.0);
      ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) resource.Children)[4]).To = new double?(PlainModeBehavior.NerestTopPositionSmall);
      resource.Begin();
    }

    public void AnimateNearestStation(bool isDragAnimation = false)
    {
      MainViewModel mainStatic = Locator.MainStatic;
      if (mainStatic.NearestStation == null)
        return;
      if (!mainStatic.IsPlainMode)
      {
        Storyboard resource = (Storyboard) ((FrameworkElement) this.MapRoot).Resources[(object) "gpsAnimation"];
        Ellipse name1 = (Ellipse) ((FrameworkElement) this.MapRoot).FindName(GpsBehavior.GpsEllipse);
        Ellipse name2 = (Ellipse) ((FrameworkElement) this.MapRoot).FindName(GpsBehavior.GpsEllipseSmall);
        if (((FrameworkElement) name1).Height < GpsBehavior.InitialSize)
        {
          GpsBehavior.UpdatePosition(name1, name2, mainStatic.NearestStation);
          ((Timeline) resource).BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(400.0));
        }
        ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) resource.Children)[3]).To = new double?(mainStatic.NearestStation.SchemePosition.X - 39.0);
        ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) resource.Children)[4]).To = new double?(mainStatic.NearestStation.SchemePosition.Y - 39.0);
        resource.Begin();
      }
      else
      {
        Ellipse name3 = (Ellipse) ((FrameworkElement) this.PlainMapCanvas).FindName(PlainModeBehavior.GpsEllipse);
        if (name3 == null)
          return;
        if (((FrameworkElement) name3).Height < (double) PlainModeBehavior.InitialSize)
          PlainModeBehavior.UpdatePosition(name3);
        Ellipse name4 = (Ellipse) ((FrameworkElement) this.PlainMapCanvas).FindName(PlainModeBehavior.GpsEllipseSmall);
        ((UIElement) name4).Opacity = 1.0;
        if (isDragAnimation)
        {
          Storyboard resource = (Storyboard) ((FrameworkElement) this.PlainMapCanvas).Resources[(object) "gpsSmallEllipseAnimation"];
          ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) resource.Children)[0]).To = new double?(36.0);
          ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) resource.Children)[1]).To = new double?(PlainModeBehavior.NerestTopPositionDrag);
          resource.Begin();
        }
        else
        {
          Canvas.SetTop((UIElement) name4, PlainModeBehavior.NerestTopPositionDrag);
          this.AnimationBigEllipse();
        }
      }
    }

    public void AnimateSelectStation(
      FrameworkElement element,
      MetroColor color,
      bool isFromDirection)
    {
      Point point = ((UIElement) element).TransformToVisual(Application.Current.RootVisual).Transform(new Point(0.0, 0.0));
      this.EllipseTransform.X = point.X;
      this.EllipseTransform.Y = point.Y;
      ((Shape) this.animateStationCircle).Fill = (Brush) MapGenerator.GetBrush(color);
      ((UIElement) this.animateStationCircle).Opacity = 1.0;
      DoubleAnimation child1 = (DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this.SelectStationAnimation.Children)[0];
      DoubleAnimation child2 = (DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this.SelectStationAnimation.Children)[2];
      DoubleAnimation child3 = (DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this.SelectStationAnimation.Children)[1];
      TimeSpan timeSpan = MetroService.Instance.AppSettings.Scheme.Type == SchemeType.Moscow ? TimeSpan.FromMilliseconds(800.0) : TimeSpan.FromMilliseconds(400.0);
      ((Timeline) child3).Duration = ((Timeline) child1).Duration = ((Timeline) child2).Duration = Duration.op_Implicit(timeSpan);
      child3.To = new double?(isFromDirection ? 203.0 : 435.0);
      this.SelectStationAnimation.Begin();
    }

    public void UpdateAppBar(bool isRouteExist, bool isStationSelect = false)
    {
      ApplicationSettings appSettings = MetroService.Instance.AppSettings;
      if (appSettings.Scheme != null && appSettings.Scheme.Type == SchemeType.SaintPetersburg)
        this.bar.SetVisible(this._mPodorosnik);
      else
        this.bar.SetHidden(this._mPodorosnik);
      this.bar.SetHidden(this._btnPlaneMode, this._btnClearRoute);
      if (isRouteExist)
      {
        this.bar.SetVisible(this._btnClearRoute, this._btnPlaneMode);
      }
      else
      {
        if (!isStationSelect)
          return;
        this.bar.SetVisible(this._btnClearRoute);
      }
    }

    private void ButtonScaleUp(object sender, GestureEventArgs e) => this.ScaleDown(-0.5);

    private void ButtonScaleDown(object sender, GestureEventArgs e) => this.ScaleDown();

    private void GestureListenerDoubleTap(object sender, GestureEventArgs e)
    {
      if (Locator.MainStatic.SearchIsActive)
        return;
      e.Handled = true;
      Point point = ((GeneralTransform) this.previousTransform).Transform(e.GetPosition((UIElement) this.MapRoot));
      this.scaleTransform.CenterX = point.X;
      this.scaleTransform.CenterY = point.Y;
      this.rotateTransform.CenterX = point.X;
      this.rotateTransform.CenterY = point.Y;
      this.ScaleDown(-0.5);
    }

    private void ScaleDown(double zoomDecrease = 0.5, double? x = null, double? y = null)
    {
      double m11 = this.previousTransform.Matrix.M11;
      double num = (m11 - zoomDecrease) / m11;
      if (x.HasValue)
        this.scaleTransform.CenterX = x.Value;
      if (y.HasValue)
        this.scaleTransform.CenterX = y.Value;
      this.scaleTransform.ScaleX = this.scaleTransform.ScaleY = num;
      ((DependencyObject) this).Dispatcher.BeginInvoke(new Action(this.TransferTransforms));
    }

    private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs args)
    {
      if (Locator.MainStatic.IsHoldStation || Locator.MainStatic.SearchIsActive)
        return;
      Point point1 = ((GeneralTransform) this.previousTransform).Transform(new Point(0.0, 0.0));
      Point point2 = ((GeneralTransform) this.previousTransform).Transform(new Point(FastKeeper.Scheme.Width, FastKeeper.Scheme.Height));
      double num1 = point2.X - point1.X;
      double num2 = point2.Y - point1.Y;
      bool flag1 = num1 < 470.0 && Math.Abs(num1 - 470.0) > 0.01;
      bool flag2 = num2 < 620.0 && Math.Abs(num2 - 620.0) > 0.01;
      if (flag1 && flag2)
        return;
      this._isDragging = true;
    }

    private void GestureListenerDragDelta(object sender, DragDeltaGestureEventArgs args)
    {
      if (Locator.MainStatic.IsHoldStation)
      {
        IEnumerable<UIElement> inHostCoordinates = VisualTreeHelper.FindElementsInHostCoordinates(args.GetPosition(Application.Current.RootVisual), Application.Current.RootVisual);
        bool flag = false;
        foreach (UIElement uiElement in inHostCoordinates)
        {
          if (uiElement is FrameworkElement element && element.DataContext is MetroStation dataContext)
          {
            flag = true;
            Locator.MainStatic.ChooseStation(dataContext, element, new double?(args.VerticalChange));
            break;
          }
        }
        if (flag)
          return;
        Locator.MainStatic.HideSelectedStationControl();
      }
      else
      {
        if (!this._isDragging)
          return;
        Point point1 = ((GeneralTransform) this.previousTransform).Transform(new Point(0.0, 0.0));
        Point point2 = ((GeneralTransform) this.previousTransform).Transform(new Point(FastKeeper.Scheme.Width, FastKeeper.Scheme.Height));
        double num1 = point2.X - point1.X;
        double num2 = point2.Y - point1.Y;
        bool flag1 = num1 > 470.0 && Math.Abs(num1 - 470.0) > 0.01;
        bool flag2 = num2 > 620.0 && Math.Abs(num2 - 620.0) > 0.01;
        if (flag1)
          this.translateTransform.X += args.HorizontalChange;
        if (!flag2)
          return;
        this.translateTransform.Y += args.VerticalChange;
      }
    }

    private void GestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs args)
    {
      if (Locator.MainStatic.IsHoldStation || !this._isDragging)
        return;
      this.TransferTransforms();
      this._isDragging = false;
    }

    private void GestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs args)
    {
      if (Locator.MainStatic.SearchIsActive)
        return;
      this._isPinching = true;
      if (!this._isPinching)
        return;
      Point position = args.GetPosition((UIElement) this.MapRoot);
      MetroService.Instance.AppState.LastPinchCenterPoint = position;
      Point point = ((GeneralTransform) this.previousTransform).Transform(position);
      this.scaleTransform.CenterX = point.X;
      this.scaleTransform.CenterY = point.Y;
      this.rotateTransform.CenterX = point.X;
      this.rotateTransform.CenterY = point.Y;
      this._ptPinchPositionStart = args.GetPosition((UIElement) this);
    }

    private void GestureListenerPinchDelta(object sender, PinchGestureEventArgs args)
    {
      if (!this._isPinching)
        return;
      this.scaleTransform.ScaleX = args.DistanceRatio;
      this.scaleTransform.ScaleY = args.DistanceRatio;
      if (Locator.SettingsStatic.TouchRotationEnabled)
        this.rotateTransform.Angle = args.TotalAngleDelta;
      Point position = args.GetPosition((UIElement) this);
      this.translateTransform.X = position.X - this._ptPinchPositionStart.X;
      this.translateTransform.Y = position.Y - this._ptPinchPositionStart.Y;
    }

    private void GestureListenerPinchCompleted(object sender, PinchGestureEventArgs args)
    {
      if (!this._isPinching)
        return;
      this.TransferTransforms();
      this._isPinching = false;
    }

    private void GestureListener_OnGestureCompleted(object sender, GestureEventArgs e)
    {
      if (!Locator.MainStatic.IsHoldStation)
        return;
      Locator.MainStatic.IsHoldStation = false;
      ((System.Windows.Controls.Control) this.PlainMap).Padding = new Thickness(0.0, 0.0, 0.0, 0.0);
      this.PlainMapScrollTransform.Y = 0.0;
      this.PlainMap.VerticalScrollBarVisibility = (ScrollBarVisibility) 1;
    }

    private void GestureListener_OnHold(object sender, GestureEventArgs e)
    {
      if (Locator.MainStatic.SearchIsActive)
        return;
      Locator.MainStatic.IsHoldStation = true;
      ((System.Windows.Controls.Control) this.PlainMap).Padding = new Thickness(0.0, 0.0, 0.0, -this.PlainMap.VerticalOffset);
      this.PlainMap.VerticalScrollBarVisibility = (ScrollBarVisibility) 0;
      this.PlainMapScrollTransform.Y = -this.PlainMap.VerticalOffset;
      if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is MetroStation dataContext))
        return;
      MainViewModel mainStatic = Locator.MainStatic;
      FrameworkElement element = originalSource;
      double? verticalChange = new double?();
      mainStatic.ChooseStation(dataContext, element, verticalChange);
    }

    private void TransferTransforms()
    {
      this._allowFocus = false;
      this.previousTransform.Matrix = this.Multiply(this.previousTransform.Matrix, this.currentTransform.Value);
      double dx = -this.translateTransform.X;
      double dy = -this.translateTransform.Y;
      bool adjustScale = false;
      bool adjustTranslate = false;
      double shouldBeScale = 0.0;
      double shouldBeTranslateX = 0.0;
      double shouldBeTranslateY = 0.0;
      double m11 = this.previousTransform.Matrix.M11;
      if (m11 > 3.2 && Math.Abs(m11 - 3.2) > 0.01)
      {
        adjustScale = true;
        shouldBeScale = 3.2;
      }
      if (!adjustScale && m11 < Constants.MinMapScale && Math.Abs(Constants.MinMapScale - m11) > 0.01)
      {
        adjustScale = true;
        shouldBeScale = Constants.MinMapScale;
      }
      Point point1 = ((GeneralTransform) this.previousTransform).Transform(new Point(0.0, 0.0));
      Point point2 = ((GeneralTransform) this.previousTransform).Transform(new Point(FastKeeper.Scheme.Width, FastKeeper.Scheme.Height));
      double actualWidth = point2.X - point1.X;
      double actualHeight = point2.Y - point1.Y;
      if (!adjustScale)
      {
        if (point1.X >= 0.0 && point1.X <= 480.0 && point1.Y >= 0.0 && point1.Y <= 622.0 && point2.X >= 0.0 && point2.X <= 480.0 && point2.Y >= 0.0)
        {
          double y = point2.Y;
        }
        if (actualHeight <= 622.0 && actualWidth <= 480.0)
        {
          if (point1.X < 0.0 && point1.X < -0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateX = -point1.X;
          }
          if (point1.Y < 0.0 && point1.Y < -0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateY = -point1.Y;
          }
          if (point2.X > 480.0 && point2.X - 480.0 > 0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateX = 480.0 - point2.X;
          }
          if (point2.Y > 622.0 && point2.Y - 622.0 > 0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateY = 622.0 - point2.Y;
          }
        }
        else
        {
          if (point1.X > Constants.ScreenLeftLimit && point1.X - Constants.ScreenLeftLimit > 0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateX = Constants.ScreenLeftLimit - point1.X;
          }
          if (point1.Y > Constants.ScreenTopLimit && point1.Y - Constants.ScreenTopLimit > 0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateY = Constants.ScreenTopLimit - point1.Y;
          }
          if (point2.X < Constants.ScreenRightLimit && Constants.ScreenRightLimit - point2.X > 0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateX = Constants.ScreenRightLimit - point2.X;
          }
          if (point2.Y < Constants.ScreenBottomLimit && Constants.ScreenBottomLimit - point2.Y > 0.01)
          {
            adjustTranslate = true;
            shouldBeTranslateY = Constants.ScreenBottomLimit - point2.Y;
          }
        }
      }
      bool flag = Math.Abs(actualHeight - 620.0) > 0.01;
      if (actualHeight < 620.0 && flag)
      {
        adjustTranslate = true;
        shouldBeTranslateY = -this.previousTransform.Matrix.OffsetY;
      }
      this.scaleTransform.ScaleX = this.scaleTransform.ScaleY = 1.0;
      this.scaleTransform.CenterX = this.scaleTransform.CenterY = 0.0;
      this.rotateTransform.Angle = 0.0;
      this.rotateTransform.CenterX = this.rotateTransform.CenterY = 0.0;
      this.translateTransform.X = this.translateTransform.Y = 0.0;
      this.RunRestorationAnimations(dx, dy, shouldBeScale, adjustTranslate, adjustScale, shouldBeTranslateX, shouldBeTranslateY, actualWidth, actualHeight);
    }

    private void RunRestorationAnimations(
      double dx,
      double dy,
      double shouldBeScale,
      bool adjustTranslate,
      bool adjustScale,
      double shouldBeTranslateX,
      double shouldBeTranslateY,
      double actualWidth,
      double actualHeight)
    {
      if (!this._allowRestorationAnimation || !adjustTranslate && !adjustScale)
        return;
      bool flag1 = Math.Abs(actualWidth - 470.0) > 0.01;
      bool flag2 = Math.Abs(actualHeight - 620.0) > 0.01;
      Storyboard storyboard = new Storyboard();
      if (adjustTranslate)
      {
        if (Math.Abs(shouldBeTranslateX) > 0.0 || actualWidth > 470.0 && flag1)
        {
          DoubleAnimation doubleAnimation1 = new DoubleAnimation();
          doubleAnimation1.To = new double?(shouldBeTranslateX);
          DoubleAnimation doubleAnimation2 = doubleAnimation1;
          CircleEase circleEase1 = new CircleEase();
          ((EasingFunctionBase) circleEase1).EasingMode = (EasingMode) 0;
          CircleEase circleEase2 = circleEase1;
          doubleAnimation2.EasingFunction = (IEasingFunction) circleEase2;
          ((Timeline) doubleAnimation1).Duration = Constants.AnimationDuration;
          DoubleAnimation doubleAnimation3 = doubleAnimation1;
          Storyboard.SetTarget((Timeline) doubleAnimation3, (DependencyObject) this.translateTransform);
          Storyboard.SetTargetProperty((Timeline) doubleAnimation3, new PropertyPath((object) TranslateTransform.XProperty));
          ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation3);
        }
        if (Math.Abs(shouldBeTranslateY) > 0.0 || actualHeight > 620.0 && flag2)
        {
          DoubleAnimation doubleAnimation4 = new DoubleAnimation();
          doubleAnimation4.To = new double?(shouldBeTranslateY);
          DoubleAnimation doubleAnimation5 = doubleAnimation4;
          CircleEase circleEase3 = new CircleEase();
          ((EasingFunctionBase) circleEase3).EasingMode = (EasingMode) 0;
          CircleEase circleEase4 = circleEase3;
          doubleAnimation5.EasingFunction = (IEasingFunction) circleEase4;
          ((Timeline) doubleAnimation4).Duration = Constants.AnimationDuration;
          DoubleAnimation doubleAnimation6 = doubleAnimation4;
          Storyboard.SetTarget((Timeline) doubleAnimation6, (DependencyObject) this.translateTransform);
          Storyboard.SetTargetProperty((Timeline) doubleAnimation6, new PropertyPath((object) TranslateTransform.YProperty));
          ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation6);
        }
      }
      if (adjustScale)
      {
        bool flag3 = actualWidth < 470.0 && flag1;
        bool flag4 = actualHeight < 620.0 && flag2;
        if (this._isLoaded && (flag3 || flag4))
        {
          this.previousTransform.Matrix = this._initialMatrix;
          return;
        }
        double m11 = this.previousTransform.Matrix.M11;
        Point point = ((GeneralTransform) this.previousTransform).Transform(MetroService.Instance.AppState.LastPinchCenterPoint);
        DoubleAnimation doubleAnimation7 = new DoubleAnimation();
        doubleAnimation7.To = new double?(shouldBeScale / m11);
        DoubleAnimation doubleAnimation8 = doubleAnimation7;
        CircleEase circleEase5 = new CircleEase();
        ((EasingFunctionBase) circleEase5).EasingMode = (EasingMode) 0;
        CircleEase circleEase6 = circleEase5;
        doubleAnimation8.EasingFunction = (IEasingFunction) circleEase6;
        Storyboard.SetTarget((Timeline) doubleAnimation7, (DependencyObject) this.scaleTransform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation7, new PropertyPath((object) ScaleTransform.ScaleXProperty));
        ((Timeline) doubleAnimation7).Duration = Constants.AnimationDuration;
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation7);
        DoubleAnimation doubleAnimation9 = new DoubleAnimation();
        doubleAnimation9.To = new double?(shouldBeScale / m11);
        DoubleAnimation doubleAnimation10 = doubleAnimation9;
        CircleEase circleEase7 = new CircleEase();
        ((EasingFunctionBase) circleEase7).EasingMode = (EasingMode) 0;
        CircleEase circleEase8 = circleEase7;
        doubleAnimation10.EasingFunction = (IEasingFunction) circleEase8;
        Storyboard.SetTarget((Timeline) doubleAnimation9, (DependencyObject) this.scaleTransform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation9, new PropertyPath((object) ScaleTransform.ScaleYProperty));
        ((Timeline) doubleAnimation9).Duration = Constants.AnimationDuration;
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation9);
        DoubleAnimation doubleAnimation11 = new DoubleAnimation();
        doubleAnimation11.To = new double?(point.X);
        DoubleAnimation doubleAnimation12 = doubleAnimation11;
        CircleEase circleEase9 = new CircleEase();
        ((EasingFunctionBase) circleEase9).EasingMode = (EasingMode) 0;
        CircleEase circleEase10 = circleEase9;
        doubleAnimation12.EasingFunction = (IEasingFunction) circleEase10;
        Storyboard.SetTarget((Timeline) doubleAnimation11, (DependencyObject) this.scaleTransform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation11, new PropertyPath((object) ScaleTransform.CenterXProperty));
        ((Timeline) doubleAnimation11).Duration = Constants.AnimationDuration;
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation11);
        DoubleAnimation doubleAnimation13 = new DoubleAnimation();
        doubleAnimation13.To = new double?(point.Y);
        DoubleAnimation doubleAnimation14 = doubleAnimation13;
        CircleEase circleEase11 = new CircleEase();
        ((EasingFunctionBase) circleEase11).EasingMode = (EasingMode) 0;
        CircleEase circleEase12 = circleEase11;
        doubleAnimation14.EasingFunction = (IEasingFunction) circleEase12;
        Storyboard.SetTarget((Timeline) doubleAnimation13, (DependencyObject) this.scaleTransform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation13, new PropertyPath((object) ScaleTransform.CenterYProperty));
        ((Timeline) doubleAnimation13).Duration = Constants.AnimationDuration;
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation13);
        DoubleAnimation doubleAnimation15 = new DoubleAnimation();
        doubleAnimation15.To = new double?(dx);
        DoubleAnimation doubleAnimation16 = doubleAnimation15;
        CircleEase circleEase13 = new CircleEase();
        ((EasingFunctionBase) circleEase13).EasingMode = (EasingMode) 0;
        CircleEase circleEase14 = circleEase13;
        doubleAnimation16.EasingFunction = (IEasingFunction) circleEase14;
        Storyboard.SetTarget((Timeline) doubleAnimation15, (DependencyObject) this.translateTransform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation15, new PropertyPath((object) TranslateTransform.XProperty));
        ((Timeline) doubleAnimation15).Duration = Constants.AnimationDuration;
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation15);
        DoubleAnimation doubleAnimation17 = new DoubleAnimation();
        doubleAnimation17.To = new double?(dy);
        DoubleAnimation doubleAnimation18 = doubleAnimation17;
        CircleEase circleEase15 = new CircleEase();
        ((EasingFunctionBase) circleEase15).EasingMode = (EasingMode) 0;
        CircleEase circleEase16 = circleEase15;
        doubleAnimation18.EasingFunction = (IEasingFunction) circleEase16;
        Storyboard.SetTarget((Timeline) doubleAnimation17, (DependencyObject) this.translateTransform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation17, new PropertyPath((object) TranslateTransform.YProperty));
        ((Timeline) doubleAnimation17).Duration = Constants.AnimationDuration;
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation17);
      }
      ((Timeline) storyboard).FillBehavior = (FillBehavior) 0;
      this._allowRestorationAnimation = false;
      storyboard.Begin();
      ((Timeline) storyboard).Completed += (EventHandler) ((o, e) =>
      {
        this.TransferTransforms();
        this._allowRestorationAnimation = true;
      });
    }

    private Matrix Multiply(Matrix A, Matrix B) => new Matrix(A.M11 * B.M11 + A.M12 * B.M21, A.M11 * B.M12 + A.M12 * B.M22, A.M21 * B.M11 + A.M22 * B.M21, A.M21 * B.M12 + A.M22 * B.M22, A.OffsetX * B.M11 + A.OffsetY * B.M21 + B.OffsetX, A.OffsetX * B.M12 + A.OffsetY * B.M22 + B.OffsetY);

    private void Bar_OnStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
    {
      ((DoubleAnimation) ((PresentationFrameworkCollection<Timeline>) this.LogoAnimation.Children)[0]).To = new double?(e.IsMenuVisible ? -8.0 : -80.0);
      this.LogoAnimation.Begin();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/MetroMapView.xaml", UriKind.Relative));
      this.SearchStationActivateAnimationFrom = (Storyboard) ((FrameworkElement) this).FindName("SearchStationActivateAnimationFrom");
      this.SearchStationActivateAnimationTo = (Storyboard) ((FrameworkElement) this).FindName("SearchStationActivateAnimationTo");
      this.SearchStationDeactivateAnimationFrom = (Storyboard) ((FrameworkElement) this).FindName("SearchStationDeactivateAnimationFrom");
      this.SearchStationDeactivateAnimationFromFast = (Storyboard) ((FrameworkElement) this).FindName("SearchStationDeactivateAnimationFromFast");
      this.SearchStationDeactivateAnimationToFast = (Storyboard) ((FrameworkElement) this).FindName("SearchStationDeactivateAnimationToFast");
      this.SearchStationDeactivateAnimationTo = (Storyboard) ((FrameworkElement) this).FindName("SearchStationDeactivateAnimationTo");
      this.MapTypeSwitchAnimation = (Storyboard) ((FrameworkElement) this).FindName("MapTypeSwitchAnimation");
      this.MapTypeSwitchBackAnimation = (Storyboard) ((FrameworkElement) this).FindName("MapTypeSwitchBackAnimation");
      this.SelectStationAnimation = (Storyboard) ((FrameworkElement) this).FindName("SelectStationAnimation");
      this.LogoAnimation = (Storyboard) ((FrameworkElement) this).FindName("LogoAnimation");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.SchemeMap = (Grid) ((FrameworkElement) this).FindName("SchemeMap");
      this.SchemeMapTransform = (TranslateTransform) ((FrameworkElement) this).FindName("SchemeMapTransform");
      this.MapRoot = (Canvas) ((FrameworkElement) this).FindName("MapRoot");
      this.cacheMode = (BitmapCache) ((FrameworkElement) this).FindName("cacheMode");
      this.previousTransform = (MatrixTransform) ((FrameworkElement) this).FindName("previousTransform");
      this.currentTransform = (TransformGroup) ((FrameworkElement) this).FindName("currentTransform");
      this.scaleTransform = (ScaleTransform) ((FrameworkElement) this).FindName("scaleTransform");
      this.rotateTransform = (RotateTransform) ((FrameworkElement) this).FindName("rotateTransform");
      this.translateTransform = (TranslateTransform) ((FrameworkElement) this).FindName("translateTransform");
      this.RouteRoot = (Canvas) ((FrameworkElement) this).FindName("RouteRoot");
      this.PlainMap = (ScrollViewer) ((FrameworkElement) this).FindName("PlainMap");
      this.PlainMapScrollTransform = (TranslateTransform) ((FrameworkElement) this).FindName("PlainMapScrollTransform");
      this.PlainMapTransform = (PlaneProjection) ((FrameworkElement) this).FindName("PlainMapTransform");
      this.PlainMapCanvas = (Canvas) ((FrameworkElement) this).FindName("PlainMapCanvas");
      this.revertButton = (TapButton) ((FrameworkElement) this).FindName("revertButton");
      this.GridRoute = (Grid) ((FrameworkElement) this).FindName("GridRoute");
      this.RouteTransform = (TranslateTransform) ((FrameworkElement) this).FindName("RouteTransform");
      this.StartStationGrid = (Grid) ((FrameworkElement) this).FindName("StartStationGrid");
      this.StartLine = (Line) ((FrameworkElement) this).FindName("StartLine");
      this.EndStationGrid = (Grid) ((FrameworkElement) this).FindName("EndStationGrid");
      this.EndLine = (Line) ((FrameworkElement) this).FindName("EndLine");
      this.pivot = (Pivot) ((FrameworkElement) this).FindName("pivot");
      this.animateStationCircle = (Ellipse) ((FrameworkElement) this).FindName("animateStationCircle");
      this.EllipseTransform = (TranslateTransform) ((FrameworkElement) this).FindName("EllipseTransform");
      this.logoImage = (Image) ((FrameworkElement) this).FindName("logoImage");
      this.LogoTransform = (TranslateTransform) ((FrameworkElement) this).FindName("LogoTransform");
      this.bar = (BindableApplicationBar) ((FrameworkElement) this).FindName("bar");
    }
  }
}
