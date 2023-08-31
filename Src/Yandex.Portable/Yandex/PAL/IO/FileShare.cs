// Decompiled with JetBrains decompiler
// Type: Yandex.PAL.IO.FileShare
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.PAL.IO
{
  [Flags]
  public enum FileShare
  {
    None = 0,
    Read = 1,
    Write = 2,
    ReadWrite = Write | Read, // 0x00000003
    Delete = 4,
    Inheritable = 16, // 0x00000010
  }
}
