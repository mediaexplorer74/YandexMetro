// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Events.PositionChangedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Maps.Events
{
  [ComVisible(true)]
  public class PositionChangedEventArgs : EventArgs
  {
    public PositionChangedEventArgs(
      Point relativePoint,
      double zoomLevel,
      PositionChangeType positionChangeType)
    {
      this.Position = relativePoint;
      this.ZoomLevel = zoomLevel;
      this.PositionChangeType = positionChangeType;
    }

    public Point Position { get; private set; }

    public double ZoomLevel { get; private set; }

    public PositionChangeType PositionChangeType { get; set; }
  }
}
