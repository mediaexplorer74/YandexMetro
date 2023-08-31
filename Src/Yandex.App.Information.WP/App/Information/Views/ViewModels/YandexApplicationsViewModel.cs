// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ViewModels.YandexApplicationsViewModel
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using Yandex.App.Information.AppList;
using Yandex.App.Information.Views.ViewModels.Interfaces;

namespace Yandex.App.Information.Views.ViewModels
{
  public class YandexApplicationsViewModel : IApplicationsViewModel
  {
    private readonly IAppListViewModel _appListViewModel;

    public YandexApplicationsViewModel(
      [NotNull] IAppListViewModel appListViewModel,
      [NotNull] IApplicationInformationViewModel applicationInformationViewModel,
      string applicationId)
    {
      if (appListViewModel == null)
        throw new ArgumentNullException(nameof (appListViewModel));
      if (applicationInformationViewModel == null)
        throw new ArgumentNullException(nameof (applicationInformationViewModel));
      this._appListViewModel = appListViewModel;
      this._appListViewModel.Parameters = new AppListParameters()
      {
        ApplicationId = applicationId,
        Platform = "wp"
      };
      this.ApplicationInformation = applicationInformationViewModel;
    }

    public IAppListViewModel AppListViewModel => this._appListViewModel;

    public IApplicationInformationViewModel ApplicationInformation { get; private set; }
  }
}
