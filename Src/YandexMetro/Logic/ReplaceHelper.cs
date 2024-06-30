// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.ReplaceHelper
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

namespace Yandex.Metro.Logic
{
  public static class ReplaceHelper
  {
    public static string ReplaceChar(string value) => string.IsNullOrWhiteSpace(value) ? (string) null : value.ToLower().Replace("ё", "е").Replace("й", "и").Replace("ў", "у").Replace("і", "и");
  }
}
