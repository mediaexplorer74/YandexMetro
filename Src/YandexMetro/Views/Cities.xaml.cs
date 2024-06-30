// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Cities
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Diagnostics;
using System.Windows;
using Yandex.Metro.Logic;

namespace Yandex.Metro.Views
{
  public class Cities : MetroPage
  {
    private bool _contentLoaded;

    public Cities() => this.InitializeComponent();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Cities.xaml", UriKind.Relative));
    }
  }
}
