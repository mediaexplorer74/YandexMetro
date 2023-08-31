// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.WeakDelegateCommand
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Y.UI.Common
{
  public class WeakDelegateCommand : ICommand
  {
    private readonly Action _executeMethod;
    private readonly Func<bool> _canExecuteMethod;
    private List<WeakReference> _canExecuteChangedHandlers;

    public WeakDelegateCommand(Action executeMethod)
      : this(executeMethod, (Func<bool>) null)
    {
    }

    public WeakDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
    {
      this._executeMethod = executeMethod != null || canExecuteMethod != null ? executeMethod : throw new ArgumentNullException(nameof (executeMethod), "Resources.DelegateCommandDelegatesCannotBeNull");
      this._canExecuteMethod = canExecuteMethod;
    }

    public virtual bool CanExecute(object parameter) => this._canExecuteMethod == null || this._canExecuteMethod();

    public void Execute(object parameter)
    {
      if (this._executeMethod == null)
        return;
      this._executeMethod();
    }

    bool ICommand.CanExecute(object parameter) => this.CanExecute(parameter);

    public event EventHandler CanExecuteChanged
    {
      add => WeakEventHandlerManager.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
      remove => WeakEventHandlerManager.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
    }

    void ICommand.Execute(object parameter) => this.Execute(parameter);

    protected virtual void OnCanExecuteChanged() => WeakEventHandlerManager.CallWeakReferenceHandlers((object) this, this._canExecuteChangedHandlers);

    public void RaiseCanExecuteChanged() => this.OnCanExecuteChanged();
  }
}
