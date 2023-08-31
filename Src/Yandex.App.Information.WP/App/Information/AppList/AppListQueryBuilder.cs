// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.AppListQueryBuilder
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.App.Information.AppList
{
  public class AppListQueryBuilder : IQueryBuilder
  {
    private readonly IAppListUrlProvider _urlProvider;

    public AppListQueryBuilder([NotNull] IAppListUrlProvider urlProvider) => this._urlProvider = urlProvider != null ? urlProvider : throw new ArgumentNullException(nameof (urlProvider));

    public string GetQuery(AppListRequestParameters parameters)
    {
      Dictionary<string, object> parameters1 = new Dictionary<string, object>();
      parameters1["id"] = (object) parameters.ApplicationId;
      parameters1["lang"] = (object) parameters.CultureInfo.TwoLetterISOLanguageName;
      parameters1["platform"] = (object) parameters.Platform;
      parameters1["geo"] = (object) parameters.GeoCode;
      StringBuilder stringBuilder = new StringBuilder(this._urlProvider.GetBaseUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters1));
      return stringBuilder.ToString();
    }
  }
}
