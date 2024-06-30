// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.MetroPage
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Phone.Shell;
using System.Windows;
using System.Windows.Data;
using Y.UI.Common;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Logic
{
  public class MetroPage : BasePage
  {
    public override void InitSystemTray()
    {
      Binding binding = new Binding("ProgressIndicatorIsActive")
      {
        Source = (object) Locator.ProgressStatic
      };
      ProgressIndicator progressIndicator = new ProgressIndicator();
      BindingOperations.SetBinding((DependencyObject) progressIndicator, ProgressIndicator.IsVisibleProperty, (BindingBase) binding);
      BindingOperations.SetBinding((DependencyObject) progressIndicator, ProgressIndicator.IsIndeterminateProperty, (BindingBase) binding);
      ((DependencyObject) this).SetValue(SystemTray.ProgressIndicatorProperty, (object) progressIndicator);
    }
  }
}
