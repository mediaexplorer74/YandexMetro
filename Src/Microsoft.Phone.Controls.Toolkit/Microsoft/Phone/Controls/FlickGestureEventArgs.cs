// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.FlickGestureEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  public class FlickGestureEventArgs : GestureEventArgs
  {
    private Point _velocity;

    internal FlickGestureEventArgs(Point hostOrigin, Point velocity)
      : base(hostOrigin, hostOrigin)
    {
      this._velocity = velocity;
    }

    public double HorizontalVelocity => this._velocity.X;

    public double VerticalVelocity => this._velocity.Y;

    public double Angle => MathHelpers.GetAngle(this._velocity.X, this._velocity.Y);

    public Orientation Direction => Math.Abs(this._velocity.X) < Math.Abs(this._velocity.Y) ? (Orientation) 0 : (Orientation) 1;
  }
}
