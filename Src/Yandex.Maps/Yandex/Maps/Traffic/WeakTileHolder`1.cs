// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.WeakTileHolder`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Traffic
{
  internal class WeakTileHolder<T> where T : class, ITile, Yandex.Common.ICloneable
  {
    private readonly T _tile;
    private readonly WeakReference _bitmapSource;

    public WeakTileHolder(T tile)
    {
      this._tile = (object) tile != null ? tile.Clone() as T : throw new ArgumentNullException(nameof (tile));
      if ((object) this._tile == null)
        throw new Exception("tile.Clone() returned null or object not of type " + (object) typeof (T));
      this._tile.BitmapSource = (object) null;
      this._bitmapSource = new WeakReference(tile.BitmapSource);
    }

    public ITileInfo Key => this._tile.TileInfo;

    public T GetStrongTile()
    {
      T strongTile = (T) this._tile.Clone();
      strongTile.BitmapSource = this._bitmapSource.Target;
      return strongTile;
    }
  }
}
