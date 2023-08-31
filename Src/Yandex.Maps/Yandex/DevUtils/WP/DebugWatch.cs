// Decompiled with JetBrains decompiler
// Type: Yandex.DevUtils.WP.DebugWatch
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Diagnostics;

namespace Yandex.DevUtils.WP
{
  internal class DebugWatch
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
