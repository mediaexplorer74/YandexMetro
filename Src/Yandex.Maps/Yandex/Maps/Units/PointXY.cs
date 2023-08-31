// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Units.PointXY
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Media;

namespace Yandex.Maps.Units
{
  internal struct PointXY
  {
    private double? _modulus;
    public long X;
    public long Y;

    public PointXY(long x, long y)
      : this()
    {
      this.X = x;
      this.Y = y;
    }

    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public long ValueX
    {
      get => this.X;
      set => this.X = value;
    }

    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public long ValueY
    {
      get => this.Y;
      set => this.Y = value;
    }

    public double Modulus => !this._modulus.HasValue ? (this._modulus = new double?(Math.Sqrt((double) (this.X * this.X + this.Y * this.Y)))).Value : this._modulus.Value;

    public static PointXY operator -(PointXY a, PointXY b) => new PointXY(a.X - b.X, a.Y - b.Y);

    public static PointXY operator +(PointXY a, PointXY b) => new PointXY(a.X + b.X, a.Y + b.Y);

    public static long operator *(PointXY a, PointXY b) => a.X * b.X + a.Y * b.Y;

    public static long VectorProductModulus(PointXY a, PointXY b) => a.X * b.Y - a.Y * b.X;

    public static implicit operator Point(PointXY pointXY) => new Point((double) pointXY.X, (double) pointXY.Y);

    public static explicit operator PointXY(Point point) => new PointXY((long) point.X, (long) point.Y);

    public override string ToString() => string.Format("X: {0} Y: {1}", (object) this.X, (object) this.Y);
  }
}
