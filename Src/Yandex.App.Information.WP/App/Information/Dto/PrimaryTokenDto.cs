// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Dto.PrimaryTokenDto
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.Xml.Serialization;

namespace Yandex.App.Information.Dto
{
  public class PrimaryTokenDto
  {
    [XmlElement("TemplateType5")]
    public TemplateType5Dto TemplateType5 { get; set; }

    [XmlAttribute("TokenID")]
    public string TokenID { get; set; }

    [XmlAttribute("TaskName")]
    public string TaskName { get; set; }
  }
}
