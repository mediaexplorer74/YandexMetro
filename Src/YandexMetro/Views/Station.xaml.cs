// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Station
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Y.Metro.ServiceLayer.FastScheme;
using Yandex.Metro.Logic;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views
{
  public class Station : MetroPage
  {
    internal Grid LayoutRoot;
    private bool _contentLoaded;

    public Station() => this.InitializeComponent();

    private void ContextMenuOpened(object sender, RoutedEventArgs e)
    {
      MenuItem menuItem = (MenuItem) ((PresentationFrameworkCollection<object>) ((ItemsControl) sender).Items)[0];
      MetroStation station = (MetroStation) menuItem.CommandParameter;
      ((Control) menuItem).IsEnabled = !((IEnumerable<MetroStation>) Locator.MainStatic.Favorites).Any<MetroStation>((Func<MetroStation, bool>) (r => r.Id == station.Id));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Station.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
    }
  }
}
