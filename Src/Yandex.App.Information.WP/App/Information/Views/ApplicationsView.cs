// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ApplicationsView
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Yandex.App.Information.Views
{
  public class ApplicationsView : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal AppListControl AppListControl;
    private bool _contentLoaded;

    public ApplicationsView() => this.InitializeComponent();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.App.Information.WP;component/Views/ApplicationsView.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) ((FrameworkElement) this).FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) ((FrameworkElement) this).FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) ((FrameworkElement) this).FindName("PageTitle");
      this.AppListControl = (AppListControl) ((FrameworkElement) this).FindName("AppListControl");
    }
  }
}
