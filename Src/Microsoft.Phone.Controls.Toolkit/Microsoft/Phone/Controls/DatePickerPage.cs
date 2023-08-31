// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.DatePickerPage
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
  public class DatePickerPage : DateTimePickerPageBase
  {
    internal VisualStateGroup VisibilityStates;
    internal VisualState Open;
    internal VisualState Closed;
    internal PlaneProjection PlaneProjection;
    internal Rectangle SystemTrayPlaceholder;
    internal LoopingSelector SecondarySelector;
    internal LoopingSelector TertiarySelector;
    internal LoopingSelector PrimarySelector;
    private bool _contentLoaded;

    public DatePickerPage()
    {
      this.InitializeComponent();
      this.PrimarySelector.DataSource = (ILoopingSelectorDataSource) new YearDataSource();
      this.SecondarySelector.DataSource = (ILoopingSelectorDataSource) new MonthDataSource();
      this.TertiarySelector.DataSource = (ILoopingSelectorDataSource) new DayDataSource();
      this.InitializeDateTimePickerPage(this.PrimarySelector, this.SecondarySelector, this.TertiarySelector);
    }

    protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern() => DateTimePickerPageBase.GetSelectorsOrderedByCulturePattern(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpperInvariant(), new char[3]
    {
      'Y',
      'M',
      'D'
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
      Application.LoadComponent((object) this, new Uri("/Microsoft.Phone.Controls.Toolkit;component/DateTimePickers/DatePickerPage.xaml", UriKind.Relative));
      this.VisibilityStates = (VisualStateGroup) ((FrameworkElement) this).FindName("VisibilityStates");
      this.Open = (VisualState) ((FrameworkElement) this).FindName("Open");
      this.Closed = (VisualState) ((FrameworkElement) this).FindName("Closed");
      this.PlaneProjection = (PlaneProjection) ((FrameworkElement) this).FindName("PlaneProjection");
      this.SystemTrayPlaceholder = (Rectangle) ((FrameworkElement) this).FindName("SystemTrayPlaceholder");
      this.SecondarySelector = (LoopingSelector) ((FrameworkElement) this).FindName("SecondarySelector");
      this.TertiarySelector = (LoopingSelector) ((FrameworkElement) this).FindName("TertiarySelector");
      this.PrimarySelector = (LoopingSelector) ((FrameworkElement) this).FindName("PrimarySelector");
    }
  }
}
