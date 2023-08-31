// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.PositionDispatcherBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Maps.PositionManager.Operations;
using Yandex.Media;
using Yandex.Positioning;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.PositionManager
{
  internal class PositionDispatcherBase : 
    IPositionDispatcherBase,
    IDisposable,
    IManipulationWrapperInitializable
  {
    private const double ZoomLevelEpsilon = 0.001;
    private readonly IGeoPixelConverter _geoPixelConverter;
    private readonly object _mapPositionSyncObj = new object();
    private readonly IMonitor _monitor;
    private readonly object _operationExecuted = new object();
    protected readonly Queue<PositionOperation> _operationsQueue = new Queue<PositionOperation>();
    private readonly object _operationsQueueNotEmpty = new object();
    protected readonly object _operationsQueueSync = new object();
    protected readonly IPositionManager _positionManager;
    protected readonly IPositionWatcherWrapper _positionWatcher;
    private readonly IViewportPointConveter _viewportPointConveter;
    protected PositionOperation _currentOperation;
    private OperationStatus _dispatcherMode;
    private bool _disposed;
    private bool _isUserInteractionEnabled;
    private IManipulationWrapper _manipulationWrapper;

    [UsedImplicitly]
    public PositionDispatcherBase(
      [NotNull] IPositionWatcherWrapper positionWatcher,
      [NotNull] IPositionManager positionManager,
      [NotNull] IGeoPixelConverter geoPixelConverter,
      [NotNull] IViewportPointConveter viewportPointConveter,
      [NotNull] IThread thread,
      [NotNull] IMonitor monitor)
    {
      if (positionWatcher == null)
        throw new ArgumentNullException(nameof (positionWatcher));
      if (positionManager == null)
        throw new ArgumentNullException(nameof (positionManager));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      if (viewportPointConveter == null)
        throw new ArgumentNullException(nameof (viewportPointConveter));
      if (thread == null)
        throw new ArgumentNullException(nameof (thread));
      if (monitor == null)
        throw new ArgumentNullException(nameof (monitor));
      this._geoPixelConverter = geoPixelConverter;
      this._viewportPointConveter = viewportPointConveter;
      this._monitor = monitor;
      this._positionManager = positionManager;
      this._positionManager.PositionChanged += new EventHandler<Yandex.Maps.Events.PositionChangedEventArgs>(this.PositionManagerPositionChanged);
      this._positionManager.PointsEnumerationFinished += new EventHandler<Yandex.Maps.Events.PositionChangedEventArgs>(this.PositionManagerPointsEnumerationFinished);
      this._positionWatcher = positionWatcher;
      this._positionWatcher.ResetPositionFollowing();
      this.IsUserInteractionEnabled = true;
      thread.Start(new Action(this.OperationsWorker));
    }

    private double ZoomLevel { get; set; }

    private Rect Viewport
    {
      get => this._positionManager.Viewport;
      set => this._positionManager.Viewport = value;
    }

    protected OperationStatus Mode
    {
      get => this._dispatcherMode;
      set
      {
        if (this._dispatcherMode == value)
          return;
        this._dispatcherMode = value;
        this.OnOperationStatusChanged(new OperationStatusChangedEventArgs(value));
      }
    }

    public AnimationLevel AnimationLevel
    {
      get => this._positionManager.AnimationLevel;
      set => this._positionManager.AnimationLevel = value;
    }

    public bool IsUserInteractionEnabled
    {
      get => this._isUserInteractionEnabled;
      set
      {
        if (this._isUserInteractionEnabled == value)
          return;
        this._isUserInteractionEnabled = value;
        IManipulationWrapper manipulationWrapper = this.ManipulationWrapper;
        if (manipulationWrapper == null)
          return;
        this.UpdateManipulationWrapper(value, manipulationWrapper);
      }
    }

    public event EventHandler<Yandex.Maps.Events.PositionChangedEventArgs> PositionChanged;

    public event EventHandler<OperationStatusChangedEventArgs> OperationStatusChanged;

    public void ResetPositionFollowing() => this._positionWatcher.ResetPositionFollowing();

    public void MoveTo(Point relativeTargetPoint)
    {
      MovePositionOperation positionOperation = new MovePositionOperation();
      positionOperation.RelativePoint = relativeTargetPoint;
      positionOperation.Source = PositionOperationSource.Manual;
      this.TryProcessNewOperation((PositionOperation) positionOperation);
    }

    public void ZoomTo(double newZoomLevel, Point? screenScaleCenter = null)
    {
      ZoomPositionOperation positionOperation = new ZoomPositionOperation();
      positionOperation.ScreenScaleCenter = screenScaleCenter;
      positionOperation.Zoom = newZoomLevel;
      positionOperation.Source = PositionOperationSource.Manual;
      this.TryProcessNewOperation((PositionOperation) positionOperation);
    }

    public void MapPositionChanged(double zoomLevel, Rect viewport)
    {
      lock (this._mapPositionSyncObj)
      {
        this.Viewport = viewport;
        this.ZoomLevel = zoomLevel;
      }
      IManipulationWrapper manipulationWrapper = this.ManipulationWrapper;
      if (manipulationWrapper != null)
        this.UpdateManipulationWrapperScreenOffset(manipulationWrapper);
      this._positionWatcher.PositionChanged(zoomLevel, viewport);
    }

    public void ResendPositionChangedEvent() => this.TryProcessNewOperation((PositionOperation) new ResendEventOperation());

    public Position TargetPosition => this._positionManager.TargetPosition;

    public void ZoomIn(Point screenScaleCenter) => this.ZoomTo(Math.Ceiling(this._positionManager.TargetPosition.Zoom + 0.1), new Point?(screenScaleCenter));

    public void ZoomInWithCurrentPositionAsScaleCenter() => this.ZoomToWithCurrentPositionAsScaleCenter(Math.Ceiling(this._positionManager.TargetPosition.Zoom + 0.1));

    public void ZoomOut(Point screenScaleCenter) => this.ZoomTo(Math.Floor(this._positionManager.TargetPosition.Zoom - 0.1), new Point?(screenScaleCenter));

    public void ZoomOutWithCurrentPositionAsScaleCenter() => this.ZoomToWithCurrentPositionAsScaleCenter(Math.Floor(this._positionManager.TargetPosition.Zoom - 0.1));

    public void Dispose()
    {
      this._disposed = true;
      lock (this._operationExecuted)
        this._monitor.Pulse(this._operationExecuted);
      lock (this._operationsQueueNotEmpty)
        this._monitor.Pulse(this._operationsQueueNotEmpty);
      if (this._positionManager == null)
        return;
      this._positionManager.Dispose();
    }

    private void ZoomToWithCurrentPositionAsScaleCenter(double newZoomLevel)
    {
      ZoomPositionOperation positionOperation = new ZoomPositionOperation();
      positionOperation.UseCurrentPositionAsScaleCenter = true;
      positionOperation.ScreenScaleCenter = new Point?();
      positionOperation.Zoom = newZoomLevel;
      positionOperation.Source = PositionOperationSource.Manual;
      this.TryProcessNewOperation((PositionOperation) positionOperation);
    }

    private void OperationsWorker()
    {
      while (!this._disposed)
      {
        if (this._positionManager.IsEnumerationInProcess)
        {
          lock (this._operationExecuted)
          {
            if (this._positionManager.IsEnumerationInProcess)
              this._monitor.Wait(this._operationExecuted);
          }
        }
        if (this._operationsQueue.Count == 0)
        {
          lock (this._operationsQueueNotEmpty)
          {
            if (this._operationsQueue.Count == 0)
              this._monitor.Wait(this._operationsQueueNotEmpty);
          }
        }
        PositionOperation positionOperation = (PositionOperation) null;
        lock (this._operationsQueueSync)
        {
          if (this._operationsQueue.Count != 0)
          {
            positionOperation = this._operationsQueue.Dequeue();
            while (this._operationsQueue.Count != 0)
            {
              if (this._operationsQueue.Peek().Type == positionOperation.Type)
                positionOperation = this._operationsQueue.Dequeue();
              else
                break;
            }
          }
        }
        if (positionOperation != null)
        {
          this.Mode = OperationStatus.Busy;
          this._currentOperation = positionOperation;
          if (this.ExecuteOperation(positionOperation, this._operationsQueue.Count == 0))
            this.OnOperationExecuted();
        }
      }
    }

    private void OnOperationExecuted()
    {
      this._currentOperation = (PositionOperation) null;
      if (this._operationsQueue.Count != 0)
        return;
      this._positionWatcher.ResetPositionFollowing();
      this.Mode = OperationStatus.Idle;
    }

    private void PositionManagerPositionChanged(object sender, Yandex.Maps.Events.PositionChangedEventArgs e) => this.OnPositionChanged(e);

    private void PositionManagerPointsEnumerationFinished(object sender, Yandex.Maps.Events.PositionChangedEventArgs e)
    {
      this.OnPositionChanged(e);
      this.OnOperationExecuted();
      lock (this._operationExecuted)
        this._monitor.Pulse(this._operationExecuted);
    }

    private void OnOperationStatusChanged(OperationStatusChangedEventArgs e)
    {
      EventHandler<OperationStatusChangedEventArgs> operationStatusChanged = this.OperationStatusChanged;
      if (operationStatusChanged == null)
        return;
      operationStatusChanged((object) this, e);
    }

    protected void OnPositionChanged(Yandex.Maps.Events.PositionChangedEventArgs e)
    {
      EventHandler<Yandex.Maps.Events.PositionChangedEventArgs> positionChanged = this.PositionChanged;
      if (positionChanged == null)
        return;
      positionChanged((object) this, e);
    }

    protected void TryProcessNewOperation(PositionOperation positionOperation)
    {
      if (!this.CheckNewOperation(positionOperation))
        return;
      if (positionOperation.Source == PositionOperationSource.Manipulation)
      {
        this.ExecuteOperation(positionOperation, false);
      }
      else
      {
        lock (this._operationsQueueSync)
        {
          if (positionOperation.Type == PositionOperationType.ResendPositionChanged && this._operationsQueue.Count != 0)
            return;
          this._operationsQueue.Enqueue(positionOperation);
          if (this._currentOperation != null && this._currentOperation.Type == PositionOperationType.JumpToCurrentPosition && positionOperation.Source == PositionOperationSource.Gps)
          {
            this._positionManager.FinalizeLastQueueIfExists();
            lock (this._operationExecuted)
              this._monitor.Pulse(this._operationExecuted);
          }
          else if (this._operationsQueue.Count == 1)
          {
            if (this._currentOperation != null && this._currentOperation.Type != positionOperation.Type)
            {
              if (this._positionManager.IsEnumerationInProcess)
                goto label_18;
            }
            lock (this._operationExecuted)
              this._monitor.Pulse(this._operationExecuted);
          }
        }
label_18:
        lock (this._operationsQueueNotEmpty)
          this._monitor.Pulse(this._operationsQueueNotEmpty);
      }
    }

    private bool CheckNewOperation(PositionOperation positionOperation) => !(positionOperation is ZoomPositionOperation positionOperation1) || positionOperation1.Source != PositionOperationSource.Manipulation || Math.Abs(positionOperation1.Zoom - this._positionManager.CurrentPosition.Zoom) > 0.001;

    protected virtual bool ExecuteOperation(PositionOperation positionOperation, bool withAnimation)
    {
      switch (positionOperation.Type)
      {
        case PositionOperationType.Move:
          return this._positionManager.MoveTo(((MovePositionOperation) positionOperation).RelativePoint, withAnimation);
        case PositionOperationType.Zoom:
          ZoomPositionOperation positionOperation1 = (ZoomPositionOperation) positionOperation;
          if (positionOperation1.UseCurrentPositionAsScaleCenter)
            positionOperation1.ScreenScaleCenter = this.GetScaleCenterForZoom();
          return this._positionManager.ZoomTo(positionOperation1.Zoom, positionOperation1.ScreenScaleCenter, withAnimation);
        case PositionOperationType.JumpToCurrentPosition:
          this._positionWatcher.ScheduleJumpToCurrentLocation();
          GeoCoordinate lastCoordinates = this._positionWatcher.LastCoordinates;
          return lastCoordinates != null && this._positionManager.MoveTo(this._geoPixelConverter.CoordinatesToRelativePoint(lastCoordinates), withAnimation);
        case PositionOperationType.ResendPositionChanged:
          if (this._positionManager.CurrentPosition.IsInitialised)
            this.OnPositionChanged(new Yandex.Maps.Events.PositionChangedEventArgs(this._positionManager.CurrentPosition.RelativePoint, this._positionManager.CurrentPosition.Zoom, PositionChangeType.Final));
          return true;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private Point? GetScaleCenterForZoom()
    {
      if (this._positionWatcher.Status != GeoPositionStatus.Ready)
        return new Point?();
      GeoCoordinate lastCoordinates = this._positionWatcher.LastCoordinates;
      if (lastCoordinates != null)
      {
        lock (this._mapPositionSyncObj)
        {
          Point relativePoint = this._geoPixelConverter.CoordinatesToRelativePoint(lastCoordinates);
          if (this.Viewport.Contains(this._viewportPointConveter.RelativePointToViewportPoint(relativePoint, this.ZoomLevel)))
            return new Point?(relativePoint);
        }
      }
      return new Point?();
    }

    [CanBeNull]
    protected IManipulationWrapper ManipulationWrapper
    {
      get => this._manipulationWrapper;
      set
      {
        if (this._manipulationWrapper == value)
          return;
        IManipulationWrapper manipulationWrapper = this._manipulationWrapper;
        this._manipulationWrapper = value;
        this.OnManipulationWrapperChanged(manipulationWrapper, value);
      }
    }

    protected virtual void OnManipulationWrapperChanged(
      IManipulationWrapper oldValue,
      IManipulationWrapper newValue)
    {
      if (newValue == null)
        return;
      newValue.SetCurrentPosition(this._positionManager.CurrentPosition);
      this.UpdateManipulationWrapper(this.IsUserInteractionEnabled, newValue);
      this.UpdateManipulationWrapperScreenOffset(newValue);
    }

    public void Initialize(IManipulationWrapper manipulationWrapper) => this.ManipulationWrapper = manipulationWrapper != null ? manipulationWrapper : throw new ArgumentNullException(nameof (manipulationWrapper));

    private void UpdateManipulationWrapper(
      bool isUserInteractionEnabled,
      [NotNull] IManipulationWrapper manipulationWrapper)
    {
      if (isUserInteractionEnabled)
        manipulationWrapper.EnableManipulations();
      else
        manipulationWrapper.DisableManipulations();
    }

    private void UpdateManipulationWrapperScreenOffset([NotNull] IManipulationWrapper manipulationWrapper) => manipulationWrapper.SetScreenCenterOffset(new Point(this.Viewport.Width * 0.5, this.Viewport.Height * 0.5));
  }
}
