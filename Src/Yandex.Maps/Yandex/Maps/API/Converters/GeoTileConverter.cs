// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Converters.GeoTileConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.API.Converters
{
  [ComVisible(true)]
  public class GeoTileConverter : IGeoTileConverter
  {
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly IConfigMediator _configMediator;

    public GeoTileConverter(IGeoPixelConverter geoPixelConverter, IConfigMediator configMediator)
    {
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      this._geoPixelConverter = geoPixelConverter;
      this._configMediator = configMediator;
    }

    public void GetTileInfoGeoCoordinates(
      ITileInfo tileInfo,
      out GeoCoordinate topLeft,
      out GeoCoordinate bottomRight)
    {
      long x = (long) this._configMediator.DisplayTileSize.Width * (long) tileInfo.X;
      long y = (long) this._configMediator.DisplayTileSize.Height * (long) tileInfo.Y;
      topLeft = this._geoPixelConverter.ZoomPointToCoordinates(new Point((double) x, (double) y), tileInfo.Zoom);
      bottomRight = this._geoPixelConverter.ZoomPointToCoordinates(new Point((double) (x + (long) this._configMediator.DisplayTileSize.Width - 1L), (double) (y + (long) this._configMediator.DisplayTileSize.Height - 1L)), tileInfo.Zoom);
    }
  }
}
