// Decompiled with JetBrains decompiler
// Type: Yandex.Input.TouchPoint
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Yandex.Input.Interfaces;
using Yandex.Media;

namespace Yandex.Input
{
  [ComVisible(false)]
  public struct TouchPoint : ITouchPoint
  {
    public TouchPoint(TouchPoint touchPoint)
      : this()
    {
      this.Action = (TouchAction) touchPoint.Action;
      this.Position = new Point(touchPoint.Position.X, touchPoint.Position.Y);
      this.DirectlyOver = (object) touchPoint.TouchDevice.DirectlyOver;
    }

    public TouchPoint(TouchAction touchAction, Point position)
      : this()
    {
      this.Action = (TouchAction) touchAction;
      this.Position = position;
    }

    public TouchAction Action { get; set; }

    public Point Position { get; set; }

    [Obsolete("Is not supported on WP8")]
    public Size Size { get; set; }

    public object DirectlyOver { get; set; }
  }
}
