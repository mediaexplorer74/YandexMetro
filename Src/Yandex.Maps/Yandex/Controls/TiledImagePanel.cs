// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.TiledImagePanel
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Yandex.Controls
{
  [ComVisible(false)]
  public class TiledImagePanel : Canvas
  {
    public static readonly DependencyProperty TileImageSourceProperty = DependencyProperty.Register(nameof (TileImageSource), typeof (ImageSource), typeof (TiledImagePanel), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty TileImageSourceUriProperty = DependencyProperty.Register(nameof (TileImageSourceUri), typeof (string), typeof (TiledImagePanel), new PropertyMetadata((object) null, new PropertyChangedCallback(TiledImagePanel.TileImageSourceUriPropertyChangedCallback)));
    public static readonly DependencyProperty TileImageHeightProperty = DependencyProperty.Register(nameof (TileImageHeight), typeof (int), typeof (TiledImagePanel), new PropertyMetadata((object) 100));
    public static readonly DependencyProperty TileImageWidthProperty = DependencyProperty.Register(nameof (TileImageWidth), typeof (int), typeof (TiledImagePanel), new PropertyMetadata((object) 100));
    public static readonly DependencyProperty HorizontalPeriodProperty = DependencyProperty.Register(nameof (HorizontalPeriod), typeof (int), typeof (TiledImagePanel), new PropertyMetadata((object) 100));
    public static readonly DependencyProperty VerticalPeriodProperty = DependencyProperty.Register(nameof (VerticalPeriod), typeof (int), typeof (TiledImagePanel), new PropertyMetadata((object) 100));
    private volatile bool _exitLoop;
    private readonly object _childrenLock = new object();

    public ImageSource TileImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(TiledImagePanel.TileImageSourceProperty);
      set => ((DependencyObject) this).SetValue(TiledImagePanel.TileImageSourceProperty, (object) value);
    }

    public string TileImageSourceUri
    {
      get => (string) ((DependencyObject) this).GetValue(TiledImagePanel.TileImageSourceUriProperty);
      set => ((DependencyObject) this).SetValue(TiledImagePanel.TileImageSourceUriProperty, (object) value);
    }

    private static void TileImageSourceUriPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is TiledImagePanel tiledImagePanel))
        return;
      tiledImagePanel.TileImageSource = (ImageSource) new BitmapImage(new Uri((string) e.NewValue, UriKind.RelativeOrAbsolute));
    }

    public int TileImageHeight
    {
      get => (int) ((DependencyObject) this).GetValue(TiledImagePanel.TileImageHeightProperty);
      set => ((DependencyObject) this).SetValue(TiledImagePanel.TileImageHeightProperty, (object) value);
    }

    public int TileImageWidth
    {
      get => (int) ((DependencyObject) this).GetValue(TiledImagePanel.TileImageWidthProperty);
      set => ((DependencyObject) this).SetValue(TiledImagePanel.TileImageWidthProperty, (object) value);
    }

    public int HorizontalPeriod
    {
      get => (int) ((DependencyObject) this).GetValue(TiledImagePanel.HorizontalPeriodProperty);
      set => ((DependencyObject) this).SetValue(TiledImagePanel.HorizontalPeriodProperty, (object) value);
    }

    public int VerticalPeriod
    {
      get => (int) ((DependencyObject) this).GetValue(TiledImagePanel.VerticalPeriodProperty);
      set => ((DependencyObject) this).SetValue(TiledImagePanel.VerticalPeriodProperty, (object) value);
    }

    public TiledImagePanel() => ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.TiledImagePanelSizeChanged);

    private void TiledImagePanelSizeChanged(object sender, SizeChangedEventArgs e) => this.Rebuild();

    private void Rebuild()
    {
      double actualWidth = ((FrameworkElement) this).ActualWidth;
      double actualHeight = ((FrameworkElement) this).ActualHeight;
      this._exitLoop = true;
      lock (this._childrenLock)
      {
        this._exitLoop = false;
        int num = 0;
        int tileImageWidth = this.TileImageWidth;
        int tileImageHeight = this.TileImageHeight;
        for (int index1 = 0; (double) index1 < actualWidth && !this._exitLoop; index1 += tileImageWidth)
        {
          for (int index2 = 0; (double) index2 < actualHeight && !this._exitLoop; index2 += tileImageHeight)
          {
            Image image1;
            if (num < ((PresentationFrameworkCollection<UIElement>) ((Panel) this).Children).Count)
            {
              image1 = (Image) ((PresentationFrameworkCollection<UIElement>) ((Panel) this).Children)[num];
            }
            else
            {
              Image image2 = new Image();
              image2.Stretch = (Stretch) 0;
              ((FrameworkElement) image2).Width = (double) this.TileImageWidth;
              ((FrameworkElement) image2).Height = (double) this.TileImageHeight;
              image1 = image2;
              ((FrameworkElement) image1).SetBinding(Image.SourceProperty, new Binding()
              {
                Path = new PropertyPath("TileImageSource", new object[0]),
                Source = (object) this
              });
              ((PresentationFrameworkCollection<UIElement>) ((Panel) this).Children).Add((UIElement) image1);
            }
            ++num;
            Canvas.SetLeft((UIElement) image1, (double) index1);
            Canvas.SetTop((UIElement) image1, (double) index2);
          }
        }
        int count = ((PresentationFrameworkCollection<UIElement>) ((Panel) this).Children).Count;
        for (int index = num + 1; index < count && !this._exitLoop; ++index)
          ((PresentationFrameworkCollection<UIElement>) ((Panel) this).Children).RemoveAt(num + 1);
      }
    }
  }
}
