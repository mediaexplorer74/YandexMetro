// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PhoneTextBox
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(Name = "LengthIndicatorVisible", GroupName = "LengthIndicatorStates")]
  [TemplatePart(Name = "LengthIndicator", Type = typeof (TextBlock))]
  [TemplateVisualState(Name = "LengthIndicatorHidden", GroupName = "LengthIndicatorStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
  [TemplatePart(Name = "Text", Type = typeof (TextBox))]
  [TemplatePart(Name = "HintContent", Type = typeof (ContentControl))]
  public class PhoneTextBox : TextBox
  {
    private const string RootGridName = "RootGrid";
    private const string TextBoxName = "Text";
    private const string HintContentName = "HintContent";
    private const string HintBorderName = "HintBorder";
    private const string LengthIndicatorName = "LengthIndicator";
    private const string ActionIconCanvasName = "ActionIconCanvas";
    private const string MeasurementTextBlockName = "MeasurementTextBlock";
    private const string ActionIconBorderName = "ActionIconBorder";
    private const string LengthIndicatorStates = "LengthIndicatorStates";
    private const string LengthIndicatorVisibleState = "LengthIndicatorVisible";
    private const string LengthIndicatorHiddenState = "LengthIndicatorHidden";
    private const string CommonStates = "CommonStates";
    private const string DisabledState = "Disabled";
    private const string ReadOnlyState = "ReadOnly";
    private const string FocusStates = "FocusStates";
    private const string FocusedState = "Focused";
    private const string UnfocusedState = "Unfocused";
    private Grid RootGrid;
    private TextBox TextBox;
    private TextBlock MeasurementTextBlock;
    private Brush ForegroundBrushInactive = (Brush) Application.Current.Resources[(object) "PhoneTextBoxReadOnlyBrush"];
    private Brush ForegroundBrushEdit;
    private ContentControl HintContent;
    private Border HintBorder;
    private TextBlock LengthIndicator;
    private Border ActionIconBorder;
    private bool _ignorePropertyChange;
    private bool _ignoreFocus;
    public static readonly DependencyProperty HintProperty = DependencyProperty.Register(nameof (Hint), typeof (string), typeof (PhoneTextBox), new PropertyMetadata(new PropertyChangedCallback(PhoneTextBox.OnHintPropertyChanged)));
    public static readonly DependencyProperty HintStyleProperty = DependencyProperty.Register(nameof (HintStyle), typeof (Style), typeof (PhoneTextBox), (PropertyMetadata) null);
    public static readonly DependencyProperty ActualHintVisibilityProperty = DependencyProperty.Register(nameof (ActualHintVisibility), typeof (Visibility), typeof (PhoneTextBox), (PropertyMetadata) null);
    public static readonly DependencyProperty LengthIndicatorVisibleProperty = DependencyProperty.Register(nameof (LengthIndicatorVisible), typeof (bool), typeof (PhoneTextBox), (PropertyMetadata) null);
    public static readonly DependencyProperty LengthIndicatorThresholdProperty = DependencyProperty.Register(nameof (LengthIndicatorThreshold), typeof (int), typeof (PhoneTextBox), new PropertyMetadata(new PropertyChangedCallback(PhoneTextBox.OnLengthIndicatorThresholdChanged)));
    public static readonly DependencyProperty DisplayedMaxLengthProperty = DependencyProperty.Register(nameof (DisplayedMaxLength), typeof (int), typeof (PhoneTextBox), new PropertyMetadata(new PropertyChangedCallback(PhoneTextBox.DisplayedMaxLengthChanged)));
    public static readonly DependencyProperty ActionIconProperty = DependencyProperty.Register(nameof (ActionIcon), typeof (ImageSource), typeof (PhoneTextBox), new PropertyMetadata(new PropertyChangedCallback(PhoneTextBox.OnActionIconChanged)));
    public static readonly DependencyProperty HidesActionItemWhenEmptyProperty = DependencyProperty.Register(nameof (HidesActionItemWhenEmpty), typeof (bool), typeof (PhoneTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(PhoneTextBox.OnActionIconChanged)));

    public string Hint
    {
      get => ((DependencyObject) this).GetValue(PhoneTextBox.HintProperty) as string;
      set => ((DependencyObject) this).SetValue(PhoneTextBox.HintProperty, (object) value);
    }

    public Style HintStyle
    {
      get => ((DependencyObject) this).GetValue(PhoneTextBox.HintStyleProperty) as Style;
      set => ((DependencyObject) this).SetValue(PhoneTextBox.HintStyleProperty, (object) value);
    }

    public Visibility ActualHintVisibility
    {
      get => (Visibility) ((DependencyObject) this).GetValue(PhoneTextBox.ActualHintVisibilityProperty);
      set => ((DependencyObject) this).SetValue(PhoneTextBox.ActualHintVisibilityProperty, (object) value);
    }

    private static void OnHintPropertyChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs args)
    {
      if (!(sender is PhoneTextBox phoneTextBox) || phoneTextBox.HintContent == null)
        return;
      phoneTextBox.UpdateHintVisibility();
    }

    private void UpdateHintVisibility()
    {
      if (this.HintContent == null)
        return;
      if (string.IsNullOrEmpty(this.Text))
      {
        this.ActualHintVisibility = (Visibility) 0;
        ((Control) this).Foreground = this.ForegroundBrushInactive;
      }
      else
        this.ActualHintVisibility = (Visibility) 1;
    }

    protected virtual void OnLostFocus(RoutedEventArgs e)
    {
      this.UpdateHintVisibility();
      base.OnLostFocus(e);
    }

    protected virtual void OnGotFocus(RoutedEventArgs e)
    {
      if (this._ignoreFocus)
      {
        this._ignoreFocus = false;
        ((Control) (Application.Current.RootVisual as Frame)).Focus();
      }
      else
      {
        ((Control) this).Foreground = this.ForegroundBrushEdit;
        if (this.HintContent != null)
          this.ActualHintVisibility = (Visibility) 1;
        base.OnGotFocus(e);
      }
    }

    public bool LengthIndicatorVisible
    {
      get => (bool) ((DependencyObject) this).GetValue(PhoneTextBox.LengthIndicatorVisibleProperty);
      set => ((DependencyObject) this).SetValue(PhoneTextBox.LengthIndicatorVisibleProperty, (object) value);
    }

    public int LengthIndicatorThreshold
    {
      get => (int) ((DependencyObject) this).GetValue(PhoneTextBox.LengthIndicatorThresholdProperty);
      set => ((DependencyObject) this).SetValue(PhoneTextBox.LengthIndicatorThresholdProperty, (object) value);
    }

    private static void OnLengthIndicatorThresholdChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs args)
    {
      PhoneTextBox phoneTextBox = sender as PhoneTextBox;
      if (phoneTextBox._ignorePropertyChange)
        phoneTextBox._ignorePropertyChange = false;
      else if (phoneTextBox.LengthIndicatorThreshold < 0)
      {
        phoneTextBox._ignorePropertyChange = true;
        ((DependencyObject) phoneTextBox).SetValue(PhoneTextBox.LengthIndicatorThresholdProperty, args.OldValue);
        throw new ArgumentOutOfRangeException("LengthIndicatorThreshold", "The length indicator visibility threshold must be greater than zero.");
      }
    }

    public int DisplayedMaxLength
    {
      get => (int) ((DependencyObject) this).GetValue(PhoneTextBox.DisplayedMaxLengthProperty);
      set => ((DependencyObject) this).SetValue(PhoneTextBox.DisplayedMaxLengthProperty, (object) value);
    }

    private static void DisplayedMaxLengthChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs args)
    {
      PhoneTextBox phoneTextBox = sender as PhoneTextBox;
      if (phoneTextBox.DisplayedMaxLength > phoneTextBox.MaxLength && phoneTextBox.MaxLength > 0)
        throw new ArgumentOutOfRangeException("DisplayedMaxLength", "The displayed maximum length cannot be greater than the MaxLength.");
    }

    private void UpdateLengthIndicatorVisibility()
    {
      if (this.RootGrid == null || this.LengthIndicator == null)
        return;
      bool flag = true;
      if (this.LengthIndicatorVisible)
      {
        this.LengthIndicator.Text = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}", new object[2]
        {
          (object) this.Text.Length,
          (object) (this.DisplayedMaxLength > 0 ? this.DisplayedMaxLength : this.MaxLength)
        });
        if (this.Text.Length >= this.LengthIndicatorThreshold)
          flag = false;
      }
      VisualStateManager.GoToState((Control) this, flag ? "LengthIndicatorHidden" : "LengthIndicatorVisible", false);
    }

    public ImageSource ActionIcon
    {
      get => ((DependencyObject) this).GetValue(PhoneTextBox.ActionIconProperty) as ImageSource;
      set => ((DependencyObject) this).SetValue(PhoneTextBox.ActionIconProperty, (object) value);
    }

    public bool HidesActionItemWhenEmpty
    {
      get => (bool) ((DependencyObject) this).GetValue(PhoneTextBox.HidesActionItemWhenEmptyProperty);
      set => ((DependencyObject) this).SetValue(PhoneTextBox.HidesActionItemWhenEmptyProperty, (object) value);
    }

    public event EventHandler ActionIconTapped;

    private static void OnActionIconChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs args)
    {
      if (!(sender is PhoneTextBox phoneTextBox))
        return;
      phoneTextBox.UpdateActionIconVisibility();
    }

    private void UpdateActionIconVisibility()
    {
      if (this.ActionIconBorder == null)
        return;
      if (this.ActionIcon == null || this.HidesActionItemWhenEmpty && string.IsNullOrEmpty(this.Text))
      {
        ((UIElement) this.ActionIconBorder).Visibility = (Visibility) 1;
        this.HintBorder.Padding = new Thickness(0.0);
      }
      else
      {
        ((UIElement) this.ActionIconBorder).Visibility = (Visibility) 0;
        if (this.TextWrapping == 2)
          return;
        this.HintBorder.Padding = new Thickness(0.0, 0.0, 48.0, 0.0);
      }
    }

    private void OnActionIconTapped(object sender, RoutedEventArgs e)
    {
      this._ignoreFocus = true;
      EventHandler actionIconTapped = this.ActionIconTapped;
      if (actionIconTapped == null)
        return;
      actionIconTapped(sender, (EventArgs) e);
    }

    private void ResizeTextBox()
    {
      if (this.ActionIcon == null || this.TextWrapping != 2)
        return;
      ((FrameworkElement) this.MeasurementTextBlock).Width = ((FrameworkElement) this).ActualWidth;
      if (((FrameworkElement) this.MeasurementTextBlock).ActualHeight > ((FrameworkElement) this).ActualHeight - 72.0)
      {
        ((FrameworkElement) this).Height = ((FrameworkElement) this).ActualHeight + 72.0;
      }
      else
      {
        if (((FrameworkElement) this).ActualHeight <= ((FrameworkElement) this.MeasurementTextBlock).ActualHeight + 144.0)
          return;
        ((FrameworkElement) this).Height = ((FrameworkElement) this).ActualHeight - 72.0;
      }
    }

    public PhoneTextBox() => ((Control) this).DefaultStyleKey = (object) typeof (PhoneTextBox);

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      if (this.TextBox != null)
        this.TextBox.TextChanged -= new TextChangedEventHandler(this.OnTextChanged);
      if (this.ActionIconBorder != null)
        ((UIElement) this.ActionIconBorder).MouseLeftButtonDown -= new MouseButtonEventHandler(this.OnActionIconTapped);
      this.RootGrid = ((Control) this).GetTemplateChild("RootGrid") as Grid;
      this.TextBox = ((Control) this).GetTemplateChild("Text") as TextBox;
      this.ForegroundBrushEdit = ((Control) this).Foreground;
      this.HintContent = ((Control) this).GetTemplateChild("HintContent") as ContentControl;
      this.HintBorder = ((Control) this).GetTemplateChild("HintBorder") as Border;
      if (this.HintContent != null)
        this.UpdateHintVisibility();
      this.LengthIndicator = ((Control) this).GetTemplateChild("LengthIndicator") as TextBlock;
      this.ActionIconBorder = ((Control) this).GetTemplateChild("ActionIconBorder") as Border;
      if (this.RootGrid != null && this.LengthIndicator != null)
        this.UpdateLengthIndicatorVisibility();
      if (this.TextBox != null)
        this.TextBox.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
      if (this.ActionIconBorder != null)
      {
        ((UIElement) this.ActionIconBorder).MouseLeftButtonDown += new MouseButtonEventHandler(this.OnActionIconTapped);
        this.UpdateActionIconVisibility();
      }
      this.MeasurementTextBlock = ((Control) this).GetTemplateChild("MeasurementTextBlock") as TextBlock;
    }

    private void OnTextChanged(object sender, RoutedEventArgs e)
    {
      this.UpdateLengthIndicatorVisibility();
      this.UpdateActionIconVisibility();
      this.UpdateHintVisibility();
      this.ResizeTextBox();
    }
  }
}
