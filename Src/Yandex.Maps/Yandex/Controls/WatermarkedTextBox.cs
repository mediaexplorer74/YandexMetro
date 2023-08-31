// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.WatermarkedTextBox
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;
using System.Windows.Controls;

namespace Yandex.Controls
{
  internal class WatermarkedTextBox : TextBox
  {
    public static readonly DependencyProperty IsWatermarkedProperty = DependencyProperty.Register(nameof (IsWatermarked), typeof (bool), typeof (WatermarkedTextBox), new PropertyMetadata((object) false));
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof (Watermark), typeof (string), typeof (WatermarkedTextBox), (PropertyMetadata) null);

    public WatermarkedTextBox() => ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.FilterTextBoxLoaded);

    private void FilterTextBoxLoaded(object sender, RoutedEventArgs e)
    {
      ((UIElement) this).GotFocus += new RoutedEventHandler(this.OnGotFocus);
      ((UIElement) this).LostFocus += new RoutedEventHandler(this.OnLostFocus);
      this.OnLostFocus((object) this, (RoutedEventArgs) null);
    }

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
      if (this.Watermark == null || !(this.Text == this.Watermark) || this.IsWatermarked)
        return;
      this.IsWatermarked = true;
      this.Text = "";
      ((Control) this).FontStyle = FontStyles.Normal;
      ((Control) this).Foreground.Opacity = 1.0;
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
      if (this.Watermark == null || !(this.Text == ""))
        return;
      this.IsWatermarked = false;
      this.Text = this.Watermark;
      ((Control) this).FontStyle = FontStyles.Italic;
      ((Control) this).Foreground.Opacity = 0.5;
    }

    public bool IsWatermarked
    {
      get => (bool) ((DependencyObject) this).GetValue(WatermarkedTextBox.IsWatermarkedProperty);
      private set => ((DependencyObject) this).SetValue(WatermarkedTextBox.IsWatermarkedProperty, (object) value);
    }

    public string Watermark
    {
      get => (string) ((DependencyObject) this).GetValue(WatermarkedTextBox.WatermarkProperty);
      set => ((DependencyObject) this).SetValue(WatermarkedTextBox.WatermarkProperty, (object) value);
    }
  }
}
