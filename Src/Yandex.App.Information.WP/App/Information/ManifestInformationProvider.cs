// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.ManifestInformationProvider
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Linq;
using Yandex.App.Information.Dto;
using Yandex.Serialization;

namespace Yandex.App.Information
{
  [UsedImplicitly]
  public class ManifestInformationProvider : IManifestInformationProvider
  {
    public ManifestInformation Info { get; private set; }

    public void Initialize(Stream manifestStream) => this.ConvertFromDto(new GenericXmlSerializer<ManifestDto>().Deserialize(manifestStream, false));

    private void ConvertFromDto(ManifestDto manifestDto) => this.Info = new ManifestInformation()
    {
      ImageUri = manifestDto.App.Tokens.First<PrimaryTokenDto>((Func<PrimaryTokenDto, bool>) (t => t.TaskName == "_default")).TemplateType5.TileImage.Uri,
      ApplicationName = manifestDto.App.Title
    };
  }
}
