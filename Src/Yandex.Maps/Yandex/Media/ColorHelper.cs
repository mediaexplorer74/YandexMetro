// Decompiled with JetBrains decompiler
// Type: Yandex.Media.ColorHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows.Media;

namespace Yandex.Media
{
  internal static class ColorHelper
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
