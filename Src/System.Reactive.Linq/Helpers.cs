// Decompiled with JetBrains decompiler
// Type: System.Reactive.Helpers
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive
{
  internal static class Helpers
  {
    public static int? GetLength<T>(IEnumerable<T> source)
    {
      switch (source)
      {
        case T[] objArray:
          return new int?(objArray.Length);
        case IList<T> objList:
          return new int?(objList.Count);
        default:
          return new int?();
      }
    }

    public static IObservable<T> Unpack<T>(IObservable<T> source)
    {
      bool flag;
      do
      {
        flag = false;
        if (source is IEvaluatableObservable<T> evaluatableObservable)
        {
          source = evaluatableObservable.Eval();
          flag = true;
        }
      }
      while (flag);
      return source;
    }
  }
}
