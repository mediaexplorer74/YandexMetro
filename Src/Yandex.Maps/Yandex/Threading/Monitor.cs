// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Monitor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Threading.Interfaces;

namespace Yandex.Threading
{
  [UsedImplicitly]
  internal class Monitor : IMonitor
  {
    public void Enter(object obj) => System.Threading.Monitor.Enter(obj);

    public void Exit(object obj) => System.Threading.Monitor.Exit(obj);

    public void Pulse(object obj) => System.Threading.Monitor.Pulse(obj);

    public void PulseAll(object obj) => System.Threading.Monitor.PulseAll(obj);

    public bool Wait(object obj) => System.Threading.Monitor.Wait(obj);

    public bool Wait(object obj, TimeSpan timeSpan) => System.Threading.Monitor.Wait(obj, timeSpan);
  }
}
