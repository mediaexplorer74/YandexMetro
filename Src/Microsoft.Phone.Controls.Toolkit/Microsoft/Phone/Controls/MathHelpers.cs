// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.MathHelpers
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Xna.Framework;
using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  internal static class MathHelpers
  {
    public static double GetAngle(double deltaX, double deltaY)
    {
      double num = Math.Atan2(deltaY, deltaX);
      if (num < 0.0)
        num = 2.0 * Math.PI + num;
      return num * 360.0 / (2.0 * Math.PI);
    }

    public static double GetDistance(Point p0, Point p1)
    {
      double num1 = p0.X - p1.X;
      double num2 = p0.Y - p1.Y;
      return Math.Sqrt(num1 * num1 + num2 * num2);
    }

    public static Point ToPoint(this Vector2 v) => new Point((double) v.X, (double) v.Y);
  }
}
