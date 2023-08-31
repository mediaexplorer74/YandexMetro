// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.ApplicationInformationBase
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Yandex.App.Information.Interfaces;

namespace Yandex.App.Information
{
  public class ApplicationInformationBase : IApplicationInformation
  {
    private static readonly DateTime EraStartDate = new DateTime(2000, 1, 1);
    private readonly ILiceseAgreementQueryBuilder _liceseAgreementQueryBuilder;
    private static readonly Regex _versionRegex = new Regex(", Version=(?<Version>\\d+.\\d+.\\d+.\\d+)", RegexOptions.Singleline);

    public ApplicationInformationBase(
      Assembly applicationAssembly,
      ILiceseAgreementQueryBuilder liceseAgreementQueryBuilder,
      [NotNull] IManifestInformationProvider manifestInformationProvider)
      : this(applicationAssembly, liceseAgreementQueryBuilder, manifestInformationProvider, (string) null)
    {
    }

    public ApplicationInformationBase(
      Assembly applicationAssembly,
      ILiceseAgreementQueryBuilder liceseAgreementQueryBuilder,
      [NotNull] IManifestInformationProvider manifestInformationProvider,
      string uuid)
    {
      if (liceseAgreementQueryBuilder == null)
        throw new ArgumentNullException(nameof (liceseAgreementQueryBuilder));
      if (manifestInformationProvider == null)
        throw new ArgumentNullException(nameof (manifestInformationProvider));
      this._liceseAgreementQueryBuilder = liceseAgreementQueryBuilder;
      this.Version = new Version(ApplicationInformationBase.GetVersion(applicationAssembly.FullName));
      this.DateCreated = ApplicationInformationBase.EraStartDate + TimeSpan.FromDays((double) this.Version.Build);
      this.Image = manifestInformationProvider.Info.Image;
      this.ApplicationName = manifestInformationProvider.Info.ApplicationName.ToUpper();
      this.Uuid = uuid;
    }

    private static string GetVersion(string fullName)
    {
      try
      {
        Match match = ApplicationInformationBase._versionRegex.Match(fullName);
        return match.Success ? match.Groups["Version"].Value : "0.0.0.0";
      }
      catch
      {
        return "0.0.0.0";
      }
    }

    public BitmapImage Image { get; private set; }

    public string ApplicationName { get; private set; }

    public Version Version { get; private set; }

    public DateTime DateCreated { get; private set; }

    public int PublishYear { get; protected set; }

    public string LicenseAgreementUri => this._liceseAgreementQueryBuilder.GetLicenseAgreementUri();

    public virtual string Uuid { get; private set; }

    public virtual string FeedbackEmail { get; protected set; }
  }
}
