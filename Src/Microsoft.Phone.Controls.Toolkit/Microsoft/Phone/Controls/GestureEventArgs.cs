// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.GestureEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  public class GestureEventArgs : EventArgs
  {
    protected Point GestureOrigin { get; private set; }

    protected Point TouchPosition { get; private set; }

    internal GestureEventArgs(Point gestureOrigin, Point position)
    {
      this.GestureOrigin = gestureOrigin;
      this.TouchPosition = position;
    }

    public object OriginalSource { get; internal set; }

    public bool Handled { get; set; }

    public Point GetPosition(UIElement relativeTo) => GestureEventArgs.GetPosition(relativeTo, this.TouchPosition);

    protected static Point GetPosition(UIElement relativeTo, Point point)
    {
      if (relativeTo == null)
        relativeTo = Application.Current.RootVisual;
      return relativeTo != null ? relativeTo.TransformToVisual((UIElement) null).Inverse.Transform(point) : point;
    }
  }
}
