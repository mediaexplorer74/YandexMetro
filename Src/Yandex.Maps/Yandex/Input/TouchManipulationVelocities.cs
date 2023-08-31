// Decompiled with JetBrains decompiler
// Type: Yandex.Input.TouchManipulationVelocities
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Input
{
  [ComVisible(true)]
  public struct TouchManipulationVelocities
  {
    public TouchManipulationVelocities(Point expansionVelocity, Point linearVelocity)
      : this()
    {
      this.ExpansionVelocity = expansionVelocity;
      this.LinearVelocity = linearVelocity;
    }

    public TouchManipulationVelocities(
      double expansionVelocityX,
      double expansionVelocityY,
      double linearVelocityX,
      double linearVelocityY)
      : this()
    {
      this.ExpansionVelocity = new Point(expansionVelocityX, expansionVelocityY);
      this.LinearVelocity = new Point(linearVelocityX, linearVelocityY);
    }

    public Point ExpansionVelocity { get; private set; }

    public Point LinearVelocity { get; private set; }
  }
}
