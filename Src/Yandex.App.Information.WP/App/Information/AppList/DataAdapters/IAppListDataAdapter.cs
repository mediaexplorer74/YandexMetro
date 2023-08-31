// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.DataAdapters.IAppListDataAdapter
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System.Collections.Generic;
using Yandex.App.Information.AppList.Dto;
using Yandex.App.Information.Models;

namespace Yandex.App.Information.AppList.DataAdapters
{
  public interface IAppListDataAdapter
  {
    [NotNull]
    IList<AppItem> ReadAppList([NotNull] Apps dto);
  }
}
