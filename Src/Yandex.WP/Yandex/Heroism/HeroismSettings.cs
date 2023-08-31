// Decompiled with JetBrains decompiler
// Type: Yandex.Heroism.HeroismSettings
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using Yandex.App;

namespace Yandex.Heroism
{
  public class HeroismSettings : IHeroismSettings
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
