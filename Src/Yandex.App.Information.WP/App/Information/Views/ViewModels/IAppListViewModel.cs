// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ViewModels.IAppListViewModel
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System.Collections.Generic;
using Yandex.App.Information.AppList;
using Yandex.App.Information.Models;

namespace Yandex.App.Information.Views.ViewModels
{
  public interface IAppListViewModel
  {
    IEnumerable<AppItem> AppItems { get; }

    AppListParameters Parameters { get; set; }

    bool LoadingHasFailed { get; }

    bool IsLoading { get; }
  }
}
