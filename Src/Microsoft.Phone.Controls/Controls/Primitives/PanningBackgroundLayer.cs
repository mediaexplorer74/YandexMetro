// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.PanningBackgroundLayer
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Windows;

namespace Microsoft.Phone.Controls.Primitives
{
  public class PanningBackgroundLayer : PanningLayer
  {
    protected override double PanRate
    {
      get
      {
        double panRate = 1.0;
        if (this.Owner != null && this.ContentPresenter != null)
          panRate = (Math.Max((double) this.Owner.ViewportWidth, ((FrameworkElement) this.ContentPresenter).ActualWidth) - (double) (this.Owner.ViewportWidth / 5 * 4)) / (double) Math.Max(this.Owner.ViewportWidth, this.Owner.ItemsWidth);
        return panRate;
      }
    }
  }
}
