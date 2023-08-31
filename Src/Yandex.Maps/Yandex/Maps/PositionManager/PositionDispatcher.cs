// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.PositionDispatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls.PositionManager.Event;
using Yandex.Maps.Events;
using Yandex.Maps.PositionManager.Event;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Maps.PositionManager.Operations;
using Yandex.Media;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.PositionManager
{
  [UsedImplicitly]
  internal class PositionDispatcher : 
    PositionDispatcherBase,
    IPositionDispatcher,
    IPositionDispatcherBase,
    IDisposable
  {
    public PositionDispatcher(
      [NotNull] IPositionWatcherWrapper positionWatcher,
      [NotNull] IPositionManager positionManager,
      [NotNull] IGeoPixelConverter geoPixelConverter,
      [NotNull] IViewportPointConveter viewportPointConveter,
      [NotNull] IThread thread,
      [NotNull] IMonitor monitor)
      : base(positionWatcher, positionManager, geoPixelConverter, viewportPointConveter, thread, monitor)
    {
      this.IsPositionWatchingEnabled = true;
      this._positionWatcher.PositionMove += new EventHandler<PositionMoveEventArgs>(this.PositionWatcherPositionMove);
    }

    public void JumpToCurrentLocation()
    {
      lock (this._operationsQueueSync)
      {
        this._operationsQueue.Clear();
        JumpToCurrentPositionOperation positionOperation = new JumpToCurrentPositionOperation();
        positionOperation.Source = PositionOperationSource.Manual;
        this.TryProcessNewOperation((PositionOperation) positionOperation);
      }
    }

    public bool IsPositionWatchingEnabled { get; set; }

    private void PositionWatcherPositionMove(object sender, PositionMoveEventArgs e)
    {
      if (!this.IsPositionWatchingEnabled)
        return;
      MovePositionOperation positionOperation = new MovePositionOperation();
      positionOperation.RelativePoint = e.RelativePoint;
      positionOperation.Source = PositionOperationSource.Gps;
      this.TryProcessNewOperation((PositionOperation) positionOperation);
    }

    private void ManipulationWrapperManipulationZoom(object sender, ManipulationZoomEventArgs e)
    {
      ZoomPositionOperation positionOperation = new ZoomPositionOperation();
      positionOperation.ScreenScaleCenter = new Point?(e.ScalePoint);
      positionOperation.Zoom = e.Zoom;
      positionOperation.Source = PositionOperationSource.Manipulation;
      this.TryProcessNewOperation((PositionOperation) positionOperation);
    }

    private void ManipulationWrapperManipulationMove(object sender, ManipulationMoveEventArgs e)
    {
      MovePositionOperation positionOperation = new MovePositionOperation();
      positionOperation.RelativePoint = e.RelativePoint;
      positionOperation.Source = PositionOperationSource.Manipulation;
      this.TryProcessNewOperation((PositionOperation) positionOperation);
    }

    private void ManipulationWrapperManipulationStarted(object sender, EventArgs e)
    {
      lock (this._operationsQueueSync)
        this._operationsQueue.Clear();
      this._positionManager.FinalizeLastQueueIfExists();
      this._positionWatcher.DisablePositionFollowing();
      this.Mode = OperationStatus.Normal;
    }

    private void ManipulationWrapperManipulationCompleted(
      object sender,
      ManipulationCompletedEventArgs e)
    {
      if (e.Velocity.HasValue && (e.Velocity.Value.X != 0.0 || e.Velocity.Value.Y != 0.0))
      {
        lock (this._operationsQueueSync)
        {
          if (this._operationsQueue.Count != 0 || this._positionManager.IsEnumerationInProcess)
            return;
          this._positionManager.MoveByVelocity(e.Velocity.Value);
        }
      }
      else
      {
        this.OnPositionChanged(new PositionChangedEventArgs(this._positionManager.CurrentPosition.RelativePoint, this._positionManager.CurrentPosition.Zoom, PositionChangeType.Final));
        this._positionWatcher.ResetPositionFollowing();
        if (this._positionManager.IsEnumerationInProcess)
          return;
        this.Mode = OperationStatus.Idle;
      }
    }

    protected override void OnManipulationWrapperChanged(
      IManipulationWrapper oldValue,
      IManipulationWrapper newValue)
    {
      base.OnManipulationWrapperChanged(oldValue, newValue);
      if (oldValue != null)
      {
        oldValue.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.ManipulationWrapperManipulationCompleted);
        oldValue.ManipulationStarted -= new EventHandler(this.ManipulationWrapperManipulationStarted);
        oldValue.ManipulationMove -= new EventHandler<ManipulationMoveEventArgs>(this.ManipulationWrapperManipulationMove);
        oldValue.ManipulationZoom -= new EventHandler<ManipulationZoomEventArgs>(this.ManipulationWrapperManipulationZoom);
      }
      if (newValue == null)
        return;
      newValue.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.ManipulationWrapperManipulationCompleted);
      newValue.ManipulationStarted += new EventHandler(this.ManipulationWrapperManipulationStarted);
      newValue.ManipulationMove += new EventHandler<ManipulationMoveEventArgs>(this.ManipulationWrapperManipulationMove);
      newValue.ManipulationZoom += new EventHandler<ManipulationZoomEventArgs>(this.ManipulationWrapperManipulationZoom);
    }
  }
}
