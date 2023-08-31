// Decompiled with JetBrains decompiler
// Type: Yandex.Input.TouchManipulationDelta
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Input
{
  [ComVisible(true)]
  public class TouchManipulationDelta
  {
    public TouchManipulationDelta()
    {
      this.Scale = new Point(1.0, 1.0);
      this.Translation = new Point(0.0, 0.0);
      this.SingleScale = 1.0;
    }

    public TouchManipulationDelta(Point translation, double scale, Point scaleCenter)
    {
      this.Translation = translation;
      this.SingleScale = scale;
      this.ScaleCenter = scaleCenter;
    }

    public TouchManipulationDelta(
      double scaleX,
      double scaleY,
      double translationX,
      double translationY)
    {
      this.Scale = new Point(scaleX, scaleY);
      this.SingleScale = scaleX;
      this.Translation = new Point(translationX, translationY);
    }

    public double SingleScale { get; set; }

    public Point Scale { get; set; }

    public Point Translation { get; set; }

    public Point ScaleCenter { get; set; }
  }
}
