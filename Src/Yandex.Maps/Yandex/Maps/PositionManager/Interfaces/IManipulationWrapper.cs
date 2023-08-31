// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.Interfaces.IManipulationWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.PositionManager.Event;
using Yandex.Media;

namespace Yandex.Maps.PositionManager.Interfaces
{
  internal interface IManipulationWrapper
  {
    void SetCurrentPosition(Position value);

    void SetScreenCenterOffset(Point value);

    void EnableManipulations();

    void DisableManipulations();

    event EventHandler<ManipulationCompletedEventArgs> ManipulationCompleted;

    event EventHandler ManipulationStarted;

    event EventHandler<ManipulationMoveEventArgs> ManipulationMove;

    event EventHandler<ManipulationZoomEventArgs> ManipulationZoom;
  }
}
