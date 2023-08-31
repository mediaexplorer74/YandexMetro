// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.TileFactory
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class TileFactory : ITileFactory<ITile>
  {
    public ITile CreateTile(ITileInfo tileInfo, TileStatus tileStatus) => tileInfo != null ? (ITile) new Tile(tileInfo, tileStatus) : throw new ArgumentNullException(nameof (tileInfo));

    public ITile CreateTile(
      ITileInfo tileInfo,
      byte[] bytes,
      int dataOffset,
      int dataLength,
      TileStatus tileStatus)
    {
      ITile tile = this.CreateTile(tileInfo, tileStatus);
      tile.Bytes = bytes;
      tile.DataLength = dataLength;
      tile.DataOffset = dataOffset;
      return tile;
    }

    public ITile CreateTile(
      int x,
      int y,
      byte zoom,
      BaseLayers layers,
      byte[] bytes,
      int dataOffset,
      int dataLength,
      TileStatus tileStatus)
    {
      return this.CreateTile((ITileInfo) new TileInfo(x, y, zoom, layers), bytes, dataOffset, dataLength, tileStatus);
    }

    public ITile CopyTile(ITile tile, ITileInfo newTileInfo) => (ITile) new Tile(newTileInfo, tile.Status)
    {
      Records = tile.Records,
      BitmapSource = tile.BitmapSource,
      Bytes = tile.Bytes,
      DataLength = tile.DataLength,
      DataOffset = tile.DataOffset,
      HeaderLength = tile.HeaderLength,
      Metadata = tile.Metadata,
      Scale = tile.Scale
    };
  }
}
