// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.WeakDelegateCommand`1
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Y.UI.Common
{
  public class WeakDelegateCommand<T> : ICommand
  {
    private readonly Action<T> _executeMethod;
    private readonly Func<T, bool> _canExecuteMethod;
    private List<WeakReference> _canExecuteChangedHandlers;

    public WeakDelegateCommand(Action<T> executeMethod)
      : this(executeMethod, (Func<T, bool>) null)
    {
    }

    public WeakDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
    {
      this._executeMethod = executeMethod != null || canExecuteMethod != null ? executeMethod : throw new ArgumentNullException(nameof (executeMethod), "Resources.DelegateCommandDelegatesCannotBeNull");
      this._canExecuteMethod = canExecuteMethod;
    }

    public virtual bool CanExecute(T parameter) => this._canExecuteMethod == null || this._canExecuteMethod(parameter);

    public void Execute(T parameter)
    {
      if (this._executeMethod == null)
        return;
      this._executeMethod(parameter);
    }

    bool ICommand.CanExecute(object parameter) => this.CanExecute((T) parameter);

    public event EventHandler CanExecuteChanged
    {
      add => WeakEventHandlerManager.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
      remove => WeakEventHandlerManager.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
    }

    void ICommand.Execute(object parameter) => this.Execute((T) parameter);

    protected virtual void OnCanExecuteChanged() => WeakEventHandlerManager.CallWeakReferenceHandlers((object) this, this._canExecuteChangedHandlers);

    public void RaiseCanExecuteChanged() => this.OnCanExecuteChanged();
  }
}
