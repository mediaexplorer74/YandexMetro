// Decompiled with JetBrains decompiler
// Type: Yandex.Heroism.IHeroismSettings
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

namespace Yandex.Heroism
{
  public interface IHeroismSettings
  {
    bool IsHeroismModeEnabled { get; }

    bool IsHeroismModeEnabledPendingValue { get; set; }
  }
}
