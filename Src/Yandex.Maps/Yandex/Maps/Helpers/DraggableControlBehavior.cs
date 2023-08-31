// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Helpers.DraggableControlBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Yandex.Input.Events;
using Yandex.Ioc;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Helpers.Events;
using Yandex.Maps.IoC;
using Yandex.Media;

namespace Yandex.Maps.Helpers
{
  internal class DraggableControlBehavior : Behavior<FrameworkElement>
  {
    private static readonly TimeSpan SpanCheckTimeout = TimeSpan.FromMilliseconds(25.0);
    private Point _currentViewportPosition;
    private Map _map;
    private Point _mapCenterOffset;
    private bool _subscribed;
    private IDisposable _timer;
    private IViewportPointConveter _viewportPointConveter;
    private readonly DraggableControlBehaviorHelper _draggableControlBehaviorHelper;

    public DraggableControlBehavior() => this._draggableControlBehaviorHelper = IocSingleton<ControlsIocInitializer>.Resolve<DraggableControlBehaviorHelper>();

    private double ZoomLevel => this._map.ZoomLevel;

    private IViewportPointConveter ViewportPointConveter => this._viewportPointConveter ?? (this._viewportPointConveter = this._viewportPointConveter = IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>());

    public event EventHandler<DragCompletedEventArgs> Completed;

    private void OnCompleted(DragCompletedEventArgs e)
    {
      EventHandler<DragCompletedEventArgs> completed = this.Completed;
      if (completed == null)
        return;
      completed((object) this, e);
    }

    protected override void OnAttached()
    {
      this.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObjectLoaded);
      this.AssociatedObject.Unloaded += new RoutedEventHandler(this.AssociatedObjectUnloaded);
      base.OnAttached();
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
    {
      if (!this._subscribed)
        return;
      ((UIElement) this.AssociatedObject).Hold -= new EventHandler<GestureEventArgs>(this.AssociatedObjectHold);
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      this._map = ((DependencyObject) this.AssociatedObject).GetVisualAncestors().OfType<Map>().SingleOrDefault<Map>();
      if (this._map == null)
        return;
      this._subscribed = true;
      ((UIElement) this.AssociatedObject).Hold += new EventHandler<GestureEventArgs>(this.AssociatedObjectHold);
    }

    private void AssociatedObjectHold(object sender, GestureEventArgs e)
    {
      if (this._map.TouchHandler == null)
        return;
      e.Handled = true;
      this._map.MapPresenter.DisableManipulations();
      this._map.MapPresenter.DisableMapReload();
      Point position = e.GetPosition((UIElement) this._map);
      Rect viewport = this._map.Viewport;
      this._currentViewportPosition = new Point(viewport.Left + position.X, viewport.Top + position.Y);
      this._mapCenterOffset = new Point(position.X - viewport.Width / 2.0, position.Y - viewport.Height / 2.0);
      MapLayer.SetLocation((DependencyObject) this.AssociatedObject, this.ViewportPointConveter.ViewportPointToCoordinates(this._currentViewportPosition, this.ZoomLevel));
      this._map.TouchHandler.ManipulationDelta += new EventHandler<TouchManipulationDeltaEventArgs>(this.TouchHandler_ManipulationDelta);
      this._map.TouchHandler.ManipulationCompleted += new EventHandler<TouchManipulationCompletedEventArgs>(this.TouchHandler_ManipulationCompleted);
      this._timer = Observable.Interval(DraggableControlBehavior.SpanCheckTimeout).ObserveOnDispatcher<long>().Subscribe<long>(new Action<long>(this.OnTimerTick));
    }

    private void OnTimerTick(long t)
    {
      if (!this._draggableControlBehaviorHelper.UpdateCurrentPosition(this._map.ContentPadding, this._map.Viewport, ref this._currentViewportPosition))
        return;
      this._map.Center = this.ViewportPointConveter.ViewportPointToCoordinates(new Point(this._currentViewportPosition.X - this._mapCenterOffset.X, this._currentViewportPosition.Y - this._mapCenterOffset.Y), this.ZoomLevel);
      MapLayer.SetLocation((DependencyObject) this.AssociatedObject, this.ViewportPointConveter.ViewportPointToCoordinates(this._currentViewportPosition, this.ZoomLevel));
    }

    private void TouchHandler_ManipulationCompleted(
      object sender,
      TouchManipulationCompletedEventArgs e)
    {
      if (this._timer != null)
        this._timer.Dispose();
      if (this._map.TouchHandler != null)
      {
        this._map.TouchHandler.ManipulationDelta -= new EventHandler<TouchManipulationDeltaEventArgs>(this.TouchHandler_ManipulationDelta);
        this._map.TouchHandler.ManipulationCompleted -= new EventHandler<TouchManipulationCompletedEventArgs>(this.TouchHandler_ManipulationCompleted);
      }
      this._map.MapPresenter.EnableManipulations();
      this._map.MapPresenter.EnableMapReload();
      this.OnCompleted(new DragCompletedEventArgs(this.ViewportPointConveter.ViewportPointToCoordinates(this._currentViewportPosition, this.ZoomLevel)));
    }

    private void TouchHandler_ManipulationDelta(object sender, TouchManipulationDeltaEventArgs e)
    {
      Point translation = e.DeltaManipulation.Translation;
      this._currentViewportPosition = new Point(translation.X + this._currentViewportPosition.X, translation.Y + this._currentViewportPosition.Y);
      this._mapCenterOffset.X += translation.X;
      this._mapCenterOffset.Y += translation.Y;
      MapLayer.SetLocation((DependencyObject) this.AssociatedObject, this.ViewportPointConveter.ViewportPointToCoordinates(this._currentViewportPosition, this.ZoomLevel));
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      if (this._timer != null)
        this._timer.Dispose();
      if (this.AssociatedObject == null)
        return;
      this.AssociatedObject.Loaded -= new RoutedEventHandler(this.AssociatedObjectLoaded);
      this.AssociatedObject.Unloaded -= new RoutedEventHandler(this.AssociatedObjectUnloaded);
    }
  }
}
