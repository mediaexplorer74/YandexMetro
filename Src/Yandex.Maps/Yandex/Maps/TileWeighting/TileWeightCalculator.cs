// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TileWeighting.TileWeightCalculator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Linq;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers;
using Yandex.Media;

namespace Yandex.Maps.TileWeighting
{
  internal class TileWeightCalculator : ITileWeightCalculator
  {
    private readonly IViewportTileConverter _viewportTileConverter;
    private readonly IDisplayTileSize _displayTileSize;
    private readonly IZoomLevelConverter _zoomLevelConverter;

    public TileWeightCalculator(
      [NotNull] IViewportTileConverter viewportTileConverter,
      [NotNull] IConfigMediator configMediator,
      [NotNull] IZoomLevelConverter zoomLevelConverter)
    {
      if (viewportTileConverter == null)
        throw new ArgumentNullException(nameof (viewportTileConverter));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (zoomLevelConverter == null)
        throw new ArgumentNullException(nameof (zoomLevelConverter));
      this._viewportTileConverter = viewportTileConverter;
      this._displayTileSize = configMediator.DisplayTileSize;
      this._zoomLevelConverter = zoomLevelConverter;
    }

    public ITileWeight GetTileWeight(ITileInfo tile, ViewportRect viewportRect)
    {
      Rect rect = this.ScaleTileRect(this.GetTileRect(tile), tile.Zoom, viewportRect.ZoomLevel);
      Point rectCenter1 = TileWeightCalculator.GetRectCenter(rect);
      Point rectCenter2 = TileWeightCalculator.GetRectCenter(viewportRect.Rect);
      return (ITileWeight) new TileWeight()
      {
        IsCurrentLayerTile = ((int) tile.Zoom == (int) ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel)),
        IsVisibleOnScreen = this._viewportTileConverter.ViewportRectToTiles(viewportRect, tile.Zoom).Any<ITileInfo>((Func<ITileInfo, bool>) (x => x.EqualsCoordinates(tile))),
        AreaSizeOnScreen = (rect.Width * rect.Height),
        DistanceToScreenCenter = (Math.Abs(rectCenter1.X - rectCenter2.X) + Math.Abs(rectCenter1.Y - rectCenter2.Y))
      };
    }

    private Rect GetTileRect(ITileInfo tileInfo)
    {
      Point tileTopLeftPoint = this._viewportTileConverter.GetTileTopLeftPoint(tileInfo);
      return new Rect(tileTopLeftPoint.X, tileTopLeftPoint.Y, (double) this._displayTileSize.Width, (double) this._displayTileSize.Height);
    }

    private Rect ScaleTileRect(Rect tileRect, byte fromZoom, double toZoomLevel)
    {
      double stretchFactor = this._zoomLevelConverter.ZoomLevelToStretchFactor(toZoomLevel, fromZoom);
      return new Rect(tileRect.X * stretchFactor, tileRect.Y * stretchFactor, tileRect.Width * stretchFactor, tileRect.Height * stretchFactor);
    }

    private static Point GetRectCenter(Rect rect) => new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
  }
}
