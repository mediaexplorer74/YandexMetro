// Decompiled with JetBrains decompiler
// Type: Yandex.Log.CapptainLoggerHelper
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;

namespace Yandex.Log
{
  public class CapptainLoggerHelper
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
  }
}
