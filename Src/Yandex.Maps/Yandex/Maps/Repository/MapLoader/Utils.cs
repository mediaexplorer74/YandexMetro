// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.Utils
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Maps.Repository.MapLoader
{
  internal static class Utils
  {
    public static int GetBitFree(byte[] buf, int bitIn)
    {
      int length = buf.Length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        int num = (int) buf[index1];
        for (int index2 = 7; index2 >= 0; --index2)
        {
          if ((num >> index2 & 1) == bitIn)
            return (index1 << 3) + (8 - index2);
        }
      }
      return -1;
    }

    public static int getBit(byte[] buf, ushort bitPosition)
    {
      int num = (int) bitPosition - 1;
      int index = num >> 3;
      return ((int) buf[index] & (int) byte.MaxValue) >> 7 - num % 8 & 1;
    }

    public static void SetBit(byte[] buf, int bitIn, int bitPosition)
    {
      int num1 = bitPosition - 1;
      int index = num1 >> 3;
      int num2 = (int) buf[index] & (int) byte.MaxValue;
      if (bitIn == 1)
      {
        int num3 = num2 | 1 << 7 - num1 % 8;
        buf[index] = (byte) num3;
      }
      else
      {
        if (bitIn != 0)
          return;
        int num4 = ~(~num2 | 1 << 7 - num1 % 8);
        buf[index] = (byte) num4;
      }
    }

    public static short CalcIndex(int x, int y) => (short) (x & (int) sbyte.MaxValue | y << 7);

    public static int CalcMortonNumber(int x, int y)
    {
      x = (x | x << 8) & 16711935;
      x = (x | x << 4) & 252645135;
      x = (x | x << 2) & 858993459;
      x = (x | x << 1) & 1431655765;
      y = (y | y << 8) & 16711935;
      y = (y | y << 4) & 252645135;
      y = (y | y << 2) & 858993459;
      y = (y | y << 1) & 1431655765;
      return x | y << 1;
    }
  }
}
