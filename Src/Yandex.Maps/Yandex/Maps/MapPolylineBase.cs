// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapPolylineBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Shapes;

namespace Yandex.Maps
{
  [TemplatePart(Name = "Shape", Type = typeof (Polyline))]
  [ComVisible(false)]
  public class MapPolylineBase : MapShape
  {
    public MapPolylineBase() => this.DefaultStyleKey = (object) typeof (MapPolylineBase);

    protected override void ClearShape()
    {
      if (!(this.Shape is Polyline shape))
        return;
      ((PresentationFrameworkCollection<Point>) shape.Points).Clear();
    }

    protected override void AddPoint(Point point)
    {
      if (!(this.Shape is Polyline shape))
        return;
      ((PresentationFrameworkCollection<Point>) shape.Points).Add(point);
    }
  }
}
