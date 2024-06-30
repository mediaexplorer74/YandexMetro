// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Partial.SelectStation
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views.Partial
{
  public class SelectStation : UserControl
  {
    private bool _contentLoaded;

    public SelectStation() => this.InitializeComponent();

    private void StartButtonTap(object sender, GestureEventArgs e)
    {
      MainViewModel mainStatic = Locator.MainStatic;
      if (!mainStatic.IsRouteAvailable && !mainStatic.IsPlainMode && !mainStatic.SearchIsActive)
        mainStatic.MetroMap.AnimateSelectStation(mainStatic.SelectElement, mainStatic.SelectedStation.LineReference.Color, true);
      mainStatic.StartStationCommand.Execute(mainStatic.SelectedStation);
    }

    private void EndButtonTap(object sender, GestureEventArgs e)
    {
      MainViewModel mainStatic = Locator.MainStatic;
      if (!mainStatic.IsRouteAvailable && !mainStatic.IsPlainMode && !mainStatic.SearchIsActive)
        mainStatic.MetroMap.AnimateSelectStation(mainStatic.SelectElement, mainStatic.SelectedStation.LineReference.Color, false);
      mainStatic.EndStationCommand.Execute(mainStatic.SelectedStation);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Partial/SelectStation.xaml", UriKind.Relative));
    }
  }
}
