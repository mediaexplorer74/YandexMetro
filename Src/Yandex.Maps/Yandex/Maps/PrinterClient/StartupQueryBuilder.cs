// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.StartupQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.DataAdapters;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.WebUtils;

namespace Yandex.Maps.PrinterClient
{
  [UsedImplicitly]
  internal class StartupQueryBuilder : IStartupQueryBuilder
  {
    private readonly IPrinterUrlProvider _startupUrlProvider;
    private readonly IVersionStringAdapter _versionStringAdapter;

    public StartupQueryBuilder(
      [NotNull] IPrinterUrlProvider printerUrlProvider,
      [NotNull] IVersionStringAdapter versionStringAdapter)
    {
      if (printerUrlProvider == null)
        throw new ArgumentNullException(nameof (printerUrlProvider));
      if (versionStringAdapter == null)
        throw new ArgumentNullException(nameof (versionStringAdapter));
      this._startupUrlProvider = printerUrlProvider;
      this._versionStringAdapter = versionStringAdapter;
    }

    public string GetStartupQuery(
      Version app_version,
      string app_platform,
      double screen_w,
      double screen_h,
      string uuid,
      string manufacturer,
      string model,
      string platformtype,
      string os_version,
      bool utf)
    {
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters[nameof (app_version)] = (object) this._versionStringAdapter.Process(app_version);
      parameters[nameof (app_platform)] = (object) app_platform;
      parameters[nameof (screen_w)] = (object) screen_w;
      parameters[nameof (screen_h)] = (object) screen_h;
      parameters[nameof (uuid)] = (object) uuid;
      parameters[nameof (manufacturer)] = (object) manufacturer;
      parameters[nameof (model)] = (object) model;
      parameters[nameof (platformtype)] = (object) platformtype;
      parameters[nameof (os_version)] = (object) os_version;
      StringBuilder stringBuilder = new StringBuilder(this._startupUrlProvider.GetRootStartupUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters));
      if (utf)
        stringBuilder.Append("&utf");
      return stringBuilder.ToString();
    }
  }
}
