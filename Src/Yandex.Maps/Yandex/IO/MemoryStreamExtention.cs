// Decompiled with JetBrains decompiler
// Type: Yandex.IO.MemoryStreamExtention
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.IO;

namespace Yandex.IO
{
  internal static class MemoryStreamExtention
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
