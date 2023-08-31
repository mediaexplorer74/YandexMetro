// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.ViewportPoint
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public struct ViewportPoint
  {
    public ViewportPoint(Point point, double zoomLevel)
      : this()
    {
      this.Point = point;
      this.ZoomLevel = zoomLevel;
    }

    public double ZoomLevel { get; set; }

    public Point Point { get; set; }

    public override bool Equals(object obj) => obj != null && obj is ViewportPoint viewportPoint && viewportPoint.Point == this.Point && viewportPoint.ZoomLevel == this.ZoomLevel;

    public override int GetHashCode() => this.Point.GetHashCode() ^ this.ZoomLevel.GetHashCode();
  }
}
