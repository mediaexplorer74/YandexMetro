// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.MetroAppInformationViewModel
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Yandex.App.Information.Public;
using Yandex.App.Information.Views.ViewModels.Interfaces;
using Yandex.Metro.Resources;

namespace Yandex.Metro.ViewModel
{
  public class MetroAppInformationViewModel : DependencyObject, IApplicationInformationViewModel
  {
    private const int MajorVersion = 1;
    private const int MinorVersion = 2;
    private readonly DateTime _buildDate = new DateTime(2016, 10, 7);
    private DefaultApplicationInformationViewModel _default = new DefaultApplicationInformationViewModel();
    public static readonly DependencyProperty UuidProperty = DependencyProperty.Register(nameof (Uuid), typeof (string), typeof (MetroAppInformationViewModel), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LicenseAgreementUriProperty = DependencyProperty.Register(nameof (LicenseAgreementUri), typeof (string), typeof (MetroAppInformationViewModel), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty FeedbackEmailProperty = DependencyProperty.Register(nameof (FeedbackEmail), typeof (string), typeof (MetroAppInformationViewModel), new PropertyMetadata((PropertyChangedCallback) null));

    public Visibility FeedbackButtonVisibility => this._default.FeedbackButtonVisibility;

    public string ApplicationName => "Яндекс.Метро";

    public string BuildString => string.Empty;

    public string CopyrightString => string.Format(Localization.CopyrightFormat, (object) this._buildDate.Year);

    public BitmapImage Image => this._default.Image;

    public void ImageDoubleTap() => this._default.ImageDoubleTap();

    public string VersionBuildNumbler => this._default.VersionBuildNumbler;

    public string VersionString => string.Format(Localization.VersionFormat, (object) 1, (object) 2, (object) this._buildDate);

    public string Uuid
    {
      get => (string) this.GetValue(MetroAppInformationViewModel.UuidProperty);
      set => this.SetValue(MetroAppInformationViewModel.UuidProperty, (object) value);
    }

    public string LicenseAgreementUri
    {
      get => (string) this.GetValue(MetroAppInformationViewModel.LicenseAgreementUriProperty);
      set => this.SetValue(MetroAppInformationViewModel.LicenseAgreementUriProperty, (object) value);
    }

    public string FeedbackEmail
    {
      get => (string) this.GetValue(MetroAppInformationViewModel.FeedbackEmailProperty);
      set => this.SetValue(MetroAppInformationViewModel.FeedbackEmailProperty, (object) value);
    }
  }
}
