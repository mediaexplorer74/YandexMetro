// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.StaticPositionDispatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Maps.PositionManager.Operations;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.PositionManager
{
  internal class StaticPositionDispatcher : PositionDispatcherBase
  {
    public StaticPositionDispatcher(
      [NotNull] IPositionWatcherWrapper positionWatcher,
      [NotNull] IPositionManager positionManager,
      [NotNull] IGeoPixelConverter geoPixelConverter,
      [NotNull] IViewportPointConveter viewportPointConveter,
      [NotNull] IThread thread,
      [NotNull] IMonitor monitor)
      : base(positionWatcher, positionManager, geoPixelConverter, viewportPointConveter, thread, monitor)
    {
    }

    protected override bool ExecuteOperation(
      PositionOperation positionOperation,
      bool withAnimation)
    {
      return base.ExecuteOperation(positionOperation, false);
    }
  }
}
