// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.DateTimePickerBase
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
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls
{
  [TemplatePart(Name = "DateTimeButton", Type = typeof (ButtonBase))]
  public class DateTimePickerBase : Control
  {
    private const string ButtonPartName = "DateTimeButton";
    private ButtonBase _dateButtonPart;
    private PhoneApplicationFrame _frame;
    private object _frameContentWhenOpened;
    private NavigationInTransition _savedNavigationInTransition;
    private NavigationOutTransition _savedNavigationOutTransition;
    private IDateTimePickerPage _dateTimePickerPage;
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (DateTime?), typeof (DateTimePickerBase), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimePickerBase.OnValueChanged)));
    public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register(nameof (ValueString), typeof (string), typeof (DateTimePickerBase), (PropertyMetadata) null);
    public static readonly DependencyProperty ValueStringFormatProperty = DependencyProperty.Register(nameof (ValueStringFormat), typeof (string), typeof (DateTimePickerBase), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimePickerBase.OnValueStringFormatChanged)));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (DateTimePickerBase), (PropertyMetadata) null);
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (DateTimePickerBase), (PropertyMetadata) null);
    public static readonly DependencyProperty PickerPageUriProperty = DependencyProperty.Register(nameof (PickerPageUri), typeof (Uri), typeof (DateTimePickerBase), (PropertyMetadata) null);

    public event EventHandler<DateTimeValueChangedEventArgs> ValueChanged;

    [TypeConverter(typeof (TimeTypeConverter))]
    public DateTime? Value
    {
      get => (DateTime?) ((DependencyObject) this).GetValue(DateTimePickerBase.ValueProperty);
      set => ((DependencyObject) this).SetValue(DateTimePickerBase.ValueProperty, (object) value);
    }

    private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((DateTimePickerBase) o).OnValueChanged((DateTime?) e.OldValue, (DateTime?) e.NewValue);

    private void OnValueChanged(DateTime? oldValue, DateTime? newValue)
    {
      this.UpdateValueString();
      this.OnValueChanged(new DateTimeValueChangedEventArgs(oldValue, newValue));
    }

    protected virtual void OnValueChanged(DateTimeValueChangedEventArgs e)
    {
      EventHandler<DateTimeValueChangedEventArgs> valueChanged = this.ValueChanged;
      if (valueChanged == null)
        return;
      valueChanged((object) this, e);
    }

    public string ValueString
    {
      get => (string) ((DependencyObject) this).GetValue(DateTimePickerBase.ValueStringProperty);
      private set => ((DependencyObject) this).SetValue(DateTimePickerBase.ValueStringProperty, (object) value);
    }

    public string ValueStringFormat
    {
      get => (string) ((DependencyObject) this).GetValue(DateTimePickerBase.ValueStringFormatProperty);
      set => ((DependencyObject) this).SetValue(DateTimePickerBase.ValueStringFormatProperty, (object) value);
    }

    private static void OnValueStringFormatChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((DateTimePickerBase) o).OnValueStringFormatChanged();
    }

    private void OnValueStringFormatChanged() => this.UpdateValueString();

    public object Header
    {
      get => ((DependencyObject) this).GetValue(DateTimePickerBase.HeaderProperty);
      set => ((DependencyObject) this).SetValue(DateTimePickerBase.HeaderProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(DateTimePickerBase.HeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(DateTimePickerBase.HeaderTemplateProperty, (object) value);
    }

    public Uri PickerPageUri
    {
      get => (Uri) ((DependencyObject) this).GetValue(DateTimePickerBase.PickerPageUriProperty);
      set => ((DependencyObject) this).SetValue(DateTimePickerBase.PickerPageUriProperty, (object) value);
    }

    protected virtual string ValueStringFormatFallback => "{0}";

    public virtual void OnApplyTemplate()
    {
      if (this._dateButtonPart != null)
        this._dateButtonPart.Click -= new RoutedEventHandler(this.OnDateButtonClick);
      ((FrameworkElement) this).OnApplyTemplate();
      this._dateButtonPart = this.GetTemplateChild("DateTimeButton") as ButtonBase;
      if (this._dateButtonPart == null)
        return;
      this._dateButtonPart.Click += new RoutedEventHandler(this.OnDateButtonClick);
    }

    private void OnDateButtonClick(object sender, RoutedEventArgs e) => this.OpenPickerPage();

    private void UpdateValueString() => this.ValueString = string.Format((IFormatProvider) CultureInfo.CurrentCulture, this.ValueStringFormat ?? this.ValueStringFormatFallback, new object[1]
    {
      (object) this.Value
    });

    private void OpenPickerPage()
    {
      if ((Uri) null == this.PickerPageUri)
        throw new ArgumentException("PickerPageUri property must not be null.");
      if (this._frame != null)
        return;
      this._frame = Application.Current.RootVisual as PhoneApplicationFrame;
      if (this._frame == null)
        return;
      this._frameContentWhenOpened = ((ContentControl) this._frame).Content;
      if (this._frameContentWhenOpened is UIElement contentWhenOpened)
      {
        this._savedNavigationInTransition = TransitionService.GetNavigationInTransition(contentWhenOpened);
        TransitionService.SetNavigationInTransition(contentWhenOpened, (NavigationInTransition) null);
        this._savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(contentWhenOpened);
        TransitionService.SetNavigationOutTransition(contentWhenOpened, (NavigationOutTransition) null);
      }
      ((Frame) this._frame).Navigated += new NavigatedEventHandler(this.OnFrameNavigated);
      ((Frame) this._frame).NavigationStopped += new NavigationStoppedEventHandler(this.OnFrameNavigationStoppedOrFailed);
      ((Frame) this._frame).NavigationFailed += new NavigationFailedEventHandler(this.OnFrameNavigationStoppedOrFailed);
      ((Frame) this._frame).Navigate(this.PickerPageUri);
    }

    private void ClosePickerPage()
    {
      if (this._frame != null)
      {
        ((Frame) this._frame).Navigated -= new NavigatedEventHandler(this.OnFrameNavigated);
        ((Frame) this._frame).NavigationStopped -= new NavigationStoppedEventHandler(this.OnFrameNavigationStoppedOrFailed);
        ((Frame) this._frame).NavigationFailed -= new NavigationFailedEventHandler(this.OnFrameNavigationStoppedOrFailed);
        if (this._frameContentWhenOpened is UIElement contentWhenOpened)
        {
          TransitionService.SetNavigationInTransition(contentWhenOpened, this._savedNavigationInTransition);
          this._savedNavigationInTransition = (NavigationInTransition) null;
          TransitionService.SetNavigationOutTransition(contentWhenOpened, this._savedNavigationOutTransition);
          this._savedNavigationOutTransition = (NavigationOutTransition) null;
        }
        this._frame = (PhoneApplicationFrame) null;
        this._frameContentWhenOpened = (object) null;
      }
      if (this._dateTimePickerPage == null)
        return;
      if (this._dateTimePickerPage.Value.HasValue)
        this.Value = new DateTime?(this._dateTimePickerPage.Value.Value);
      this._dateTimePickerPage = (IDateTimePickerPage) null;
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
      if (e.Content == this._frameContentWhenOpened)
      {
        this.ClosePickerPage();
      }
      else
      {
        if (this._dateTimePickerPage != null)
          return;
        this._dateTimePickerPage = e.Content as IDateTimePickerPage;
        if (this._dateTimePickerPage == null)
          return;
        this._dateTimePickerPage.Value = new DateTime?(this.Value.GetValueOrDefault(DateTime.Now));
      }
    }

    private void OnFrameNavigationStoppedOrFailed(object sender, EventArgs e) => this.ClosePickerPage();
  }
}
