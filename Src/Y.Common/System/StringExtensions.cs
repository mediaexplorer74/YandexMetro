// Decompiled with JetBrains decompiler
// Type: System.StringExtensions
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace System
{
  public static class StringExtensions
  {
    public static Color ToColor(this string value)
    {
      if (value == null)
        return Colors.Transparent;
      if (value[0] == '#')
        value = value.Remove(0, 1);
      int length = value.Length;
      switch (length)
      {
        case 6:
        case 8:
          if (StringExtensions.IsHexColor(value))
          {
            if (length == 8)
              return Color.FromArgb(byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber));
            if (length == 6)
              return Color.FromArgb(byte.MaxValue, byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber));
            break;
          }
          break;
      }
      string[] strArray = value.Split(new char[2]
      {
        ',',
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray != null)
      {
        if (strArray.Length == 4)
          return Color.FromArgb(byte.Parse(strArray[0]), byte.Parse(strArray[1]), byte.Parse(strArray[2]), byte.Parse(strArray[3]));
        if (strArray.Length == 3)
          return Color.FromArgb(byte.MaxValue, byte.Parse(strArray[0]), byte.Parse(strArray[1]), byte.Parse(strArray[2]));
      }
      return Colors.Transparent;
    }

    private static bool IsHexColor(string value) => value != null && ((IEnumerable<char>) value.ToCharArray()).All<char>(new Func<char, bool>(Uri.IsHexDigit));
  }
}
