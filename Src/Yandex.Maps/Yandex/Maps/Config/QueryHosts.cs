// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.QueryHosts
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.PrinterClient.Config;
using Yandex.Patterns;

namespace Yandex.Maps.Config
{
  [UsedImplicitly]
  internal sealed class QueryHosts : IStateService
  {
    private ServiceState _state;
    private Dictionary<HostTypes, string> _storage;

    public QueryHosts([NotNull] Dictionary<HostTypes, string> storage) => this._storage = storage != null ? storage : throw new ArgumentNullException(nameof (storage));

    public event EventHandler<StateChangedEventArgs> StateChanged;

    private void OnStateChanged(StateChangedEventArgs e)
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

    public bool TryGetValue(HostTypes key, out string value) => this._storage.TryGetValue(key, out value);

    public void SetHosts([NotNull] IEnumerable<QueryHost> hosts)
    {
      this.State = ServiceState.Ready;
      this._storage = hosts.ToDictionary<QueryHost, HostTypes, string>((Func<QueryHost, HostTypes>) (item => item.HostType), (Func<QueryHost, string>) (item => item.Value));
    }
  }
}
