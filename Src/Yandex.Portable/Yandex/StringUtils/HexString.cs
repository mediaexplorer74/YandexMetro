// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.HexString
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;

namespace Yandex.StringUtils
{
  public class HexString
  {
    public static string HexStr([NotNull] byte[] data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      char[] chArray1 = new char[16]
      {
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F'
      };
      int num1 = 0;
      int num2 = 0;
      int length = data.Length;
      char[] chArray2 = new char[length * 2];
      while (num1 < length)
      {
        byte num3 = data[num1++];
        char[] chArray3 = chArray2;
        int index1 = num2;
        int num4 = index1 + 1;
        int num5 = (int) chArray1[(int) num3 / 16];
        chArray3[index1] = (char) num5;
        char[] chArray4 = chArray2;
        int index2 = num4;
        num2 = index2 + 1;
        int num6 = (int) chArray1[(int) num3 % 16];
        chArray4[index2] = (char) num6;
      }
      return new string(chArray2, 0, chArray2.Length);
    }
  }
}
