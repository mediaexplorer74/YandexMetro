// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.KineticPositionManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Maps.PositionManager.InterimPointsEnumerators.Interfaces;
using Yandex.Media;
using Yandex.PAL.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.PositionManager
{
  internal class KineticPositionManager : IPositionManager, IDisposable
  {
    private readonly Position _currentPosition;
    private readonly object _interimPointsEnumeratorNotEmpty = new object();
    private readonly IInterimPointsHelper _interimPointsHelper;
    private readonly IMonitor _monitor;
    private readonly IPositionManagerRestictions _positionManagerRestictions;
    private readonly object _positionsQueueSync = new object();
    private readonly IStopwatch _stopwatch;
    private readonly Position _targetPosition;
    private readonly IZoomInfo _zoomInfo;
    private readonly IZoomLevelConverter _zoomLevelConverter;
    private bool _disposed;
    private IInterimPointsEnumerator _interimPointsEnumerator;
    private Rect _viewport;

    public KineticPositionManager(
      [NotNull] IZoomLevelConverter zoomLevelConverter,
      [NotNull] IZoomInfo zoomInfo,
      [NotNull] IPositionManagerRestictions positionManagerRestictions,
      [NotNull] IInterimPointsHelper interimPointsHelper,
      [NotNull] IThread thread,
      [NotNull] IStopwatch stopwatch,
      [NotNull] IMonitor monitor)
    {
      if (zoomLevelConverter == null)
        throw new ArgumentNullException(nameof (zoomLevelConverter));
      if (zoomInfo == null)
        throw new ArgumentNullException(nameof (zoomInfo));
      if (interimPointsHelper == null)
        throw new ArgumentNullException(nameof (interimPointsHelper));
      if (stopwatch == null)
        throw new ArgumentNullException(nameof (stopwatch));
      if (monitor == null)
        throw new ArgumentNullException(nameof (monitor));
      this._zoomLevelConverter = zoomLevelConverter;
      this._zoomInfo = zoomInfo;
      this._stopwatch = stopwatch;
      this._monitor = monitor;
      this.AnimationLevel = AnimationLevel.None;
      this._currentPosition = new Position();
      this._targetPosition = new Position();
      thread.Start(new Action(this.InterimPositionsWorker));
      this._interimPointsHelper = interimPointsHelper;
      this._positionManagerRestictions = positionManagerRestictions;
    }

    public Rect Viewport
    {
      get => this._viewport;
      set
      {
        this._viewport = value;
        this._positionManagerRestictions.Viewport = value;
        this.OnViewportChanged();
      }
    }

    public event EventHandler<PositionChangedEventArgs> PositionChanged;

    public event EventHandler<PositionChangedEventArgs> PointsEnumerationFinished;

    public AnimationLevel AnimationLevel { get; set; }

    public Position TargetPosition => this._targetPosition;

    public Position CurrentPosition => this._currentPosition;

    public void Dispose()
    {
      this.OnDispose();
      GC.SuppressFinalize((object) this);
    }

    public bool IsEnumerationInProcess
    {
      get
      {
        IInterimPointsEnumerator pointsEnumerator = this._interimPointsEnumerator;
        return pointsEnumerator != null && !pointsEnumerator.IsEnumerationFinished;
      }
    }

    public void MoveByVelocity(Point velocityInPixelsPerMillisecond)
    {
      lock (this._positionsQueueSync)
      {
        velocityInPixelsPerMillisecond = this._positionManagerRestictions.VelocityRestriction(velocityInPixelsPerMillisecond);
        this._interimPointsEnumerator = this._interimPointsHelper.GetVelocityInterimPointsEnumerator(this.CurrentPosition, velocityInPixelsPerMillisecond);
        this._stopwatch.Reset();
        lock (this._interimPointsEnumeratorNotEmpty)
          this._monitor.Pulse(this._interimPointsEnumeratorNotEmpty);
      }
    }

    public bool MoveTo(Point relativeTargetPoint, bool withAnimation = true)
    {
      this.TargetPosition.RelativePoint = relativeTargetPoint;
      return this.UpdateRelativePointAndZoomLevel(withAnimation);
    }

    public bool ZoomTo(double zoomLevel, Point? relativeScaleCenter = null, bool withAnimation = true) => this.ZoomToInternal(zoomLevel, withAnimation, relativeScaleCenter);

    public void FinalizeLastQueueIfExists()
    {
      Position newPosition = (Position) null;
      if (this._interimPointsEnumerator == null)
        return;
      lock (this._positionsQueueSync)
      {
        if (this._interimPointsEnumerator == null)
          return;
        if (this._interimPointsEnumerator.UseLastInterimPoint)
          newPosition = this._interimPointsEnumerator.GetLastInterimPoint(this._stopwatch.ElapsedMilliseconds);
        this._interimPointsEnumerator = (IInterimPointsEnumerator) null;
      }
      if (!(newPosition != (Position) null))
        return;
      this.SetRelativePointAndZoomLevel(newPosition);
    }

    private bool UpdateRelativePointAndZoomLevel(bool isFinal, Point? relativeScaleCenter = null)
    {
      if (!this.TargetPosition.IsInitialised)
        return true;
      if (isFinal && this.AnimationLevel == AnimationLevel.Full && this.CurrentPosition.IsInitialised && this._interimPointsHelper.DistanceIsSuitableForAnimation(this.CurrentPosition, this.TargetPosition))
      {
        this.StartMoveAndZoomToInternal(relativeScaleCenter);
        return false;
      }
      this.FinalizeLastQueueIfExists();
      this.SetRelativePointAndZoomLevel(this.TargetPosition, relativeScaleCenter);
      if (isFinal)
        this.OnPointsEnumerationFinished();
      return true;
    }

    private bool ZoomToInternal(
      double targetZoomLevel,
      bool withAnimation,
      Point? relativeScaleCenter = null)
    {
      if (targetZoomLevel > (double) this._zoomInfo.MaxVisibleZoom || targetZoomLevel < (double) this._zoomInfo.MinZoom)
        return true;
      double num = targetZoomLevel;
      while (!this._positionManagerRestictions.MapIsGreaterThanViewport((byte) Math.Floor(targetZoomLevel)))
      {
        if (num > this.CurrentPosition.Zoom)
          ++targetZoomLevel;
        else if (++targetZoomLevel > this.CurrentPosition.Zoom)
          return true;
      }
      this.TargetPosition.Zoom = targetZoomLevel;
      return this.UpdateRelativePointAndZoomLevel(withAnimation, relativeScaleCenter);
    }

    private void OnPointsEnumerationFinished(PositionChangedEventArgs e)
    {
      EventHandler<PositionChangedEventArgs> enumerationFinished = this.PointsEnumerationFinished;
      if (enumerationFinished == null)
        return;
      enumerationFinished((object) this, e);
    }

    private void OnPositionChanged(PositionChangedEventArgs e)
    {
      EventHandler<PositionChangedEventArgs> positionChanged = this.PositionChanged;
      if (positionChanged == null)
        return;
      positionChanged((object) this, e);
    }

    private void SetRelativePointAndZoomLevel(Position newPosition) => this.SetRelativePointAndZoomLevel(newPosition, newPosition.RelativeScaleCenter);

    private void SetRelativePointAndZoomLevel(Position newPosition, Point? relativeScaleCenter)
    {
      Point relativePoint = !relativeScaleCenter.HasValue ? newPosition.RelativePoint : this.RecalculateRelativeTargetPoint(newPosition.Zoom, relativeScaleCenter.Value);
      this._positionManagerRestictions.UpdateRelativePointIfViewportIsOutOfMap(ref relativePoint, newPosition.Zoom);
      newPosition.RelativePoint = relativePoint;
      this.CurrentPosition.RelativePoint = newPosition.RelativePoint;
      this.CurrentPosition.Zoom = newPosition.Zoom;
      this.OnPositionChanged(new PositionChangedEventArgs(this.CurrentPosition.RelativePoint, this.CurrentPosition.Zoom, PositionChangeType.Interim));
    }

    private Point RecalculateRelativeTargetPoint(double zoomLevel, Point relativeScaleCenter)
    {
      double stretchFactor = this._zoomLevelConverter.ZoomLevelToStretchFactor(this.CurrentPosition.Zoom - zoomLevel, (byte) 0);
      Point relativePoint = this.CurrentPosition.RelativePoint;
      double num1 = relativeScaleCenter.X - relativePoint.X;
      double num2 = relativeScaleCenter.Y - relativePoint.Y;
      return new Point(-num1 * stretchFactor + relativeScaleCenter.X, -num2 * stretchFactor + relativeScaleCenter.Y);
    }

    private void StartMoveAndZoomToInternal(Point? relativeScaleCenter)
    {
      lock (this._positionsQueueSync)
      {
        this._interimPointsEnumerator = this._interimPointsHelper.GetInterimPointsEnumerator(this.CurrentPosition, this.TargetPosition, relativeScaleCenter);
        this._stopwatch.Restart();
        lock (this._interimPointsEnumeratorNotEmpty)
          this._monitor.Pulse(this._interimPointsEnumeratorNotEmpty);
      }
    }

    private void OnViewportChanged()
    {
      if (this.CurrentPosition == (Position) null || !this.CurrentPosition.IsInitialised)
        return;
      do
        ;
      while (!this.CheckZoomlevelAndZoomOutIfNeeded(this.CurrentPosition.Zoom));
    }

    private bool CheckZoomlevelAndZoomOutIfNeeded(double zoomLevel)
    {
      double zoomLevel1 = Math.Floor(zoomLevel);
      if (this._positionManagerRestictions.MapIsGreaterThanViewport((byte) zoomLevel1))
        return true;
      this.ZoomTo(Math.Ceiling(zoomLevel1 + 0.1), new Point?(), false);
      return false;
    }

    private void InterimPositionsWorker()
    {
      while (!this._disposed)
      {
        if (this.IsEnumerationInProcess)
        {
          lock (this._interimPointsEnumeratorNotEmpty)
            this._monitor.Wait(this._interimPointsEnumeratorNotEmpty, this._interimPointsHelper.StepTimeout);
        }
        else
        {
          lock (this._interimPointsEnumeratorNotEmpty)
            this._monitor.Wait(this._interimPointsEnumeratorNotEmpty);
        }
        bool flag1 = false;
        bool flag2 = false;
        Position newPosition = (Position) null;
        lock (this._positionsQueueSync)
        {
          if (this._interimPointsEnumerator != null)
          {
            double elapsedMilliseconds = this._stopwatch.ElapsedMilliseconds;
            this._stopwatch.Restart();
            newPosition = this._interimPointsEnumerator.GetNextInterimPoint(elapsedMilliseconds);
            flag2 = newPosition != (Position) null && (!this._interimPointsEnumerator.StopInterimPointsEnumerationIfCenterPointIsNotValidForZoomlevel || this._positionManagerRestictions.CheckRelativePointIfViewportIsOutOfMap(newPosition.RelativePoint, newPosition.Zoom));
            flag1 = this._interimPointsEnumerator.IsEnumerationFinished;
            if (flag1)
              this.TargetPosition.RelativePoint = this.CurrentPosition.RelativePoint;
          }
        }
        if (flag2)
          this.SetRelativePointAndZoomLevel(newPosition);
        if (flag1)
          this.OnPointsEnumerationFinished();
      }
    }

    private void OnPointsEnumerationFinished() => this.OnPointsEnumerationFinished(new PositionChangedEventArgs(this.CurrentPosition.RelativePoint, this.CurrentPosition.Zoom, PositionChangeType.Final));

    ~KineticPositionManager() => this.OnDispose();

    private void OnDispose()
    {
      this._disposed = true;
      try
      {
        lock (this._interimPointsEnumeratorNotEmpty)
          this._monitor.Pulse(this._interimPointsEnumeratorNotEmpty);
      }
      catch (ObjectDisposedException ex)
      {
      }
    }
  }
}
