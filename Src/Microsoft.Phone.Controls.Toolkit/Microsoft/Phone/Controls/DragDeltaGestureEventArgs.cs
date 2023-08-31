// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.DragDeltaGestureEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  public class DragDeltaGestureEventArgs : GestureEventArgs
  {
    internal DragDeltaGestureEventArgs(
      Point gestureOrigin,
      Point currentPosition,
      Point change,
      Orientation direction)
      : base(gestureOrigin, currentPosition)
    {
      this.HorizontalChange = change.X;
      this.VerticalChange = change.Y;
      this.Direction = direction;
    }

    public double HorizontalChange { get; private set; }

    public double VerticalChange { get; private set; }

    public Orientation Direction { get; private set; }
  }
}
