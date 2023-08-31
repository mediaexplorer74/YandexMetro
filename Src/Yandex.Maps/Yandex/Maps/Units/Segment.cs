// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Units.Segment
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.Units.Interfaces;

namespace Yandex.Maps.Units
{
  internal class Segment : ISegment
  {
    private PointXY? _vector;

    public Segment(PointXY start, PointXY finish)
    {
      this.Start = start;
      this.Finish = finish;
    }

    public PointXY Start { get; private set; }

    public PointXY Finish { get; private set; }

    public PointXY Middle => new PointXY((long) ((double) (this.Start.X + this.Finish.X) * 0.5), (long) ((double) (this.Start.Y + this.Finish.Y) * 0.5));

    public double Length { get; set; }

    public DistanceXY LengthXY => new DistanceXY((long) (this.Finish - this.Start).Modulus);

    public PointXY Vector => !this._vector.HasValue ? (this._vector = new PointXY?(this.Finish - this.Start)).Value : this._vector.Value;

    public override string ToString() => string.Format("Start: {0}, finish {1}", (object) this.Start, (object) this.Finish);
  }
}
