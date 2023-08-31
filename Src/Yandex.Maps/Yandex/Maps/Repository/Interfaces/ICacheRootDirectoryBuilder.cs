// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.ICacheRootDirectoryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Text;
using Yandex.Maps.API;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface ICacheRootDirectoryBuilder
  {
    [NotNull]
    string CacheRoot { get; }

    [NotNull]
    StringBuilder GetRootDirectoryWithTileType();

    [NotNull]
    StringBuilder GetRootDirectoryWithTileType(uint version);

    [NotNull]
    StringBuilder GetRootDirectoryWithTileType(
      short cacheFileScale,
      TileType tileType,
      uint version);
  }
}
