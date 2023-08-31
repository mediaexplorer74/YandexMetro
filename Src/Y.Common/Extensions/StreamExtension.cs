// Decompiled with JetBrains decompiler
// Type: Y.Common.Extensions.StreamExtension
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System.IO;
using System.Text;

namespace Y.Common.Extensions
{
  public static class StreamExtension
  {
    public static string UnzipData(this Stream source)
    {
      StreamReader streamReader = new StreamReader(source);
      StringBuilder stringBuilder = new StringBuilder();
      while (!streamReader.EndOfStream)
        stringBuilder.Append(streamReader.ReadLine());
      return stringBuilder.ToString();
    }
  }
}
