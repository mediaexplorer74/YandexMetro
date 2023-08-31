// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.ImagePool
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Media.Imaging
{
  internal class ImagePool : DispatcheredPool<Image>, IImagePool, IPool<Image>
  {
    private Size _imageSize;

    public ImagePool(Size imageSize, IUiDispatcher uiDispatcher)
      : base(uiDispatcher)
    {
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._imageSize = imageSize;
    }

    public new void Push([NotNull] Image image)
    {
      if (image == null)
        throw new ArgumentNullException(nameof (image));
      image.Source = (ImageSource) null;
      base.Push(image);
    }

    protected override Image CreateObject()
    {
      Size imageSize = this.GetImageSize();
      Image image = new Image();
      ((FrameworkElement) image).VerticalAlignment = (VerticalAlignment) 0;
      ((FrameworkElement) image).HorizontalAlignment = (HorizontalAlignment) 0;
      image.Stretch = (Stretch) 2;
      ((FrameworkElement) image).Width = imageSize.Width;
      ((FrameworkElement) image).Height = imageSize.Height;
      return image;
    }

    protected virtual Size GetImageSize() => this._imageSize;
  }
}
