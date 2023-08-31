// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.MobileYandexMapsZoomInfo
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class MobileYandexMapsZoomInfo : IZoomInfo
  {
    private const double DefaultTileSize = 128.0;
    private const int MaxVisibleZoomField = 17;
    private readonly IConfigMediator _configMediator;
    private readonly double _tileMult;
    private readonly long _tilesAt0Zoom;

    public MobileYandexMapsZoomInfo(
      [NotNull] ITileSize tileSize,
      [NotNull] IConfigMediator configMediator,
      byte maxZoom)
    {
      if (tileSize == null)
        throw new ArgumentNullException(nameof (tileSize));
      this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));
      this.MaxZoom = maxZoom;
      this._tileMult = 128.0 / (double) tileSize.Height;
      this._tilesAt0Zoom = (long) (4.0 * this._tileMult * this._tileMult);
    }

    public ulong GetWidthInPixels(byte zoom) => (ulong) ((double) ((long) (1 << (int) zoom + 1) * (long) this._configMediator.DisplayTileSize.Width) * this._tileMult);

    public ulong GetHeightInPixels(byte zoom) => (ulong) ((double) ((long) (1 << (int) zoom + 1) * (long) this._configMediator.DisplayTileSize.Height) * this._tileMult);

    public uint GetWidthInTiles(byte zoom) => (uint) ((double) (1 << (int) zoom + 1) * this._tileMult);

    public uint GetHeightInTiles(byte zoom) => (uint) ((double) (1 << (int) zoom + 1) * this._tileMult);

    public long GetNumberOfTilesInZoom(byte zoom) => this._tilesAt0Zoom << ((int) zoom << 1);

    public byte MaxZoom { get; private set; }

    public byte MaxVisibleZoom => 17;

    public byte MinZoom => 0;
  }
}
