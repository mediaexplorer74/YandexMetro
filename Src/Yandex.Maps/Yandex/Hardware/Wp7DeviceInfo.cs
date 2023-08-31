// Decompiled with JetBrains decompiler
// Type: Yandex.Hardware.Wp7DeviceInfo
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Info;
using System;

namespace Yandex.Hardware
{
  internal class Wp7DeviceInfo : IDeviceInfo
  {
    public const string DeviceManufacturerProperty = "DeviceManufacturer";
    public const string DeviceNameProperty = "DeviceName";

    public string GetDeviceManufacturer()
    {
      string empty = string.Empty;
      object obj;
      if (DeviceExtendedProperties.TryGetValue("DeviceManufacturer", ref obj))
        empty = obj.ToString();
      return empty;
    }

    public string GetDeviceName()
    {
      string empty = string.Empty;
      object obj;
      if (DeviceExtendedProperties.TryGetValue("DeviceName", ref obj))
        empty = obj.ToString();
      return empty;
    }

    public uint GetScreenWidth() => 480;

    public string Platform => Environment.OSVersion.Platform.ToString();

    public string OSVersion => Environment.OSVersion.Version.ToString();

    public uint GetScreenHeight() => 800;
  }
}
