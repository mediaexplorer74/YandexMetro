// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TrafficTileZoomLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Traffic;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Media;
using Yandex.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [UsedImplicitly]
  internal class TrafficTileZoomLayer : TileZoomLayer<IJamTile>
  {
    private const double RenderScale = 1.0;
    private readonly IJamRender _jamRender;
    private readonly IViewportTileConverter _pixelTileConverter;
    private readonly IConfigMediator _configMediator;
    private readonly IBulkWriteableBitmapBuilder _bulkWritableBitmapBuilder;
    private readonly ITileRectangleBreaker _tileRectangleBreaker;
    private readonly IUiDispatcher _uiDispatcher;
    private readonly IDisplayTileSize _displayTileSize;
    private readonly object _renderLock = new object();

    public TrafficTileZoomLayer(
      byte zoom,
      [NotNull] TileLayer tileLayer,
      [NotNull] IJamRender jamRender,
      [NotNull] IViewportTileConverter pixelTileConverter,
      [NotNull] IConfigMediator configMediator,
      [NotNull] IBulkWriteableBitmapBuilder bulkWritableBitmapBuilder,
      [NotNull] ITileDrawManager tileDrawManager,
      [NotNull] ITileRectangleBreaker tileRectangleBreaker,
      [NotNull] IUiDispatcher uiDispatcher)
      : base(zoom, tileLayer, tileDrawManager)
    {
      if (jamRender == null)
        throw new ArgumentNullException(nameof (jamRender));
      if (pixelTileConverter == null)
        throw new ArgumentNullException(nameof (pixelTileConverter));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (bulkWritableBitmapBuilder == null)
        throw new ArgumentNullException(nameof (bulkWritableBitmapBuilder));
      if (tileRectangleBreaker == null)
        throw new ArgumentNullException(nameof (tileRectangleBreaker));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._jamRender = jamRender;
      this._pixelTileConverter = pixelTileConverter;
      this._configMediator = configMediator;
      this._bulkWritableBitmapBuilder = bulkWritableBitmapBuilder;
      this._tileRectangleBreaker = tileRectangleBreaker;
      this._uiDispatcher = uiDispatcher;
      this._displayTileSize = configMediator.DisplayTileSize;
    }

    protected override void OnDisplayTiles(IList<IJamTile> tiles)
    {
      IList<IJamTile> list1 = (IList<IJamTile>) tiles.Where<IJamTile>((Func<IJamTile, bool>) (tile => (int) tile.TileInfo.Zoom == (int) this.Zoom && tile.TileInfo.Layer == BaseLayers.trf)).ToList<IJamTile>();
      base.OnDisplayTiles(list1);
      this.DrawTiles(list1);
      IList<IJamTile> list2 = (IList<IJamTile>) list1.Where<IJamTile>((Func<IJamTile, bool>) (tile => tile.BitmapSource == null && tile.JamTracks != null)).ToList<IJamTile>();
      this.RenderTiles(list2);
      this.DrawTiles(list2);
    }

    private void DrawTiles(IList<IJamTile> readyTiles)
    {
      foreach (IJamTile tile in readyTiles.Where<IJamTile>((Func<IJamTile, bool>) (tile => tile.BitmapSource != null)))
        this.DrawTile(tile);
    }

    private void RenderTiles(IList<IJamTile> tiles)
    {
      foreach (IGrouping<IList<Track>, IJamTile> source in tiles.GroupBy<IJamTile, IList<Track>>((Func<IJamTile, IList<Track>>) (tile => tile.JamTracks)))
      {
        List<IJamTile> list1 = source.ToList<IJamTile>();
        foreach (IEnumerable<ITileInfo> rectangle in this._tileRectangleBreaker.GetRectangles(list1.Select<IJamTile, ITileInfo>((Func<IJamTile, ITileInfo>) (tile => tile.TileInfo))))
        {
          List<ITileInfo> rectangle1 = rectangle.ToList<ITileInfo>();
          List<IJamTile> list2 = list1.Where<IJamTile>((Func<IJamTile, bool>) (tile => rectangle1.Contains(tile.TileInfo))).ToList<IJamTile>();
          this.RenderRectangle(source.Key, list2);
        }
      }
    }

    private void RenderRectangle(IList<Track> jamTracks, List<IJamTile> tiles)
    {
      IJamTile jamTile = tiles.FirstOrDefault<IJamTile>();
      if (jamTile == null)
        return;
      byte zoom = jamTile.TileInfo.Zoom;
      Point tileTopLeftPoint1 = this._pixelTileConverter.GetTileTopLeftPoint(jamTile.TileInfo);
      Point tileTopLeftPoint2 = this._pixelTileConverter.GetTileTopLeftPoint(tiles.Last<IJamTile>().TileInfo);
      Point point = new Point(tileTopLeftPoint2.X + (double) this._displayTileSize.Width, tileTopLeftPoint2.Y + (double) this._displayTileSize.Height);
      int num1 = (int) (point.X - tileTopLeftPoint1.X);
      int num2 = (int) (point.Y - tileTopLeftPoint1.Y);
      int[] numArray = new int[num1 * num2];
      if (!this._jamRender.Render(jamTracks, zoom, numArray, num1, num2, (int) tileTopLeftPoint1.X, (int) tileTopLeftPoint1.Y, 1.0, this._configMediator.TwilightModeEnabled))
        return;
      this.CutJamsToTiles((IList<IJamTile>) tiles, tileTopLeftPoint1, num1, num2, numArray);
    }

    private bool CutJamsToTiles(
      IList<IJamTile> tiles,
      Point firstTopLeft,
      int width,
      int height,
      int[] pixels)
    {
      bool tiles1 = false;
      while (this.OperationMode != OperationMode.Full)
      {
        if (this.OperationMode == OperationMode.Disabled)
          return false;
        lock (this._renderLock)
          Monitor.Wait(this._renderLock);
      }
      IList<WriteableBitmap> writableBitmaps = this._bulkWritableBitmapBuilder.CreateWritableBitmaps(tiles.Count);
      int num = 0;
      foreach (IJamTile tile in (IEnumerable<IJamTile>) tiles)
      {
        while (this.OperationMode != OperationMode.Full)
        {
          if (this.OperationMode == OperationMode.Disabled)
            return false;
          lock (this._renderLock)
            Monitor.Wait(this._renderLock);
        }
        WriteableBitmap writeableBitmap = writableBitmaps[num++];
        uint width1 = this._configMediator.DisplayTileSize.Width;
        uint height1 = this._configMediator.DisplayTileSize.Height;
        int[] pixels1 = writeableBitmap.Pixels;
        tile.BitmapSource = (object) writeableBitmap;
        Point tileTopLeftPoint = this._pixelTileConverter.GetTileTopLeftPoint(tile.TileInfo);
        Rect sourceRect = new Rect(tileTopLeftPoint.X - firstTopLeft.X, tileTopLeftPoint.Y - firstTopLeft.Y, (double) width1, (double) height1);
        if (sourceRect.Right > (double) width || sourceRect.Bottom > (double) height || sourceRect.Left < 0.0 || sourceRect.Top < 0.0)
          return false;
        Blitting.Blit(pixels1, (int) width1, (int) height1, new Rect(0.0, 0.0, (double) width1, (double) height1), pixels, width, height, sourceRect, false);
        tiles1 = true;
      }
      return tiles1;
    }
  }
}
