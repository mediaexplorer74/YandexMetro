// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.DefaultCommunicatorBase`3
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal abstract class DefaultCommunicatorBase<TQueryBuilder, TParameters, TResult> : 
    CommunicatorBase<TQueryBuilder, TParameters, TResult>
    where TQueryBuilder : class, IQueryBuilder
    where TResult : class
  {
    protected DefaultCommunicatorBase(
      [NotNull] TQueryBuilder queryBuilder,
      [NotNull] IGenericXmlSerializer<TResult> serializer,
      [NotNull] IWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(queryBuilder, serializer, webClientFactory, itemCounter)
    {
    }

    protected override void ExecuteInternal(TParameters requestParameters, Uri uri)
    {
      IHttpWebRequest request = this._webClientFactory.CreateGetHttpWebRequest(uri);
      request.BeginGetResponse((AsyncCallback) (asyncResult => this.GetResponseCallback(request, asyncResult)), (object) requestParameters);
    }
  }
}
