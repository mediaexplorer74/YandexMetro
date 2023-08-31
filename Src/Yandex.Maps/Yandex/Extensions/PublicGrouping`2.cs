// Decompiled with JetBrains decompiler
// Type: Yandex.Extensions.PublicGrouping`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Extensions
{
  internal class PublicGrouping<TKey, TElement> : 
    IGrouping<TKey, TElement>,
    IEnumerable<TElement>,
    IEnumerable
  {
    private readonly IGrouping<TKey, TElement> _internalGrouping;

    public PublicGrouping(IGrouping<TKey, TElement> internalGrouping) => this._internalGrouping = internalGrouping;

    public override bool Equals(object obj) => obj is PublicGrouping<TKey, TElement> publicGrouping && this.Key.Equals((object) publicGrouping.Key);

    public override int GetHashCode() => this.Key.GetHashCode();

    public TKey Key => this._internalGrouping.Key;

    public IEnumerator<TElement> GetEnumerator() => this._internalGrouping.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._internalGrouping.GetEnumerator();
  }
}
