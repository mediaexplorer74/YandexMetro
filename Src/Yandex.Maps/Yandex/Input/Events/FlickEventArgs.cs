// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Events.FlickEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Media;

namespace Yandex.Input.Events
{
  internal class FlickEventArgs : EventArgs
  {
    public Point Origin { get; private set; }

    public TouchManipulationVelocities Velocities { get; private set; }

    public FlickEventArgs(Point origin, TouchManipulationVelocities velocities)
    {
      this.Origin = origin;
      this.Velocities = velocities;
    }
  }
}
