// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.ClipToBounds
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Phone.Controls.Primitives
{
  public class ClipToBounds : DependencyObject
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (ClipToBounds), new PropertyMetadata((object) false, new PropertyChangedCallback(ClipToBounds.OnIsEnabledPropertyChanged)));

    public static bool GetIsEnabled(DependencyObject obj) => (bool) obj.GetValue(ClipToBounds.IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(ClipToBounds.IsEnabledProperty, (object) value);

    private static void OnIsEnabledPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(obj is FrameworkElement frameworkElement))
        return;
      if ((bool) e.NewValue)
        frameworkElement.SizeChanged += new SizeChangedEventHandler(ClipToBounds.OnSizeChanged);
      else
        frameworkElement.SizeChanged -= new SizeChangedEventHandler(ClipToBounds.OnSizeChanged);
    }

    private static void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      ((UIElement) frameworkElement).Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, frameworkElement.ActualWidth, frameworkElement.ActualHeight)
      };
    }
  }
}
