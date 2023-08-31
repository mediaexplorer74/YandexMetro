// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TileZoomLayer`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Windows;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Media.Imaging;

namespace Yandex.Maps.ContentLayers
{
  internal class TileZoomLayer<T> : VisibleTileLayer<T>, ITileZoomLayer<T>, IVisibleTileLayer<T> where T : ITile
  {
    private readonly TileLayer _tileLayer;
    private readonly ITileDrawManager _tileDrawManager;

    public TileZoomLayer(byte zoom, [NotNull] TileLayer tileLayer, [NotNull] ITileDrawManager tileDrawManager)
    {
      this._tileLayer = tileLayer;
      this._tileDrawManager = tileDrawManager;
      this._tileDrawManager.TileLayer = tileLayer;
      this.Zoom = zoom;
      this.OperationMode = OperationMode.Full;
    }

    public byte Zoom { get; private set; }

    public double Scale
    {
      set => this._tileLayer.Scale = value;
      get => this._tileLayer.Scale;
    }

    public UIElement Control => (UIElement) this._tileLayer;

    public OperationMode OperationMode { get; set; }

    protected void DrawTile(T tile) => this._tileDrawManager.DrawTiles((IEnumerable<ITile>) new ITile[1]
    {
      (ITile) tile
    }, RenderContentMode.ClearRender);

    protected override void OnDisposeTiles()
    {
      base.OnDisposeTiles();
      this._tileDrawManager.DisposeTiles();
    }

    public override IList<ITileInfo> DisposeTilesExcept(IList<ITileInfo> tiles)
    {
      IList<ITileInfo> tilesToDispose = base.DisposeTilesExcept(tiles);
      this._tileDrawManager.DisposeTiles((IEnumerable<ITileInfo>) tilesToDispose);
      return tilesToDispose;
    }

    protected override void OnDisposeTiles(IList<ITileInfo> tilesToDispose)
    {
      base.OnDisposeTiles(tilesToDispose);
      this._tileDrawManager.DisposeTiles((IEnumerable<ITileInfo>) tilesToDispose);
    }
  }
}
