// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Helpers.GeocodeManagerProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Ioc;
using Yandex.Maps.Geocoding.Interfaces;
using Yandex.Maps.IoC;

namespace Yandex.Maps.Helpers
{
  [ComVisible(false)]
  public static class GeocodeManagerProvider
  {
    public static IGeocodeManager GetGeocodeManager() => IocSingleton<ControlsIocInitializer>.Resolve<IGeocodeManager>();
  }
}
