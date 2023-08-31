// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.ImageResponseItem
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Controls.Images
{
  internal class ImageResponseItem
  {
    public ImageResponseItem(ImageQueueItem item, IHttpWebRequest webRequest)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      if (webRequest == null)
        throw new ArgumentNullException(nameof (webRequest));
      this.Item = item;
      this.WebRequest = webRequest;
    }

    public ImageQueueItem Item { get; private set; }

    public IHttpWebRequest WebRequest { get; private set; }
  }
}
