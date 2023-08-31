// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.EventArgs.PrinterStartupEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.PrinterClient.Config;

namespace Yandex.Maps.PrinterClient.EventArgs
{
  internal class PrinterStartupEventArgs : System.EventArgs
  {
    public PrinterStartupEventArgs(StartupParameters config) => this.Config = config != null ? config : throw new ArgumentNullException(nameof (config));

    public StartupParameters Config { get; set; }
  }
}
