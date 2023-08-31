// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.StreamCommunicatorBase`3
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal abstract class StreamCommunicatorBase<TQueryBuilder, TParameters, TResult> : 
    CommunicatorBase<TQueryBuilder, TParameters, TResult>
    where TQueryBuilder : class, IQueryBuilder
    where TParameters : class
    where TResult : class
  {
    protected StreamCommunicatorBase(
      [NotNull] TQueryBuilder queryBuilder,
      [NotNull] IGenericXmlSerializer<TResult> serializer,
      [NotNull] IWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(queryBuilder, serializer, webClientFactory, itemCounter)
    {
      this.Boundary = "***" + DateTime.Now.Ticks.ToString("x") + "***";
    }

    protected override void ExecuteInternal(TParameters requestParameters, Uri uri)
    {
      IHttpWebRequest request = this._webClientFactory.CreatePostHttpWebRequest(uri, this.Boundary);
      request.BeginGetRequestStream((AsyncCallback) (as1 => this.GetRequestStreamCallback(requestParameters, as1, request)), (object) requestParameters);
    }

    protected string Boundary { get; private set; }

    private void GetRequestStreamCallback(
      TParameters requestParameters,
      IAsyncResult as1,
      IHttpWebRequest request)
    {
      using (Stream requestStream = request.EndGetRequestStream(as1))
        this.WriteDataToStream(requestStream, requestParameters);
      request.BeginGetResponse((AsyncCallback) (as2 => this.GetResponseCallback(request, as2)), (object) requestParameters);
    }

    protected abstract void WriteDataToStream(Stream postStream, TParameters requestParameters);
  }
}
