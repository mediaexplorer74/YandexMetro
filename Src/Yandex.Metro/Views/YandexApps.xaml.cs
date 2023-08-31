// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.YandexApps
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Y.Common;
using Yandex.Metro.Logic;

namespace Yandex.Metro.Views
{
  public class YandexApps : MetroPage
  {
    private readonly CultureInfo _currentCulture;
    internal StackPanel TitlePanel;
    private bool _contentLoaded;

    public YandexApps()
    {
      this._currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureHelper.RuRu;
      this.InitializeComponent();
    }

    protected virtual void OnNavigatedFrom(NavigationEventArgs e)
    {
      Thread.CurrentThread.CurrentCulture = this._currentCulture;
      ((Page) this).OnNavigatedFrom(e);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/YandexApps.xaml", UriKind.Relative));
      this.TitlePanel = (StackPanel) ((FrameworkElement) this).FindName("TitlePanel");
    }
  }
}
