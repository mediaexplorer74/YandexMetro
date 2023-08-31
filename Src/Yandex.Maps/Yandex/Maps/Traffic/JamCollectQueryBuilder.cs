// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamCollectQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Text;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  internal class JamCollectQueryBuilder : IJamCollectQueryBuilder, IQueryBuilder
  {
    private readonly IPrinterUrlProvider _printerUrlProvider;

    public JamCollectQueryBuilder(IPrinterUrlProvider printerUrlProvider) => this._printerUrlProvider = printerUrlProvider != null ? printerUrlProvider : throw new ArgumentNullException(nameof (printerUrlProvider));

    public string GetJamCollectQuery(string uuid, bool compressed, long packetid)
    {
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters[nameof (uuid)] = (object) uuid;
      parameters[nameof (compressed)] = compressed ? (object) "1" : (object) "0";
      parameters[nameof (packetid)] = (object) packetid;
      StringBuilder stringBuilder = new StringBuilder(this._printerUrlProvider.GetTrafficCollectUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters));
      return stringBuilder.ToString();
    }
  }
}
