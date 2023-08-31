// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.QueryHost
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Common;
using Yandex.Maps.Config;

namespace Yandex.Maps.PrinterClient.Config
{
  [ComVisible(true)]
  public class QueryHost
  {
    [XmlText]
    public string Value { get; set; }

    [XmlAttribute("type")]
    public string HostTypeString
    {
      get => this.HostType.ToString();
      set
      {
        try
        {
          this.HostType = (HostTypes) Enum.Parse(typeof (HostTypes), value, true);
        }
        catch (Exception ex)
        {
          Logger.TrackException(ex);
        }
      }
    }

    [XmlIgnore]
    public HostTypes HostType { get; set; }
  }
}
