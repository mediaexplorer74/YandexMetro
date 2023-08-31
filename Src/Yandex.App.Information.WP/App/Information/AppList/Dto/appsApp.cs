// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.Dto.appsApp
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.Xml.Serialization;

namespace Yandex.App.Information.AppList.Dto
{
  public class appsApp
  {
    [XmlAttribute]
    public string lang { get; set; }

    [XmlAttribute]
    public string img { get; set; }

    [XmlAttribute]
    public string desc { get; set; }

    [XmlAttribute]
    public string url { get; set; }

    [XmlAttribute]
    public string id { get; set; }

    [XmlAttribute]
    public string platform { get; set; }

    [XmlAttribute]
    public string title { get; set; }
  }
}
