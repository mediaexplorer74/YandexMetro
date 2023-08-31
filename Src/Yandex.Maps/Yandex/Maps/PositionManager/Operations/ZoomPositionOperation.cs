// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.Operations.ZoomPositionOperation
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Media;

namespace Yandex.Maps.PositionManager.Operations
{
  internal class ZoomPositionOperation : PositionOperation
  {
    public ZoomPositionOperation() => this.Type = PositionOperationType.Zoom;

    public double Zoom { get; set; }

    public Point? ScreenScaleCenter { get; set; }

    public bool UseCurrentPositionAsScaleCenter { get; set; }
  }
}
