// Decompiled with JetBrains decompiler
// Type: System.Reactive.HashSet`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive
{
  internal class HashSet<T>
  {
    private readonly Dictionary<T, object> _set;
    private bool _hasNull;

    public HashSet(IEqualityComparer<T> comparer)
    {
      this._set = new Dictionary<T, object>(comparer);
      this._hasNull = false;
    }

    public bool Add(T value)
    {
      if ((object) value == null)
      {
        if (this._hasNull)
          return false;
        this._hasNull = true;
        return true;
      }
      if (this._set.ContainsKey(value))
        return false;
      this._set[value] = (object) null;
      return true;
    }
  }
}
