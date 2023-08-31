// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.DefaultCommunicatorBase`3
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  public abstract class DefaultCommunicatorBase<TQueryBuilder, TParameters, TResult> : 
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
