// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.ImageQueueItem
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows.Controls;

namespace Yandex.Controls.Images
{
  internal class ImageQueueItem
  {
    private readonly WeakReference _image;

    public ImageQueueItem(Image image, Uri uri)
    {
      this._image = image != null ? new WeakReference((object) image) : throw new ArgumentNullException(nameof (image));
      this.Uri = uri;
    }

    public Uri Uri { get; private set; }

    public Image Image => this._image.IsAlive ? this._image.Target as Image : (Image) null;
  }
}
