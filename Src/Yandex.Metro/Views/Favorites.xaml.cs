// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Favorites
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common.Control;
using Yandex.Metro.Logic;
using Yandex.Metro.Resources;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views
{
  public class Favorites : MetroPage, IFavorites
  {
    private BindableApplicationBarIconButton _editButton;
    private BindableApplicationBarIconButton _addButton;
    private BindableApplicationBarIconButton _removeButton;
    internal Grid LayoutRoot;
    internal MultiselectList multiSelectList;
    internal BindableApplicationBar bar;
    private bool _contentLoaded;

    public Favorites()
    {
      this.InitializeComponent();
      this.InitButtons();
      Locator.MainStatic.IFavorites = (IFavorites) this;
      this.UpdateAppBar(false);
    }

    private void InitButtons()
    {
      this._editButton = new BindableApplicationBarIconButton()
      {
        IconUri = new Uri("/Images/edit.png", UriKind.RelativeOrAbsolute),
        Text = Localization.FavoriteButton_Edit,
        Command = (ICommand) Locator.MainStatic.FavoritesSelectionCommand,
        CommandParameter = (object) true
      };
      this._addButton = new BindableApplicationBarIconButton()
      {
        IconUri = new Uri("/Images/add.png", UriKind.RelativeOrAbsolute),
        Text = Localization.FavoriteButton_Add,
        Command = (ICommand) Locator.MainStatic.ShowFavoritesSelection
      };
      this._removeButton = new BindableApplicationBarIconButton()
      {
        IconUri = new Uri("/Toolkit.Content/ApplicationBar.Delete.png", UriKind.RelativeOrAbsolute),
        Text = Localization.FavoriteButton_Remove
      };
      this._removeButton.Click += new EventHandler(this.RemoveButtonClick);
    }

    private void RemoveButtonClick(object sender, EventArgs e)
    {
      MainViewModel main = Locator.MainStatic;
      foreach (MetroStation metroStation in this.multiSelectList.SelectedItems.OfType<MetroStation>().Where<MetroStation>((Func<MetroStation, bool>) (station => ((Collection<MetroStation>) main.Favorites).Contains(station))).ToList<MetroStation>())
      {
        MetroStation item = metroStation;
        ApplicationSettings settings = MetroService.Instance.AppSettings;
        Y.Metro.ServiceLayer.Entities.Favorites favorites = settings.Favorites.FirstOrDefault<Y.Metro.ServiceLayer.Entities.Favorites>((Func<Y.Metro.ServiceLayer.Entities.Favorites, bool>) (r => r.SchemaId == settings.Scheme.Id && r.StationId == item.Id));
        settings.Favorites.Remove(favorites);
        ((Collection<MetroStation>) main.Favorites).Remove(item);
      }
      this.multiSelectList.IsSelectionEnabled = false;
    }

    public void UpdateAppBar(bool isSelectionOn)
    {
      if (isSelectionOn)
      {
        this.bar.SetHidden(this._addButton, this._editButton);
        this.bar.SetVisible(this._removeButton);
      }
      else
      {
        this.bar.SetHidden(this._removeButton);
        this.bar.SetVisible(this._addButton);
        if (Locator.MainStatic.Favorites == null || ((Collection<MetroStation>) Locator.MainStatic.Favorites).Count <= 0)
          return;
        this.bar.SetVisible(this._editButton);
      }
    }

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      base.OnBackKeyPress(e);
      if (!this.multiSelectList.IsSelectionEnabled)
        return;
      this.multiSelectList.IsSelectionEnabled = false;
      e.Cancel = true;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Favorites.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.multiSelectList = (MultiselectList) ((FrameworkElement) this).FindName("multiSelectList");
      this.bar = (BindableApplicationBar) ((FrameworkElement) this).FindName("bar");
    }
  }
}
