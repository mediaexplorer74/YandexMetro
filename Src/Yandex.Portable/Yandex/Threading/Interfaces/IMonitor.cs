// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Interfaces.IMonitor
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.Threading.Interfaces
{
  public interface IMonitor
  {
    void Enter(object obj);

    void Exit(object obj);

    void Pulse(object obj);

    void PulseAll(object obj);

    bool Wait(object obj);

    bool Wait(object obj, TimeSpan timeSpan);
  }
}
