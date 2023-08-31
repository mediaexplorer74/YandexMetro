// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Models.AppItem
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;

namespace Yandex.App.Information.Models
{
  public sealed class AppItem
  {
    public Uri Icon { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public Uri AppUri { get; private set; }

    public AppItem(Uri icon, string name, string description, Uri appUri)
    {
      this.Icon = icon;
      this.Name = name;
      this.Description = description;
      this.AppUri = appUri;
    }
  }
}
