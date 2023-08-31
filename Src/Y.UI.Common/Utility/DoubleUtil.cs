// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.DoubleUtil
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;

namespace Y.UI.Common.Utility
{
  public static class DoubleUtil
  {
    internal const double DblEpsilonRelative1 = 1.1102230246251568E-16;

    public static bool AreClose(double value1, double value2)
    {
      if (value1 == value2)
        return true;
      double num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 1.1102230246251568E-16;
      double num2 = value1 - value2;
      return -num1 < num2 && num1 > num2;
    }
  }
}
