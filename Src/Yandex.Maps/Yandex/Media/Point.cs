// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Point
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.Media
{
  [ComVisible(true)]
  public struct Point
  {
    public Point(double x, double y)
      : this()
    {
      this.X = x;
      this.Y = y;
    }

    public double X { get; set; }

    public double Y { get; set; }

    public override string ToString() => string.Format("Point X:{0} Y:{1}", (object) this.X, (object) this.Y);

    public static bool operator ==(Point a, Point b) => a.Equals(b);

    public static bool operator !=(Point a, Point b) => !a.Equals(b);

    public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

    public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

    public static double operator *(Point a, Point b) => a.X * b.Y + a.Y * b.X;

    public static Point operator *(Point a, double b) => new Point(a.X * b, a.Y * b);

    public double Modulus => Math.Sqrt(this.X * this.X + this.Y * this.Y);

    public Point Normalize()
    {
      double modulus = this.Modulus;
      return new Point(this.X / modulus, this.Y / modulus);
    }

    public override bool Equals(object obj) => obj != null && obj is Point point && this.X == point.X && this.Y == point.Y;

    public bool Equals(Point p) => this.X == p.X && this.Y == p.Y;

    public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode();
  }
}
