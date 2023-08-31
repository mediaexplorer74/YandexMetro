// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ViewModels.AboutViewModelBase
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using Yandex.App.Information.Views.ViewModels.Interfaces;

namespace Yandex.App.Information.Views.ViewModels
{
  public abstract class AboutViewModelBase : IAboutViewModel
  {
    public AboutViewModelBase(
      IApplicationInformationViewModel applicationInformationViewModel)
    {
      this.ApplicationInformation = applicationInformationViewModel != null ? applicationInformationViewModel : throw new ArgumentNullException(nameof (applicationInformationViewModel));
    }

    public IApplicationInformationViewModel ApplicationInformation { get; private set; }

    public abstract void NavigateToApplications();
  }
}
