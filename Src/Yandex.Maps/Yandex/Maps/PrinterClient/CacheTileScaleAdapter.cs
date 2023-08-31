// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.CacheTileScaleAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.PrinterClient.Interfaces;

namespace Yandex.Maps.PrinterClient
{
  internal class CacheTileScaleAdapter : ICacheTileScaleAdapter
  {
    private readonly ITileScaleAdapter _tileScaleAdapter;

    public CacheTileScaleAdapter([NotNull] ITileScaleAdapter tileScaleAdapter) => this._tileScaleAdapter = tileScaleAdapter != null ? tileScaleAdapter : throw new ArgumentNullException(nameof (tileScaleAdapter));

    public short GetCacheScale(double tilesStretchFactor) => (short) ((int) this.GetCacheFileScale(tilesStretchFactor) - 100);

    public short GetCacheFileScale(double tilesStretchFactor) => (short) (this._tileScaleAdapter.Convert(tilesStretchFactor) * 100.0);
  }
}
