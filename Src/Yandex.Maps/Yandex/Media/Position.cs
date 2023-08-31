// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Position
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Media
{
  internal class Position
  {
    private Point? _relativePoint;
    private double? _zoom;

    public Point RelativePoint
    {
      get => !this._relativePoint.HasValue ? new Point() : this._relativePoint.Value;
      set => this._relativePoint = new Point?(value);
    }

    public double Zoom
    {
      get => !this._zoom.HasValue ? double.NaN : this._zoom.Value;
      set => this._zoom = new double?(value);
    }

    public Point? RelativeScaleCenter { get; set; }

    public bool IsInitialised => this._zoom.HasValue && this._relativePoint.HasValue;

    public static bool operator ==(Position p1, Position p2) => object.Equals((object) p1, (object) p2);

    public static bool operator !=(Position p1, Position p2) => !object.Equals((object) p1, (object) p2);

    public override bool Equals(object obj)
    {
      Position position = obj as Position;
      return !(position == (Position) null) && object.Equals((object) position.Zoom, (object) this.Zoom) && object.Equals((object) position.RelativePoint, (object) this.RelativePoint);
    }

    public override int GetHashCode() => this.RelativePoint.GetHashCode() ^ this.Zoom.GetHashCode();
  }
}
