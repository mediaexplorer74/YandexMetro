// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Events.TouchEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Input.Interfaces;

namespace Yandex.Input.Events
{
  [ComVisible(true)]
  public class TouchEventArgs : EventArgs
  {
    public TouchEventArgs(
      double timestamp,
      IList<ITouchPoint> touchPoints,
      ITouchPoint primaryTouchPoint)
    {
      this.TimestampMilliseconds = timestamp;
      this.TouchPoints = touchPoints;
      this.PrimaryTouchPoint = primaryTouchPoint;
    }

    public double TimestampMilliseconds { get; private set; }

    public IList<ITouchPoint> TouchPoints { get; private set; }

    public ITouchPoint PrimaryTouchPoint { get; private set; }
  }
}
