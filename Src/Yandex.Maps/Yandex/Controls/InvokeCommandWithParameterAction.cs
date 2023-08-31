// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.InvokeCommandWithParameterAction
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Yandex.Controls
{
  internal sealed class InvokeCommandWithParameterAction : TriggerAction<DependencyObject>
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (InvokeCommandWithParameterAction), (PropertyMetadata) null);
    private string _commandName;

    public string CommandName
    {
      get => this._commandName;
      set
      {
        if (this.CommandName == value)
          return;
        this._commandName = value;
      }
    }

    public ICommand Command
    {
      get => (ICommand) this.GetValue(InvokeCommandWithParameterAction.CommandProperty);
      set => this.SetValue(InvokeCommandWithParameterAction.CommandProperty, (object) value);
    }

    protected override void Invoke(object parameter)
    {
      if (this.AssociatedObject == null)
        return;
      ICommand command = this.ResolveCommand();
      if (command == null || !command.CanExecute(parameter))
        return;
      command.Execute(parameter);
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
