// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Interfaces.IPrinterTileHeaderReader
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.IO;
using Yandex.Maps.PrinterClient.Response;
using Yandex.Maps.PrinterClient.Tiles;

namespace Yandex.Maps.PrinterClient.Interfaces
{
  internal interface IPrinterTileHeaderReader
  {
    PrinterTileResponseHeader ReadPrinterTileResponseHeader(
      BinaryReader data,
      IList<TileRequest> tiles,
      uint protocolVersion);
  }
}
