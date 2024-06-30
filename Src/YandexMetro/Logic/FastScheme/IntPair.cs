// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.FastScheme.IntPair
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

namespace Yandex.Metro.Logic.FastScheme
{
  public static class IntPair
  {
    public static int Hash(int a, int b) => a >= b ? (b << 16) + a : (a << 16) + b;
  }
}
