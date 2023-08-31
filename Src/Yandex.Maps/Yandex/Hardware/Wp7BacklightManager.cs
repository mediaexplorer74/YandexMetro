// Decompiled with JetBrains decompiler
// Type: Yandex.Hardware.Wp7BacklightManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Shell;
using System;
using Yandex.Hardware.Interfaces;

namespace Yandex.Hardware
{
  internal class Wp7BacklightManager : IBacklightManager
  {
    private BackLightMode _mode;

    public BackLightMode Mode
    {
      get => this._mode;
      set
      {
        this._mode = value;
        switch (this._mode)
        {
          case BackLightMode.SystemDefaults:
            PhoneApplicationService.Current.UserIdleDetectionMode = (IdleDetectionMode) 0;
            break;
          case BackLightMode.AlwaysOn:
            PhoneApplicationService.Current.UserIdleDetectionMode = (IdleDetectionMode) 1;
            break;
          case BackLightMode.AlwaysOff:
            throw new NotSupportedException("BackLightMode.AlwaysOff backlight mode is not supported on WP7.");
          default:
            throw new NotSupportedException("Mode value is not handled.");
        }
      }
    }

    public BackLightState BackLightState { get; set; }
  }
}
