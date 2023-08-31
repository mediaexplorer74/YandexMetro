// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Interfaces.IPrinterTileRequestSizeConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.API;

namespace Yandex.Maps.PrinterClient.Interfaces
{
  internal interface IPrinterTileRequestSizeConverter
  {
    TileType ConvertSize(int sizeInPixels);
  }
}
