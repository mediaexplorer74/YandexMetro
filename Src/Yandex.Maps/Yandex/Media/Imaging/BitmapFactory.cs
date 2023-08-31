// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.BitmapFactory
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Windows.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;

namespace Yandex.Media.Imaging
{
  internal class BitmapFactory : IBitmapFactory
  {
    public BitmapSource GetBitmap(byte[] data, int offset, int length)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (length <= 0)
        throw new ArgumentOutOfRangeException(nameof (length));
      if (offset + length > data.Length)
        throw new ArgumentOutOfRangeException("offset + length > data.Length");
      using (MemoryStream memoryStream = new MemoryStream(data, offset, length))
      {
        BitmapImage bitmap = new BitmapImage()
        {
          CreateOptions = (BitmapCreateOptions) 18
        };
        ((BitmapSource) bitmap).SetSource((Stream) memoryStream);
        return (BitmapSource) bitmap;
      }
    }

    public BitmapSource GetBitmap(Stream stream)
    {
      if (stream == null || stream.Length == 0L)
        return (BitmapSource) null;
      BitmapImage bitmap = new BitmapImage()
      {
        CreateOptions = (BitmapCreateOptions) 18
      };
      try
      {
        ((BitmapSource) bitmap).SetSource(stream);
        return (BitmapSource) bitmap;
      }
      catch (Exception ex)
      {
        return (BitmapSource) null;
      }
    }
  }
}
