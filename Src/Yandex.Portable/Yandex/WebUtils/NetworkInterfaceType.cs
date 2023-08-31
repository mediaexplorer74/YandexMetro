// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.NetworkInterfaceType
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.WebUtils
{
  public enum NetworkInterfaceType
  {
    None = 0,
    Unknown = 1,
    Ethernet = 6,
    TokenRing = 9,
    Fddi = 15, // 0x0000000F
    BasicIsdn = 20, // 0x00000014
    PrimaryIsdn = 21, // 0x00000015
    Ppp = 23, // 0x00000017
    Loopback = 24, // 0x00000018
    Ethernet3Megabit = 26, // 0x0000001A
    Slip = 28, // 0x0000001C
    Atm = 37, // 0x00000025
    GenericModem = 48, // 0x00000030
    FastEthernetT = 62, // 0x0000003E
    Isdn = 63, // 0x0000003F
    FastEthernetFx = 69, // 0x00000045
    Wireless80211 = 71, // 0x00000047
    AsymmetricDsl = 94, // 0x0000005E
    RateAdaptDsl = 95, // 0x0000005F
    SymmetricDsl = 96, // 0x00000060
    VeryHighSpeedDsl = 97, // 0x00000061
    IPOverAtm = 114, // 0x00000072
    GigabitEthernet = 117, // 0x00000075
    Tunnel = 131, // 0x00000083
    MultiRateSymmetricDsl = 143, // 0x0000008F
    HighPerformanceSerialBus = 144, // 0x00000090
    MobileBroadbandGsm = 145, // 0x00000091
    MobileBroadbandCdma = 146, // 0x00000092
  }
}
