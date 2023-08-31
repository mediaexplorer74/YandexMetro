// Decompiled with JetBrains decompiler
// Type: Y.Common.Extensions.EnumExtensions
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;

namespace Y.Common.Extensions
{
  public static class EnumExtensions
  {
    public static bool HasFlag(this Enum target, Enum flagToCheck)
    {
      if (target == null)
        return false;
      if (flagToCheck == null)
        throw new ArgumentNullException(nameof (flagToCheck));
      ulong num = Enum.IsDefined(target.GetType(), (object) flagToCheck) ? Convert.ToUInt64((object) flagToCheck) : throw new ArgumentException(string.Format("Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.", (object) flagToCheck.GetType(), (object) target.GetType()));
      return ((long) Convert.ToUInt64((object) target) & (long) num) == (long) num;
    }
  }
}
