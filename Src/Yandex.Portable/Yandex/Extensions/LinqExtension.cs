// Decompiled with JetBrains decompiler
// Type: Yandex.Extensions.LinqExtension
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Extensions
{
  public static class LinqExtension
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
