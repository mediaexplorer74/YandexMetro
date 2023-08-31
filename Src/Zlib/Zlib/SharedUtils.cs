// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.SharedUtils
// Assembly: Zlib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 235FDED2-38DA-4349-9C02-D4B9C65CF362
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Zlib.dll

using System.IO;
using System.Text;

namespace Ionic.Zlib
{
  internal class SharedUtils
  {
    public static int URShift(int number, int bits) => number >>> bits;

    public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
    {
      if (target.Length == 0)
        return 0;
      char[] buffer = new char[target.Length];
      int num = sourceTextReader.Read(buffer, start, count);
      if (num == 0)
        return -1;
      for (int index = start; index < start + num; ++index)
        target[index] = (byte) buffer[index];
      return num;
    }

    internal static byte[] ToByteArray(string sourceString) => Encoding.UTF8.GetBytes(sourceString);

    internal static char[] ToCharArray(byte[] byteArray) => Encoding.UTF8.GetChars(byteArray);
  }
}
