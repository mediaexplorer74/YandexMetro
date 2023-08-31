// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ScaleRuleControl
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Yandex.DevUtils;
using Yandex.Ioc;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Media;
using Yandex.Patterns;
using Yandex.StringUtils.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class ScaleRuleControl : Control
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ScaleRuleControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register(nameof (ZoomLevel), typeof (double), typeof (ScaleRuleControl), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ScaleRuleControl.ZoomLevelChanged)));
    public static readonly DependencyProperty PlainViewPortProperty = DependencyProperty.Register(nameof (PlainViewPort), typeof (Rect), typeof (ScaleRuleControl), new PropertyMetadata((object) new Rect(), new PropertyChangedCallback(ScaleRuleControl.PlainViewPortChanged)));
    public static readonly DependencyProperty OperationStatusProperty = DependencyProperty.Register(nameof (OperationStatus), typeof (OperationStatus), typeof (ScaleRuleControl), new PropertyMetadata((object) OperationStatus.Idle, new PropertyChangedCallback(ScaleRuleControl.OnOperationStatusChanged)));
    private readonly IConfigMediator _configMediator;
    private readonly IDistanceFormatterUtil _distanceFormatterUtil;
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly IPrinterManager _printerManager;
    private readonly IUiDispatcher _uiDispatcher;
    private readonly IViewportPointConveter _viewportPointConveter;

    public ScaleRuleControl()
    {
      this.DefaultStyleKey = (object) typeof (ScaleRuleControl);
      if (DesignerProperties.IsInDesignMode)
        return;
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.ScaleRuleControlSizeChanged);
      this._geoPixelConverter = IocSingleton<ControlsIocInitializer>.Resolve<IGeoPixelConverter>();
      this._distanceFormatterUtil = IocSingleton<ControlsIocInitializer>.Resolve<IDistanceFormatterUtil>();
      this._viewportPointConveter = IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>();
      this._configMediator = IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>();
      this._uiDispatcher = IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>();
      this._printerManager = IocSingleton<ControlsIocInitializer>.Resolve<IPrinterManager>();
      this._printerManager.ExecuteWhenReady(new Action(this.UpdateVisibilityAndText));
    }

    public Rect PlainViewPort
    {
      get => (Rect) ((DependencyObject) this).GetValue(ScaleRuleControl.PlainViewPortProperty);
      set => ((DependencyObject) this).SetValue(ScaleRuleControl.PlainViewPortProperty, (object) value);
    }

    public double ZoomLevel
    {
      get => (double) ((DependencyObject) this).GetValue(ScaleRuleControl.ZoomLevelProperty);
      set => ((DependencyObject) this).SetValue(ScaleRuleControl.ZoomLevelProperty, (object) value);
    }

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(ScaleRuleControl.TextProperty);
      set => ((DependencyObject) this).SetValue(ScaleRuleControl.TextProperty, (object) value);
    }

    public OperationStatus OperationStatus
    {
      get => (OperationStatus) ((DependencyObject) this).GetValue(ScaleRuleControl.OperationStatusProperty);
      set => ((DependencyObject) this).SetValue(ScaleRuleControl.OperationStatusProperty, (object) value);
    }

    private static void ZoomLevelChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ((ScaleRuleControl) dependencyObject).UpdateVisibilityAndText();
    }

    private static void PlainViewPortChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ((ScaleRuleControl) dependencyObject).UpdateVisibilityAndText();
    }

    private static void OnOperationStatusChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ((ScaleRuleControl) dependencyObject).UpdateVisibilityAndText();
    }

    private void UpdateVisibilityAndText()
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        if (this.OperationStatus != OperationStatus.Idle)
          return;
        ObjectShowInterval lineShowInterval = this._configMediator.ScaleLineShowInterval;
        if (lineShowInterval == null)
          ((UIElement) this).Visibility = (Visibility) 1;
        else if (this.ZoomLevel >= (double) lineShowInterval.MinZoom && this.ZoomLevel <= (double) lineShowInterval.MaxZoom)
        {
          ((UIElement) this).Visibility = (Visibility) 0;
          this.UpdateText();
        }
        else
          ((UIElement) this).Visibility = (Visibility) 1;
      }));
    }

    private void ScaleRuleControlSizeChanged(object sender, SizeChangedEventArgs e) => this.UpdateText();

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      if (DesignerProperties.IsInDesignMode)
        return;
      this.UpdateText();
    }

    private void UpdateText()
    {
      Point relativePoint = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(((FrameworkElement) this).ActualWidth, 0.0), this.ZoomLevel)) with
      {
        Y = this.PlainViewPort.Top + this.PlainViewPort.Height / 2.0
      };
      this.Text = this._distanceFormatterUtil.GetDistanceString(this._geoPixelConverter.RelativeDistanceToMeters(this._geoPixelConverter.RelativePointToCoordinates(relativePoint).Latitude, relativePoint.X), false);
    }
  }
}
