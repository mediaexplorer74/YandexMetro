// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ApplicationInformationControl
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Yandex.App.Information.Views.ViewModels.Interfaces;
using Yandex.Common;

namespace Yandex.App.Information.Views
{
  public class ApplicationInformationControl : UserControl
  {
    private bool _contentLoaded;

    public ApplicationInformationControl() => this.InitializeComponent();

    private IApplicationInformationViewModel ViewModel => ((FrameworkElement) this).DataContext is IApplicationInformationViewModel dataContext ? dataContext : throw new Exception("Unsupported data context type");

    private void ImageDoubleTap(object sender, GestureEventArgs e) => this.ViewModel.ImageDoubleTap();

    public event EventHandler ApplicationsClick;

    private void ApplicationsTap(object sender, GestureEventArgs e)
    {
      EventHandler applicationsClick = this.ApplicationsClick;
      if (applicationsClick == null)
        return;
      applicationsClick((object) this, (EventArgs) e);
    }

    private void LicenseAgreementTap(object sender, GestureEventArgs e) => ShellHelper.TryOpenLinkInWebBrowser(new Uri(this.ViewModel.LicenseAgreementUri, UriKind.Absolute));

    private void FeedbackTap(object sender, GestureEventArgs e)
    {
      try
      {
        new EmailComposeTask()
        {
          To = this.ViewModel.FeedbackEmail,
          Subject = (Resources.FeedbackSubject + " " + this.ViewModel.ApplicationName + " WP7 " + this.ViewModel.VersionBuildNumbler + " UUID " + this.ViewModel.Uuid)
        }.Show();
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.App.Information.WP;component/Views/ApplicationInformationControl.xaml", UriKind.Relative));
    }
  }
}
