// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.PanningLayer
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Phone.Controls.Primitives
{
  [TemplatePart(Name = "LeftWraparound", Type = typeof (Rectangle))]
  [TemplatePart(Name = "PanningTransform", Type = typeof (TranslateTransform))]
  [TemplatePart(Name = "RightWraparound", Type = typeof (Rectangle))]
  [TemplatePart(Name = "ContentPresenter", Type = typeof (ContentPresenter))]
  [TemplatePart(Name = "LocalTransform", Type = typeof (TranslateTransform))]
  public class PanningLayer : ContentControl
  {
    private const string LocalTransformName = "LocalTransform";
    private const string PanningTransformName = "PanningTransform";
    private const string LeftWraparoundName = "LeftWraparound";
    private const string RightWraparoundName = "RightWraparound";
    private const string ContentPresenterName = "ContentPresenter";
    protected static readonly Duration Immediately = new Duration(TimeSpan.Zero);
    private readonly IEasingFunction _easingFunction = (IEasingFunction) new ExponentialEase()
    {
      Exponent = 5.0
    };
    private ContentPresenter contentPresenter;
    private TransformAnimator animator;
    private bool isStatic;

    protected TranslateTransform LocalTransform { get; set; }

    protected TranslateTransform PanningTransform { get; set; }

    protected Rectangle LeftWraparound { get; set; }

    protected Rectangle RightWraparound { get; set; }

    public PanningLayer()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanningLayer);
      ((Control) this).DefaultStyleKey = (object) typeof (PanningLayer);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.PanningLayer_Loaded);
    }

    private void PanningLayer_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.PanningLayer_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanningLayer);
    }

    public virtual void OnApplyTemplate()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.PanningLayer);
      ((FrameworkElement) this).OnApplyTemplate();
      this.LocalTransform = ((Control) this).GetTemplateChild("LocalTransform") as TranslateTransform;
      this.PanningTransform = ((Control) this).GetTemplateChild("PanningTransform") as TranslateTransform;
      this.LeftWraparound = ((Control) this).GetTemplateChild("LeftWraparound") as Rectangle;
      this.RightWraparound = ((Control) this).GetTemplateChild("RightWraparound") as Rectangle;
      this.ContentPresenter = ((Control) this).GetTemplateChild("ContentPresenter") as ContentPresenter;
      this.animator = this.PanningTransform != null ? new TransformAnimator(this.PanningTransform) : (TransformAnimator) null;
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.PanningLayer);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanningLayer);
      Size size = ((FrameworkElement) this).MeasureOverride(availableSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanningLayer);
      return size;
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanningLayer);
      Size size = ((FrameworkElement) this).ArrangeOverride(finalSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanningLayer);
      return size;
    }

    protected internal ContentPresenter ContentPresenter
    {
      get => this.contentPresenter;
      set
      {
        if (this.contentPresenter != null)
          ((FrameworkElement) this.contentPresenter).SizeChanged -= new SizeChangedEventHandler(this.OnContentSizeChanged);
        this.contentPresenter = value;
        if (this.contentPresenter == null)
          return;
        ((FrameworkElement) this.contentPresenter).SizeChanged += new SizeChangedEventHandler(this.OnContentSizeChanged);
      }
    }

    protected virtual double PanRate => 1.0;

    public void GoTo(int targetOffset, Duration duration, Action completionAction)
    {
      if (this.animator == null || this.IsStatic)
        return;
      this.animator.GoTo((double) (int) ((double) targetOffset * this.PanRate), duration, this._easingFunction, completionAction);
    }

    public virtual void Wraparound(int direction)
    {
      if (direction < 0)
        this.GoTo((int) ((this.ActualOffset + ((FrameworkElement) this.ContentPresenter).ActualWidth) / this.PanRate), PanningLayer.Immediately, (Action) null);
      else
        this.GoTo((int) ((this.ActualOffset - ((FrameworkElement) this.ContentPresenter).ActualWidth) / this.PanRate), PanningLayer.Immediately, (Action) null);
    }

    internal void Refresh() => this.UpdateWrappingRectangles();

    protected virtual void UpdateWrappingRectangles()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
      bool flag = !(this.Content is ItemsPresenter) || this.Owner.Panel == null || this.Owner.Panel.VisibleChildren.Count < 3;
      ((UIElement) this.LeftWraparound).Visibility = ((UIElement) this.RightWraparound).Visibility = this.IsStatic || !flag ? (Visibility) 1 : (Visibility) 0;
      if (!this.IsStatic && flag)
      {
        ((FrameworkElement) this.RightWraparound).Height = ((FrameworkElement) this.LeftWraparound).Height = ((FrameworkElement) this.ContentPresenter).ActualHeight;
        ((FrameworkElement) this.RightWraparound).Width = ((FrameworkElement) this.LeftWraparound).Width = (double) this.Owner.ViewportWidth;
        ((FrameworkElement) this.LeftWraparound).Margin = ((FrameworkElement) this.RightWraparound).Margin = ((FrameworkElement) this.ContentPresenter).Margin;
        WriteableBitmap writeableBitmap1 = new WriteableBitmap(this.Owner.ViewportWidth, (int) ((FrameworkElement) this.ContentPresenter).ActualHeight);
        TranslateTransform translateTransform = new TranslateTransform();
        writeableBitmap1.Render((UIElement) this.ContentPresenter, (Transform) translateTransform);
        writeableBitmap1.Invalidate();
        ((Shape) this.RightWraparound).Fill = (Brush) new ImageBrush()
        {
          ImageSource = (ImageSource) writeableBitmap1
        };
        WriteableBitmap writeableBitmap2 = new WriteableBitmap(this.Owner.ViewportWidth, (int) ((FrameworkElement) this.ContentPresenter).ActualHeight);
        translateTransform.X = (double) this.Owner.ViewportWidth - ((FrameworkElement) this.ContentPresenter).ActualWidth;
        writeableBitmap2.Render((UIElement) this.ContentPresenter, (Transform) translateTransform);
        writeableBitmap2.Invalidate();
        ((Shape) this.LeftWraparound).Fill = (Brush) new ImageBrush()
        {
          ImageSource = (ImageSource) writeableBitmap2
        };
      }
      if (this.LocalTransform != null)
      {
        double num = ((UIElement) this.LeftWraparound).Visibility == null ? -((FrameworkElement) this.LeftWraparound).Width - ((FrameworkElement) this.LeftWraparound).Margin.Left : 0.0;
        this.LocalTransform.X = this.IsStatic ? 0.0 : num - ((FrameworkElement) this.LeftWraparound).Margin.Right;
      }
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_UPDATEWRAPPING, PerfLog.PanningLayer);
    }

    internal bool IsStatic
    {
      get => this.isStatic;
      set
      {
        if (value == this.isStatic)
          return;
        this.isStatic = value;
        if (this.isStatic)
          this.ActualOffset = 0.0;
        else
          this.UpdateWrappingRectangles();
      }
    }

    internal Panorama Owner { get; set; }

    internal double ActualOffset
    {
      get => this.PanningTransform == null ? 0.0 : this.PanningTransform.X;
      private set
      {
        if (this.PanningTransform == null)
          return;
        this.PanningTransform.X = value;
      }
    }

    private void OnContentSizeChanged(object sender, SizeChangedEventArgs e) => this.UpdateWrappingRectangles();
  }
}
