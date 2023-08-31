// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.MapKitStartupQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Yandex.App.Information;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.DataAdapters;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.WebUtils;

namespace Yandex.Maps.PrinterClient
{
  internal class MapKitStartupQueryBuilder : IStartupQueryBuilder
  {
    private readonly IPrinterUrlProvider _startupUrlProvider;
    private readonly IVersionStringAdapter _versionStringAdapter;
    private readonly string _productId;

    public MapKitStartupQueryBuilder(
      [NotNull] IPrinterUrlProvider printerUrlProvider,
      [NotNull] IVersionStringAdapter versionStringAdapter,
      [NotNull] IManifestInformationProvider manifestInformationProvider)
    {
      if (printerUrlProvider == null)
        throw new ArgumentNullException(nameof (printerUrlProvider));
      if (versionStringAdapter == null)
        throw new ArgumentNullException(nameof (versionStringAdapter));
      if (manifestInformationProvider == null)
        throw new ArgumentNullException(nameof (manifestInformationProvider));
      this._startupUrlProvider = printerUrlProvider;
      this._versionStringAdapter = versionStringAdapter;
      this._productId = manifestInformationProvider.Info.ProductId;
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
      parameters["ymk_version"] = (object) this._versionStringAdapter.Process(app_version);
      parameters[nameof (app_platform)] = (object) app_platform;
      parameters[nameof (screen_w)] = (object) screen_w;
      parameters[nameof (screen_h)] = (object) screen_h;
      parameters[nameof (uuid)] = (object) uuid;
      parameters[nameof (manufacturer)] = (object) manufacturer;
      parameters[nameof (model)] = (object) model;
      parameters[nameof (platformtype)] = (object) platformtype;
      parameters[nameof (os_version)] = (object) os_version;
      parameters["app_id"] = (object) this._productId;
      StringBuilder stringBuilder = new StringBuilder(this._startupUrlProvider.GetRootStartupUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters));
      if (utf)
        stringBuilder.Append("&utf");
      return stringBuilder.ToString();
    }
  }
}
