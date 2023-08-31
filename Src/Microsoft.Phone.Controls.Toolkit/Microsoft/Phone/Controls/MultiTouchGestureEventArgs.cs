// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.MultiTouchGestureEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  public class MultiTouchGestureEventArgs : GestureEventArgs
  {
    protected Point GestureOrigin2 { get; private set; }

    protected Point TouchPosition2 { get; private set; }

    internal MultiTouchGestureEventArgs(
      Point gestureOrigin,
      Point gestureOrigin2,
      Point position,
      Point position2)
      : base(gestureOrigin, position)
    {
      this.GestureOrigin2 = gestureOrigin2;
      this.TouchPosition2 = position2;
    }

    public Point GetPosition(UIElement relativeTo, int index)
    {
      if (index == 0)
        return this.GetPosition(relativeTo);
      if (index == 1)
        return GestureEventArgs.GetPosition(relativeTo, this.TouchPosition2);
      throw new ArgumentOutOfRangeException(nameof (index));
    }
  }
}
