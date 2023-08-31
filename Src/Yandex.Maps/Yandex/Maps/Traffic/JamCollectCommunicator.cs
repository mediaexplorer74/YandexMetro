// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamCollectCommunicator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Yandex.ItemsCounter;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Traffic.DTO.Collect;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Serialization.Interfaces;
using Yandex.StringUtils;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  internal class JamCollectCommunicator : 
    StreamCommunicatorBase<IJamCollectQueryBuilder, JamCollectSenderParameters, JamCollectPoints>,
    IJamCollectSender,
    ICommunicator<JamCollectSenderParameters, JamCollectPoints>
  {
    private readonly IConfigMediator _configMediator;
    private readonly IContentCompressor _contentCompressor;
    private readonly IJamCollectPostDataBuilder _jamCollectPostDataBuilder;

    public JamCollectCommunicator(
      [NotNull] IConfigMediator configMediator,
      [NotNull] IGenericXmlSerializer<JamCollectPoints> jamCollectPointsSerializer,
      [NotNull] IJamCollectQueryBuilder jamCollectQueryBuilder,
      [NotNull] IJamCollectPostDataBuilder jamCollectPostDataBuilder,
      [NotNull] IMapWebClientFactory webClientFactory,
      [NotNull] IContentCompressor contentCompressor,
      [NotNull] IItemCounter itemCounter)
      : base(jamCollectQueryBuilder, jamCollectPointsSerializer, (IWebClientFactory) webClientFactory, itemCounter)
    {
      this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));
      this._jamCollectPostDataBuilder = jamCollectPostDataBuilder != null ? jamCollectPostDataBuilder : throw new ArgumentNullException(nameof (jamCollectPostDataBuilder));
      this._contentCompressor = contentCompressor != null ? contentCompressor : throw new ArgumentNullException(nameof (contentCompressor));
    }

    public override void Request(JamCollectSenderParameters parameters)
    {
      string uuid = this._configMediator.Uuid;
      byte[] numArray = this._contentCompressor.Compress(this._serializer.Serialize(new JamCollectPoints(parameters.JamCollectPoints.Select<JamCollectPointData, JamCollectPoint>((Func<JamCollectPointData, JamCollectPoint>) (p => new JamCollectPoint(p))).ToList<JamCollectPoint>())));
      long packetid = (long) Crc32Util.Crc32(XorUtil.Xor(numArray, Encoding.UTF8.GetBytes(uuid)));
      PostData jamCollectPostData = this._jamCollectPostDataBuilder.GetJamCollectPostData(this.Boundary, uuid, packetid, numArray, this._contentCompressor.ContentType);
      parameters.PostData = jamCollectPostData;
      string jamCollectQuery = this._queryBuilder.GetJamCollectQuery(uuid, true, packetid);
      this.Execute(parameters, jamCollectQuery);
    }

    protected override void AfterRequestExecuted(
      JamCollectSenderParameters requestParameters,
      JamCollectPoints result)
    {
    }

    protected override void WriteDataToStream(
      Stream postStream,
      JamCollectSenderParameters requestParameters)
    {
      requestParameters.PostData.WriteToStream(postStream);
    }

    protected override void ProcessResultStream(
      JamCollectSenderParameters parameters,
      Stream stream)
    {
      this.OnRequestCompleted(parameters, (JamCollectPoints) null);
    }
  }
}
