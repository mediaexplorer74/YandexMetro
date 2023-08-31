﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.Interfaces.IPositionWatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Positioning.Events;

namespace Yandex.Positioning.Interfaces
{
  internal interface IPositionWatcher
  {
    event EventHandler<PositionChangedEventArgs> PositionChanged;

    event EventHandler<PositionWatcherStatusChangedEventArgs> StatusUpdated;

    bool Enabled { get; set; }

    GeoPosition LastPosition { get; }

    GeoPositionStatus Status { get; }
  }
}
