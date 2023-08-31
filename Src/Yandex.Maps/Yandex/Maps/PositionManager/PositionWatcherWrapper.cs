// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.PositionWatcherWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Threading;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls.PositionManager.Event;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Media;
using Yandex.Positioning;
using Yandex.Positioning.Interfaces;

namespace Yandex.Maps.PositionManager
{
  [UsedImplicitly]
  internal class PositionWatcherWrapper : IPositionWatcherWrapper, IManipulationWrapperInitializable
  {
    private readonly IGeoPixelConverter _geoPixelConverter;
    private IManipulationWrapper _manipulationWrapper;
    private readonly object _positionSyncObj = new object();
    private readonly IPositionWatcher _positionWatcher;
    private readonly IViewportPointConveter _viewportPointConveter;
    private bool _canPinMarkerPoint;
    private bool _jumpToCurrentLocation;
    private GeoCoordinate _lastCoordinates;
    private Rect _viewport;
    private double _zoomLevel;

    public PositionWatcherWrapper(
      IPositionWatcher positionWatcher,
      IGeoPixelConverter geoPixelConverter,
      IViewportPointConveter viewportPointConveter)
    {
      if (positionWatcher == null)
        throw new ArgumentNullException(nameof (positionWatcher));
      if (geoPixelConverter == null)
        throw new ArgumentNullException(nameof (geoPixelConverter));
      if (viewportPointConveter == null)
        throw new ArgumentNullException(nameof (viewportPointConveter));
      this._positionWatcher = positionWatcher;
      this._geoPixelConverter = geoPixelConverter;
      this._viewportPointConveter = viewportPointConveter;
      this._positionWatcher.PositionChanged += new EventHandler<PositionChangedEventArgs>(this.PositionWatcherPositionChanged);
    }

    public event EventHandler<PositionMoveEventArgs> PositionMove;

    public void ScheduleJumpToCurrentLocation()
    {
      this._jumpToCurrentLocation = true;
      this.PinnedMarkerPointCenterScreenOffset = new Point?();
      this._canPinMarkerPoint = true;
    }

    public Point? PinnedMarkerPointCenterScreenOffset { get; private set; }

    public GeoPositionStatus Status => this._positionWatcher.Status;

    public GeoCoordinate LastCoordinates => this._lastCoordinates;

    public event EventHandler PositionChangedWithoutMapMoving;

    public void PositionChanged(double zoomLevel, Rect viewport)
    {
      lock (this._positionSyncObj)
      {
        this._viewport = viewport;
        this._zoomLevel = zoomLevel;
      }
    }

    private void ManipulationWrapperManipulationStarted(object sender, EventArgs e) => this.PinnedMarkerPointCenterScreenOffset = new Point?();

    private void OnPositionChangedWithoutMapMoving(EventArgs e)
    {
      EventHandler withoutMapMoving = this.PositionChangedWithoutMapMoving;
      if (withoutMapMoving == null)
        return;
      withoutMapMoving((object) this, e);
    }

    private static bool PointIsVisible(Point positionPoint, Rect viewPortRect) => viewPortRect.Left <= positionPoint.X && positionPoint.X <= viewPortRect.Right && viewPortRect.Top <= positionPoint.Y && positionPoint.Y <= viewPortRect.Bottom;

    private Point CoordinatesToViewportPoint(GeoCoordinate value) => this._viewportPointConveter.RelativePointToViewportPoint(this._geoPixelConverter.CoordinatesToRelativePoint(value), this._zoomLevel);

    private void PositionWatcherPositionChanged(object sender, PositionChangedEventArgs e) => ThreadPool.QueueUserWorkItem((WaitCallback) (argument => this.UpdatePinnedMarkerPointCenterScreenOffset(e)), (object) null);

    private void UpdatePinnedMarkerPointCenterScreenOffset(PositionChangedEventArgs e)
    {
      GeoCoordinate geoCoordinate = e.GeoPosition.GeoCoordinate;
      if (this._lastCoordinates != null && this._lastCoordinates.Equals((object) geoCoordinate))
        return;
      Point? nullable = this.PinnedMarkerPointCenterScreenOffset;
      if (this._canPinMarkerPoint && !nullable.HasValue)
      {
        lock (this._positionSyncObj)
        {
          Point viewportPoint = this.CoordinatesToViewportPoint(this._lastCoordinates ?? geoCoordinate);
          if (PositionWatcherWrapper.PointIsVisible(viewportPoint, this._viewport))
          {
            nullable = !this._jumpToCurrentLocation ? new Point?(new Point(this._viewport.Width * 0.5 - viewportPoint.X + this._viewport.X, this._viewport.Height * 0.5 - viewportPoint.Y + this._viewport.Y)) : new Point?(new Point(0.0, 0.0));
            this.PinnedMarkerPointCenterScreenOffset = nullable;
          }
        }
      }
      this._lastCoordinates = geoCoordinate;
      this._jumpToCurrentLocation = false;
      if (nullable.HasValue)
      {
        Point viewportPoint = this.CoordinatesToViewportPoint(geoCoordinate);
        Point point = nullable.Value;
        Point relativePoint = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(viewportPoint.X + point.X, viewportPoint.Y + point.Y), this._zoomLevel));
        this.OnPositionMove(new PositionMoveEventArgs()
        {
          RelativePoint = relativePoint
        });
      }
      else
        this.OnPositionChangedWithoutMapMoving(EventArgs.Empty);
    }

    private void OnPositionMove(PositionMoveEventArgs e)
    {
      EventHandler<PositionMoveEventArgs> positionMove = this.PositionMove;
      if (positionMove == null)
        return;
      positionMove((object) this, e);
    }

    public void ResetPositionFollowing()
    {
      this._canPinMarkerPoint = true;
      this.PinnedMarkerPointCenterScreenOffset = new Point?();
    }

    public void DisablePositionFollowing()
    {
      this._jumpToCurrentLocation = false;
      this._canPinMarkerPoint = false;
      this.PinnedMarkerPointCenterScreenOffset = new Point?();
    }

    public void Initialize(IManipulationWrapper manipulationWrapper)
    {
      if (manipulationWrapper == null)
        throw new ArgumentNullException(nameof (manipulationWrapper));
      if (this._manipulationWrapper != null)
        this._manipulationWrapper.ManipulationStarted -= new EventHandler(this.ManipulationWrapperManipulationStarted);
      this._manipulationWrapper = manipulationWrapper;
      this._manipulationWrapper.ManipulationStarted += new EventHandler(this.ManipulationWrapperManipulationStarted);
    }
  }
}
