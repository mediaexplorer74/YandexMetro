// Decompiled with JetBrains decompiler
// Type: Yandex.Input.PageTouchWrapper`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Yandex.Input.Events;
using Yandex.Input.Interfaces;

namespace Yandex.Input
{
  internal class PageTouchWrapper<TPage> : ITouchWrapper where TPage : Page
  {
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly Frame _frame;

    public PageTouchWrapper()
    {
      this._stopwatch.Start();
      this._frame = (Frame) Application.Current.RootVisual;
      this._frame.Navigated += (NavigatedEventHandler) ((param0, param1) => this.CheckSubscription());
      this.CheckSubscription();
    }

    private void CheckSubscription()
    {
      if (((ContentControl) this._frame).Content is TPage)
      {
        Touch.FrameReported -= new TouchFrameEventHandler(this.TouchFrameReported);
        Touch.FrameReported += new TouchFrameEventHandler(this.TouchFrameReported);
      }
      else
        Touch.FrameReported -= new TouchFrameEventHandler(this.TouchFrameReported);
    }

    private void TouchFrameReported(object sender, TouchFrameEventArgs e)
    {
      TouchPointCollection touchPoints;
      TouchPoint primaryTouchPoint1;
      try
      {
        touchPoints = e.GetTouchPoints((UIElement) this._frame);
        if (touchPoints == null)
          return;
        primaryTouchPoint1 = e.GetPrimaryTouchPoint((UIElement) this._frame);
      }
      catch
      {
        return;
      }
      ITouchPoint primaryTouchPoint2 = (ITouchPoint) new TouchPoint(primaryTouchPoint1);
      if (primaryTouchPoint1.TouchDevice != null)
        primaryTouchPoint2.DirectlyOver = (object) primaryTouchPoint1.TouchDevice.DirectlyOver;
      this.OnFrameReported(new TouchEventArgs((double) this._stopwatch.ElapsedMilliseconds, (IList<ITouchPoint>) ((IEnumerable<TouchPoint>) touchPoints).Select<TouchPoint, ITouchPoint>((Func<TouchPoint, ITouchPoint>) (t => (ITouchPoint) new TouchPoint(t))).ToList<ITouchPoint>(), primaryTouchPoint2));
    }

    public event EventHandler<TouchEventArgs> FrameReported;

    protected virtual void OnFrameReported(TouchEventArgs e)
    {
      EventHandler<TouchEventArgs> frameReported = this.FrameReported;
      if (frameReported == null)
        return;
      frameReported((object) this, e);
    }
  }
}
