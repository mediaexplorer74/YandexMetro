// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TileWeighting.TileWeight
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Maps.TileWeighting
{
  internal struct TileWeight : ITileWeight, IComparable<ITileWeight>, IComparable<TileWeight>
  {
    public bool IsVisibleOnScreen { get; set; }

    public bool IsCurrentLayerTile { get; set; }

    public double AreaSizeOnScreen { get; set; }

    public double DistanceToScreenCenter { get; set; }

    public int CompareTo(ITileWeight other)
    {
      if (!(other is TileWeight other1))
        throw new ArgumentOutOfRangeException(nameof (other));
      return this.CompareTo(other1);
    }

    public int CompareTo(TileWeight other)
    {
      if (this.IsVisibleOnScreen != other.IsVisibleOnScreen)
        return this.IsVisibleOnScreen.CompareTo(other.IsVisibleOnScreen);
      if (this.IsCurrentLayerTile != other.IsCurrentLayerTile)
        return this.IsCurrentLayerTile.CompareTo(other.IsCurrentLayerTile);
      return Math.Floor(this.AreaSizeOnScreen) != Math.Floor(other.AreaSizeOnScreen) ? Math.Sign(this.AreaSizeOnScreen - other.AreaSizeOnScreen) : -Math.Sign(this.DistanceToScreenCenter - other.DistanceToScreenCenter);
    }

    public static bool operator >(TileWeight w1, TileWeight w2) => w1.CompareTo(w2) > 0;

    public static bool operator <(TileWeight w1, TileWeight w2) => w2 > w1;
  }
}
