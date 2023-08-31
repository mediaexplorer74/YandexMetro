// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Threading.DispatcherHelper
// Assembly: GalaSoft.MvvmLight.Extras.WP71, Version=3.0.0.19987, Culture=neutral, PublicKeyToken=null
// MVID: 57152B65-77D5-4127-ACD4-AE83D05B8401
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.Extras.WP71.dll

using System;
using System.Windows;
using System.Windows.Threading;

namespace GalaSoft.MvvmLight.Threading
{
  public static class DispatcherHelper
  {
    public static Dispatcher UIDispatcher { get; private set; }

    public static void CheckBeginInvokeOnUI(Action action)
    {
      if (DispatcherHelper.UIDispatcher.CheckAccess())
        action();
      else
        DispatcherHelper.UIDispatcher.BeginInvoke(action);
    }

    public static void Initialize()
    {
      if (DispatcherHelper.UIDispatcher != null)
        return;
      DispatcherHelper.UIDispatcher = ((DependencyObject) Deployment.Current).Dispatcher;
    }
  }
}
