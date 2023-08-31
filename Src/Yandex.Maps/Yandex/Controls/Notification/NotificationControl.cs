// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Notification.NotificationControl
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Yandex.Controls.Notification
{
  [TemplatePart(Name = "border", Type = typeof (Border))]
  internal class NotificationControl : Control
  {
    private const string BorderName = "border";
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (NotificationControl), new PropertyMetadata((object) string.Empty));
    private bool _beginStoryboard;
    private Storyboard _storyboard;

    public NotificationControl() => this.DefaultStyleKey = (object) typeof (NotificationControl);

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(NotificationControl.TextProperty);
      set => ((DependencyObject) this).SetValue(NotificationControl.TextProperty, (object) value);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._storyboard = (Storyboard) ((FrameworkElement) this.GetTemplateChild("border")).Resources[(object) "HideStoryboard"];
      if (!this._beginStoryboard)
        return;
      this._storyboard.Begin();
    }

    public void BeginStoryboard()
    {
      if (this._storyboard == null)
        this._beginStoryboard = true;
      else
        this._storyboard.Begin();
    }
  }
}
