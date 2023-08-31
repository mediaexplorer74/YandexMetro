// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.ITileFactory`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface ITileFactory<T> where T : ITile
  {
    T CreateTile(ITileInfo tileInfo, TileStatus tileStatus);

    T CreateTile(
      ITileInfo tileInfo,
      byte[] bytes,
      int dataOffset,
      int dataLength,
      TileStatus tileStatus);

    T CreateTile(
      int x,
      int y,
      byte zoom,
      BaseLayers layers,
      byte[] bytes,
      int dataOffset,
      int dataLength,
      TileStatus tileStatus);

    T CopyTile(T tile, ITileInfo newTileInfo);
  }
}
