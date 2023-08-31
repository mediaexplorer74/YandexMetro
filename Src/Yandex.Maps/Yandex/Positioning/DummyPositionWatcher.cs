// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.DummyPositionWatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Positioning.Events;
using Yandex.Positioning.Interfaces;

namespace Yandex.Positioning
{
  internal class DummyPositionWatcher : IPositionWatcher
  {
    public event EventHandler<PositionChangedEventArgs> PositionChanged;

    public event EventHandler<PositionWatcherStatusChangedEventArgs> StatusUpdated;

    public bool Enabled { get; set; }

    public GeoPosition LastPosition => new GeoPosition(new GeoCoordinate(55.733837, 37.588088), new DateTimeOffset(), 0.0, 0.0, 0.0, 0.0, 0.0);

    public GeoPositionStatus Status => GeoPositionStatus.Disabled;
  }
}
