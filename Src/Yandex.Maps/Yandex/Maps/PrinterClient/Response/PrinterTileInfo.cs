// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Response.PrinterTileInfo
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.PrinterClient.Response
{
  internal class PrinterTileInfo : TileInfo
  {
    public ushort StatusCode;
    public uint DataLength;
    public uint DataOffset;

    public PrinterTileInfo(int x, int y, byte zoom, BaseLayers layer)
      : base(x, y, zoom, layer)
    {
    }

    public PrinterTileInfo(ITileInfo requestedTile)
      : base(requestedTile)
    {
    }
  }
}
