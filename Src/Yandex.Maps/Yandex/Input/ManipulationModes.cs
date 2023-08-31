// Decompiled with JetBrains decompiler
// Type: Yandex.Input.ManipulationModes
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Input
{
  [Flags]
  internal enum ManipulationModes : uint
  {
    None = 0,
    TranslateX = 1,
    TranslateY = 2,
    TranslateRailsX = 4,
    TranslateRailsY = 8,
    Rotate = 16, // 0x00000010
    Scale = 32, // 0x00000020
    TranslateInertia = 64, // 0x00000040
    RotateInertia = 128, // 0x00000080
    ScaleInertia = 256, // 0x00000100
    All = 65535, // 0x0000FFFF
    System = 65536, // 0x00010000
  }
}
