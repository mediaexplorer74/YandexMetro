﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.Operations.MovePositionOperation
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Media;

namespace Yandex.Maps.PositionManager.Operations
{
  internal class MovePositionOperation : PositionOperation
  {
    public MovePositionOperation() => this.Type = PositionOperationType.Move;

    public Point RelativePoint { get; set; }
  }
}