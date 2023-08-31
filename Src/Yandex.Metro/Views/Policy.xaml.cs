// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Policy
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Y.UI.Common;
using Yandex.Metro.Logic;

namespace Yandex.Metro.Views
{
  public class Policy : PhoneApplicationPage
  {
    private bool _contentLoaded;

    public Policy() => this.InitializeComponent();

    private void ButtonAcceptClick(object sender, RoutedEventArgs e)
    {
      MetroService.Instance.AppSettings.GpsEnabled = true;
      PageNavigationService.GoBack();
    }

    private void ButtonCancelClick(object sender, RoutedEventArgs e)
    {
      MetroService.Instance.AppSettings.GpsEnabled = false;
      PageNavigationService.GoBack();
    }

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      MetroService.Instance.AppSettings.GpsEnabled = false;
      base.OnBackKeyPress(e);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Policy.xaml", UriKind.Relative));
    }
  }
}
