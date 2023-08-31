// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.ObservableCommunicatorBase`2
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  public abstract class ObservableCommunicatorBase<TParameters, TResult> : 
    IObservableCommunicator<TParameters, TResult>
    where TResult : class
  {
    private readonly IItemCounter _itemCounter;
    private readonly INetworkInterfaceWrapper _networkInterfaceWrapper;
    protected readonly IQueryBuilder<TParameters> _queryBuilder;
    protected readonly IGenericXmlSerializer<TResult> _serializer;
    protected readonly IWebClientFactory _webClientFactory;

    protected ObservableCommunicatorBase(
      [NotNull] IQueryBuilder<TParameters> queryBuilder,
      [NotNull] IGenericXmlSerializer<TResult> serializer,
      [NotNull] IWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper)
    {
      if (queryBuilder == null)
        throw new ArgumentNullException(nameof (queryBuilder));
      if (serializer == null)
        throw new ArgumentNullException(nameof (serializer));
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
      if (itemCounter == null)
        throw new ArgumentNullException(nameof (itemCounter));
      if (networkInterfaceWrapper == null)
        throw new ArgumentNullException(nameof (networkInterfaceWrapper));
      this._queryBuilder = queryBuilder;
      this._serializer = serializer;
      this._webClientFactory = webClientFactory;
      this._itemCounter = itemCounter;
      this._networkInterfaceWrapper = networkInterfaceWrapper;
    }

    public virtual IObservable<TResult> Request(TParameters parameters) => Observable.Create<TResult>((Func<IObserver<TResult>, IDisposable>) (o =>
    {
      bool networkAvailable = this._networkInterfaceWrapper.GetIsNetworkAvailable();
      IDisposable disposable = this.GetRequestObservable(parameters, networkAvailable).Subscribe<TResult>(new Action<TResult>(o.OnNext), new Action<Exception>(o.OnError), new Action(o.OnCompleted));
      if (!networkAvailable)
        return disposable;
      return (IDisposable) new CompositeDisposable(new IDisposable[2]
      {
        disposable,
        Disposable.Create((Action) (() => this._itemCounter.Decrement()))
      });
    }));

    private IObservable<TResult> GetRequestObservable(
      TParameters parameters,
      bool isNetworkAvailable)
    {
      return this.CreateWebRequest(new Uri(this._queryBuilder.GetQuery(parameters)), isNetworkAvailable).ObserveOn<IHttpWebRequest>((IScheduler) ThreadPoolScheduler.Instance).SelectMany<IHttpWebRequest, IWebResponse, TResult>(new Func<IHttpWebRequest, IObservable<IWebResponse>>(this.GetResponseStreamAsync), (Func<IHttpWebRequest, IWebResponse, TResult>) ((request, response) => this.ProcessResultStream(parameters, response.GetResponseStream())));
    }

    private IObservable<IWebResponse> GetResponseStreamAsync(IHttpWebRequest request) => Observable.FromAsyncPattern<IWebResponse>(new Func<AsyncCallback, object, IAsyncResult>(request.BeginGetResponse), new Func<IAsyncResult, IWebResponse>(request.EndGetResponse))();

    private IObservable<IHttpWebRequest> CreateWebRequest(Uri uri, bool isNetworkAvailable) => !isNetworkAvailable ? Observable.Throw<IHttpWebRequest>((Exception) new WebException()) : Observable.Create<IHttpWebRequest>((Func<IObserver<IHttpWebRequest>, Action>) (observer =>
    {
      try
      {
        this._itemCounter.Increment();
        observer.OnNext(this._webClientFactory.CreateGetHttpWebRequest(uri));
        observer.OnCompleted();
      }
      catch (Exception ex)
      {
        observer.OnError(ex);
      }
      return (Action) (() => { });
    }));

    protected virtual void AfterRequestExecuted(TParameters requestParameters, TResult result)
    {
    }

    protected virtual TResult ProcessResultStream(TParameters parameters, Stream stream)
    {
      TResult result = this._serializer.Deserialize(stream);
      this.AfterRequestExecuted(parameters, result);
      return result;
    }
  }
}
