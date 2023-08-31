// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.MathUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Utilities
{
  internal class MathUtils
  {
    public static int IntLength(int i)
    {
      if (i < 0)
        throw new ArgumentOutOfRangeException();
      return i == 0 ? 1 : (int) Math.Floor(Math.Log10((double) i)) + 1;
    }

    public static int HexToInt(char h)
    {
      if (h >= '0' && h <= '9')
        return (int) h - 48;
      if (h >= 'a' && h <= 'f')
        return (int) h - 97 + 10;
      return h >= 'A' && h <= 'F' ? (int) h - 65 + 10 : -1;
    }

    public static char IntToHex(int n) => n <= 9 ? (char) (n + 48) : (char) (n - 10 + 97);

    public static int GetDecimalPlaces(double value)
    {
      int y = 10;
      double num = Math.Pow(0.1, (double) y);
      if (value == 0.0)
        return 0;
      int decimalPlaces;
      for (decimalPlaces = 0; value - Math.Floor(value) > num && decimalPlaces < y; ++decimalPlaces)
        value *= 10.0;
      return decimalPlaces;
    }

    public static int? Min(int? val1, int? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new int?(Math.Min(val1.Value, val2.Value));
    }

    public static int? Max(int? val1, int? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new int?(Math.Max(val1.Value, val2.Value));
    }

    public static double? Min(double? val1, double? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new double?(Math.Min(val1.Value, val2.Value));
    }

    public static double? Max(double? val1, double? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new double?(Math.Max(val1.Value, val2.Value));
    }

    public static bool ApproxEquals(double d1, double d2) => Math.Abs(d1 - d2) < Math.Abs(d1) * 1E-06;
  }
}
