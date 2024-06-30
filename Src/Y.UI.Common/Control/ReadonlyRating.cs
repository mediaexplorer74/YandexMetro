// Y.UI.Common.Control.ReadonlyRating


using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace Y.UI.Common.Control
{
  public partial class ReadonlyRating : UserControl //System.Windows.Controls.Control
  {
    private static BitmapImage _image = (BitmapImage) null;
    private Rectangle rectValue;
    private ImageBrush imgBrush;
    private bool _initialized;
    public static readonly DependencyProperty ValueProperty 
            = DependencyProperty.Register(nameof (Value), typeof (double), 
                typeof (ReadonlyRating), new PropertyMetadata((object) 0.0, 
                    new PropertyChangedCallback(ReadonlyRating.OnValueChanged)));

    public static readonly DependencyProperty RatingFillBrushProperty 
            = DependencyProperty.Register(nameof (RatingFillBrush), typeof (Brush),
                typeof (ReadonlyRating), new PropertyMetadata((PropertyChangedCallback) null));

        public ReadonlyRating()
        {
            this.DefaultStyleKey = (object)typeof(ReadonlyRating);
        }

        public double Value
    {
      get => (double) ((DependencyObject) this).GetValue(ReadonlyRating.ValueProperty);
      set => ((DependencyObject) this).SetValue(ReadonlyRating.ValueProperty, (object) value);
    }

    public Brush RatingFillBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(ReadonlyRating.RatingFillBrushProperty);
      set => ((DependencyObject) this).SetValue(ReadonlyRating.RatingFillBrushProperty, (object) value);
    }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ReadonlyRating).UpdateValue((double)e.NewValue);
        }

        public virtual void OnApplyTemplate()
    {
      this.OnApplyTemplate();
      this.rectValue = (Rectangle) this.GetTemplateChild("rectValue");
      this.imgBrush = (ImageBrush) this.GetTemplateChild("imgBrush");
      this.imgBrush.ImageSource = ReadonlyRating.GetImageSourceLazy();
      this._initialized = true;
      if (this.Value <= 0.0)
        return;
      this.UpdateValue(this.Value);
    }

    private void UpdateValue(double ratingValue)
    {
      if (ratingValue < 0.0 || !this._initialized)
        return;
      ((FrameworkElement) this.rectValue).Width 
                = Math.Round(ratingValue * 2.0) / 2.0 * ((FrameworkElement) this).Width / 5.0;
    }

    private static ImageSource GetImageSourceLazy()
    {
      if (ReadonlyRating._image == null)
        ReadonlyRating._image = new BitmapImage()
        {
          CreateOptions = (BitmapCreateOptions) 16,
          UriSource = new Uri("/Images/5stars.png", UriKind.RelativeOrAbsolute)
        };
      return (ImageSource) ReadonlyRating._image;
    }
  }
}
