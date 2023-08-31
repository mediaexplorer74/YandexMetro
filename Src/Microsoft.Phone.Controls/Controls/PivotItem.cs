// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PivotItem
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(Name = "Right", GroupName = "Position States")]
  [TemplateVisualState(Name = "Left", GroupName = "Position States")]
  [TemplateVisualState(Name = "Center", GroupName = "Position States")]
  public class PivotItem : ContentControl
  {
    private const string PivotPositionsGroupName = "Position States";
    private const string PivotStateCenter = "Center";
    private const string PivotStateLeft = "Left";
    private const string PivotStateRight = "Right";
    private const string ContentName = "Content";
    private AnimationDirection? _firstAnimation = new AnimationDirection?();
    private AnimationDirection _direction;
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (PivotItem), new PropertyMetadata((PropertyChangedCallback) null));

    public object Header
    {
      get => ((DependencyObject) this).GetValue(PivotItem.HeaderProperty);
      set => ((DependencyObject) this).SetValue(PivotItem.HeaderProperty, value);
    }

    public PivotItem()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PivotItem);
      ((Control) this).DefaultStyleKey = (object) typeof (PivotItem);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.PivotItem_Loaded);
    }

    private void PivotItem_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.PivotItem_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PivotItem);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotItem);
      Size size = ((FrameworkElement) this).ArrangeOverride(finalSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotItem);
      return size;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotItem);
      Size size = ((FrameworkElement) this).MeasureOverride(availableSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotItem);
      return size;
    }

    public virtual void OnApplyTemplate()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotItem);
      ((FrameworkElement) this).OnApplyTemplate();
      AnimationDirection? firstAnimation = this._firstAnimation;
      this.MoveTo(AnimationDirection.Center);
      if (firstAnimation.HasValue)
        this.MoveTo(firstAnimation.Value);
      this._firstAnimation = new AnimationDirection?(AnimationDirection.Center);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotItem);
    }

    internal void MoveTo(AnimationDirection direction)
    {
      bool useTransitions = direction != AnimationDirection.Center;
      this._direction = direction;
      if (!this._firstAnimation.HasValue && useTransitions)
        this._firstAnimation = new AnimationDirection?(direction);
      else
        this.UpdateVisualStates(useTransitions);
    }

    private void UpdateVisualStates(bool useTransitions) => VisualStateManager.GoToState((Control) this, PivotItem.StateNameFromAnimationDirection(this._direction), useTransitions);

    private static string StateNameFromAnimationDirection(AnimationDirection direction)
    {
      switch (direction)
      {
        case AnimationDirection.Center:
          return "Center";
        case AnimationDirection.Left:
          return "Left";
        case AnimationDirection.Right:
          return "Right";
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
