// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.BitmapTileZoomLayer`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Common;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.TwilightMode;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  internal class BitmapTileZoomLayer<T> : TileZoomLayer<T> where T : ITile
  {
    private readonly ITileRenderPreprocessor _tileRenderPreprocessor;
    private readonly IBitmapFactory _bitmapFactory;
    private readonly IUiDispatcher _uiDispatcher;

    public BitmapTileZoomLayer(
      byte zoom,
      ITileDrawManager tileDrawManager,
      TileLayer tileLayer,
      [NotNull] ITileRenderPreprocessor tileRenderPreprocessor,
      [NotNull] IBitmapFactory bitmapFactory,
      [NotNull] IUiDispatcher uiDispatcher)
      : base(zoom, tileLayer, tileDrawManager)
    {
      if (tileRenderPreprocessor == null)
        throw new ArgumentNullException(nameof (tileRenderPreprocessor));
      if (bitmapFactory == null)
        throw new ArgumentNullException(nameof (bitmapFactory));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._tileRenderPreprocessor = tileRenderPreprocessor;
      this._bitmapFactory = bitmapFactory;
      this._uiDispatcher = uiDispatcher;
    }

    protected override void OnDisplayTiles(IList<T> readyTiles)
    {
      T[] array = readyTiles.Where<T>((Func<T, bool>) (tile => (int) tile.TileInfo.Zoom == (int) this.Zoom)).ToArray<T>();
      base.OnDisplayTiles((IList<T>) array);
      IEnumerable<IGrouping<bool, T>> groupings = ((IEnumerable<T>) array).GroupBy<T, bool>((Func<T, bool>) (tile => tile.BitmapSource == null && tile.TileInfo.Layer != BaseLayers.meta && tile.Records != null && tile.Records.Length > 0));
      IList<BitmapTileZoomLayer<T>.PreprocessedTile> preprocessedTiles = (IList<BitmapTileZoomLayer<T>.PreprocessedTile>) new List<BitmapTileZoomLayer<T>.PreprocessedTile>();
      foreach (IGrouping<bool, T> grouping in groupings)
      {
        if (grouping.Key)
        {
          foreach (T obj in (IEnumerable<T>) grouping)
          {
            TileRecord record = obj.Records[0];
            byte[] dest;
            int destOffset;
            int destLength;
            this._tileRenderPreprocessor.PreprocessRenderedData(obj.Bytes, (int) ((long) (obj.DataOffset + obj.HeaderLength) + (long) record.DataOffset), (int) record.DataLength, out dest, out destOffset, out destLength);
            preprocessedTiles.Add(new BitmapTileZoomLayer<T>.PreprocessedTile()
            {
              Tile = obj,
              Data = dest,
              Offset = destOffset,
              Length = destLength
            });
          }
        }
        else
        {
          foreach (T tile in (IEnumerable<T>) grouping)
            this.DrawTile(tile);
        }
      }
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        foreach (BitmapTileZoomLayer<T>.PreprocessedTile preprocessedTile in (IEnumerable<BitmapTileZoomLayer<T>.PreprocessedTile>) preprocessedTiles)
        {
          try
          {
            preprocessedTile.Tile.BitmapSource = (object) this._bitmapFactory.GetBitmap(preprocessedTile.Data, preprocessedTile.Offset, preprocessedTile.Length);
            this.DrawTile(preprocessedTile.Tile);
          }
          catch (ArgumentException ex)
          {
            Logger.SendError((Exception) ex);
          }
        }
      }));
    }

    private struct PreprocessedTile
    {
      public byte[] Data { get; set; }

      public int Offset { get; set; }

      public int Length { get; set; }

      public T Tile { get; set; }
    }
  }
}
