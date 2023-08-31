// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.IManifestInformationProvider
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.IO;

namespace Yandex.App.Information
{
  public interface IManifestInformationProvider
  {
    ManifestInformation Info { get; }

    void Initialize(Stream manifestStream);
  }
}
