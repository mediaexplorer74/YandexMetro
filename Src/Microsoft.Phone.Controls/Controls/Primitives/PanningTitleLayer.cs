// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.PanningTitleLayer
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Phone.Controls.Primitives
{
  public class PanningTitleLayer : PanningLayer
  {
    private double WidthAdjustment => (double) this.Owner.ViewportWidth * 0.625;

    protected override double PanRate
    {
      get
      {
        double panRate = 1.0;
        if (this.Owner != null && this.ContentPresenter != null)
          panRate = (Math.Max((double) this.Owner.ViewportWidth, ((FrameworkElement) this.ContentPresenter).ActualWidth + this.WidthAdjustment) - (double) (this.Owner.ViewportWidth / 5 * 4)) / (double) Math.Max(this.Owner.ViewportWidth, this.Owner.ItemsWidth);
        return panRate;
      }
    }

    public override void Wraparound(int direction)
    {
      if (direction < 0)
        this.GoTo((int) ((this.ActualOffset + ((FrameworkElement) this.ContentPresenter).ActualWidth + this.WidthAdjustment) / this.PanRate), PanningLayer.Immediately, (Action) null);
      else
        this.GoTo((int) ((this.ActualOffset - ((FrameworkElement) this.ContentPresenter).ActualWidth - this.WidthAdjustment) / this.PanRate), PanningLayer.Immediately, (Action) null);
    }

    protected override void UpdateWrappingRectangles()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
      bool flag = !(this.Content is ItemsPresenter) || this.Owner.Panel == null || this.Owner.Panel.VisibleChildren.Count < 3;
      ((UIElement) this.RightWraparound).Visibility = this.IsStatic || !flag ? (Visibility) 1 : (Visibility) 0;
      if (!this.IsStatic && flag)
      {
        ((FrameworkElement) this.RightWraparound).Height = ((FrameworkElement) this.ContentPresenter).ActualHeight;
        ((FrameworkElement) this.RightWraparound).Width = (double) this.Owner.ViewportWidth;
        WriteableBitmap writeableBitmap = new WriteableBitmap(this.Owner.ViewportWidth, (int) ((FrameworkElement) this.ContentPresenter).ActualHeight);
        TranslateTransform translateTransform = new TranslateTransform();
        writeableBitmap.Render((UIElement) this.ContentPresenter, (Transform) translateTransform);
        writeableBitmap.Invalidate();
        ((Shape) this.RightWraparound).Fill = (Brush) new ImageBrush()
        {
          ImageSource = (ImageSource) writeableBitmap
        };
      }
      ((FrameworkElement) this.RightWraparound).Margin = new Thickness(this.WidthAdjustment + (double) (int) ((double) this.Owner.ViewportWidth * 0.1), 0.0, 0.0, 0.0);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
    }
  }
}
