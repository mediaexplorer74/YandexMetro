// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Rect
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Extensions;

namespace Yandex.Media
{
  [ComVisible(true)]
  public struct Rect
  {
    public Rect(double x, double y, double width, double height)
      : this()
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }

    public double X { get; set; }

    public double Y { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Left => this.X;

    public double Top => this.Y;

    public double Right => this.X + this.Width;

    public double Bottom => this.Y + this.Height;

    public bool Intersects(Rect destRect) => this.Width >= 0.0 && destRect.Width >= 0.0 && destRect.X <= this.X + this.Width && destRect.X + destRect.Width >= this.X && destRect.Y <= this.Y + this.Height && destRect.Y + destRect.Height >= this.Y;

    public bool Contains(Rect destRect) => destRect.Right <= this.Right && destRect.Left >= this.Left && destRect.Top >= this.Top && destRect.Bottom <= this.Bottom;

    public bool Contains(Point point) => point.X >= this.Left && point.X <= this.Right && point.Y >= this.Top && point.Y <= this.Bottom;

    public override bool Equals(object obj) => obj is Rect rect && rect.X == this.X && rect.Y == this.Y && rect.Width == this.Width && rect.Height == this.Height;

    public static bool operator ==(Rect a, Rect b) => a.Equals((object) b);

    public static bool operator !=(Rect a, Rect b) => !a.Equals((object) b);

    public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Width.GetHashCode() ^ this.Height.GetHashCode();

    public override string ToString() => string.Format("X = {0}, Y = {1}, Width = {2}, Height = {3}, Right = {4}, Bottom = {5}", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height, (object) this.Right, (object) this.Bottom);

    public static Rect CreateBoundingRect(IList<Point> points)
    {
      if (points == null)
        throw new ArgumentNullException(nameof (points));
      double x = points.MinOrDefault<Point, double>((Func<Point, double>) (point => point.X));
      double y = points.MinOrDefault<Point, double>((Func<Point, double>) (point => point.Y));
      double num1 = points.MaxOrDefault<Point, double>((Func<Point, double>) (point => point.X));
      double num2 = points.MaxOrDefault<Point, double>((Func<Point, double>) (point => point.Y));
      return new Rect(x, y, num1 - x, num2 - y);
    }
  }
}
