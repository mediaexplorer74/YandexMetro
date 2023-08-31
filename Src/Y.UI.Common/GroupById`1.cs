// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.GroupById`1
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Y.UI.Common
{
  public class GroupById<T> : ObservableCollection<T>
  {
    public GroupById(int id, IEnumerable<T> items)
      : base(items)
    {
      this.Id = id;
    }

    public override bool Equals(object obj) => obj is GroupById<T> groupById && this.Id.Equals(groupById.Id);

    public int Id { get; set; }
  }
}
