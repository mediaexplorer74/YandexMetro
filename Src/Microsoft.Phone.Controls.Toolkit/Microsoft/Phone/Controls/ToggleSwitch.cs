// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ToggleSwitch
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  [TemplatePart(Name = "Switch", Type = typeof (ToggleSwitchButton))]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  public class ToggleSwitch : ContentControl
  {
    private const string CommonStates = "CommonStates";
    private const string NormalState = "Normal";
    private const string DisabledState = "Disabled";
    private const string SwitchPart = "Switch";
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SwitchForegroundProperty = DependencyProperty.Register(nameof (SwitchForeground), typeof (Brush), typeof (ToggleSwitch), (PropertyMetadata) null);
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool?), typeof (ToggleSwitch), new PropertyMetadata((object) false, new PropertyChangedCallback(ToggleSwitch.OnIsCheckedChanged)));
    private ToggleSwitchButton _toggleButton;
    private bool _wasContentSet;

    public object Header
    {
      get => ((DependencyObject) this).GetValue(ToggleSwitch.HeaderProperty);
      set => ((DependencyObject) this).SetValue(ToggleSwitch.HeaderProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ToggleSwitch.HeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(ToggleSwitch.HeaderTemplateProperty, (object) value);
    }

    public Brush SwitchForeground
    {
      get => (Brush) ((DependencyObject) this).GetValue(ToggleSwitch.SwitchForegroundProperty);
      set => ((DependencyObject) this).SetValue(ToggleSwitch.SwitchForegroundProperty, (object) value);
    }

    [TypeConverter(typeof (NullableBoolConverter))]
    public bool? IsChecked
    {
      get => (bool?) ((DependencyObject) this).GetValue(ToggleSwitch.IsCheckedProperty);
      set => ((DependencyObject) this).SetValue(ToggleSwitch.IsCheckedProperty, (object) value);
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToggleSwitch toggleSwitch = (ToggleSwitch) d;
      if (toggleSwitch._toggleButton == null)
        return;
      toggleSwitch._toggleButton.IsChecked = (bool?) e.NewValue;
    }

    public event EventHandler<RoutedEventArgs> Checked;

    public event EventHandler<RoutedEventArgs> Unchecked;

    public event EventHandler<RoutedEventArgs> Indeterminate;

    public event EventHandler<RoutedEventArgs> Click;

    public ToggleSwitch() => ((Control) this).DefaultStyleKey = (object) typeof (ToggleSwitch);

    private void SetDefaultContent()
    {
      Binding binding = new Binding("IsChecked")
      {
        Source = (object) this,
        Converter = (IValueConverter) new OffOnConverter()
      };
      ((FrameworkElement) this).SetBinding(ContentControl.ContentProperty, binding);
    }

    private void ChangeVisualState(bool useTransitions)
    {
      if (((Control) this).IsEnabled)
        VisualStateManager.GoToState((Control) this, "Normal", useTransitions);
      else
        VisualStateManager.GoToState((Control) this, "Disabled", useTransitions);
    }

    protected virtual void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      this._wasContentSet = true;
      if (!DesignerProperties.IsInDesignTool || newContent != null || ((FrameworkElement) this).GetBindingExpression(ContentControl.ContentProperty) != null)
        return;
      this.SetDefaultContent();
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      if (!this._wasContentSet && ((FrameworkElement) this).GetBindingExpression(ContentControl.ContentProperty) == null)
        this.SetDefaultContent();
      if (this._toggleButton != null)
      {
        this._toggleButton.Checked -= new RoutedEventHandler(this.OnChecked);
        this._toggleButton.Unchecked -= new RoutedEventHandler(this.OnUnchecked);
        this._toggleButton.Indeterminate -= new RoutedEventHandler(this.OnIndeterminate);
        ((ButtonBase) this._toggleButton).Click -= new RoutedEventHandler(this.OnClick);
      }
      this._toggleButton = ((Control) this).GetTemplateChild("Switch") as ToggleSwitchButton;
      if (this._toggleButton != null)
      {
        this._toggleButton.Checked += new RoutedEventHandler(this.OnChecked);
        this._toggleButton.Unchecked += new RoutedEventHandler(this.OnUnchecked);
        this._toggleButton.Indeterminate += new RoutedEventHandler(this.OnIndeterminate);
        ((ButtonBase) this._toggleButton).Click += new RoutedEventHandler(this.OnClick);
        this._toggleButton.IsChecked = this.IsChecked;
      }
      ((Control) this).IsEnabledChanged += (DependencyPropertyChangedEventHandler) delegate
      {
        this.ChangeVisualState(true);
      };
      this.ChangeVisualState(false);
    }

    private void OnChecked(object sender, RoutedEventArgs e)
    {
      this.IsChecked = new bool?(true);
      SafeRaise.Raise<RoutedEventArgs>(this.Checked, (object) this, e);
    }

    private void OnUnchecked(object sender, RoutedEventArgs e)
    {
      this.IsChecked = new bool?(false);
      SafeRaise.Raise<RoutedEventArgs>(this.Unchecked, (object) this, e);
    }

    private void OnIndeterminate(object sender, RoutedEventArgs e)
    {
      this.IsChecked = new bool?();
      SafeRaise.Raise<RoutedEventArgs>(this.Indeterminate, (object) this, e);
    }

    private void OnClick(object sender, RoutedEventArgs e) => SafeRaise.Raise<RoutedEventArgs>(this.Click, (object) this, e);

    public virtual string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{{ToggleSwitch IsChecked={0}, Content={1}}}", new object[2]
    {
      (object) this.IsChecked,
      this.Content
    });
  }
}
