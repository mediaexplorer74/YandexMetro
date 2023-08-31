// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.InvertButton
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Y.UI.Common.Control
{
  public class InvertButton : Button
  {
    private const string OpacityImageBrushName = "OpacityImageBrush";
    private const string ContentBodyName = "ContentBody";
    private const string ImageElementName = "ButtonForeground";
    protected ImageBrush OpacityImageBrush;
    protected ContentControl ContentBody;
    protected Shape ImageElement;
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (InvertButton), new PropertyMetadata((object) (Orientation) 0));
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (ImageSource), typeof (InvertButton), new PropertyMetadata(new PropertyChangedCallback(InvertButton.OnImageSource)));
    public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(nameof (ImageMargin), typeof (Thickness), typeof (InvertButton), new PropertyMetadata((PropertyChangedCallback) null));

    public virtual void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.OpacityImageBrush = ((System.Windows.Controls.Control) this).GetTemplateChild("OpacityImageBrush") as ImageBrush;
      this.ContentBody = ((System.Windows.Controls.Control) this).GetTemplateChild("ContentBody") as ContentControl;
      this.ImageElement = ((System.Windows.Controls.Control) this).GetTemplateChild("ButtonForeground") as Shape;
      if (this.ImageSource != null)
        this.SetImageBrush(this.ImageSource);
      if (this.ContentBody == null)
        return;
      double num = -(((System.Windows.Controls.Control) this.ContentBody).FontSize / 8.0);
      ((FrameworkElement) this.ContentBody).Margin = new Thickness(0.0, -(((System.Windows.Controls.Control) this.ContentBody).FontSize / 2.0) - num, 0.0, num);
    }

    public Orientation Orientation
    {
      get => (Orientation) ((DependencyObject) this).GetValue(InvertButton.OrientationProperty);
      set => ((DependencyObject) this).SetValue(InvertButton.OrientationProperty, (object) value);
    }

    public ImageSource ImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(InvertButton.ImageSourceProperty);
      set => ((DependencyObject) this).SetValue(InvertButton.ImageSourceProperty, (object) value);
    }

    public Thickness ImageMargin
    {
      get => (Thickness) ((DependencyObject) this).GetValue(InvertButton.ImageMarginProperty);
      set => ((DependencyObject) this).SetValue(InvertButton.ImageMarginProperty, (object) value);
    }

    private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      if (!(o is InvertButton invertButton) || e.NewValue == e.OldValue)
        return;
      invertButton.SetImageBrush(e.NewValue as ImageSource);
    }

    private void SetImageBrush(ImageSource brush)
    {
      if (this.OpacityImageBrush == null)
        return;
      this.OpacityImageBrush.ImageSource = brush;
    }
  }
}
