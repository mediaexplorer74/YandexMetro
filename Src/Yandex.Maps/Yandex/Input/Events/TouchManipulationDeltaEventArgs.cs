// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Events.TouchManipulationDeltaEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Media;

namespace Yandex.Input.Events
{
  internal class TouchManipulationDeltaEventArgs : EventArgs
  {
    public TouchManipulationDeltaEventArgs(bool isInertial)
    {
      this.IsInertial = isInertial;
      this.CumulativeManipulation = new TouchManipulationDelta();
      this.DeltaManipulation = new TouchManipulationDelta();
      this.Velocities = new TouchManipulationVelocities(new Point(), new Point());
    }

    public TouchManipulationDelta CumulativeManipulation { get; set; }

    public TouchManipulationDelta DeltaManipulation { get; set; }

    public TouchManipulationVelocities Velocities { get; set; }

    public object EventSource { get; set; }

    public bool IsInertial { get; private set; }
  }
}
