// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ViewModels.ApplicationInformationViewModel
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;
using Yandex.App.Information.Interfaces;
using Yandex.App.Information.Views.ViewModels.Interfaces;

namespace Yandex.App.Information.Views.ViewModels
{
  public class ApplicationInformationViewModel : DependencyObject, IApplicationInformationViewModel
  {
    private readonly IApplicationInformation _info;

    public ApplicationInformationViewModel([NotNull] IApplicationInformation info) => this._info = info != null ? info : throw new ArgumentNullException(nameof (info));

    public string VersionString
    {
      get
      {
        if (!(this._info.Version != (Version) null))
          return (string) null;
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.VersionFormat, (object) this._info.Version.Major, (object) this._info.Version.Minor, (object) this._info.DateCreated);
      }
    }

    public string BuildString => this._info.Version != (Version) null ? string.Format(Resources.BuildFormat, (object) this._info.Version.Build, (object) this._info.Version.Revision) : (string) null;

    public string CopyrightString
    {
      get
      {
        string str = this._info.PublishYear.ToString();
        if (this._info.DateCreated.Year != this._info.PublishYear)
          str = str + "–" + (object) this._info.DateCreated.Year;
        return string.Format(Resources.CopyrightFormat, (object) str);
      }
    }

    public string LicenseAgreementUri => this._info.LicenseAgreementUri;

    public BitmapImage Image => this._info.Image;

    public virtual string ApplicationName => this._info.ApplicationName;

    public string Uuid => this._info.Uuid;

    public string VersionBuildNumbler => this._info.Version != (Version) null ? this._info.Version.ToString() : (string) null;

    public string FeedbackEmail => this._info.FeedbackEmail;

    public Visibility FeedbackButtonVisibility => !string.IsNullOrEmpty(this._info.FeedbackEmail) ? (Visibility) 0 : (Visibility) 1;

    public virtual void ImageDoubleTap()
    {
      string uuid = this.Uuid;
      if (string.IsNullOrWhiteSpace(uuid))
        return;
      Clipboard.SetText(uuid);
      MessageBox.Show(string.Format(Resources.UuidMessageFormat, (object) uuid));
    }
  }
}
