﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.UiActionsLog
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Config
{
  [ComVisible(true)]
  public class UiActionsLog
  {
    [XmlAttribute("events")]
    public string Events { get; set; }

    [XmlText]
    public string Value { get; set; }
  }
}
