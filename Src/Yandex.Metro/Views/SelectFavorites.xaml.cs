// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.SelectFavorites
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common;
using Yandex.Metro.Logic;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views
{
  public class SelectFavorites : MetroPage
  {
    internal Grid LayoutRoot;
    private bool _contentLoaded;

    public SelectFavorites() => this.InitializeComponent();

    private void LongListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LongListSelector longListSelector = (LongListSelector) sender;
      if (longListSelector == null || longListSelector.SelectedItem == null || !(longListSelector.SelectedItem is MetroStation))
        return;
      Locator.MainStatic.FavoritesAddCommand.Execute((MetroStation) longListSelector.SelectedItem);
      PageNavigationService.GoBack();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/SelectFavorites.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
    }
  }
}
