// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.ImageManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Yandex.Controls.Images
{
  internal class ImageManager : DependencyObject
  {
    public static readonly DependencyProperty UriSourceProperty = DependencyProperty.RegisterAttached("UriSource", typeof (Uri), typeof (ImageManager), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageManager.UriSourcePropertyChangedCallback)));

    public static Uri GetUriSource(Image element) => element != null ? (Uri) ((DependencyObject) element).GetValue(ImageManager.UriSourceProperty) : throw new ArgumentNullException(nameof (element));

    public static void SetUriSource(Image element, Uri value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      ((DependencyObject) element).SetValue(ImageManager.UriSourceProperty, (object) value);
    }

    private static void UriSourcePropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is Image image))
        return;
      Uri newValue = e.NewValue as Uri;
      if ((Uri) null == newValue)
        return;
      if (DesignerProperties.IsInDesignTool)
      {
        try
        {
          image.Source = (ImageSource) new BitmapImage(newValue);
        }
        catch
        {
        }
      }
      else
        ImageDispatcher.Enqueue(new ImageQueueItem(image, newValue));
    }
  }
}
