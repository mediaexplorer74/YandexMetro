// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.FastScheme.ListExtensions
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;

namespace Yandex.Metro.Logic.FastScheme
{
  public static class ListExtensions
  {
    public static List<T> Copy<T>(this List<T> list)
    {
      T[] objArray = new T[list.Count];
      list.CopyTo(objArray);
      return new List<T>((IEnumerable<T>) objArray);
    }
  }
}
