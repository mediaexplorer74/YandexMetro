// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.BannerConfig
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Config
{
  [ComVisible(true)]
  public class BannerConfig
  {
    [XmlAttribute("timeout")]
    public int Timeout { get; set; }

    [XmlAttribute("url_text")]
    public string UrlText { get; set; }

    [XmlAttribute("phone_text")]
    public string PhoneText { get; set; }

    [XmlAttribute("action_url")]
    public string ActionUrl { get; set; }

    [XmlAttribute("action_phone")]
    public string ActionPhone { get; set; }

    [XmlAttribute("call_warning")]
    public string CallWarning { get; set; }

    [XmlAttribute("call_counter")]
    public string CallCounter { get; set; }

    [XmlAttribute("banner_timeout")]
    public int BannerTimeout { get; set; }

    [XmlAttribute("user_inactive")]
    public int UserInactive { get; set; }

    [XmlAttribute("first_timeout")]
    public int FirstTimeout { get; set; }

    [XmlAttribute("action")]
    public string Action { get; set; }

    [XmlText]
    public string BannerUrl { get; set; }
  }
}
