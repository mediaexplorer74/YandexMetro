// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.BaseLayers
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.Maps.API
{
  [Flags]
  [ComVisible(true)]
  public enum BaseLayers
  {
    none = 0,
    map = 1,
    sat = 2,
    skl = 4,
    pmap = 8,
    pskl = 16, // 0x00000010
    trf = 32, // 0x00000020
    mrks = 64, // 0x00000040
    ftrs = 128, // 0x00000080
    [Obsolete("Use: vmp2, vlb2, vrds, vpnt")] vmap = 256, // 0x00000100
    meta = 512, // 0x00000200
    vmp2 = 1024, // 0x00000400
    vlb2 = 2048, // 0x00000800
    vrds = 4096, // 0x00001000
    vpnt = 8192, // 0x00002000
  }
}
