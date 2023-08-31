// Decompiled with JetBrains decompiler
// Type: Yandex.Heroism.HeroismSettings
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.App;

namespace Yandex.Heroism
{
  internal class HeroismSettings : IHeroismSettings
  {
    private const string IsHeroismModeEnabledKey = "IsHeroismModeEnabled";
    private readonly IApplicationSettings _applicationSettings;

    public bool IsHeroismModeEnabled { get; private set; }

    public HeroismSettings([NotNull] IApplicationSettings applicationSettings)
    {
      this._applicationSettings = applicationSettings != null ? applicationSettings : throw new ArgumentNullException(nameof (applicationSettings));
      this.IsHeroismModeEnabled = this.IsHeroismModeEnabledPendingValue;
    }

    public bool IsHeroismModeEnabledPendingValue
    {
      get
      {
        bool flag;
        return this._applicationSettings.TryGetValue<bool>("IsHeroismModeEnabled", out flag) && flag;
      }
      set => this._applicationSettings["IsHeroismModeEnabled"] = (object) value;
    }
  }
}
