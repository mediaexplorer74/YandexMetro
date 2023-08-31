// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PanoramaItem
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  public class PanoramaItem : ContentControl
  {
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (PanoramaItem), (PropertyMetadata) null);
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (PanoramaItem), (PropertyMetadata) null);
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (PanoramaItem), new PropertyMetadata((object) (Orientation) 0, new PropertyChangedCallback(PanoramaItem.OnOrientationChanged)));

    internal int StartPosition { get; set; }

    internal int ItemWidth { get; set; }

    public PanoramaItem()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaItem);
      ((Control) this).DefaultStyleKey = (object) typeof (PanoramaItem);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.PanoramaItem_Loaded);
    }

    private void PanoramaItem_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.PanoramaItem_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaItem);
    }

    public object Header
    {
      get => ((DependencyObject) this).GetValue(PanoramaItem.HeaderProperty);
      set => ((DependencyObject) this).SetValue(PanoramaItem.HeaderProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(PanoramaItem.HeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(PanoramaItem.HeaderTemplateProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) ((DependencyObject) this).GetValue(PanoramaItem.OrientationProperty);
      set => ((DependencyObject) this).SetValue(PanoramaItem.OrientationProperty, (object) value);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaItem);
      Size size = ((FrameworkElement) this).MeasureOverride(availableSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaItem);
      return size;
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaItem);
      Size size = ((FrameworkElement) this).ArrangeOverride(finalSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaItem);
      return size;
    }

    private static void OnOrientationChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      PanoramaItem panoramaItem = (PanoramaItem) obj;
      ((UIElement) panoramaItem).InvalidateMeasure();
      if (!(VisualTreeHelper.GetParent((DependencyObject) panoramaItem) is FrameworkElement parent))
        return;
      ((UIElement) parent).InvalidateMeasure();
    }
  }
}
