// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.HubTile
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(Name = "Flipped", GroupName = "ImageState")]
  [TemplateVisualState(Name = "Collapsed", GroupName = "ImageState")]
  [TemplatePart(Name = "MessageBlock", Type = typeof (TextBlock))]
  [TemplateVisualState(Name = "Expanded", GroupName = "ImageState")]
  [TemplateVisualState(Name = "Semiexpanded", GroupName = "ImageState")]
  [TemplatePart(Name = "BackTitleBlock", Type = typeof (TextBlock))]
  [TemplatePart(Name = "NotificationBlock", Type = typeof (TextBlock))]
  public class HubTile : Control
  {
    private const string ImageStates = "ImageState";
    private const string Expanded = "Expanded";
    private const string Semiexpanded = "Semiexpanded";
    private const string Collapsed = "Collapsed";
    private const string Flipped = "Flipped";
    private const string NotificationBlock = "NotificationBlock";
    private const string MessageBlock = "MessageBlock";
    private const string BackTitleBlock = "BackTitleBlock";
    private TextBlock _notificationBlock;
    private TextBlock _messageBlock;
    private TextBlock _backTitleBlock;
    internal int _stallingCounter;
    internal bool _canDrop;
    internal bool _canFlip;
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof (Source), typeof (ImageSource), typeof (HubTile), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (HubTile), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(HubTile.OnTitleChanged)));
    public static readonly DependencyProperty NotificationProperty = DependencyProperty.Register(nameof (Notification), typeof (string), typeof (HubTile), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(HubTile.OnBackContentChanged)));
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (HubTile), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(HubTile.OnBackContentChanged)));
    public static readonly DependencyProperty DisplayNotificationProperty = DependencyProperty.Register(nameof (DisplayNotification), typeof (bool), typeof (HubTile), new PropertyMetadata((object) false, new PropertyChangedCallback(HubTile.OnBackContentChanged)));
    public static readonly DependencyProperty IsFrozenProperty = DependencyProperty.Register(nameof (IsFrozen), typeof (bool), typeof (HubTile), new PropertyMetadata((object) false, new PropertyChangedCallback(HubTile.OnIsFrozenChanged)));
    public static readonly DependencyProperty GroupTagProperty = DependencyProperty.Register(nameof (GroupTag), typeof (string), typeof (HubTile), new PropertyMetadata((object) string.Empty));
    private static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof (State), typeof (ImageState), typeof (HubTile), new PropertyMetadata((object) ImageState.Expanded, new PropertyChangedCallback(HubTile.OnImageStateChanged)));

    public ImageSource Source
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(HubTile.SourceProperty);
      set => ((DependencyObject) this).SetValue(HubTile.SourceProperty, (object) value);
    }

    public string Title
    {
      get => (string) ((DependencyObject) this).GetValue(HubTile.TitleProperty);
      set => ((DependencyObject) this).SetValue(HubTile.TitleProperty, (object) value);
    }

    private static void OnTitleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      HubTile hubTile = (HubTile) obj;
      if (string.IsNullOrEmpty((string) e.NewValue))
      {
        hubTile._canDrop = false;
        hubTile.State = ImageState.Expanded;
      }
      else
        hubTile._canDrop = true;
    }

    public string Notification
    {
      get => (string) ((DependencyObject) this).GetValue(HubTile.NotificationProperty);
      set => ((DependencyObject) this).SetValue(HubTile.NotificationProperty, (object) value);
    }

    private static void OnBackContentChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      HubTile hubTile = (HubTile) obj;
      if (!string.IsNullOrEmpty(hubTile.Notification) && hubTile.DisplayNotification || !string.IsNullOrEmpty(hubTile.Message) && !hubTile.DisplayNotification)
      {
        hubTile._canFlip = true;
      }
      else
      {
        hubTile._canFlip = false;
        hubTile.State = ImageState.Expanded;
      }
    }

    public string Message
    {
      get => (string) ((DependencyObject) this).GetValue(HubTile.MessageProperty);
      set => ((DependencyObject) this).SetValue(HubTile.MessageProperty, (object) value);
    }

    public bool DisplayNotification
    {
      get => (bool) ((DependencyObject) this).GetValue(HubTile.DisplayNotificationProperty);
      set => ((DependencyObject) this).SetValue(HubTile.DisplayNotificationProperty, (object) value);
    }

    public bool IsFrozen
    {
      get => (bool) ((DependencyObject) this).GetValue(HubTile.IsFrozenProperty);
      set => ((DependencyObject) this).SetValue(HubTile.IsFrozenProperty, (object) value);
    }

    private static void OnIsFrozenChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      HubTile tile = (HubTile) obj;
      if ((bool) e.NewValue)
        HubTileService.FreezeHubTile(tile);
      else
        HubTileService.UnfreezeHubTile(tile);
    }

    public string GroupTag
    {
      get => (string) ((DependencyObject) this).GetValue(HubTile.GroupTagProperty);
      set => ((DependencyObject) this).SetValue(HubTile.GroupTagProperty, (object) value);
    }

    internal ImageState State
    {
      get => (ImageState) ((DependencyObject) this).GetValue(HubTile.StateProperty);
      set => ((DependencyObject) this).SetValue(HubTile.StateProperty, (object) value);
    }

    private static void OnImageStateChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((HubTile) obj).UpdateVisualState();
    }

    private void UpdateVisualState()
    {
      string str;
      switch (this.State)
      {
        case ImageState.Expanded:
          str = "Expanded";
          break;
        case ImageState.Semiexpanded:
          str = "Semiexpanded";
          break;
        case ImageState.Collapsed:
          str = "Collapsed";
          break;
        case ImageState.Flipped:
          str = "Flipped";
          break;
        default:
          str = "Expanded";
          break;
      }
      VisualStateManager.GoToState((Control) this, str, true);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._notificationBlock = this.GetTemplateChild("NotificationBlock") as TextBlock;
      this._messageBlock = this.GetTemplateChild("MessageBlock") as TextBlock;
      this._backTitleBlock = this.GetTemplateChild("BackTitleBlock") as TextBlock;
      if (this._notificationBlock != null)
        ((FrameworkElement) this._notificationBlock).SetBinding(UIElement.VisibilityProperty, new Binding()
        {
          Source = (object) this,
          Path = new PropertyPath("DisplayNotification", new object[0]),
          Converter = (IValueConverter) new VisibilityConverter(),
          ConverterParameter = (object) false
        });
      if (this._messageBlock != null)
        ((FrameworkElement) this._messageBlock).SetBinding(UIElement.VisibilityProperty, new Binding()
        {
          Source = (object) this,
          Path = new PropertyPath("DisplayNotification", new object[0]),
          Converter = (IValueConverter) new VisibilityConverter(),
          ConverterParameter = (object) true
        });
      if (this._backTitleBlock != null)
        ((FrameworkElement) this._backTitleBlock).SetBinding(TextBlock.TextProperty, new Binding()
        {
          Source = (object) this,
          Path = new PropertyPath("Title", new object[0]),
          Converter = (IValueConverter) new MultipleToSingleLineStringConverter()
        });
      this.UpdateVisualState();
    }

    public HubTile()
    {
      this.DefaultStyleKey = (object) typeof (HubTile);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.HubTile_Loaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.HubTile_Unloaded);
    }

    private void HubTile_Loaded(object sender, RoutedEventArgs e) => HubTileService.InitializeReference(this);

    private void HubTile_Unloaded(object sender, RoutedEventArgs e) => HubTileService.FinalizeReference(this);
  }
}
