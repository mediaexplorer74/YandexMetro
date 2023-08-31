// Decompiled with JetBrains decompiler
// Type: Yandex.Extensions.EnumerableExtensions
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Observable;

namespace Yandex.Extensions
{
  internal static class EnumerableExtensions
  {
    public static IObservable<T> TrickleOnDispatcher<T>(
      this IEnumerable<T> source,
      byte groupSize,
      TimeSpan? trickleInterval = null)
    {
      return source.ToObservable<T>().TrickleOnDispatcher<T>(groupSize, trickleInterval);
    }
  }
}
