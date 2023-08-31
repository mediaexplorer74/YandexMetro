// Decompiled with JetBrains decompiler
// Type: Clarity.Phone.Extensions.TransformCreationMode
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Clarity.Phone.Extensions
{
  [Flags]
  internal enum TransformCreationMode
  {
    None = 0,
    Create = 1,
    AddToGroup = 2,
    CombineIntoGroup = 4,
    IgnoreIdentityMatrix = 8,
    CreateOrAddAndIgnoreMatrix = IgnoreIdentityMatrix | AddToGroup | Create, // 0x0000000B
    Default = CreateOrAddAndIgnoreMatrix, // 0x0000000B
  }
}
