// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Extensions.TaskHelper
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;

namespace Y.UI.Common.Extensions
{
  public class TaskHelper
  {
    public static void SafeExec(Action action)
    {
      try
      {
        action();
      }
      catch (InvalidOperationException ex)
      {
        if (ex.Message != null && ex.Message.Contains("2147220990"))
          return;
        throw;
      }
    }
  }
}
