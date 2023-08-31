// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.ContentCompressor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Ionic.Zlib;
using System.IO;
using System.Text;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal class ContentCompressor : IContentCompressor
  {
    private const int BufferSize = 4096;

    public byte[] Compress(string value)
    {
      using (MemoryStream memoryStream1 = new MemoryStream())
      {
        MemoryStream memoryStream2 = (MemoryStream) null;
        try
        {
          memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes(value));
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream1, CompressionMode.Compress))
          {
            byte[] buffer = new byte[4096];
            int count;
            while ((count = memoryStream2.Read(buffer, 0, 4096)) > 0)
              gzipStream.Write(buffer, 0, count);
          }
        }
        finally
        {
          memoryStream2?.Dispose();
        }
        return memoryStream1.ToArray();
      }
    }

    public string ContentType => "application/gzip";
  }
}
