// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterTileHeaderReader
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.PrinterClient.Response;
using Yandex.Maps.PrinterClient.Tiles;

namespace Yandex.Maps.PrinterClient
{
  [UsedImplicitly]
  internal class PrinterTileHeaderReader : IPrinterTileHeaderReader
  {
    public PrinterTileResponseHeader ReadPrinterTileResponseHeader(
      BinaryReader data,
      IList<TileRequest> requestedTiles,
      uint protocolVersion)
    {
      PrinterTileResponseHeader tileResponseHeader = data != null ? new PrinterTileResponseHeader()
      {
        TileCount = data.ReadUInt16()
      } : throw new ArgumentNullException(nameof (data));
      data.ReadBytes(2);
      tileResponseHeader.Tiles = new PrinterTileInfo[(int) tileResponseHeader.TileCount];
      for (int index = 0; index < (int) tileResponseHeader.TileCount; ++index)
      {
        PrinterTileInfo printerTileInfo = new PrinterTileInfo((ITileInfo) requestedTiles[index])
        {
          StatusCode = data.ReadUInt16(),
          DataLength = protocolVersion >= 2U ? data.ReadUInt32() : (uint) data.ReadUInt16(),
          DataOffset = data.ReadUInt32()
        };
        tileResponseHeader.Tiles[index] = printerTileInfo;
      }
      return tileResponseHeader;
    }
  }
}
