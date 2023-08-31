// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Monitor
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using Yandex.Threading.Interfaces;

namespace Yandex.Threading
{
  [UsedImplicitly]
  public class Monitor : IMonitor
  {
    public void Enter(object obj) => System.Threading.Monitor.Enter(obj);

    public void Exit(object obj) => System.Threading.Monitor.Exit(obj);

    public void Pulse(object obj) => System.Threading.Monitor.Pulse(obj);

    public void PulseAll(object obj) => System.Threading.Monitor.PulseAll(obj);

    public bool Wait(object obj) => System.Threading.Monitor.Wait(obj);

    public bool Wait(object obj, TimeSpan timeSpan) => System.Threading.Monitor.Wait(obj, timeSpan);
  }
}
