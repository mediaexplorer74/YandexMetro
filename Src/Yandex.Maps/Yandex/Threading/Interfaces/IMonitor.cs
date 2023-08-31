// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Interfaces.IMonitor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Threading.Interfaces
{
  internal interface IMonitor
  {
    void Enter(object obj);

    void Exit(object obj);

    void Pulse(object obj);

    void PulseAll(object obj);

    bool Wait(object obj);

    bool Wait(object obj, TimeSpan timeSpan);
  }
}
