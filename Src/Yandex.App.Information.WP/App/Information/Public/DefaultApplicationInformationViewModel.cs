// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Public.DefaultApplicationInformationViewModel
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.Windows;
using Yandex.App.Information.Interfaces;
using Yandex.App.Information.Views.ViewModels;
using Yandex.App.Information.Views.ViewModels.Interfaces;

namespace Yandex.App.Information.Public
{
  public class DefaultApplicationInformationViewModel : 
    ApplicationInformationViewModel,
    IApplicationInformationViewModel
  {
    public static readonly DependencyProperty LicenseAgreementUriProperty = DependencyProperty.Register(nameof (LicenseAgreementUri), typeof (string), typeof (DefaultApplicationInformationViewModel), (PropertyMetadata) null);
    public static readonly DependencyProperty UuidProperty = DependencyProperty.Register(nameof (Uuid), typeof (string), typeof (DefaultApplicationInformationViewModel), (PropertyMetadata) null);
    public static readonly DependencyProperty FeedbackEmailProperty = DependencyProperty.Register(nameof (FeedbackEmail), typeof (string), typeof (DefaultApplicationInformationViewModel), (PropertyMetadata) null);

    public DefaultApplicationInformationViewModel()
      : base((IApplicationInformation) new DefaultApplicationInformation((ILiceseAgreementQueryBuilder) new DefaultApplicationInformationViewModel.DummyLiceseAgreementQueryBuilder()))
    {
    }

    public new string LicenseAgreementUri
    {
      get => (string) this.GetValue(DefaultApplicationInformationViewModel.LicenseAgreementUriProperty);
      set => this.SetValue(DefaultApplicationInformationViewModel.LicenseAgreementUriProperty, (object) value);
    }

    public new string Uuid
    {
      get => (string) this.GetValue(DefaultApplicationInformationViewModel.UuidProperty);
      set => this.SetValue(DefaultApplicationInformationViewModel.UuidProperty, (object) value);
    }

    public new string FeedbackEmail
    {
      get => (string) this.GetValue(DefaultApplicationInformationViewModel.FeedbackEmailProperty);
      set => this.SetValue(DefaultApplicationInformationViewModel.FeedbackEmailProperty, (object) value);
    }

    public new Visibility FeedbackButtonVisibility => (Visibility) 0;

    private class DummyLiceseAgreementQueryBuilder : ILiceseAgreementQueryBuilder
    {
      public string GetLicenseAgreementUri() => (string) null;
    }
  }
}
