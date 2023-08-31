// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.InterimPointsEnumerators.Interfaces.IInterimPointsEnumerator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Yandex.Media;

namespace Yandex.Maps.PositionManager.InterimPointsEnumerators.Interfaces
{
  internal interface IInterimPointsEnumerator
  {
    bool StopInterimPointsEnumerationIfCenterPointIsNotValidForZoomlevel { get; }

    bool UseLastInterimPoint { get; }

    bool IsEnumerationFinished { get; }

    [CanBeNull]
    Position GetNextInterimPoint(double timeFromLastCallInMilliseconds);

    [CanBeNull]
    Position GetLastInterimPoint(double timeFromLastCallInMilliseconds);
  }
}
