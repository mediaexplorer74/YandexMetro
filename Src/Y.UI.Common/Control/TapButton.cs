// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.TapButton
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Windows;
using System.Windows.Input;

namespace Y.UI.Common.Control
{
  public class TapButton : System.Windows.Controls.Control
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (TapButton), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (TapButton), new PropertyMetadata((PropertyChangedCallback) null));

    public ICommand Command
    {
      get => (ICommand) ((DependencyObject) this).GetValue(TapButton.CommandProperty);
      set => ((DependencyObject) this).SetValue(TapButton.CommandProperty, (object) value);
    }

    public object CommandParameter
    {
      get => ((DependencyObject) this).GetValue(TapButton.CommandParameterProperty);
      set => ((DependencyObject) this).SetValue(TapButton.CommandParameterProperty, value);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      ((UIElement) this).Tap += new EventHandler<GestureEventArgs>(this.TapButton_Tap);
    }

    private void TapButton_Tap(object sender, GestureEventArgs e)
    {
      ICommand command = this.Command;
      if (command == null || !command.CanExecute(this.CommandParameter))
        return;
      command.Execute(this.CommandParameter);
      e.Handled = true;
    }

    public TapButton() => this.DefaultStyleKey = (object) typeof (TapButton);
  }
}
