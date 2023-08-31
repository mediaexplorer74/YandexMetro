// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Units.DistanceXY
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Maps.Units
{
  internal struct DistanceXY
  {
    public long Value;

    public DistanceXY(long value)
      : this()
    {
      this.Value = value;
    }

    public static implicit operator long(DistanceXY distance) => distance.Value;

    public static implicit operator DistanceXY(long value) => new DistanceXY(value);

    public static bool operator <(DistanceXY d1, DistanceXY d2) => d1.Value < d2.Value;

    public static bool operator >(DistanceXY d1, DistanceXY d2) => d1.Value > d2.Value;

    public static bool operator <=(DistanceXY d1, DistanceXY d2) => d1.Value <= d2.Value;

    public static bool operator >=(DistanceXY d1, DistanceXY d2) => d1.Value >= d2.Value;

    public static DistanceXY operator +(DistanceXY d1, DistanceXY d2) => (DistanceXY) (d1.Value + d2.Value);

    public static DistanceXY operator -(DistanceXY d1, DistanceXY d2) => (DistanceXY) (d1.Value - d2.Value);
  }
}
