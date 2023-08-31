// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Command.RelayCommand
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace GalaSoft.MvvmLight.Command
{
  public class RelayCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute)
      : this(execute, (Func<bool>) null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      this._execute = execute != null ? execute : throw new ArgumentNullException(nameof (execute));
      this._canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
      EventHandler canExecuteChanged = this.CanExecuteChanged;
      if (canExecuteChanged == null)
        return;
      canExecuteChanged((object) this, EventArgs.Empty);
    }

    [DebuggerStepThrough]
    public bool CanExecute(object parameter) => this._canExecute == null || this._canExecute();

    public void Execute(object parameter) => this._execute();
  }
}
