// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Converters.ViewportTileConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.API.Converters
{
  [ComVisible(true)]
  public class ViewportTileConverter : IViewportTileConverter
  {
    private readonly IConfigMediator _configMediator;
    private readonly IZoomLevelConverter _zoomLevelConverter;

    public ViewportTileConverter(
      IConfigMediator configMediator,
      IZoomInfo zoomInfo,
      IZoomLevelConverter zoomLevelConverter)
    {
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (zoomInfo == null)
        throw new ArgumentNullException(nameof (zoomInfo));
      if (zoomLevelConverter == null)
        throw new ArgumentNullException(nameof (zoomLevelConverter));
      this._configMediator = configMediator;
      this._zoomLevelConverter = zoomLevelConverter;
    }

    public Point GetTileTopLeftPoint(ITileInfo tileInfo) => new Point((double) ((long) this._configMediator.DisplayTileSize.Width * (long) tileInfo.X), (double) ((long) this._configMediator.DisplayTileSize.Height * (long) tileInfo.Y));

    public IEnumerable<ITileInfo> GetTilesByArea(Rect visibleArea, byte zoom)
    {
      if (visibleArea.Width > 0.0 && visibleArea.Height > 0.0)
      {
        ITileInfo bottomRightTile;
        ITileInfo topLeftTile;
        this.GetCornerTiles(visibleArea, zoom, out bottomRightTile, out topLeftTile);
        int x1 = topLeftTile.X;
        int x2 = bottomRightTile.X;
        int y1 = topLeftTile.Y;
        int y2 = bottomRightTile.Y;
        for (int y = y1; y <= y2; ++y)
        {
          for (int x = x1; x <= x2; ++x)
            yield return (ITileInfo) new TileInfo(x, y, zoom, BaseLayers.none);
        }
      }
    }

    public Rect GetTilesArea(ViewportRect visibleArea, byte zoom)
    {
      if (visibleArea.Rect.Width <= 0.0 || visibleArea.Rect.Height <= 0.0)
        return new Rect();
      double stretchFactor = this._zoomLevelConverter.ZoomLevelToStretchFactor(visibleArea.ZoomLevel, zoom);
      double num = 1.0 / stretchFactor;
      ITileInfo bottomRightTile;
      ITileInfo topLeftTile;
      this.GetCornerTiles(new Rect(visibleArea.Rect.X * num, visibleArea.Rect.Y * num, visibleArea.Rect.Width * num, visibleArea.Rect.Height * num), zoom, out bottomRightTile, out topLeftTile);
      Point tileTopLeftPoint = this.GetTileTopLeftPoint(topLeftTile);
      return new Rect(tileTopLeftPoint.X * stretchFactor, tileTopLeftPoint.Y * stretchFactor, (double) ((long) (bottomRightTile.X - topLeftTile.X + 1) * (long) this._configMediator.DisplayTileSize.Width) * stretchFactor, (double) ((long) (bottomRightTile.Y - topLeftTile.Y + 1) * (long) this._configMediator.DisplayTileSize.Height) * stretchFactor);
    }

    public ITileInfo GetTileByPoint(Point point, byte zoom) => (ITileInfo) new TileInfo((int) (point.X / (double) this._configMediator.DisplayTileSize.Width) + (point.X >= 0.0 ? 0 : -1), (int) (point.Y / (double) this._configMediator.DisplayTileSize.Height) + (point.Y >= 0.0 ? 0 : -1), zoom, BaseLayers.none);

    public Point ZoomPoint(Point point, uint oldZoom, byte zoom) => new Point(point.X * Math.Pow(2.0, (double) ((int) zoom - (int) oldZoom)), point.Y * Math.Pow(2.0, (double) ((int) zoom - (int) oldZoom)));

    public IEnumerable<ITileInfo> ViewportRectToTiles(ViewportRect viewportRect, byte zoom)
    {
      double num = 1.0 / this._zoomLevelConverter.ZoomLevelToStretchFactor(viewportRect.ZoomLevel, zoom);
      return this.GetTilesByArea(new Rect(viewportRect.Rect.X * num, viewportRect.Rect.Y * num, viewportRect.Rect.Width * num, viewportRect.Rect.Height * num), zoom);
    }

    private void GetCornerTiles(
      Rect visibleArea,
      byte zoom,
      out ITileInfo bottomRightTile,
      out ITileInfo topLeftTile)
    {
      Rect rect = visibleArea;
      --rect.Width;
      --rect.Height;
      double y1 = rect.Y;
      double y2 = rect.Y + rect.Height;
      topLeftTile = this.GetTileByPoint(new Point(rect.X, y1), zoom);
      bottomRightTile = this.GetTileByPoint(new Point(rect.X + rect.Width, y2), zoom);
    }
  }
}
