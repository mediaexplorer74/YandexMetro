// Decompiled with JetBrains decompiler
// Type: Yandex.Media.ColorHelper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Windows.Media;

namespace Yandex.Media
{
  public static class ColorHelper
  {
    public static Color FromArgb(int value)
    {
      byte num1 = (byte) (value >> 24);
      if (num1 == (byte) 0)
        num1 = byte.MaxValue;
      byte num2 = (byte) (value >> 16);
      byte num3 = (byte) (value >> 8);
      byte num4 = (byte) value;
      return Color.FromArgb(num1, num2, num3, num4);
    }
  }
}
