// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.IdSortComparer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class IdSortComparer : IComparer<ITile>
  {
    public int Compare(ITile x, ITile y) => x.TileInfo.IdSort.CompareTo(x.TileInfo.IdSort);
  }
}
