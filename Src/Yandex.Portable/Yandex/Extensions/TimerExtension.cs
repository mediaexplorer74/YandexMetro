﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Extensions.TimerExtension
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;
using System.Threading;

namespace Yandex.Extensions
{
  public static class TimerExtension
  {
    public static void Start([NotNull] this Timer timer, int dueTime)
    {
      if (timer == null)
        throw new ArgumentNullException(nameof (timer));
      timer.Change(dueTime, -1);
    }

    public static void Stop([NotNull] this Timer timer)
    {
      if (timer == null)
        throw new ArgumentNullException(nameof (timer));
      timer.Change(-1, -1);
    }
  }
}
