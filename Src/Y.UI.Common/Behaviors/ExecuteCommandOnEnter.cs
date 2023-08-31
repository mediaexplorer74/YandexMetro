// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Behaviors.ExecuteCommandOnEnter
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Y.UI.Common.Behaviors
{
  public class ExecuteCommandOnEnter : Behavior<TextBox>
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (ExecuteCommandOnEnter), new PropertyMetadata((PropertyChangedCallback) null));

    public ICommand Command
    {
      get => (ICommand) this.GetValue(ExecuteCommandOnEnter.CommandProperty);
      set => this.SetValue(ExecuteCommandOnEnter.CommandProperty, (object) value);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      ((UIElement) this.AssociatedObject).KeyUp += new KeyEventHandler(this.AssociatedObject_KeyUp);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      ((UIElement) this.AssociatedObject).KeyUp -= new KeyEventHandler(this.AssociatedObject_KeyUp);
    }

    private void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != 3 && e.PlatformKeyCode != 10 || string.IsNullOrEmpty(this.AssociatedObject.Text) || this.Command == null)
        return;
      ((Control) (((FrameworkElement) this.AssociatedObject).Parent as FrameworkElement).FindPageBaseParent())?.Focus();
      this.Command.Execute((object) this.AssociatedObject.Text);
      e.Handled = true;
    }
  }
}
