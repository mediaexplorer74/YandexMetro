// Decompiled with JetBrains decompiler
// Type: Clarity.Phone.Extensions.TransformCreationMode
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;

namespace Clarity.Phone.Extensions
{
  [Flags]
  public enum TransformCreationMode
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
