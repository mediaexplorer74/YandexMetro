// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.BasePage
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Y.UI.Common
{
  public class BasePage : PhoneApplicationPage
  {
    public BasePage() => ((FrameworkElement) this).Loaded += (RoutedEventHandler) ((s, e) => this.InitSystemTray());

    public virtual void InitSystemTray()
    {
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (((Page) this).NavigationContext.QueryString.ContainsKey("forgetPreviousPage") && Convert.ToBoolean(((Page) this).NavigationContext.QueryString["forgetPreviousPage"]))
      {
        ((Page) this).NavigationContext.QueryString.Remove("forgetPreviousPage");
        PageNavigationService.ForgetPreviousPage();
      }
      ((Page) this).OnNavigatedTo(e);
    }
  }
}
