// Decompiled with JetBrains decompiler
// Type: Yandex.PAL.IO.FileShare
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.PAL.IO
{
  [Flags]
  internal enum FileShare
  {
    None = 0,
    Read = 1,
    Write = 2,
    ReadWrite = Write | Read, // 0x00000003
    Delete = 4,
    Inheritable = 16, // 0x00000010
  }
}
