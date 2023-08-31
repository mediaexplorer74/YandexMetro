// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Helpers.Events.DragCompletedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Positioning;

namespace Yandex.Maps.Helpers.Events
{
  internal class DragCompletedEventArgs : EventArgs
  {
    public DragCompletedEventArgs(GeoCoordinate coordinates) => this.Coordinates = coordinates;

    public GeoCoordinate Coordinates { get; private set; }
  }
}
