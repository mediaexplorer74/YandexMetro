// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Events.MultiTapEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Media;

namespace Yandex.Input.Events
{
  internal class MultiTapEventArgs : EventArgs
  {
    public Point Origin { get; private set; }

    public uint FingersCount { get; private set; }

    public MultiTapEventArgs(Point origin, uint fingersCount)
    {
      this.Origin = origin;
      this.FingersCount = fingersCount;
    }
  }
}
