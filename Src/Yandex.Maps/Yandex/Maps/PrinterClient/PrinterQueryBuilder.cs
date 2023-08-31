// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterQueryBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.PrinterClient.Tiles;
using Yandex.Serialization.Interfaces;
using Yandex.StringUtils;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.PrinterClient
{
  internal class PrinterQueryBuilder : IPrinterQueryBuilder
  {
    private readonly IPrinterUrlProvider _startupUrlProvider;
    private readonly IConfigMediator _configMediator;
    private readonly IGenericXmlSerializer<TilesRequest> _tilesRequestSerializer;
    private readonly IContentCompressor _contentCompressor;
    private readonly ITileScaleAdapter _tileScaleAdapter;

    public PrinterQueryBuilder(
      [NotNull] IPrinterUrlProvider printerUrlProvider,
      [NotNull] IConfigMediator configMediator,
      [NotNull] IGenericXmlSerializer<TilesRequest> tileRequestSerializer,
      [NotNull] IContentCompressor contentCompressor,
      [NotNull] ITileScaleAdapter tileScaleAdapter)
    {
      if (printerUrlProvider == null)
        throw new ArgumentNullException(nameof (printerUrlProvider));
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (tileRequestSerializer == null)
        throw new ArgumentNullException(nameof (tileRequestSerializer));
      if (contentCompressor == null)
        throw new ArgumentNullException(nameof (contentCompressor));
      if (tileScaleAdapter == null)
        throw new ArgumentNullException(nameof (tileScaleAdapter));
      this._startupUrlProvider = printerUrlProvider;
      this._configMediator = configMediator;
      this._tilesRequestSerializer = tileRequestSerializer;
      this._contentCompressor = contentCompressor;
      this._tileScaleAdapter = tileScaleAdapter;
    }

    public Uri GetPrinterQuery(uint protocolVer)
    {
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters["uuid"] = (object) this._configMediator.Uuid;
      parameters["protocol_ver"] = (object) protocolVer;
      StringBuilder stringBuilder = new StringBuilder(this._startupUrlProvider.GetRootPrinterUrl());
      stringBuilder.Append(QueryStringUtil.ToQueryString(parameters));
      return new Uri(stringBuilder.ToString(), UriKind.Absolute);
    }

    public void BuildPostData(Stream postStream, RequestState requestState, uint protocolVersion)
    {
      byte[] text = this._contentCompressor.Compress(this._tilesRequestSerializer.Serialize(requestState.TilesRequest));
      long num = (long) Crc32Util.Crc32(XorUtil.Xor(text, Encoding.UTF8.GetBytes(this._configMediator.Uuid)));
      requestState.PostData.Boundary = requestState.Boundary;
      requestState.PostData.PlainTextParams["packetid"] = (object) num.ToString();
      requestState.PostData.PlainTextParams["uuid"] = (object) this._configMediator.Uuid;
      requestState.PostData.PlainTextParams["protocol_ver"] = (object) protocolVersion;
      requestState.PostData.PlainTextParams["gzip"] = (object) "1";
      requestState.PostData.PlainTextParams["scalefactor"] = (object) this.GetScaleFactor().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      string apiKey = this._configMediator.ApiKey;
      if (!string.IsNullOrEmpty(apiKey))
        requestState.PostData.PlainTextParams["api_key"] = (object) apiKey;
      requestState.PostData.CompressedParams["tiles"] = text;
      requestState.PostData.CompressedContentType = this._contentCompressor.ContentType;
      requestState.PostData.WriteToStream(postStream);
    }

    private double GetScaleFactor() => this._tileScaleAdapter.Convert(this._configMediator.TilesStretchFactor);
  }
}
