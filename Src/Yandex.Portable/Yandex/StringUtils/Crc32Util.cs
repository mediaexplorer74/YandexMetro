// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Crc32Util
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Text;

namespace Yandex.StringUtils
{
  public class Crc32Util
  {
    private const uint Crc32Polynomial = 3988292384;
    private static uint[] _crcTable;

    private static void BuildCrcTable()
    {
      if (Crc32Util._crcTable != null)
        return;
      Crc32Util._crcTable = new uint[256];
      for (uint index1 = 0; index1 <= (uint) byte.MaxValue; ++index1)
      {
        uint num = index1;
        for (int index2 = 8; index2 > 0; --index2)
        {
          if (((int) num & 1) == 1)
            num = num >> 1 ^ 3988292384U;
          else
            num >>= 1;
        }
        Crc32Util._crcTable[(IntPtr) index1] = num;
      }
    }

    public static uint Crc32(string str) => Crc32Util.Crc32(str == null ? new byte[0] : Encoding.UTF8.GetBytes(str));

    public static uint Crc32(byte[] buffer)
    {
      Crc32Util.BuildCrcTable();
      uint num1 = 0;
      uint num2 = uint.MaxValue;
      int length = buffer.Length;
      while (length-- != 0)
        num2 = num2 >> 8 ^ Crc32Util._crcTable[(IntPtr) (uint) (((int) num2 ^ (int) buffer[(IntPtr) num1++]) & (int) byte.MaxValue)];
      return ~num2;
    }
  }
}
