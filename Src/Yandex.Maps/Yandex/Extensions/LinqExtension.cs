// Decompiled with JetBrains decompiler
// Type: Yandex.Extensions.LinqExtension
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Extensions
{
  internal static class LinqExtension
  {
    public static TResult MinOrDefault<TSource, TResult>(
      [NotNull] this IEnumerable<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !source.Any<TSource>() ? default (TResult) : source.Min<TSource, TResult>(selector);
    }

    public static TResult MaxOrDefault<TSource, TResult>(
      [NotNull] this IEnumerable<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !source.Any<TSource>() ? default (TResult) : source.Max<TSource, TResult>(selector);
    }
  }
}
