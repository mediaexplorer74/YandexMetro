// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ViewStateManager.ViewStateManagerBase`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Yandex.Controls.ViewStateManager.Interfaces;

namespace Yandex.Controls.ViewStateManager
{
  internal abstract class ViewStateManagerBase<TViewState> : IViewStateManagerBase<TViewState>
  {
    protected readonly Dictionary<TViewState, IViewState> States = new Dictionary<TViewState, IViewState>();
    private Control _control;
    private TViewState _currentState;

    public event EventHandler StateChanged;

    private void OnStateChanged(EventArgs e)
    {
      EventHandler stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, e);
    }

    private IViewState CurrentViewState => this.States[this._currentState];

    public bool SetState(TViewState state) => this.SetStateInternal(state, false);

    protected virtual bool SetStateInternal(TViewState state, bool restore, bool checkCurrentState = true)
    {
      if (checkCurrentState && object.Equals((object) this._currentState, (object) state))
        return false;
      IViewState currentViewState = this.CurrentViewState;
      this._currentState = state;
      if (!restore)
        currentViewState.OnLeave(this.CurrentViewState);
      VisualStateManager.GoToState(this._control, this.CurrentViewState.VisualStateName, true);
      this.CurrentViewState.OnSet(restore, currentViewState);
      this.OnStateChanged(EventArgs.Empty);
      return true;
    }

    public TViewState CurrentState => this._currentState;

    protected void Init(Control control) => this._control = control;

    public abstract void LoadSavedState();
  }
}
