// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.AppListControl
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Yandex.App.Information.Models;

namespace Yandex.App.Information.Views
{
  public class AppListControl : UserControl
  {
    internal Grid LayoutRoot;
    private bool _contentLoaded;

    public AppListControl() => this.InitializeComponent();

    private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox))
        return;
      if (e.AddedItems.Count > 0 && e.AddedItems[0] is AppItem addedItem && addedItem.AppUri != (Uri) null)
        ShellHelper.TryOpenLinkInWebBrowser(addedItem.AppUri);
      ((Selector) listBox).SelectedIndex = -1;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.App.Information.WP;component/Views/AppListControl.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
    }
  }
}
