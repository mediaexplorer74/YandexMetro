// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterTileReader
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using Yandex.Common;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.PrinterClient.Response;
using Yandex.Net;

namespace Yandex.Maps.PrinterClient
{
  [UsedImplicitly]
  internal class PrinterTileReader : IPrinterTileReader
  {
    private readonly ITileFactory<ITile> _tileFactory;

    public PrinterTileReader(ITileFactory<ITile> tileFactory) => this._tileFactory = tileFactory != null ? tileFactory : throw new ArgumentNullException(nameof (tileFactory));

    [CanBeNull]
    public ITile ReadTile(BinaryReader binaryReader, PrinterTileInfo printerTile)
    {
      switch ((HttpStatusCode) printerTile.StatusCode)
      {
        case HttpStatusCode.OK:
          try
          {
            int dataLength = (int) printerTile.DataLength;
            byte[] bytes = binaryReader.ReadBytes(dataLength);
            return this._tileFactory.CreateTile((ITileInfo) printerTile, bytes, 0, dataLength, TileStatus.Ok);
          }
          catch (Exception ex)
          {
            Logger.TrackException(ex);
            return (ITile) null;
          }
        case HttpStatusCode.NotModified:
          return this._tileFactory.CreateTile((ITileInfo) printerTile, TileStatus.NotModified);
        case HttpStatusCode.RequestTimeout:
          return this._tileFactory.CreateTile((ITileInfo) printerTile, TileStatus.NeedsReload);
        default:
          return (ITile) null;
      }
    }
  }
}
