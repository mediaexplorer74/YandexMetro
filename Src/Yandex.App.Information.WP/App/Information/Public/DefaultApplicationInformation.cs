// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Public.DefaultApplicationInformation
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Yandex.App.Information.Interfaces;

namespace Yandex.App.Information.Public
{
  public class DefaultApplicationInformation : ApplicationInformationBase
  {
    private const string ManifestXmlFileName = "WMAppManifest.xml";

    private static Assembly GetEntryPointAssembly() => ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Single<Assembly>((Func<Assembly, bool>) (assembly => assembly.FullName.StartsWith(Deployment.Current.EntryPointAssembly + ",")));

    private static IManifestInformationProvider GetInitedManifestInformationProvider()
    {
      ManifestInformationProvider informationProvider = new ManifestInformationProvider();
      informationProvider.Initialize(Application.GetResourceStream(new Uri("WMAppManifest.xml", UriKind.Relative)).Stream);
      informationProvider.Info.Image = new BitmapImage(new Uri("/" + informationProvider.Info.ImageUri, UriKind.Relative));
      return (IManifestInformationProvider) informationProvider;
    }

    public DefaultApplicationInformation(
      ILiceseAgreementQueryBuilder liceseAgreementQueryBuilder)
      : base(DefaultApplicationInformation.GetEntryPointAssembly(), liceseAgreementQueryBuilder, DefaultApplicationInformation.GetInitedManifestInformationProvider())
    {
      this.PublishYear = 2012;
    }
  }
}
