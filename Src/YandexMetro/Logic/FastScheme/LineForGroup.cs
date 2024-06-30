// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.FastScheme.LineForGroup
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using Y.Metro.ServiceLayer.FastScheme;

namespace Yandex.Metro.Logic.FastScheme
{
  public struct LineForGroup : IComparable<LineForGroup>
  {
    public string Name { get; set; }

    public MetroColor Color { get; set; }

    public int CompareTo(LineForGroup other) => string.CompareOrdinal(this.Name, other.Name);
  }
}
