// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Dto.ManifestDto
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.Xml.Serialization;

namespace Yandex.App.Information.Dto
{
  [XmlRoot("Deployment", Namespace = "http://schemas.microsoft.com/windowsphone/2009/deployment")]
  public class ManifestDto
  {
    [XmlElement("App", Namespace = "")]
    public AppDto App { get; set; }
  }
}
