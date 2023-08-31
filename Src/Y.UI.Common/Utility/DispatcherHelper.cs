// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.DispatcherHelper
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Windows;
using System.Windows.Threading;

namespace Y.UI.Common.Utility
{
  public static class DispatcherHelper
  {
    public static Dispatcher UIDispatcher { get; private set; }

    public static bool CheckAccess() => DispatcherHelper.UIDispatcher.CheckAccess();

    public static void CheckBeginInvokeOnUI(Action action)
    {
      if (DispatcherHelper.UIDispatcher.CheckAccess())
        action();
      else
        DispatcherHelper.UIDispatcher.BeginInvoke(action);
    }

    public static void UI(Action action) => DispatcherHelper.CheckBeginInvokeOnUI(action);

    public static void Initialize()
    {
      if (DispatcherHelper.UIDispatcher != null)
        return;
      DispatcherHelper.UIDispatcher = ((DependencyObject) Deployment.Current).Dispatcher;
    }
  }
}
