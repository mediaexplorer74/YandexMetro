// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.PngUtil
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;

namespace Yandex.Media.Imaging
{
  internal class PngUtil
  {
    public static void InvertPngPalette(byte[] data, int offset, int length)
    {
      int int32_1;
      int position;
      bool flag;
      using (MemoryStream input = new MemoryStream(data, offset, length))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          if (binaryReader.ReadUInt64() != 727905341920923785UL)
            return;
          int int32_2;
          do
          {
            byte[] numArray = binaryReader.ReadBytes(4);
            Array.Reverse((Array) numArray);
            int32_1 = BitConverter.ToInt32(numArray, 0);
            if (int32_1 != 0)
            {
              int32_2 = BitConverter.ToInt32(binaryReader.ReadBytes(4), 0);
              position = (int) input.Position;
              binaryReader.ReadBytes(int32_1);
              binaryReader.ReadBytes(4);
            }
            else
              goto label_3;
          }
          while (int32_2 != 1163152464);
          goto label_7;
label_3:
          return;
label_7:
          flag = true;
        }
      }
      if (!flag)
        return;
      for (int index = offset + position; index < offset + position + int32_1; index += 3)
      {
        byte num1 = (byte) ((uint) byte.MaxValue - (uint) data[index]);
        byte num2 = (byte) ((uint) byte.MaxValue - (uint) data[index + 1]);
        byte num3 = (byte) ((uint) byte.MaxValue - (uint) data[index + 2]);
        data[index] = (byte) ((int) num1 * 42959 + (int) num2 * 441598 + (int) num3 * 564019 >> 20);
        data[index + 1] = (byte) ((int) num1 * 564007 + (int) num2 * 43115 + (int) num3 * 441454 >> 20);
        data[index + 2] = (byte) ((int) num1 * 441441 + (int) num2 * 564164 + (int) num3 * 42971 >> 20);
      }
    }
  }
}
