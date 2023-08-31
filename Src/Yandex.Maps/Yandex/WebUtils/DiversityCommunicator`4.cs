// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.DiversityCommunicator`4
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal abstract class DiversityCommunicator<TQueryBuilder, TParameters, TResult, TErrorResult> : 
    DefaultCommunicatorBase<TQueryBuilder, TParameters, TResult>,
    IDiversityCommunicator<TParameters, TResult, TErrorResult>,
    ICommunicator<TParameters, TResult>
    where TQueryBuilder : class, IQueryBuilder
    where TResult : class
    where TErrorResult : class
  {
    private readonly IGenericXmlSerializer<TErrorResult> _errorSerializer;

    protected DiversityCommunicator(
      [NotNull] TQueryBuilder queryBuilder,
      [NotNull] IGenericXmlSerializer<TResult> serializer,
      [NotNull] IGenericXmlSerializer<TErrorResult> errorSerializer,
      [NotNull] IWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(queryBuilder, serializer, webClientFactory, itemCounter)
    {
      this._errorSerializer = errorSerializer != null ? errorSerializer : throw new ArgumentNullException(nameof (errorSerializer));
    }

    protected override void ProcessResultStream(TParameters parameters, Stream stream)
    {
      byte[] buffer = new byte[1024];
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int count;
        while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
          memoryStream.Write(buffer, 0, count);
        try
        {
          memoryStream.Seek(0L, SeekOrigin.Begin);
          TResult result = this._serializer.Deserialize((Stream) memoryStream, true);
          this.AfterRequestExecuted(parameters, result);
          this.OnRequestCompleted(parameters, result);
        }
        catch (InvalidOperationException ex)
        {
          memoryStream.Seek(0L, SeekOrigin.Begin);
          TErrorResult result = this._errorSerializer.Deserialize((Stream) memoryStream);
          this.AfterRequestExecutedWithErrorResult(parameters, result);
          this.OnRequestCompletedWithErrorResult(parameters, result);
        }
      }
    }

    protected abstract void AfterRequestExecutedWithErrorResult(
      TParameters requestParameters,
      TErrorResult result);

    public event RequestCompletedEventHandler<TParameters, TErrorResult> RequestCompletedWithErrorResult;

    private void OnRequestCompletedWithErrorResult(TParameters parameters, TErrorResult result)
    {
      RequestCompletedEventHandler<TParameters, TErrorResult> completedWithErrorResult = this.RequestCompletedWithErrorResult;
      if (completedWithErrorResult == null)
        return;
      completedWithErrorResult((object) this, new RequestCompletedEventArgs<TParameters, TErrorResult>(parameters, result));
    }
  }
}
