// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Gestures.GestureHelper
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Phone.Gestures
{
  internal abstract class GestureHelper
  {
    private GestureHelper.DragLock _dragLock;
    private bool _dragging;
    private WeakReference _gestureSource;
    private Point _gestureOrigin;
    private readonly Size DeadZoneInPixels = new Size(12.0, 12.0);

    protected bool ShouldHandleAllDrags { get; private set; }

    protected UIElement Target { get; private set; }

    public event EventHandler<GestureEventArgs> GestureStart;

    public event EventHandler<FlickEventArgs> Flick;

    public event EventHandler<EventArgs> GestureEnd;

    public event EventHandler<DragEventArgs> HorizontalDrag;

    public event EventHandler<DragEventArgs> VerticalDrag;

    public static GestureHelper Create(UIElement target) => GestureHelper.Create(target, true);

    public static GestureHelper Create(UIElement target, bool shouldHandleAllDrags)
    {
      GestureHelper gestureHelper = (GestureHelper) new ManipulationGestureHelper(target, shouldHandleAllDrags);
      gestureHelper.Start();
      return gestureHelper;
    }

    protected GestureHelper(UIElement target, bool shouldHandleAllDrags)
    {
      this.Target = target;
      this.ShouldHandleAllDrags = shouldHandleAllDrags;
    }

    protected abstract void HookEvents();

    public void Start() => this.HookEvents();

    protected void NotifyDown(InputBaseArgs args)
    {
      GestureEventArgs args1 = new GestureEventArgs();
      this._gestureSource = new WeakReference((object) args.Source);
      this._gestureOrigin = args.Origin;
      this._dragLock = GestureHelper.DragLock.Unset;
      this._dragging = false;
      this.RaiseGestureStart(args1);
    }

    protected void NotifyMove(InputDeltaArgs args)
    {
      if (Math.Abs(args.CumulativeTranslation.X) > this.DeadZoneInPixels.Width || Math.Abs(args.CumulativeTranslation.Y) > this.DeadZoneInPixels.Height)
      {
        if (!this._dragging)
          this.ReleaseMouseCaptureAtGestureOrigin();
        this._dragging = true;
        if (this._dragLock == GestureHelper.DragLock.Unset)
        {
          double num = GestureHelper.AngleFromVector(args.CumulativeTranslation.X, args.CumulativeTranslation.Y) % 180.0;
          this._dragLock = num <= 45.0 || num >= 135.0 ? GestureHelper.DragLock.Horizontal : (num <= 45.0 || num >= 135.0 ? GestureHelper.DragLock.Free : GestureHelper.DragLock.Vertical);
        }
      }
      if (!this._dragging)
        return;
      this.RaiseDragEvents(args);
    }

    private void ReleaseMouseCaptureAtGestureOrigin()
    {
      if (this._gestureSource == null || !(this._gestureSource.Target is FrameworkElement target))
        return;
      foreach (UIElement inHostCoordinate in VisualTreeHelper.FindElementsInHostCoordinates(((UIElement) target).TransformToVisual((UIElement) null).Transform(this._gestureOrigin), Application.Current.RootVisual))
        inHostCoordinate.ReleaseMouseCapture();
    }

    protected void NotifyUp(InputCompletedArgs args)
    {
      EventArgs args1 = EventArgs.Empty;
      this._dragLock = GestureHelper.DragLock.Unset;
      this._dragging = false;
      if (args.IsInertial)
      {
        double num = GestureHelper.AngleFromVector(args.FinalLinearVelocity.X, args.FinalLinearVelocity.Y);
        if (num <= 45.0 || num >= 315.0)
          num = 0.0;
        else if (num >= 135.0 && num <= 225.0)
          num = 180.0;
        FlickEventArgs args2 = new FlickEventArgs()
        {
          Angle = num
        };
        this.ReleaseMouseCaptureAtGestureOrigin();
        this.RaiseFlick(args2);
      }
      else if (args.TotalTranslation.X != 0.0 || args.TotalTranslation.Y != 0.0)
      {
        DragEventArgs dragEventArgs = new DragEventArgs()
        {
          CumulativeDistance = args.TotalTranslation
        };
        dragEventArgs.MarkAsFinalTouchManipulation();
        args1 = (EventArgs) dragEventArgs;
      }
      this.RaiseGestureEnd(args1);
    }

    private void RaiseGestureStart(GestureEventArgs args) => SafeRaise.Raise<GestureEventArgs>(this.GestureStart, (object) this, args);

    private void RaiseFlick(FlickEventArgs args) => SafeRaise.Raise<FlickEventArgs>(this.Flick, (object) this, args);

    private void RaiseGestureEnd(EventArgs args) => SafeRaise.Raise<EventArgs>(this.GestureEnd, (object) this, args);

    private void RaiseDragEvents(InputDeltaArgs args)
    {
      DragEventArgs args1 = new DragEventArgs(args);
      if (args.DeltaTranslation.X != 0.0 && this._dragLock == GestureHelper.DragLock.Horizontal)
      {
        this.RaiseHorizontalDrag(args1);
      }
      else
      {
        if (args.DeltaTranslation.Y == 0.0 || this._dragLock != GestureHelper.DragLock.Vertical)
          return;
        this.RaiseVerticalDrag(args1);
      }
    }

    private void RaiseHorizontalDrag(DragEventArgs args) => SafeRaise.Raise<DragEventArgs>(this.HorizontalDrag, (object) this, args);

    private void RaiseVerticalDrag(DragEventArgs args) => SafeRaise.Raise<DragEventArgs>(this.VerticalDrag, (object) this, args);

    private static double AngleFromVector(double x, double y)
    {
      double num = Math.Atan2(y, x);
      if (num < 0.0)
        num = 2.0 * Math.PI + num;
      return num * 360.0 / (2.0 * Math.PI);
    }

    private enum DragLock
    {
      Unset,
      Free,
      Vertical,
      Horizontal,
    }
  }
}
