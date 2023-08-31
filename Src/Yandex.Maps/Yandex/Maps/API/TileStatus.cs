// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.TileStatus
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.Maps.API
{
  [Flags]
  [ComVisible(true)]
  public enum TileStatus
  {
    Ok = 0,
    NeedsReload = 4,
    NotModified = 8,
    NotFound = 16, // 0x00000010
    Error = 1,
    CannotLoad = Error | NeedsReload, // 0x00000005
  }
}
