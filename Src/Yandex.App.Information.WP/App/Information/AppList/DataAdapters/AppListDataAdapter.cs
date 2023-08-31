// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.DataAdapters.AppListDataAdapter
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Yandex.App.Information.AppList.Dto;
using Yandex.App.Information.Models;

namespace Yandex.App.Information.AppList.DataAdapters
{
  public class AppListDataAdapter : IAppListDataAdapter
  {
    [NotNull]
    public IList<AppItem> ReadAppList([NotNull] Apps apps)
    {
      if (apps == null)
        throw new ArgumentNullException(nameof (apps));
      IList<AppItem> appItemList = (IList<AppItem>) new List<AppItem>();
      if (apps.Items != null)
      {
        foreach (appsApp appsApp in apps.Items)
        {
          Uri result1;
          Uri.TryCreate(appsApp.img, UriKind.Absolute, out result1);
          Uri result2;
          Uri.TryCreate(appsApp.url, UriKind.Absolute, out result2);
          appItemList.Add(new AppItem(result1, appsApp.title, appsApp.desc, result2));
        }
      }
      return appItemList;
    }
  }
}
