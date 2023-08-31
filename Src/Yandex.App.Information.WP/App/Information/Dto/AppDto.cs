// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Dto.AppDto
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Yandex.App.Information.Dto
{
  public class AppDto
  {
    [XmlAttribute("ProductID")]
    public Guid ProductId { get; set; }

    [XmlAttribute("Title")]
    public string Title { get; set; }

    [XmlAttribute("Version")]
    public string Version { get; set; }

    [XmlAttribute("Author")]
    public string Author { get; set; }

    [XmlAttribute("Description")]
    public string Description { get; set; }

    [XmlAttribute("Publisher")]
    public string Publisher { get; set; }

    [XmlElement("IconPath")]
    public ImageIriDto IconPath { get; set; }

    [XmlArrayItem("PrimaryToken", typeof (PrimaryTokenDto))]
    [XmlArray("Tokens")]
    public List<PrimaryTokenDto> Tokens { get; set; }
  }
}
