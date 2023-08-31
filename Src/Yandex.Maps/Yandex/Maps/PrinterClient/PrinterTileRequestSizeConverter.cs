// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterTileRequestSizeConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API;
using Yandex.Maps.PrinterClient.Interfaces;

namespace Yandex.Maps.PrinterClient
{
  internal class PrinterTileRequestSizeConverter : IPrinterTileRequestSizeConverter
  {
    public TileType ConvertSize(int sizeInPixels)
    {
      switch (sizeInPixels)
      {
        case 128:
          return TileType.@default;
        case 256:
          return TileType.default2x;
        default:
          throw new ArgumentException("sizeInPixels does not match predefined tile sizes");
      }
    }
  }
}
