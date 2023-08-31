// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Converters.TileInfoNormalizer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API.Converters
{
  [ComVisible(true)]
  public class TileInfoNormalizer : ITileInfoNormalizer
  {
    private readonly IZoomInfo _zoomInfo;

    public TileInfoNormalizer(IZoomInfo zoomInfo) => this._zoomInfo = zoomInfo != null ? zoomInfo : throw new ArgumentNullException(nameof (zoomInfo));

    [NotNull]
    public ITileInfo ConvertToFiniteCoordinates(ITileInfo tileInfo)
    {
      uint widthInTiles = this._zoomInfo.GetWidthInTiles(tileInfo.Zoom);
      uint heightInTiles = this._zoomInfo.GetHeightInTiles(tileInfo.Zoom);
      return (ITileInfo) new TileInfo((int) (((long) tileInfo.X % (long) widthInTiles + (long) widthInTiles) % (long) widthInTiles), (int) (((long) tileInfo.Y % (long) heightInTiles + (long) heightInTiles) % (long) heightInTiles), tileInfo.Zoom, tileInfo.Layer)
      {
        MapVersion = tileInfo.MapVersion,
        Checksum = tileInfo.Checksum
      };
    }

    public bool IsNormal(ITileInfo tileInfo)
    {
      uint widthInTiles = this._zoomInfo.GetWidthInTiles(tileInfo.Zoom);
      return tileInfo.X >= 0 && tileInfo.Y >= 0 && tileInfo.Zoom >= (byte) 0 && (int) tileInfo.Zoom < (int) this._zoomInfo.MaxZoom && (long) tileInfo.X < (long) widthInTiles && (long) tileInfo.Y < (long) widthInTiles;
    }
  }
}
