// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.CommunicatorBase`3
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Net;
using Yandex.Common;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal abstract class CommunicatorBase<TQueryBuilder, TParameters, TResult> : 
    ICommunicator<TParameters, TResult>
    where TQueryBuilder : class, IQueryBuilder
    where TResult : class
  {
    protected readonly TQueryBuilder _queryBuilder;
    protected readonly IGenericXmlSerializer<TResult> _serializer;
    protected readonly IWebClientFactory _webClientFactory;
    private readonly IItemCounter _itemCounter;

    public event RequestCompletedEventHandler<TParameters, TResult> RequestCompleted;

    public event EventHandler<RequestFailedEventArgs<TParameters>> RequestFailed;

    protected CommunicatorBase(
      [NotNull] TQueryBuilder queryBuilder,
      [NotNull] IGenericXmlSerializer<TResult> serializer,
      [NotNull] IWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
    {
      if ((object) queryBuilder == null)
        throw new ArgumentNullException(nameof (queryBuilder));
      if (serializer == null)
        throw new ArgumentNullException(nameof (serializer));
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
      if (itemCounter == null)
        throw new ArgumentNullException(nameof (itemCounter));
      this._queryBuilder = queryBuilder;
      this._serializer = serializer;
      this._webClientFactory = webClientFactory;
      this._itemCounter = itemCounter;
    }

    public abstract void Request(TParameters parameters);

    protected void OnRequestFailed(Exception exception, TParameters parameters) => this.OnRequestFailed(new RequestFailedEventArgs<TParameters>(exception, parameters));

    protected void OnRequestCompleted(TParameters parameters, TResult result) => this.OnRequestCompleted(new RequestCompletedEventArgs<TParameters, TResult>(parameters, result));

    private void OnRequestFailed(RequestFailedEventArgs<TParameters> e)
    {
      EventHandler<RequestFailedEventArgs<TParameters>> requestFailed = this.RequestFailed;
      if (requestFailed == null)
        return;
      requestFailed((object) this, e);
    }

    private void OnRequestCompleted(RequestCompletedEventArgs<TParameters, TResult> e)
    {
      RequestCompletedEventHandler<TParameters, TResult> requestCompleted = this.RequestCompleted;
      if (requestCompleted == null)
        return;
      requestCompleted((object) this, e);
    }

    protected abstract void AfterRequestExecuted(TParameters requestParameters, TResult result);

    protected void Execute(TParameters requestParameters, string queryString)
    {
      try
      {
        Uri uri = new Uri(queryString);
        this._itemCounter.Increment();
        this.ExecuteInternal(requestParameters, uri);
      }
      catch (NotSupportedException ex)
      {
        this._itemCounter.Decrement();
        this.OnRequestFailed((Exception) ex, requestParameters);
      }
      catch (WebException ex)
      {
        this._itemCounter.Decrement();
        this.OnRequestFailed((Exception) ex, requestParameters);
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
        this._itemCounter.Decrement();
        this.OnRequestFailed(ex, requestParameters);
      }
    }

    protected abstract void ExecuteInternal(TParameters requestParameters, Uri uri);

    protected void GetResponseCallback(IHttpWebRequest request, IAsyncResult asyncResult)
    {
      TParameters asyncState = (TParameters) asyncResult.AsyncState;
      try
      {
        using (IWebResponse response = request.EndGetResponse(asyncResult))
        {
          using (Stream responseStream = response.GetResponseStream())
            this.ProcessResultStream(asyncState, responseStream);
        }
      }
      catch (WebException ex)
      {
        this.OnRequestFailed((Exception) ex, asyncState);
      }
      catch (InvalidOperationException ex)
      {
        this.OnRequestFailed((Exception) ex, asyncState);
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
        this.OnRequestFailed(ex, asyncState);
      }
      finally
      {
        this._itemCounter.Decrement();
      }
    }

    protected virtual void ProcessResultStream(TParameters parameters, Stream stream)
    {
      TResult result = this._serializer.Deserialize(stream);
      this.AfterRequestExecuted(parameters, result);
      this.OnRequestCompleted(parameters, result);
    }
  }
}
