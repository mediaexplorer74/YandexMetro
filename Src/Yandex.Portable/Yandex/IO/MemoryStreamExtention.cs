// Decompiled with JetBrains decompiler
// Type: Yandex.IO.MemoryStreamExtention
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System.IO;

namespace Yandex.IO
{
  public static class MemoryStreamExtention
  {
    public static MemoryStream Copy(this MemoryStream stream)
    {
      MemoryStream memoryStream = new MemoryStream();
      stream.Seek(0L, SeekOrigin.Begin);
      stream.WriteTo((Stream) memoryStream);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      return memoryStream;
    }
  }
}
