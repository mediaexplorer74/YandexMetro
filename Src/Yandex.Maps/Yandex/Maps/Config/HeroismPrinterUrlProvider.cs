// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.HeroismPrinterUrlProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.Config.Interfaces;

namespace Yandex.Maps.Config
{
  internal class HeroismPrinterUrlProvider : PrinterUrlProvider
  {
    public HeroismPrinterUrlProvider(IConfigMediator configMediator)
      : base(configMediator)
    {
    }

    public override string GetRootStartupUrl() => "http://mobile.maps.heroism.yandex.ru/startup";

    public override string GetRootPrinterUrl() => this.GetUrlByHostType(HostTypes.tiles) + "/printer";
  }
}
