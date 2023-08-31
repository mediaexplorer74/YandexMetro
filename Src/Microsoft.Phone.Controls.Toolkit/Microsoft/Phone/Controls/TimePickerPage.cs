// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TimePickerPage
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Phone.Controls
{
  public class TimePickerPage : DateTimePickerPageBase
  {
    internal VisualStateGroup VisibilityStates;
    internal VisualState Open;
    internal VisualState Closed;
    internal PlaneProjection PlaneProjection;
    internal Rectangle SystemTrayPlaceholder;
    internal LoopingSelector PrimarySelector;
    internal LoopingSelector SecondarySelector;
    internal LoopingSelector TertiarySelector;
    private bool _contentLoaded;

    public TimePickerPage()
    {
      this.InitializeComponent();
      this.PrimarySelector.DataSource = DateTimeWrapper.CurrentCultureUsesTwentyFourHourClock() ? (ILoopingSelectorDataSource) new TwentyFourHourDataSource() : (ILoopingSelectorDataSource) new TwelveHourDataSource();
      this.SecondarySelector.DataSource = (ILoopingSelectorDataSource) new MinuteDataSource();
      this.TertiarySelector.DataSource = (ILoopingSelectorDataSource) new AmPmDataSource();
      this.InitializeDateTimePickerPage(this.PrimarySelector, this.SecondarySelector, this.TertiarySelector);
    }

    protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern() => DateTimePickerPageBase.GetSelectorsOrderedByCulturePattern(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.ToUpperInvariant(), new char[3]
    {
      'H',
      'M',
      'T'
    }, new LoopingSelector[3]
    {
      this.PrimarySelector,
      this.SecondarySelector,
      this.TertiarySelector
    });

    protected virtual void OnOrientationChanged(OrientationChangedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      base.OnOrientationChanged(e);
      ((UIElement) this.SystemTrayPlaceholder).Visibility = (1 & e.Orientation) != null ? (Visibility) 0 : (Visibility) 1;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft.Phone.Controls.Toolkit;component/DateTimePickers/TimePickerPage.xaml", UriKind.Relative));
      this.VisibilityStates = (VisualStateGroup) ((FrameworkElement) this).FindName("VisibilityStates");
      this.Open = (VisualState) ((FrameworkElement) this).FindName("Open");
      this.Closed = (VisualState) ((FrameworkElement) this).FindName("Closed");
      this.PlaneProjection = (PlaneProjection) ((FrameworkElement) this).FindName("PlaneProjection");
      this.SystemTrayPlaceholder = (Rectangle) ((FrameworkElement) this).FindName("SystemTrayPlaceholder");
      this.PrimarySelector = (LoopingSelector) ((FrameworkElement) this).FindName("PrimarySelector");
      this.SecondarySelector = (LoopingSelector) ((FrameworkElement) this).FindName("SecondarySelector");
      this.TertiarySelector = (LoopingSelector) ((FrameworkElement) this).FindName("TertiarySelector");
    }
  }
}
