// Decompiled with JetBrains decompiler
// Type: Y.Common.Extensions.IEnumExtensions
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Y.Common.Extensions
{
  public static class IEnumExtensions
  {
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> iEnumerable)
    {
      ObservableCollection<T> observableCollection = new ObservableCollection<T>();
      foreach (T i in iEnumerable)
        observableCollection.Add(i);
      return observableCollection;
    }

    public static void Each<T>(this IEnumerable<T> target, Action<T> action)
    {
      if (target == null)
        return;
      foreach (T obj in target)
        action(obj);
    }
  }
}
