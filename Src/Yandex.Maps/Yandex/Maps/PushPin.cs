// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PushPin
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using DanielVaughan.WindowsPhone7Unleashed;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Yandex.Maps
{
  [TemplatePart(Name = "contentControl", Type = typeof (ContentPresenter))]
  [TemplatePart(Name = "pushpinPanel", Type = typeof (UIElement))]
  [ComVisible(false)]
  public class PushPin : ContentControl
  {
    private const string BaloonVisisblestateName = "BaloonVisible";
    private const string BaloonCollapsedStateName = "BaloonCollapsed";
    private const string CollapsedStateName = "Collapsed";
    private const string ExpandedStateName = "Expanded";
    internal const string StatePropertyName = "State";
    internal const string ContentControlName = "contentControl";
    private const string RootElementName = "pushpinPanel";
    public static readonly DependencyProperty ExpandedImageSourceProperty = DependencyProperty.Register(nameof (ExpandedImageSource), typeof (BitmapSource), typeof (PushPin), (PropertyMetadata) null);
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof (State), typeof (PushPinState), typeof (PushPin), new PropertyMetadata((object) PushPinState.Expanded, new PropertyChangedCallback(PushPin.StateChangedCallback)));
    public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(nameof (ContentVisibility), typeof (Visibility), typeof (PushPin), new PropertyMetadata((object) (Visibility) 1, new PropertyChangedCallback(PushPin.ContentVisibilityChangedCallback)));
    private UIElement _rootElement;
    private bool _isHidden;
    private ContentPresenter _contentControl;

    public BitmapSource ExpandedImageSource
    {
      get => (BitmapSource) ((DependencyObject) this).GetValue(PushPin.ExpandedImageSourceProperty);
      set => ((DependencyObject) this).SetValue(PushPin.ExpandedImageSourceProperty, (object) value);
    }

    internal event EventHandler VisibilityChanged;

    protected virtual void OnVisibilityChanged()
    {
      EventHandler visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((object) this, EventArgs.Empty);
    }

    public PushPin()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (PushPin);
      this.Content = (object) new Line();
      DependencyPropertyListener propertyListener = new DependencyPropertyListener();
      propertyListener.Changed += (EventHandler<BindingChangedEventArgs>) ((sender, e) => this.OnVisibilityChanged());
      propertyListener.Attach((FrameworkElement) this, new Binding()
      {
        Path = new PropertyPath("Visibility", new object[0]),
        Source = (object) this
      });
    }

    public virtual void OnApplyTemplate()
    {
      this.SetState(this.State);
      this.SetContentVisibility(this.ContentVisibility);
      this._rootElement = ((Control) this).GetTemplateChild("pushpinPanel") as UIElement;
      this.UpdateRootElementOpacity();
      this._contentControl = ((Control) this).GetTemplateChild("contentControl") as ContentPresenter;
      if (this._contentControl == null)
        return;
      ((UIElement) this._contentControl).Tap += new EventHandler<GestureEventArgs>(this.ContentControlOnTap);
    }

    private void ContentControlOnTap(object sender, GestureEventArgs e) => this.OnBalloonTapped(e);

    public Visibility ContentVisibility
    {
      get => (Visibility) ((DependencyObject) this).GetValue(PushPin.ContentVisibilityProperty);
      set => ((DependencyObject) this).SetValue(PushPin.ContentVisibilityProperty, (object) value);
    }

    public PushPinState State
    {
      get => (PushPinState) ((DependencyObject) this).GetValue(PushPin.StateProperty);
      set => ((DependencyObject) this).SetValue(PushPin.StateProperty, (object) value);
    }

    internal event EventHandler ContentVisibilityChanged;

    protected virtual void OnContentVisibilityChanged()
    {
      EventHandler visibilityChanged = this.ContentVisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((object) this, EventArgs.Empty);
    }

    private static void ContentVisibilityChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PushPin pushPin = (PushPin) d;
      pushPin.SetContentVisibility((Visibility) e.NewValue);
      pushPin.OnContentVisibilityChanged();
    }

    private void SetContentVisibility(Visibility value)
    {
      switch ((int) value)
      {
        case 0:
          VisualStateManager.GoToState((Control) this, "BaloonVisible", true);
          break;
        case 1:
          VisualStateManager.GoToState((Control) this, "BaloonCollapsed", true);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public event EventHandler StateChanged;

    protected virtual void OnStateChanged()
    {
      EventHandler stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, EventArgs.Empty);
    }

    private static void StateChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PushPin pushPin = (PushPin) d;
      pushPin.SetState((PushPinState) e.NewValue);
      pushPin.OnStateChanged();
    }

    private void SetState(PushPinState value)
    {
      switch (value)
      {
        case PushPinState.Collapsed:
          VisualStateManager.GoToState((Control) this, "Collapsed", true);
          break;
        case PushPinState.Expanded:
          VisualStateManager.GoToState((Control) this, "Expanded", true);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    private bool IsContentAvailable()
    {
      if (this.Content is string && string.IsNullOrWhiteSpace((string) this.Content))
        return false;
      return this.Content != null || this.ContentTemplate != null;
    }

    public event EventHandler<CancelEventArgs> PreviewTap;

    public void OnPreviewTap(CancelEventArgs e)
    {
      EventHandler<CancelEventArgs> previewTap = this.PreviewTap;
      if (previewTap == null)
        return;
      previewTap((object) this, e);
    }

    protected virtual void OnTap(GestureEventArgs e)
    {
      CancelEventArgs e1 = new CancelEventArgs();
      this.OnPreviewTap(e1);
      if (e1.Cancel)
        return;
      if (this.ContentVisibility == 1)
      {
        if (this.IsContentAvailable())
          this.ContentVisibility = (Visibility) 0;
      }
      else
        this.ContentVisibility = (Visibility) 1;
      e.Handled = true;
    }

    public event EventHandler<GestureEventArgs> BalloonTapped;

    protected internal virtual void OnBalloonTapped(GestureEventArgs e)
    {
      EventHandler<GestureEventArgs> balloonTapped = this.BalloonTapped;
      if (balloonTapped == null)
        return;
      balloonTapped((object) this, e);
    }

    internal bool IsHidden
    {
      get => this._isHidden;
      set
      {
        if (value == this._isHidden)
          return;
        this._isHidden = value;
        this.UpdateRootElementOpacity();
      }
    }

    private void UpdateRootElementOpacity()
    {
      if (this._rootElement == null)
        return;
      this._rootElement.Opacity = this._isHidden ? 0.0 : 1.0;
    }
  }
}
