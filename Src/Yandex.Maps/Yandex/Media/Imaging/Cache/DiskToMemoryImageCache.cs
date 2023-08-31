// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.Cache.DiskToMemoryImageCache
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yandex.IO;

namespace Yandex.Media.Imaging.Cache
{
  internal class DiskToMemoryImageCache : IDiskToMemoryImageCache
  {
    private readonly IFileStorage _fileStorage;
    private readonly Dictionary<string, ImageSource> _cache = new Dictionary<string, ImageSource>();

    public DiskToMemoryImageCache([NotNull] IFileStorage fileStorage) => this._fileStorage = fileStorage != null ? fileStorage : throw new ArgumentNullException(nameof (fileStorage));

    public ImageSource GetImage(string fileName)
    {
      ImageSource image;
      lock (this._cache)
      {
        if (!this._cache.TryGetValue(fileName, out image))
          image = this._cache[fileName] = (ImageSource) this.OpenImage(fileName);
      }
      return image;
    }

    [CanBeNull]
    private BitmapImage OpenImage(string fileName)
    {
      try
      {
        using (Stream stream = this._fileStorage.OpenFile(fileName, Yandex.IO.FileMode.Open, Yandex.PAL.IO.FileAccess.Read, Yandex.PAL.IO.FileShare.Read))
        {
          BitmapImage bitmapImage = new BitmapImage()
          {
            CreateOptions = (BitmapCreateOptions) 2
          };
          ((BitmapSource) bitmapImage).SetSource(stream);
          return bitmapImage;
        }
      }
      catch (Exception ex)
      {
        return (BitmapImage) null;
      }
    }
  }
}
