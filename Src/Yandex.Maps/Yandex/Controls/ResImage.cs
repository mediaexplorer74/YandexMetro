// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ResImage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Yandex.Controls
{
  internal class ResImage : DependencyObject
  {
    public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof (ResSource), typeof (ResImage), new PropertyMetadata(new PropertyChangedCallback(ResImage.SourcePropertyChangedCallback)));

    public static ResSource GetSource(Image element) => element != null ? (ResSource) ((DependencyObject) element).GetValue(ResImage.SourceProperty) : throw new ArgumentNullException(nameof (element));

    public static void SetSource(Image element, ResSource value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      ((DependencyObject) element).SetValue(ResImage.SourceProperty, (object) value);
    }

    private static void SourcePropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is Image image))
        return;
      if (!(e.NewValue is ResSource newValue))
        return;
      try
      {
        using (MemoryStream memoryStream = new MemoryStream((byte[]) new ResourceManager(newValue.BaseName, newValue.Assembly).GetObject(newValue.ResourceName)))
        {
          BitmapImage bitmapImage = new BitmapImage();
          ((BitmapSource) bitmapImage).SetSource((Stream) memoryStream);
          image.Source = (ImageSource) bitmapImage;
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
