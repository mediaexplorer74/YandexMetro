// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.NetworkInterfaceWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Net.NetworkInformation;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal class NetworkInterfaceWrapper : INetworkInterfaceWrapper
  {
    private bool _isNetworkAvailable;
    private readonly object _sync = new object();

    public NetworkInterfaceWrapper()
    {
      NetworkChange.NetworkAddressChanged += (NetworkAddressChangedEventHandler) ((sender, e) => this.UpdateIsNetworkAvailable());
      this.UpdateIsNetworkAvailable();
    }

    private void UpdateIsNetworkAvailable()
    {
      bool flag;
      lock (this._sync)
      {
        bool networkAvailable = this._isNetworkAvailable;
        this._isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
        flag = this._isNetworkAvailable != networkAvailable;
      }
      if (!flag)
        return;
      this.OnIsNetworkAvailableChanged();
    }

    public bool GetIsNetworkAvailable() => this._isNetworkAvailable;

    public event EventHandler IsNetworkAvailableChanged;

    private void OnIsNetworkAvailableChanged()
    {
      EventHandler availableChanged = this.IsNetworkAvailableChanged;
      if (availableChanged == null)
        return;
      availableChanged((object) this, EventArgs.Empty);
    }
  }
}
