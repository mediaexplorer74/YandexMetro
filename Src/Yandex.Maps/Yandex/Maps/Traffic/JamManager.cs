// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Traffic.DTO.Styles;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Patterns;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  internal class JamManager : IJamManager, IStateService
  {
    private readonly IJamStylesManager _jamStylesManager;
    private readonly IJamCollectManager _jamCollectManager;
    private ServiceState _state;

    public JamManager(
      [NotNull] IJamStylesManager jamStylesManager,
      [NotNull] IMapWebClientFactory webClientFactory,
      [NotNull] IJamCollectManager jamCollectManager)
    {
      if (jamStylesManager == null)
        throw new ArgumentNullException(nameof (jamStylesManager));
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
      if (jamCollectManager == null)
        throw new ArgumentNullException(nameof (jamCollectManager));
      this._jamStylesManager = jamStylesManager;
      this._jamCollectManager = jamCollectManager;
      this._jamStylesManager.RequestCompleted += new RequestCompletedEventHandler<object, JamStyles>(this.JamStylesManagerRequestCompleted);
    }

    public void Connect()
    {
      this._jamStylesManager.Request((object) null);
      this._jamCollectManager.Activate();
    }

    private void JamStylesManagerRequestCompleted(
      object sender,
      RequestCompletedEventArgs<object, JamStyles> e)
    {
      this.State = ServiceState.Ready;
    }

    public event EventHandler<StateChangedEventArgs> StateChanged;

    protected virtual void OnStateChanged(StateChangedEventArgs e)
    {
      EventHandler<StateChangedEventArgs> stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, e);
    }

    public ServiceState State
    {
      get => this._state;
      private set
      {
        if (this._state == value)
          return;
        this._state = value;
        this.OnStateChanged(new StateChangedEventArgs(value));
      }
    }
  }
}
