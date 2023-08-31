// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.PositionChangedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Positioning
{
  internal class PositionChangedEventArgs : EventArgs
  {
    public PositionChangedEventArgs(GeoPosition geoPosition) => this.GeoPosition = geoPosition != null ? geoPosition : throw new ArgumentNullException(nameof (geoPosition));

    public GeoPosition GeoPosition { get; private set; }
  }
}
