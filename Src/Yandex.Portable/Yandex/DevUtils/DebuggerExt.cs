// Decompiled with JetBrains decompiler
// Type: Yandex.DevUtils.DebuggerExt
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System.Diagnostics;

namespace Yandex.DevUtils
{
  public class DebuggerExt
  {
    [Conditional("DEBUG")]
    [DebuggerHidden]
    public static void BreakIfAttached()
    {
      if (!Debugger.IsAttached)
        return;
      Debugger.Break();
    }
  }
}
