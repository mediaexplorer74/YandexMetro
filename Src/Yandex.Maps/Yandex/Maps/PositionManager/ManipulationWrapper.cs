// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionManager.ManipulationWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Input.Events;
using Yandex.Input.Interfaces;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.PositionManager.Event;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.PositionManager
{
  [UsedImplicitly]
  internal class ManipulationWrapper : IManipulationWrapper, ITouchHandlerInitializable
  {
    private readonly IViewportPointConveter _viewportPointConveter;
    private readonly IZoomLevelConverter _zoomLevelConverter;
    private Position _currentPosition;
    private Point _screenCenterOffset;
    private bool _manipulationsEnabled;
    private ITouchHandler _touchHandler;
    private readonly object _manipulationStatusLock = new object();
    private bool _manipulationsSubscribed;

    public ManipulationWrapper(
      IViewportPointConveter viewportPointConveter,
      IZoomLevelConverter zoomLevelConverter)
    {
      if (viewportPointConveter == null)
        throw new ArgumentNullException(nameof (viewportPointConveter));
      if (zoomLevelConverter == null)
        throw new ArgumentNullException(nameof (zoomLevelConverter));
      this._viewportPointConveter = viewportPointConveter;
      this._zoomLevelConverter = zoomLevelConverter;
    }

    public event EventHandler<ManipulationCompletedEventArgs> ManipulationCompleted;

    public event EventHandler ManipulationStarted;

    public event EventHandler<ManipulationMoveEventArgs> ManipulationMove;

    public event EventHandler<ManipulationZoomEventArgs> ManipulationZoom;

    public void EnableManipulations()
    {
      lock (this._manipulationStatusLock)
      {
        this._manipulationsEnabled = true;
        this.SubscribeManipulations(this._touchHandler);
      }
    }

    public void DisableManipulations()
    {
      lock (this._manipulationStatusLock)
      {
        this._manipulationsEnabled = true;
        this.UnsubscribeManipulations(this._touchHandler);
      }
    }

    private void SubscribeManipulations(ITouchHandler touchHandler)
    {
      if (touchHandler == null || this._manipulationsSubscribed)
        return;
      this._manipulationsSubscribed = true;
      touchHandler.ManipulationStarted += new EventHandler<TouchManipulationStartedEventArgs>(this.ManipulationSourceManipulationStarted);
      touchHandler.ManipulationCompleted += new EventHandler<TouchManipulationCompletedEventArgs>(this.ManipulationSourceManipulationCompleted);
      touchHandler.ManipulationDelta += new EventHandler<TouchManipulationDeltaEventArgs>(this.ManipulationSourceManipulationDelta);
    }

    private void UnsubscribeManipulations(ITouchHandler touchHandler)
    {
      if (!this._manipulationsSubscribed)
        return;
      this._manipulationsSubscribed = false;
      if (touchHandler == null)
        return;
      touchHandler.ManipulationStarted -= new EventHandler<TouchManipulationStartedEventArgs>(this.ManipulationSourceManipulationStarted);
      touchHandler.ManipulationCompleted -= new EventHandler<TouchManipulationCompletedEventArgs>(this.ManipulationSourceManipulationCompleted);
      touchHandler.ManipulationDelta -= new EventHandler<TouchManipulationDeltaEventArgs>(this.ManipulationSourceManipulationDelta);
    }

    public void SetScreenCenterOffset(Point value) => this._screenCenterOffset = value;

    public void SetCurrentPosition(Position value) => this._currentPosition = value;

    private void ManipulationSourceManipulationCompleted(
      object sender,
      TouchManipulationCompletedEventArgs e)
    {
      this.OnManipulationCompleated(new ManipulationCompletedEventArgs()
      {
        Velocity = e.TotalManipulation.SingleScale == 1.0 ? new Point?(e.FinalVelocities.LinearVelocity) : new Point?()
      });
    }

    private void ManipulationSourceManipulationStarted(
      object sender,
      TouchManipulationStartedEventArgs e)
    {
      this.OnManipulationStarted(EventArgs.Empty);
    }

    private void ManipulationSourceManipulationDelta(
      object sender,
      TouchManipulationDeltaEventArgs e)
    {
      Point relativePoint1 = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(e.DeltaManipulation.Translation, this._currentPosition.Zoom));
      double zoomLevel = this._zoomLevelConverter.StretchFactorToZoomLevel(this._zoomLevelConverter.SafeZoomLevelToStretchFactor(this._currentPosition.Zoom) * e.DeltaManipulation.SingleScale);
      this.OnManipulationMove(new ManipulationMoveEventArgs()
      {
        RelativePoint = new Point(-relativePoint1.X + this._currentPosition.RelativePoint.X, -relativePoint1.Y + this._currentPosition.RelativePoint.Y)
      });
      Point relativePoint2 = this._currentPosition.RelativePoint;
      Point relativePoint3 = this._viewportPointConveter.ViewportPointToRelativePoint(new ViewportPoint(new Point(e.DeltaManipulation.ScaleCenter.X - this._screenCenterOffset.X, e.DeltaManipulation.ScaleCenter.Y - this._screenCenterOffset.Y), this._currentPosition.Zoom));
      this.OnManipulationZoom(new ManipulationZoomEventArgs()
      {
        Zoom = zoomLevel,
        ScalePoint = new Point(relativePoint2.X + relativePoint3.X, relativePoint2.Y + relativePoint3.Y)
      });
    }

    private void OnManipulationStarted(EventArgs e)
    {
      EventHandler manipulationStarted = this.ManipulationStarted;
      if (manipulationStarted == null)
        return;
      manipulationStarted((object) this, e);
    }

    private void OnManipulationCompleated(ManipulationCompletedEventArgs e)
    {
      EventHandler<ManipulationCompletedEventArgs> manipulationCompleted = this.ManipulationCompleted;
      if (manipulationCompleted == null)
        return;
      manipulationCompleted((object) this, e);
    }

    private void OnManipulationZoom(ManipulationZoomEventArgs e)
    {
      EventHandler<ManipulationZoomEventArgs> manipulationZoom = this.ManipulationZoom;
      if (manipulationZoom == null)
        return;
      manipulationZoom((object) this, e);
    }

    private void OnManipulationMove(ManipulationMoveEventArgs e)
    {
      EventHandler<ManipulationMoveEventArgs> manipulationMove = this.ManipulationMove;
      if (manipulationMove == null)
        return;
      manipulationMove((object) this, e);
    }

    public void Initialize(ITouchHandler touchHandler) => this.TouchHandler = touchHandler;

    private ITouchHandler TouchHandler
    {
      get => this._touchHandler;
      set
      {
        lock (this._manipulationStatusLock)
        {
          if (this._touchHandler == value)
            return;
          this.UnsubscribeManipulations(this._touchHandler);
          this._touchHandler = value;
          if (!this._manipulationsEnabled)
            return;
          this.SubscribeManipulations(value);
        }
      }
    }
  }
}
