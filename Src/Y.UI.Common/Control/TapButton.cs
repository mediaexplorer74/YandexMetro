// Y.UI.Common.Control.TapButton

using System;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Y.UI.Common.Control
{
  public class TapButton : UserControl
  {
    public static readonly DependencyProperty CommandProperty 
            = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (TapButton),
                new PropertyMetadata((PropertyChangedCallback) null));

    public static readonly DependencyProperty CommandParameterProperty 
            = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (TapButton), 
                new PropertyMetadata((PropertyChangedCallback) null));

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
      this.OnApplyTemplate();
      //this.Tap += new EventHandler<GestureEventArgs>(this.TapButton_Tap);
    }

    private void TapButton_Tap(object sender, EventArgs e)
    {
      ICommand command = this.Command;
      if (command == null || !command.CanExecute(this.CommandParameter))
        return;
      command.Execute(this.CommandParameter);
      //e.Handled = true;
    }

        public TapButton()
        {
            this.DefaultStyleKey = (object)typeof(TapButton);
        }
    }
}
