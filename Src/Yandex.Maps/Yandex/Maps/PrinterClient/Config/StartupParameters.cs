// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Config.StartupParameters
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.PrinterClient.Config
{
  [XmlRoot("startup")]
  [ComVisible(true)]
  public class StartupParameters
  {
    private const string changes = "changes";

    [XmlElement("wap_warning")]
    public string WapWarning { get; set; }

    [XmlElement("app")]
    public ApplicationIdentityConfig App { get; set; }

    [XmlElement("changelog")]
    public string Changelog { get; set; }

    [XmlElement("hellostr")]
    public TimeoutString HelloString { get; set; }

    [XmlElement("news")]
    public News News { get; set; }

    [XmlElement("ui_actions_log")]
    public UiActionsLog UiActionsLog { get; set; }

    [XmlElement("banner")]
    public BannerConfig Banner { get; set; }

    [XmlElement("maps_updated")]
    public string MapsUpdated { get; set; }

    [XmlElement("uuid")]
    public string Uuid { get; set; }

    [XmlElement("operator")]
    public Operator Operator { get; set; }

    [XmlElement("features")]
    public ProviderFeatures ProviderFeatures { get; set; }

    [XmlElement("maps")]
    public Map Maps { get; set; }

    [XmlArray("map_layers")]
    [XmlArrayItem("l")]
    public MapLayer[] MapLayers { get; set; }

    [XmlElement("openpos")]
    public MapPosition OpenPos { get; set; }

    [XmlArrayItem("host")]
    [XmlArray("query_hosts")]
    public QueryHost[] QueryHosts { get; set; }

    [XmlArrayItem("interval")]
    [XmlArray("objectshowintervals")]
    public ObjectInterval[] ObjectIntervals { get; set; }
  }
}
