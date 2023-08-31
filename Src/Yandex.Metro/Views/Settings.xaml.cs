// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Settings
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Y.Metro.ServiceLayer.Common;
using Y.Metro.ServiceLayer.Entities;
using Yandex.Metro.Logic;
using Yandex.Metro.Resources;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views
{
  public class Settings : MetroPage
  {
    internal TextBox AssociatedObject;
    private bool _contentLoaded;

    public Settings() => this.InitializeComponent();

    private void ApplicationInformationControlApplicationsClick(object sender, EventArgs e) => Locator.MainStatic.OtherApps.Execute((object) null);

    private void ApplicationInformationControlHold(object sender, GestureEventArgs e)
    {
      this.AssociatedObject.Text = string.Empty;
      ((UIElement) this.AssociatedObject).Visibility = (Visibility) 0;
      ((Control) this.AssociatedObject).Focus();
    }

    private void AssociatedObjectKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != 3 && e.PlatformKeyCode != 10)
        return;
      ((Control) (((FrameworkElement) this.AssociatedObject).Parent as FrameworkElement).FindPageBaseParent())?.Focus();
      this.SetAppMode();
      e.Handled = true;
    }

    private void SetAppMode()
    {
      if (!Constants.IsProdVersion && this.AssociatedObject.Text == "production1metro")
      {
        Constants.IsProdVersion = true;
        MetroService.Instance.RegisterApplication();
      }
      if (Constants.IsProdVersion && this.AssociatedObject.Text == "heroism@test")
      {
        Constants.IsProdVersion = false;
        MessageBox.Show(string.Format(Localization.UUID, (object) ServicesHelper.Uuid));
        MetroService.Instance.RegisterApplication();
      }
      ((UIElement) this.AssociatedObject).Visibility = (Visibility) 1;
    }

    private void AssociatedObjectLostFocus(object sender, RoutedEventArgs e) => this.SetAppMode();

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      SchemeMeta actualMeta = Locator.SettingsStatic.ActualMeta;
      Scheme selectedScheme = Locator.SettingsStatic.SelectedScheme;
      Language mapLanguage = Locator.SettingsStatic.MapLanguage;
      if (actualMeta.Type != selectedScheme.Type || actualMeta.Language != mapLanguage.Code)
      {
        Locator.MainStatic.ClearRoute();
        Locator.MainStatic.MetroMap.GenerateMap();
      }
      base.OnBackKeyPress(e);
    }

    private void UIElement_OnDoubleTap(object sender, GestureEventArgs e) => MessageBox.Show(string.Format(Localization.UUID, (object) ServicesHelper.Uuid));

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Settings.xaml", UriKind.Relative));
      this.AssociatedObject = (TextBox) ((FrameworkElement) this).FindName("AssociatedObject");
    }
  }
}
