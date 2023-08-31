// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ImplicitUseTargetFlags
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace JetBrains.Annotations
{
  [Flags]
  internal enum ImplicitUseTargetFlags
  {
    Default = 1,
    Itself = Default, // 0x00000001
    Members = 2,
    WithMembers = Members | Itself, // 0x00000003
  }
}
