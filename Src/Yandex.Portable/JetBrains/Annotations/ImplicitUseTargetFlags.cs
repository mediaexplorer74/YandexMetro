// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ImplicitUseTargetFlags
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace JetBrains.Annotations
{
  [Flags]
  public enum ImplicitUseTargetFlags
  {
    Default = 1,
    Itself = Default, // 0x00000001
    Members = 2,
    WithMembers = Members | Itself, // 0x00000003
  }
}
