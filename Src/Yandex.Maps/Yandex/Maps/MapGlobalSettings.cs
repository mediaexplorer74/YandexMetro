// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapGlobalSettings
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Ioc;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.IoC;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public sealed class MapGlobalSettings
  {
    private static readonly IConfigMediator ConfigMediator = IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>();
    private static readonly MapGlobalSettings _instance = new MapGlobalSettings();
    private bool _twilightModeEnabled;

    public static MapGlobalSettings Instance => MapGlobalSettings._instance;

    public string ApiKey
    {
      get => MapGlobalSettings.ConfigMediator.ApiKey;
      set
      {
        if (!string.IsNullOrEmpty(MapGlobalSettings.ConfigMediator.ApiKey))
        {
          int num = MapGlobalSettings.ConfigMediator.ApiKey != value ? 1 : 0;
        }
        MapGlobalSettings.ConfigMediator.ApiKey = value;
      }
    }

    public bool EnableLocationService
    {
      get => MapGlobalSettings.ConfigMediator.EnableLocationService;
      set => MapGlobalSettings.ConfigMediator.EnableLocationService = value;
    }

    internal bool CollectJamInformation
    {
      get => MapGlobalSettings.ConfigMediator.CollectJamInformation;
      set => MapGlobalSettings.ConfigMediator.CollectJamInformation = value;
    }

    public double TilesStretchFactor
    {
      get => MapGlobalSettings.ConfigMediator.TilesStretchFactor;
      set => MapGlobalSettings.ConfigMediator.TilesStretchFactor = value;
    }

    public bool TwilightModeEnabled
    {
      get => MapGlobalSettings.ConfigMediator.TwilightModeEnabled;
      set => MapGlobalSettings.ConfigMediator.TwilightModeEnabled = value;
    }
  }
}
