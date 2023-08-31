// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Events.TouchManipulationCompletedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Media;

namespace Yandex.Input.Events
{
  internal class TouchManipulationCompletedEventArgs : EventArgs
  {
    protected TouchManipulationCompletedEventArgs()
    {
      this.TotalManipulation = new TouchManipulationDelta();
      this.ManipulationCenter = new Point();
    }

    public TouchManipulationCompletedEventArgs(TouchManipulationVelocities finalVelocities)
      : this()
    {
      this.FinalVelocities = finalVelocities;
    }

    public TouchManipulationVelocities FinalVelocities { get; protected set; }

    public Point ManipulationCenter { get; protected set; }

    public TouchManipulationDelta TotalManipulation { get; protected set; }
  }
}
