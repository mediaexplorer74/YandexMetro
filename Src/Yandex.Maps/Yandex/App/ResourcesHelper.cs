// Decompiled with JetBrains decompiler
// Type: Yandex.App.ResourcesHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Yandex.App.Interfaces;

namespace Yandex.App
{
  internal class ResourcesHelper : IResourcesHelper
  {
    public BitmapImage LoadImage(string relativeUriString)
    {
      StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(relativeUriString, UriKind.Relative));
      BitmapImage bitmapImage = new BitmapImage();
      ((BitmapSource) bitmapImage).SetSource(resourceStream.Stream);
      return bitmapImage;
    }
  }
}
