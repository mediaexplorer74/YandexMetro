// Decompiled with JetBrains decompiler
// Type: Yandex.Log.CapptainLoggerHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;

namespace Yandex.Log
{
  internal class CapptainLoggerHelper
  {
    private const int MaxErrorNameLength = 64;
    private const int MaxDataLength = 896;

    public static void PrepareExceptionForCapptain(
      [NotNull] Exception ex,
      out string name,
      out string message,
      out string stackTrace)
    {
      string message1 = ex.Message;
      string data = ex.GetType().Name + " " + message1;
      string stackTrace1 = ex.StackTrace;
      name = CapptainLoggerHelper.TrimToMaxLength(data, 64);
      message = CapptainLoggerHelper.TrimToMaxLength(message1, 896 - name.Length);
      stackTrace = CapptainLoggerHelper.TrimToMaxLength(stackTrace1, 896 - name.Length - message.Length);
    }

    private static string TrimToMaxLength(string data, int maxLength)
    {
      int val2 = Math.Max(0, maxLength);
      return data?.Substring(0, Math.Min(data.Length, val2));
    }

    public static void PrepareExceptionForCapptain(
      [NotNull] Exception ex,
      out string message,
      out string stackTrace)
    {
      CapptainLoggerHelper.PrepareExceptionForCapptain(ex, out string _, out message, out stackTrace);
    }
  }
}
