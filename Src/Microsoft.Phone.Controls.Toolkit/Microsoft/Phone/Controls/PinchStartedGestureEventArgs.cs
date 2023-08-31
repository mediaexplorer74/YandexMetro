// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PinchStartedGestureEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;

namespace Microsoft.Phone.Controls
{
  public class PinchStartedGestureEventArgs : MultiTouchGestureEventArgs
  {
    internal PinchStartedGestureEventArgs(
      Point gestureOrigin,
      Point gestureOrigin2,
      Point pinch,
      Point pinch2)
      : base(gestureOrigin, gestureOrigin2, pinch, pinch2)
    {
    }

    public double Distance => MathHelpers.GetDistance(this.TouchPosition, this.TouchPosition2);

    public double Angle => MathHelpers.GetAngle(this.TouchPosition2.X - this.TouchPosition.X, this.TouchPosition2.Y - this.TouchPosition.Y);
  }
}
