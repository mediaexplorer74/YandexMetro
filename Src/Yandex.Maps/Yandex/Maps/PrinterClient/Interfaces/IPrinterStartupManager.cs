﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Interfaces.IPrinterStartupManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.PrinterClient.EventArgs;

namespace Yandex.Maps.PrinterClient.Interfaces
{
  internal interface IPrinterStartupManager
  {
    event EventHandler<PrinterStartupEventArgs> StartupCompleted;

    event EventHandler StartupFailed;

    void Startup(Version appVersion, string appPlatform, string uuid);
  }
}
