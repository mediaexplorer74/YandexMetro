// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.XorUtil
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Text;

namespace Yandex.StringUtils
{
  public class XorUtil
  {
    public static string Xor(string text, string password)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      if (password == null)
        throw new ArgumentNullException(nameof (password));
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      int index1 = 0;
      int index2 = 0;
      int length1 = text.Length;
      int length2 = password.Length;
      for (; index1 < length1; ++index1)
      {
        stringBuilder.Append((char) ((uint) text[index1] ^ (uint) password[index2]));
        if (index2 == length2 - 1)
          index2 = 0;
        else
          ++index2;
      }
      return stringBuilder.ToString();
    }

    public static byte[] Xor(byte[] text, byte[] password)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      if (password == null)
        throw new ArgumentNullException(nameof (password));
      byte[] numArray = new byte[text.Length];
      int index1 = 0;
      int num = password.Length - 1;
      for (int index2 = 0; index2 < text.Length; ++index2)
      {
        numArray[index2] = (byte) ((uint) text[index2] ^ (uint) password[index1]);
        if (index1 == num)
          index1 = 0;
        else
          ++index1;
      }
      return numArray;
    }
  }
}
