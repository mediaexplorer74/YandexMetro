// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.ProviderFeatures
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Maps.PrinterClient.DTO;

namespace Yandex.Maps.PrinterClient.Config
{
  [ComVisible(true)]
  public class ProviderFeatures
  {
    [XmlElement("operator_branding")]
    public OperatorBranding OperatorBranding { get; set; }

    [XmlElement("operator_portal")]
    public OperatorPortal OperatorPortal { get; set; }

    [XmlElement("operator_lbs")]
    public string OperatorLbs { get; set; }

    [XmlElement("network_scanner")]
    public NetworkServiceConfig NetworkScanner { get; set; }

    [XmlElement("traffic_collect")]
    public NetworkServiceConfig TrafficCollect { get; set; }

    [XmlElement("userpoi")]
    public FeaturesParam UserPoi { get; set; }
  }
}
