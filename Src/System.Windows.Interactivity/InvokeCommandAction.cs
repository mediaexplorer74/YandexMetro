// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.InvokeCommandAction
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Reflection;
using System.Windows.Input;

namespace System.Windows.Interactivity
{
  public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
  {
    private string commandName;
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (InvokeCommandAction), (PropertyMetadata) null);
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (InvokeCommandAction), (PropertyMetadata) null);

    public string CommandName
    {
      get => this.commandName;
      set
      {
        if (!(this.CommandName != value))
          return;
        this.commandName = value;
      }
    }

    public ICommand Command
    {
      get => (ICommand) this.GetValue(InvokeCommandAction.CommandProperty);
      set => this.SetValue(InvokeCommandAction.CommandProperty, (object) value);
    }

    public object CommandParameter
    {
      get => this.GetValue(InvokeCommandAction.CommandParameterProperty);
      set => this.SetValue(InvokeCommandAction.CommandParameterProperty, value);
    }

    protected override void Invoke(object parameter)
    {
      if (this.AssociatedObject == null)
        return;
      ICommand command = this.ResolveCommand();
      if (command == null || !command.CanExecute(this.CommandParameter))
        return;
      command.Execute(this.CommandParameter);
    }

    private ICommand ResolveCommand()
    {
      ICommand command = (ICommand) null;
      if (this.Command != null)
        command = this.Command;
      else if (this.AssociatedObject != null)
      {
        foreach (PropertyInfo property in this.AssociatedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
          if (typeof (ICommand).IsAssignableFrom(property.PropertyType) && string.Equals(property.Name, this.CommandName, StringComparison.Ordinal))
            command = (ICommand) property.GetValue((object) this.AssociatedObject, (object[]) null);
        }
      }
      return command;
    }
  }
}
