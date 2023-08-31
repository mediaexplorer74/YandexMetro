// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InternalConstants
// Assembly: Zlib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 235FDED2-38DA-4349-9C02-D4B9C65CF362
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Zlib.dll

namespace Ionic.Zlib
{
  internal static class InternalConstants
  {
    internal static readonly int MAX_BITS = 15;
    internal static readonly int BL_CODES = 19;
    internal static readonly int D_CODES = 30;
    internal static readonly int LITERALS = 256;
    internal static readonly int LENGTH_CODES = 29;
    internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;
    internal static readonly int MAX_BL_BITS = 7;
    internal static readonly int REP_3_6 = 16;
    internal static readonly int REPZ_3_10 = 17;
    internal static readonly int REPZ_11_138 = 18;
  }
}
