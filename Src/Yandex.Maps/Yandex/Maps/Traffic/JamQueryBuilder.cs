// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.StringUtils;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  [ComVisible(false)]
  public class JamQueryBuilder : IJamQueryBuilder, IQueryBuilder
  {
    private readonly IPrinterUrlProvider _printerUrlProvider;
    private readonly bool _binary;

    public JamQueryBuilder(IPrinterUrlProvider printerUrlProvider, bool binary)
    {
      this._printerUrlProvider = printerUrlProvider != null ? printerUrlProvider : throw new ArgumentNullException(nameof (printerUrlProvider));
      this._binary = binary;
    }

    public string GetStylesQuery()
    {
      StringBuilder stringBuilder = new StringBuilder(this._printerUrlProvider.GetJamStylesUrl());
      stringBuilder.Append("/tjamstyles.xml");
      return stringBuilder.ToString();
    }

    public string GetTracksQuery(
      string uuid,
      uint zoom,
      double tl_lat,
      double tl_lon,
      double br_lat,
      double br_lon,
      bool gzip,
      bool betaJams)
    {
      long num = (long) Crc32Util.Crc32(XorUtil.Xor(tl_lat.ToString((IFormatProvider) CultureInfo.InvariantCulture) + tl_lon.ToString((IFormatProvider) CultureInfo.InvariantCulture) + br_lat.ToString((IFormatProvider) CultureInfo.InvariantCulture) + br_lon.ToString((IFormatProvider) CultureInfo.InvariantCulture) + zoom.ToString((IFormatProvider) CultureInfo.InvariantCulture), uuid));
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters[nameof (uuid)] = (object) uuid;
      parameters[nameof (zoom)] = (object) zoom;
      parameters[nameof (tl_lat)] = (object) tl_lat;
      parameters[nameof (tl_lon)] = (object) tl_lon;
      parameters[nameof (br_lat)] = (object) br_lat;
      parameters[nameof (br_lon)] = (object) br_lon;
      parameters["packetid"] = (object) num;
      parameters["binary"] = (object) (this._binary ? 1 : 0);
      StringBuilder stringBuilder = new StringBuilder(this._printerUrlProvider.GetTrafficGetUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters));
      if (gzip)
        stringBuilder.Append("&gzip");
      if (betaJams)
        stringBuilder.Append("&betajams");
      return stringBuilder.ToString();
    }
  }
}
