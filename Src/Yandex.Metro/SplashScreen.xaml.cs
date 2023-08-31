// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.SplashScreen
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Diagnostics;
using System.Windows;
using Y.UI.Common;
using Yandex.Metro.Logic;

namespace Yandex.Metro
{
  public class SplashScreen : MetroPage
  {
    private bool _contentLoaded;

    public SplashScreen()
    {
      this.InitializeComponent();
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.SplashScreenLoaded);
    }

    private void SplashScreenLoaded(object sender, RoutedEventArgs e)
    {
      if (MetroService.Instance.AppSettings.PrivacyPolicyShown)
      {
        MetroService.Instance.DetectSchemasAndRedirect();
      }
      else
      {
        MetroService.Instance.AppSettings.PrivacyPolicyShown = true;
        PageNavigationService.Navigate(Constants.ToPolicy);
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/SplashScreen.xaml", UriKind.Relative));
    }
  }
}
