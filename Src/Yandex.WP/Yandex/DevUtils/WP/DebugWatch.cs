// Decompiled with JetBrains decompiler
// Type: Yandex.DevUtils.WP.DebugWatch
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Diagnostics;

namespace Yandex.DevUtils.WP
{
  public class DebugWatch
  {
    private static long _startTickCount;
    private static long _endTickCount;

    [Conditional("DEBUG")]
    public static void Reset()
    {
      DebugWatch._startTickCount = 0L;
      DebugWatch._endTickCount = 0L;
    }

    [Conditional("DEBUG")]
    public static void Start() => DebugWatch._startTickCount = (long) Environment.TickCount;

    [Conditional("DEBUG")]
    public static void Stop() => DebugWatch._endTickCount = (long) Environment.TickCount;

    [Conditional("DEBUG")]
    public static void Print(string prompt)
    {
      int tickCount = Environment.TickCount;
      long startTickCount = DebugWatch._startTickCount;
    }
  }
}
