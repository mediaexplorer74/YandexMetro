// Decompiled with JetBrains decompiler
// Type: Yandex.Input.TouchWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Yandex.Input.Events;
using Yandex.Input.Interfaces;

namespace Yandex.Input
{
  [ComVisible(false)]
  public sealed class TouchWrapper : DependencyObject, ITouchWrapper
  {
    private readonly Stopwatch _stopwatch;
    private PhoneApplicationPage _controlPage;

    public event EventHandler<TouchEventArgs> FrameReported;

    public TouchWrapper()
      : this(new Stopwatch())
    {
    }

    public TouchWrapper(Stopwatch stopwatch)
    {
      this._stopwatch = stopwatch != null ? stopwatch : throw new ArgumentNullException(nameof (stopwatch));
      this._stopwatch.Start();
      Touch.FrameReported += new TouchFrameEventHandler(this.TouchFrameReported);
    }

    private void TouchFrameReported(object sender, TouchFrameEventArgs e)
    {
      if ((this._controlPage ?? (this._controlPage = this.GetControlPage())) != ((ContentControl) Application.Current.RootVisual).Content)
        return;
      TouchPointCollection touchPoints;
      TouchPoint primaryTouchPoint1;
      try
      {
        touchPoints = e.GetTouchPoints((UIElement) this.Control);
        if (touchPoints == null)
          return;
        primaryTouchPoint1 = e.GetPrimaryTouchPoint((UIElement) this.Control);
      }
      catch (Exception ex)
      {
        return;
      }
      ITouchPoint primaryTouchPoint2 = (ITouchPoint) new TouchPoint(primaryTouchPoint1);
      if (primaryTouchPoint1.TouchDevice != null)
        primaryTouchPoint2.DirectlyOver = (object) primaryTouchPoint1.TouchDevice.DirectlyOver;
      this.OnFrameReported(new TouchEventArgs((double) this._stopwatch.ElapsedMilliseconds, (IList<ITouchPoint>) ((IEnumerable<TouchPoint>) touchPoints).Select<TouchPoint, ITouchPoint>((Func<TouchPoint, ITouchPoint>) (t => (ITouchPoint) new TouchPoint(t))).ToList<ITouchPoint>(), primaryTouchPoint2));
    }

    private void OnFrameReported(TouchEventArgs e)
    {
      EventHandler<TouchEventArgs> frameReported = this.FrameReported;
      if (frameReported == null)
        return;
      frameReported((object) this, e);
    }

    public FrameworkElement Control { get; set; }

    [CanBeNull]
    private PhoneApplicationPage GetControlPage() => this.Control == null ? (PhoneApplicationPage) null : ((DependencyObject) this.Control).GetVisualAncestorsAndSelf().OfType<PhoneApplicationPage>().FirstOrDefault<PhoneApplicationPage>();
  }
}
